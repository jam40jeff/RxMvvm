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
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    using MorseCode.RxMvvm.Common.StaticReflection;

    [Serializable]
    internal class ReadOnlyProperty<T> : ReadableObservablePropertyBase<T>, IReadOnlyProperty<T>, ISerializable
    {
        private readonly T value;

        private readonly IObservable<T> observable;

        internal ReadOnlyProperty(T value)
        {
            Contract.Ensures(this.observable != null);

            this.value = value;
            this.observable = Observable.Create<T>(
                o =>
                {
                    o.OnNext(value);
                    return Disposable.Empty;
                });

            if (this.observable == null)
            {
                throw new InvalidOperationException(
                    "Result of " + StaticReflection.GetInScopeMethodInfo(() => Observable.Create((Func<IObserver<object>, IDisposable>)null)).Name
                    + " cannot be null.");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyProperty{T}"/> class from serialized data.
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The serialization context.
        /// </param>
        [ContractVerification(false)]
        // ReSharper disable UnusedParameter.Local
        protected ReadOnlyProperty(SerializationInfo info, StreamingContext context)
            // ReSharper restore UnusedParameter.Local
            : this((T)(info.GetValue("v", typeof(T)) ?? default(T)))
        {
        }

        /// <summary>
        /// Gets the on changed observable.
        /// </summary>
        protected override IObservable<T> OnChanged
        {
            get
            {
                return this.observable;
            }
        }

        /// <summary>
        /// Gets the on set observable.
        /// </summary>
        protected override IObservable<T> OnSet
        {
            get
            {
                return this.observable;
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
            info.AddValue("v", this.value);
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <returns>
        /// The value of the property.
        /// </returns>
        protected override T GetValue()
        {
            return this.value;
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.observable != null);
        }
    }
}