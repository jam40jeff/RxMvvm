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
    internal class CancellableAsyncCalculatedProperty<TFirst, T> : CalculatedPropertyBase<T>, ISerializable
    {
        private readonly IReadableObservableProperty<TFirst> firstProperty;

        private readonly TimeSpan throttleTime;

        private readonly Func<AsyncCalculationHelper, TFirst, Task<T>> calculateValue;

        internal CancellableAsyncCalculatedProperty(
            IReadableObservableProperty<TFirst> firstProperty, TimeSpan throttleTime, Func<AsyncCalculationHelper, TFirst, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(this.firstProperty != null);
            Contract.Ensures(this.calculateValue != null);

            RxMvvmConfiguration.EnsureSerializableDelegateIfUsingSerialization(calculateValue);

            this.firstProperty = firstProperty;
            this.throttleTime = throttleTime;
            this.calculateValue = calculateValue;

            Func<AsyncCalculationHelper, TFirst, Task<IDiscriminatedUnion<object, T, Exception>>> calculate =
                async (helper, first) =>
                {
                    Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(await calculateValue(helper, first));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            // TODO: pick a better scheduler
            this.SetHelper(new CalculatedPropertyHelper(
                (resultSubject, isCalculatingSubject) =>
                {
                    CompositeDisposable d = new CompositeDisposable();
                    IScheduler scheduler = Scheduler.Default;

                    IDisposable scheduledTask = scheduler.ScheduleAsync(
                        async (s, t) =>
                        {
                            await s.Yield();
                            resultSubject.OnNext(
                                await calculate(new AsyncCalculationHelper(s, t), firstProperty.Value));
                        });
                    d.Add(scheduledTask);

                    IObservable<TFirst> o = throttleTime > TimeSpan.Zero
                                                ? firstProperty.Throttle(throttleTime, scheduler)
                                                : firstProperty.ObserveOn(scheduler);
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
                                                await calculate(new AsyncCalculationHelper(s, t), v));
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
                }));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellableAsyncCalculatedProperty{TFirst,T}"/> class. 
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
                (TimeSpan)(info.GetValue("t", typeof(TimeSpan)) ?? default(TimeSpan)),
                (Func<AsyncCalculationHelper, TFirst, Task<T>>)info.GetValue("f", typeof(Func<AsyncCalculationHelper, TFirst, Task<T>>)))
        {
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
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.firstProperty != null);
            Contract.Invariant(this.calculateValue != null);
        }
    }
}