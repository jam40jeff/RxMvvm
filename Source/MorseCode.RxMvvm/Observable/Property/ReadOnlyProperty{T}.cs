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

namespace MorseCode.RxMvvm.Observable.Property
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Common;

    internal class ReadOnlyProperty<T> : ReadableObservablePropertyBase<T>, IReadOnlyProperty<T>
    {
        private readonly Lazy<T> value;

        private readonly IObservable<T> observable;

        internal ReadOnlyProperty(Lazy<T> value)
        {
            Contract.Requires<ArgumentNullException>(value != null, "value");
            Contract.Ensures(this.value != null);
            Contract.Ensures(this.observable != null);

            this.value = value;
            this.observable = Observable.Return(value.Value);

            if (this.observable == null)
            {
                throw new InvalidOperationException(
                    "Result of " + StaticReflection.GetInScopeMethodInfo(() => Observable.Return<object>(null)).Name
                    + " cannot be null.");
            }
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
        /// Gets the value of the property.
        /// </summary>
        /// <returns>
        /// The value of the property.
        /// </returns>
        protected override T GetValue()
        {
            return this.value.Value;
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.value != null);
            Contract.Invariant(this.observable != null);
        }
    }
}