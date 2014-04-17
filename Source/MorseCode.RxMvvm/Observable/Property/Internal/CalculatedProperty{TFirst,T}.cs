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
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Common.DiscriminatedUnion;

    [Serializable]
    internal class CalculatedProperty<TFirst, T> : CalculatedPropertyBase<T>, ISerializable
    {
        private readonly IReadableObservableProperty<TFirst> firstProperty;

        private readonly Func<TFirst, T> calculateValue;

        private readonly CalculatedPropertyHelper helper;

        internal CalculatedProperty(IReadableObservableProperty<TFirst> firstProperty, Func<TFirst, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(this.firstProperty != null);
            Contract.Ensures(this.calculateValue != null);
            Contract.Ensures(this.helper != null);

            RxMvvm.EnsureSerializableDelegateIfUsingSerialization(calculateValue);

            this.firstProperty = firstProperty;
            this.calculateValue = calculateValue;

            Func<TFirst, IDiscriminatedUnion<object, T, Exception>> calculate = first =>
                {
                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion = DiscriminatedUnion.First<object, T, Exception>(calculateValue(first));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            this.helper = new CalculatedPropertyHelper(
                (resultSubject, isCalculatingSubject) =>
                {
                    resultSubject.OnNext(calculate(firstProperty.Value));

                    return firstProperty.Subscribe(
                        v =>
                        {
                            isCalculatingSubject.OnNext(true);

                            try
                            {
                                resultSubject.OnNext(calculate(v));
                            }
                            catch (Exception e)
                            {
                                resultSubject.OnNext(DiscriminatedUnion.Second<object, T, Exception>(e));
                            }

                            isCalculatingSubject.OnNext(false);
                        });
                });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedProperty{TFirst,T}"/> class from serialized data.
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The serialization context.
        /// </param>
        [ContractVerification(false)]
        // ReSharper disable UnusedParameter.Local
        protected CalculatedProperty(SerializationInfo info, StreamingContext context)
            // ReSharper restore UnusedParameter.Local
            : this(
                (IReadableObservableProperty<TFirst>)info.GetValue("p1", typeof(IReadableObservableProperty<TFirst>)),
                (Func<TFirst, T>)info.GetValue("f", typeof(Func<TFirst, T>)))
        {
        }

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
            Contract.Invariant(this.helper != null);
        }
    }
}