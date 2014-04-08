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
    using System.Diagnostics.Contracts;

    internal class ObservableCollectionChanged<T> : IObservableCollectionChanged<T>
    {
        private readonly IReadOnlyList<T> oldItems;

        private readonly IReadOnlyList<T> newItems;

        internal ObservableCollectionChanged(IReadOnlyList<T> oldItems, IReadOnlyList<T> newItems)
        {
            Contract.Ensures(this.oldItems != null);
            Contract.Ensures(this.newItems != null);

            this.oldItems = oldItems ?? new T[0];
            this.newItems = newItems ?? new T[0];
        }

        IReadOnlyList<T> IObservableCollectionChanged<T>.OldItems
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyList<T>>() != null);

                return this.oldItems;
            }
        }

        IReadOnlyList<T> IObservableCollectionChanged<T>.NewItems
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyList<T>>() != null);

                return this.newItems;
            }
        }

        void IObservableCollectionChanged<T>.Process(Action<IReadOnlyList<T>> processOldItems, Action<IReadOnlyList<T>> processNewItems)
        {
            processOldItems(this.oldItems);
            processNewItems(this.newItems);
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.oldItems != null);
            Contract.Invariant(this.newItems != null);
        }
    }
}