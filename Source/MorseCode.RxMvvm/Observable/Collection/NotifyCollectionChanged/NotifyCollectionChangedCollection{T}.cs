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
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reactive.Concurrency;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Common.StaticReflection;

    internal class NotifyCollectionChangedCollection<T> : ReadOnlyNotifyCollectionChangedCollection<T>,
                                                          INotifyCollectionChangedCollection<T>
    {
        private readonly IObservableCollection<T> observableCollection;

        internal NotifyCollectionChangedCollection(IObservableCollection<T> observableCollection, IScheduler scheduler)
            : base(observableCollection, scheduler)
        {
            Contract.Requires<ArgumentNullException>(observableCollection != null, "observableCollection");
            Contract.Ensures(this.observableCollection != null);

            this.observableCollection = observableCollection;
        }

        int IObservableCollection<T>.Count
        {
            get
            {
                return this.Count;
            }
        }

        int ICollection<T>.Count
        {
            get
            {
                return this.Count;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return this.IsReadOnly;
            }
        }

        private int Count
        {
            get
            {
                return this.observableCollection.Count;
            }
        }

        private bool IsReadOnly
        {
            get
            {
                return this.observableCollection.IsReadOnly;
            }
        }

        T IObservableCollection<T>.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                this[index] = value;
            }
        }

        T IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                this[index] = value;
            }
        }

        T IWritableObservableCollection<T>.this[int index]
        {
            set
            {
                this[index] = value;
            }
        }

        private T this[int index]
        {
            get
            {
                return this.observableCollection[index];
            }

            set
            {
                this.observableCollection[index] = value;
            }
        }

        void IObservableCollection<T>.Insert(int index, T item)
        {
            this.Insert(index, item);
        }

        void IObservableCollection<T>.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        void IObservableCollection<T>.Add(T item)
        {
            this.Add(item);
        }

        [ContractVerification(false)]
        void IObservableCollection<T>.Clear()
        {
            this.Clear();
        }

        bool IObservableCollection<T>.Contains(T item)
        {
            return this.Contains(item);
        }

        void IObservableCollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        bool IObservableCollection<T>.Remove(T item)
        {
            return this.Remove(item);
        }

        [ContractVerification(false)]
        void IList<T>.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        int IWritableObservableCollection<T>.IndexOf(T item)
        {
            return this.IndexOf(item);
        }

        int IObservableCollection<T>.IndexOf(T item)
        {
            return this.IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
            this.Insert(index, item);
        }

        int IList<T>.IndexOf(T item)
        {
            return this.IndexOf(item);
        }

        void IWritableObservableCollection<T>.Insert(int index, T item)
        {
            this.Insert(index, item);
        }

        void IWritableObservableCollection<T>.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        void IWritableObservableCollection<T>.Add(T item)
        {
            this.Add(item);
        }

        [ContractVerification(false)]
        void ICollection<T>.Clear()
        {
            this.Clear();
        }

        [ContractVerification(false)]
        bool ICollection<T>.Contains(T item)
        {
            return this.Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        [ContractVerification(false)]
        bool ICollection<T>.Remove(T item)
        {
            return this.Remove(item);
        }

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        void IWritableObservableCollection<T>.Clear()
        {
            this.Clear();
        }

        bool IWritableObservableCollection<T>.Contains(T item)
        {
            return this.Contains(item);
        }

        void IWritableObservableCollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        bool IWritableObservableCollection<T>.Remove(T item)
        {
            return this.Remove(item);
        }

        private void Insert(int index, T item)
        {
            this.observableCollection.Insert(index, item);
        }

        private void RemoveAt(int index)
        {
            Contract.Ensures(this.Count == Contract.OldValue(this.Count) - 1);

            int previousCount = this.Count;

            this.observableCollection.RemoveAt(index);

            if (this.Count != previousCount - 1)
            {
                throw new InvalidOperationException(
                    StaticReflection<IObservableCollection<T>>.GetMemberInfo(o => o.Count).Name
                    + " must be decremented by 1 when the "
                    + StaticReflection<IObservableCollection<T>>.GetMethodInfo(o => o.RemoveAt(0)).Name
                    + " method is called.");
            }
        }

        private void Add(T item)
        {
            Contract.Ensures(this.Count > Contract.OldValue(this.Count));

            int previousCount = this.Count;

            this.observableCollection.Add(item);

            // enforce contract
            if (this.Count <= previousCount)
            {
                throw new InvalidOperationException(
                    StaticReflection<IObservableCollection<T>>.GetMemberInfo(o => o.Count).Name
                    + " must be greater than the previous "
                    + StaticReflection<IObservableCollection<T>>.GetMemberInfo(o => o.Count).Name + " after the "
                    + StaticReflection<IObservableCollection<T>>.GetMethodInfo(o => o.Clear()).Name
                    + " method is called.");
            }
        }

        private void Clear()
        {
            Contract.Ensures(this.Count == 0);

            this.observableCollection.Clear();

            // enforce contract
            if (this.Count != 0)
            {
                throw new InvalidOperationException(
                    StaticReflection<IObservableCollection<T>>.GetMemberInfo(o => o.Count).Name
                    + " must be 0 after the "
                    + StaticReflection<IObservableCollection<T>>.GetMethodInfo(o => o.Clear()).Name
                    + " method is called.");
            }
        }

        private bool Contains(T item)
        {
            Contract.Ensures(!Contract.Result<bool>() || this.Count > 0);

            bool contains = this.observableCollection.Contains(item);

            // enforce contract
            if (contains && this.Count <= 0)
            {
                throw new InvalidOperationException(
                    StaticReflection<IObservableCollection<T>>.GetMethodInfo(o => o.Contains(default(T))).Name
                    + " must return false if "
                    + StaticReflection<IObservableCollection<T>>.GetMemberInfo(o => o.Count).Name + " is 0.");
            }

            return contains;
        }

        private void CopyTo(T[] array, int arrayIndex)
        {
            this.observableCollection.CopyTo(array, arrayIndex);
        }

        private bool Remove(T item)
        {
            Contract.Ensures(
                !Contract.Result<bool>()
                || ((IObservableCollection<T>)this).Count
                >= Contract.OldValue(((IObservableCollection<T>)this).Count - 1));
            Contract.Ensures(
                !Contract.Result<bool>() || ((IList<T>)this).Count >= Contract.OldValue(((IList<T>)this).Count - 1));

            IObservableCollection<T> collection = this;
            IList<T> list = this;
            int previousCollectionCount = collection.Count;
            int previousListCount = list.Count;

            bool wasRemoved = this.observableCollection.Remove(item);

            // enforce contract
            if ((wasRemoved && collection.Count < previousCollectionCount - 1)
                || (wasRemoved && list.Count < previousListCount - 1))
            {
                throw new InvalidOperationException(
                    StaticReflection<IObservableCollection<T>>.GetMemberInfo(o => o.Count).Name
                    + " may not decrease by more than 1 when an item is removed.");
            }

            return wasRemoved;
        }

        private int IndexOf(T item)
        {
            Contract.Ensures(Contract.Result<int>() >= -1);
            Contract.Ensures(Contract.Result<int>() < ((IObservableCollection<T>)this).Count);
            Contract.Ensures(Contract.Result<int>() < ((IList<T>)this).Count);

            int index = this.observableCollection.IndexOf(item);

            // enforce contract
            IObservableCollection<T> collection = this;
            IList<T> list = this;
            if (index >= collection.Count || index >= list.Count)
            {
                throw new InvalidOperationException(
                    StaticReflection<IObservableCollection<T>>.GetMethodInfo(o => o.IndexOf(default(T))).Name
                    + " may not return a value greater than "
                    + StaticReflection<IObservableCollection<T>>.GetMemberInfo(o => o.Count).Name + ".");
            }

            return index;
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.observableCollection != null);
        }
    }
}