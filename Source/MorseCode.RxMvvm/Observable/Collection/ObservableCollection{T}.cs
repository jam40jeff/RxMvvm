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
    using System.Linq;
    using System.Reactive.Subjects;

    internal class ObservableCollection<T> : Collection<T>, IObservableCollection<T>
    {
        private readonly Subject<IObservableCollectionChanged<T>> collectionChanged;

        internal ObservableCollection()
        {
            Contract.Ensures(this.collectionChanged != null);

            this.collectionChanged = new Subject<IObservableCollectionChanged<T>>();
        }

        internal ObservableCollection(IList<T> list)
            : base(list)
        {
            Contract.Ensures(this.collectionChanged != null);

            this.collectionChanged = new Subject<IObservableCollectionChanged<T>>();
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                this.PropertyChanged += value;
            }

            remove
            {
                this.PropertyChanged -= value;
            }
        }

        private event PropertyChangedEventHandler PropertyChanged;

        IDisposable IObservable<IObservableCollectionChanged<T>>.Subscribe(
            IObserver<IObservableCollectionChanged<T>> observer)
        {
            return this.collectionChanged.Subscribe(observer);
        }

        void IDisposable.Dispose()
        {
            this.collectionChanged.Dispose();
        }

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </summary>
        protected override void ClearItems()
        {
            List<T> oldItems = this.ToList();

            base.ClearItems();

            this.collectionChanged.OnNext(new ObservableCollectionChanged<T>(oldItems, null));
            this.OnCountChanged();
            this.OnItemsChanged();
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1"/> at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be inserted.
        /// </param>
        /// <param name="item">
        /// The object to insert. The value can be null for reference types.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than zero.-or-<paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            this.collectionChanged.OnNext(new ObservableCollectionChanged<T>(null, new[] { item }));
            this.OnCountChanged();
            this.OnItemsChanged();
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to remove.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than zero.-or-<paramref name="index"/> is equal to or greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void RemoveItem(int index)
        {
            bool canRemove = index >= 0 && index < Count;
            T[] oldItems = canRemove ? new[] { this[index] } : null;

            base.RemoveItem(index);

            if (canRemove)
            {
                this.collectionChanged.OnNext(new ObservableCollectionChanged<T>(oldItems, null));
                this.OnCountChanged();
                this.OnItemsChanged();
            }
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to replace.
        /// </param>
        /// <param name="item">
        /// The new value for the element at the specified index. The value can be null for reference types.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than zero.-or-<paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void SetItem(int index, T item)
        {
            bool canSet = index >= 0 && index < Count;
            T[] oldItems = canSet ? new[] { this[index] } : null;

            base.SetItem(index, item);

            if (canSet)
            {
                this.collectionChanged.OnNext(new ObservableCollectionChanged<T>(oldItems, new[] { item }));
                this.OnItemsChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the <see cref="IReadOnlyCollection{T}.Count"/> property.
        /// </summary>
        protected virtual void OnCountChanged()
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        }

        // The property syntax in the see tag is correct.
#pragma warning disable 1584,1711,1572,1581,1580
        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the <see cref="P:IReadOnlyList{T}.Item(Int32)"/> property.
        /// </summary>
#pragma warning restore 1584,1711,1572,1581,1580
        protected virtual void OnItemsChanged()
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.collectionChanged != null);
        }
    }
}