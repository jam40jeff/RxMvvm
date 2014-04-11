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

namespace MorseCode.RxMvvm.Observable.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Common;

    internal class ReadOnlyObservableCollection<T> : ReadOnlyCollection<T>, IReadableObservableCollection<T>
    {
        private readonly IObservable<IObservableCollectionChanged<T>> collectionChanged;

        internal ReadOnlyObservableCollection(IList<T> list)
            : base(list)
        {
            Contract.Requires<ArgumentNullException>(list != null, "list");
            Contract.Ensures(this.collectionChanged != null);

            this.collectionChanged = Observable.Never<IObservableCollectionChanged<T>>();

            if (this.collectionChanged == null)
            {
                throw new InvalidOperationException(
                    "Result of " + typeof(Observable).Name + "."
                    + StaticReflection.GetInScopeMethodInfo(() => Observable.Never<IObservableCollectionChanged<T>>())
                                      .Name + " cannot be null.");
            }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        IDisposable IObservable<IObservableCollectionChanged<T>>.Subscribe(
            IObserver<IObservableCollectionChanged<T>> observer)
        {
            return this.collectionChanged.Subscribe(observer);
        }

        void IDisposable.Dispose()
        {
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.collectionChanged != null);
        }
    }
}