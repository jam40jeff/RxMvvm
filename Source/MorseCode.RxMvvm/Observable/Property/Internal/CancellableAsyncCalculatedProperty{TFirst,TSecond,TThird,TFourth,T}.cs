#region License

// Copyright 2014 MorseCode Software
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace MorseCode.RxMvvm.Observable.Property.Internal
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Threading.Tasks;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Common.DiscriminatedUnion;

    [Serializable]
    internal class CancellableAsyncCalculatedProperty<TFirst, TSecond, TThird, TFourth, T> : CalculatedPropertyBase<T>, 
                                                                                  ISerializable
    {
        private readonly IReadableObservableProperty<TFirst> firstProperty;

        private readonly IReadableObservableProperty<TSecond> secondProperty;

        private readonly IReadableObservableProperty<TThird> thirdProperty;

        private readonly IReadableObservableProperty<TFourth> fourthProperty;

        private readonly TimeSpan throttleTime;

        private readonly Func<AsyncCalculationHelper, TFirst, TSecond, TThird, TFourth, Task<T>> calculateValue;

        private readonly CalculatedPropertyHelper helper;

        internal CancellableAsyncCalculatedProperty(
            IReadableObservableProperty<TFirst> firstProperty, 
            IReadableObservableProperty<TSecond> secondProperty, 
            IReadableObservableProperty<TThird> thirdProperty, 
            IReadableObservableProperty<TFourth> fourthProperty, 
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, TSecond, TThird, TFourth, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(fourthProperty != null, "fourthProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(this.firstProperty != null);
            Contract.Ensures(this.secondProperty != null);
            Contract.Ensures(this.thirdProperty != null);
            Contract.Ensures(this.fourthProperty != null);
            Contract.Ensures(this.calculateValue != null);
            Contract.Ensures(this.helper != null);

            RxMvvm.EnsureSerializableDelegateIfUsingSerialization(calculateValue);

            this.firstProperty = firstProperty;
            this.secondProperty = secondProperty;
            this.thirdProperty = thirdProperty;
            this.fourthProperty = fourthProperty;
            this.throttleTime = throttleTime;
            this.calculateValue = calculateValue;

            Func<AsyncCalculationHelper, TFirst, TSecond, TThird, TFourth, Task<IDiscriminatedUnion<object, T, Exception>>> calculate =
                        async (helper, first, second, third, fourth) =>
                        {
                            Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                            IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                            try
                            {
                                discriminatedUnion =
                                    DiscriminatedUnion.First<object, T, Exception>(
                                        await calculateValue(helper, first, second, third, fourth));
                            }
                            catch (Exception e)
                            {
                                discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                            }

                            return discriminatedUnion;
                        };

            // TODO: pick a better scheduler
            this.helper = new CalculatedPropertyHelper(
                (resultSubject, isCalculatingSubject) =>
                {
                    CompositeDisposable d = new CompositeDisposable();
                    IScheduler scheduler = Scheduler.Default;

                    IDisposable scheduledTask = scheduler.ScheduleAsync(
                        async (s, t) =>
                        {
                            await s.Yield();
                            resultSubject.OnNext(
                                await
                                calculate(
                                    new AsyncCalculationHelper(s, t),
                                    firstProperty.Value,
                                    secondProperty.Value,
                                    thirdProperty.Value,
                                    fourthProperty.Value));
                        });
                    d.Add(scheduledTask);

                    IObservable<Tuple<TFirst, TSecond, TThird, TFourth>> o =
                        firstProperty.CombineLatest(secondProperty, thirdProperty, fourthProperty, Tuple.Create);
                    o = throttleTime > TimeSpan.Zero ? o.Throttle(throttleTime, scheduler) : o.ObserveOn(scheduler);
                    d.Add(
                        o.Subscribe(
                            v =>
                            {
                                using (scheduledTask)
                                {
                                }

                                isCalculatingSubject.OnNext(true);

                                scheduledTask = scheduler.ScheduleAsync(
                                    async (s, t) =>
                                    {
                                        try
                                        {
                                            await s.Yield();
                                            resultSubject.OnNext(
                                                await
                                                calculate(
                                                    new AsyncCalculationHelper(s, t),
                                                    v.Item1,
                                                    v.Item2,
                                                    v.Item3,
                                                    v.Item4));
                                        }
                                        catch (Exception e)
                                        {
                                            resultSubject.OnNext(
                                                DiscriminatedUnion.Second<object, T, Exception>(e));
                                        }

                                        isCalculatingSubject.OnNext(false);
                                    });
                            }));

                    return d;
                });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellableAsyncCalculatedProperty{TFirst,TSecond,TThird,TFourth,T}"/> class. 
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The serialization context.
        /// </param>
        [ContractVerification(false)]
        // ReSharper disable UnusedParameter.Local
        protected CancellableAsyncCalculatedProperty(SerializationInfo info, StreamingContext context)
            // ReSharper restore UnusedParameter.Local
            : this(
                (IReadableObservableProperty<TFirst>)info.GetValue("p1", typeof(IReadableObservableProperty<TFirst>)), 
                (IReadableObservableProperty<TSecond>)info.GetValue("p2", typeof(IReadableObservableProperty<TSecond>)), 
                (IReadableObservableProperty<TThird>)info.GetValue("p3", typeof(IReadableObservableProperty<TThird>)), 
                (IReadableObservableProperty<TFourth>)info.GetValue("p4", typeof(IReadableObservableProperty<TFourth>)), 
                (TimeSpan)(info.GetValue("t", typeof(TimeSpan)) ?? default(TimeSpan)),
                (Func<AsyncCalculationHelper, TFirst, TSecond, TThird, TFourth, Task<T>>)info.GetValue("f", typeof(Func<AsyncCalculationHelper, TFirst, TSecond, TThird, TFourth, Task<T>>)))
        {
        }

        /// <summary>
        /// Gets the helper.
        /// </summary>
        protected override CalculatedPropertyHelper Helper
        {
            get
            {
                return this.helper;
            }
        }

        /// <summary>
        /// Gets the object data to serialize.
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The serialization context.
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("p1", this.firstProperty);
            info.AddValue("p2", this.secondProperty);
            info.AddValue("p3", this.thirdProperty);
            info.AddValue("p4", this.fourthProperty);
            info.AddValue("t", this.throttleTime);
            info.AddValue("f", this.calculateValue);
        }

        /// <summary>
        /// Disposes of the property.
        /// </summary>
        protected override void Dispose()
        {
            base.Dispose();

            this.firstProperty.Dispose();
            this.secondProperty.Dispose();
            this.thirdProperty.Dispose();
            this.fourthProperty.Dispose();
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.firstProperty != null);
            Contract.Invariant(this.secondProperty != null);
            Contract.Invariant(this.thirdProperty != null);
            Contract.Invariant(this.fourthProperty != null);
            Contract.Invariant(this.calculateValue != null);
            Contract.Invariant(this.helper != null);
        }
    }
}