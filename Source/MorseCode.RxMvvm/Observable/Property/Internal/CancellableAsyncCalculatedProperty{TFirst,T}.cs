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
        private readonly IObservable<TFirst> firstProperty;

        private readonly TimeSpan throttleTime;

        private readonly Func<AsyncCalculationHelper, TFirst, Task<T>> calculateValue;

        private readonly bool isLongRunningCalculation;

        private IDisposable scheduledTask;

        internal CancellableAsyncCalculatedProperty(
            IObservable<TFirst> firstProperty, TimeSpan throttleTime, Func<AsyncCalculationHelper, TFirst, Task<T>> calculateValue, bool isLongRunningCalculation)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(this.firstProperty != null);
            Contract.Ensures(this.calculateValue != null);

            RxMvvmConfiguration.EnsureSerializableDelegateIfUsingSerialization(calculateValue);

            this.firstProperty = firstProperty;
            this.throttleTime = throttleTime;
            this.calculateValue = calculateValue;
            this.isLongRunningCalculation = isLongRunningCalculation;

            Func<AsyncCalculationHelper, TFirst, Task<IDiscriminatedUnion<object, T, Exception>>> calculate =
                async (helper, first) =>
                {
                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(await calculateValue(helper, first).ConfigureAwait(true));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            this.SetHelper(new CalculatedPropertyHelper(
                (resultSubject, isCalculatingSubject) =>
                {
                    CompositeDisposable d = new CompositeDisposable();
                    IScheduler scheduler = isLongRunningCalculation
                                               ? RxMvvmConfiguration.GetLongRunningCalculationScheduler()
                                               : RxMvvmConfiguration.GetCalculationScheduler();

                    IObservable<TFirst> o = throttleTime > TimeSpan.Zero
                                                ? firstProperty.Throttle(throttleTime, scheduler)
                                                : firstProperty.ObserveOn(scheduler);
                    d.Add(
                        o.Subscribe(
                            v =>
                            {
                                using (this.scheduledTask)
                                {
                                }

                                isCalculatingSubject.OnNext(true);

                                this.scheduledTask = scheduler.ScheduleAsync(
                                    async (s, t) =>
                                    {
                                        try
                                        {
                                            await s.Yield(t).ConfigureAwait(true);
                                            IDiscriminatedUnion<object, T, Exception> result = await calculate(new AsyncCalculationHelper(s, t), v).ConfigureAwait(true);
                                            await s.Yield(t).ConfigureAwait(true);
                                            resultSubject.OnNext(result);
                                        }
                                        catch (OperationCanceledException)
                                        {
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
                (IObservable<TFirst>)info.GetValue("p1", typeof(IObservable<TFirst>)), 
                (TimeSpan)(info.GetValue("t", typeof(TimeSpan)) ?? default(TimeSpan)),
                (Func<AsyncCalculationHelper, TFirst, Task<T>>)info.GetValue("f", typeof(Func<AsyncCalculationHelper, TFirst, Task<T>>)),
                (bool)info.GetValue("l", typeof(bool)))
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
            info.AddValue("l", this.isLongRunningCalculation);
        }

        /// <summary>
        /// Disposes of the property.
        /// </summary>
        protected override void Dispose()
        {
            base.Dispose();

            using (this.scheduledTask)
            {
            }
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.firstProperty != null);
            Contract.Invariant(this.calculateValue != null);
        }
    }
}