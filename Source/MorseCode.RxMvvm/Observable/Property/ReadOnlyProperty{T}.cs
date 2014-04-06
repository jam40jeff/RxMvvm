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
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Common;

    internal class ReadOnlyProperty<T> : IReadableObservableProperty<T>
    {
        private readonly Lazy<T> value;

        private readonly IObservable<T> observable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyProperty{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public ReadOnlyProperty(Lazy<T> value)
        {
            Contract.Requires(value != null);
            Contract.Ensures(this.value != null);
            Contract.Ensures(this.observable != null);

            this.value = value;
            this.observable = Observable.Create<T>(
                o =>
                {
                    o.OnNext(value.Value);
                    o.OnCompleted();
                    return Disposable.Empty;
                });

            if (this.observable == null)
            {
                throw new InvalidOperationException("Result of " + StaticReflection.GetInScopeMethodInfo(() => Observable.Create<object>(o => (Action)null)).Name + " cannot be null.");
            }
        }

        /// <summary>
        /// Gets the on changed.
        /// </summary>
        public IObservable<T> OnChanged
        {
            get
            {
                return this.observable;
            }
        }

        /// <summary>
        /// Gets the on set.
        /// </summary>
        public IObservable<T> OnSet
        {
            get
            {
                return this.observable;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value
        {
            get
            {
                return this.value.Value;
            }
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            return this.observable.Subscribe(observer);
        }

        void IDisposable.Dispose()
        {
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.value != null);
            Contract.Invariant(this.observable != null);
        }
    }
}