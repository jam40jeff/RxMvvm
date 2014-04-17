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
    internal class AsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, T> : CalculatedPropertyBase<T>, ISerializable
    {
        private readonly TContext context;

        private readonly IReadableObservableProperty<TFirst> firstProperty;

        private readonly IReadableObservableProperty<TSecond> secondProperty;

        private readonly TimeSpan throttleTime;

        private readonly Func<TContext, TFirst, TSecond, T> calculateValue;

        private readonly CalculatedPropertyHelper helper;

        internal AsyncCalculatedPropertyWithContext(
            TContext context,
            IReadableObservableProperty<TFirst> firstProperty,
            IReadableObservableProperty<TSecond> secondProperty,
            TimeSpan throttleTime,
            Func<TContext, TFirst, TSecond, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(this.firstProperty != null);
            Contract.Ensures(this.secondProperty != null);
            Contract.Ensures(this.calculateValue != null);
            Contract.Ensures(this.helper != null);

            RxMvvm.EnsureSerializableDelegateIfUsingSerialization(calculateValue);

            this.context = context;
            this.firstProperty = firstProperty;
            this.secondProperty = secondProperty;
            this.throttleTime = throttleTime;
            this.calculateValue = calculateValue;

            Func<TFirst, TSecond, IDiscriminatedUnion<object, T, Exception>> calculate =
                (first, second) =>
                {
                    Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(calculateValue(context, first, second));
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

                    IDisposable scheduledTask = scheduler.ScheduleAsync(async (s, t) => resultSubject.OnNext(await Task.FromResult(calculate(firstProperty.Value, secondProperty.Value))));
                    d.Add(scheduledTask);

                    IObservable<Tuple<TFirst, TSecond>> o = firstProperty.CombineLatest(
                        secondProperty, Tuple.Create);
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
                                            resultSubject.OnNext(
                                                await Task.FromResult(calculate(v.Item1, v.Item2)));
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
        /// Initializes a new instance of the <see cref="AsyncCalculatedPropertyWithContext{TContext,TFirst,TSecond,T}"/> class. 
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The serialization context.
        /// </param>
        [ContractVerification(false)]
        // ReSharper disable UnusedParameter.Local
        protected AsyncCalculatedPropertyWithContext(SerializationInfo info, StreamingContext context)
            // ReSharper restore UnusedParameter.Local
            : this(
                (TContext)(info.GetValue("c", typeof(TContext)) ?? default(TContext)),
                (IReadableObservableProperty<TFirst>)info.GetValue("p1", typeof(IReadableObservableProperty<TFirst>)),
                (IReadableObservableProperty<TSecond>)info.GetValue("p2", typeof(IReadableObservableProperty<TSecond>)),
                (TimeSpan)(info.GetValue("t", typeof(TimeSpan)) ?? default(TimeSpan)),
                (Func<TContext, TFirst, TSecond, T>)info.GetValue("f", typeof(Func<TContext, TFirst, TSecond, T>)))
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
        /// <param name="streamingContext">
        /// The serialization context.
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext streamingContext)
        {
            info.AddValue("c", this.context);
            info.AddValue("p1", this.firstProperty);
            info.AddValue("p2", this.secondProperty);
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
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.firstProperty != null);
            Contract.Invariant(this.secondProperty != null);
            Contract.Invariant(this.calculateValue != null);
            Contract.Invariant(this.helper != null);
        }
    }
}