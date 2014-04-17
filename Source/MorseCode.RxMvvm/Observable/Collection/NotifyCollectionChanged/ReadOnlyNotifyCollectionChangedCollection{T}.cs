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

namespace MorseCode.RxMvvm.Observable.Collection.NotifyCollectionChanged
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Common.StaticReflection;

    internal class ReadOnlyNotifyCollectionChangedCollection<T> : IReadableNotifyCollectionChangedCollection<T>
    {
        private readonly IReadableObservableCollection<T> observableCollection;

        private readonly IDisposable subscription;

        internal ReadOnlyNotifyCollectionChangedCollection(
            IReadableObservableCollection<T> observableCollection, IScheduler scheduler)
        {
            Contract.Requires<ArgumentNullException>(observableCollection != null, "observableCollection");
            Contract.Requires<ArgumentNullException>(scheduler != null, "scheduler");
            Contract.Ensures(this.observableCollection != null);
            Contract.Ensures(this.subscription != null);

            this.observableCollection = observableCollection;

            this.subscription = observableCollection.ObserveOn(scheduler).Subscribe(
                c =>
                {
                    int oldItemsCount = c.OldItems == null ? 0 : c.OldItems.Count;
                    int newItemsCount = c.NewItems == null ? 0 : c.NewItems.Count;
                    if (oldItemsCount == 0 && newItemsCount == 0)
                    {
                        return;
                    }

                    OnItemsChanged();
                    if (oldItemsCount != newItemsCount)
                    {
                        OnCountChanged();
                    }

                    // TODO: optimize by passing indexes through
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                });

            if (this.subscription == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IObservable<IObservableCollectionChanged<T>>>.GetMemberInfo(
                        o => o.Subscribe(null)).Name + " cannot be null.");
            }
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

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                this.CollectionChanged += value;
            }

            remove
            {
                this.CollectionChanged -= value;
            }
        }

        private event NotifyCollectionChangedEventHandler CollectionChanged;

        int IReadOnlyCollection<T>.Count
        {
            get
            {
                return this.observableCollection.Count;
            }
        }

        T IReadOnlyList<T>.this[int index]
        {
            get
            {
                return this.observableCollection[index];
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.observableCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.observableCollection.GetEnumerator();
        }

        IDisposable IObservable<IObservableCollectionChanged<T>>.Subscribe(
            IObserver<IObservableCollectionChanged<T>> observer)
        {
            return this.observableCollection.Subscribe(observer);
        }

        void IDisposable.Dispose()
        {
            this.subscription.Dispose();
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

        /// <summary>
        /// Raises the <see cref="INotifyCollectionChanged.CollectionChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = this.CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.observableCollection != null);
            Contract.Invariant(this.subscription != null);
        }
    }
}