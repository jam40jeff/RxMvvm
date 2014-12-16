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
    using System.Reactive.Linq;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Common.DiscriminatedUnion;

    [Serializable]
    internal class CalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T> : CalculatedPropertyBase<T>, ISerializable
    {
        private readonly TContext context;

        private readonly IObservable<TFirst> firstProperty;

        private readonly IObservable<TSecond> secondProperty;

        private readonly IObservable<TThird> thirdProperty;

        private readonly IObservable<TFourth> fourthProperty;

        private readonly Func<TContext, TFirst, TSecond, TThird, TFourth, T> calculateValue;

        internal CalculatedPropertyWithContext(
            TContext context, 
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            Func<TContext, TFirst, TSecond, TThird, TFourth, T> calculateValue)
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

            RxMvvmConfiguration.EnsureSerializableDelegateIfUsingSerialization(calculateValue);

            this.context = context;
            this.firstProperty = firstProperty;
            this.secondProperty = secondProperty;
            this.thirdProperty = thirdProperty;
            this.fourthProperty = fourthProperty;
            this.calculateValue = calculateValue;

            Func<TFirst, TSecond, TThird, TFourth, IDiscriminatedUnion<object, T, Exception>> calculate =
                (first, second, third, fourth) =>
                {
                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(calculateValue(context, first, second, third, fourth));
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
                    IObservable<Tuple<TFirst, TSecond, TThird, TFourth>> o = firstProperty.CombineLatest(
                        secondProperty, thirdProperty, fourthProperty, Tuple.Create);
                    return o.Subscribe(
                            v =>
                            {
                                isCalculatingSubject.OnNext(true);

                                try
                                {
                                    resultSubject.OnNext(calculate(v.Item1, v.Item2, v.Item3, v.Item4));
                                }
                                catch (Exception e)
                                {
                                    resultSubject.OnNext(
                                        DiscriminatedUnion.Second<object, T, Exception>(e));
                                }

                                isCalculatingSubject.OnNext(false);
                            });
                }));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedPropertyWithContext{TContext,TFirst,TSecond,TThird,TFourth,T}"/> class from serialized data. 
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The serialization context.
        /// </param>
        [ContractVerification(false)]
        // ReSharper disable UnusedParameter.Local
        protected CalculatedPropertyWithContext(SerializationInfo info, StreamingContext context)
            // ReSharper restore UnusedParameter.Local
            : this(
                (TContext)(info.GetValue("c", typeof(TContext)) ?? default(TContext)), 
                (IObservable<TFirst>)info.GetValue("p1", typeof(IObservable<TFirst>)),
                (IObservable<TSecond>)info.GetValue("p2", typeof(IObservable<TSecond>)),
                (IObservable<TThird>)info.GetValue("p3", typeof(IObservable<TThird>)),
                (IObservable<TFourth>)info.GetValue("p4", typeof(IObservable<TFourth>)),
                (Func<TContext, TFirst, TSecond, TThird, TFourth, T>)info.GetValue("f", typeof(Func<TContext, TFirst, TSecond, TThird, TFourth, T>)))
        {
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
            info.AddValue("p3", this.thirdProperty);
            info.AddValue("p4", this.fourthProperty);
            info.AddValue("f", this.calculateValue);
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.firstProperty != null);
            Contract.Invariant(this.secondProperty != null);
            Contract.Invariant(this.thirdProperty != null);
            Contract.Invariant(this.fourthProperty != null);
            Contract.Invariant(this.calculateValue != null);
        }
    }
}