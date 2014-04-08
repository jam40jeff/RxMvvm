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

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollectionChanged{T}"/> class.
        /// </summary>
        /// <param name="oldItems">
        /// The old items which were removed from the collection.
        /// </param>
        /// <param name="newItems">
        /// The new items which were added to the collection.
        /// </param>
        public ObservableCollectionChanged(IReadOnlyList<T> oldItems, IReadOnlyList<T> newItems)
        {
            Contract.Ensures(this.oldItems != null);
            Contract.Ensures(this.newItems != null);

            this.oldItems = oldItems ?? new T[0];
            this.newItems = newItems ?? new T[0];
        }

        /// <summary>
        /// Gets the old items which were removed from the collection.
        /// </summary>
        public IReadOnlyList<T> OldItems
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyList<T>>() != null);

                return this.oldItems;
            }
        }

        /// <summary>
        /// Gets the new items which were added to the collection.
        /// </summary>
        public IReadOnlyList<T> NewItems
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyList<T>>() != null);

                return this.newItems;
            }
        }

        /// <summary>
        /// Executes the specified processing actions for both the old items and the new items.
        /// </summary>
        /// <param name="processOldItems">
        /// The processing action for the old items.
        /// </param>
        /// <param name="processNewItems">
        /// The processing action for the new items.
        /// </param>
        public void Process(Action<IReadOnlyList<T>> processOldItems, Action<IReadOnlyList<T>> processNewItems)
        {
            Contract.Requires(processOldItems != null);
            Contract.Requires(processNewItems != null);

            processOldItems(this.OldItems);
            processNewItems(this.NewItems);
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.oldItems != null);
            Contract.Invariant(this.newItems != null);
        }
    }
}