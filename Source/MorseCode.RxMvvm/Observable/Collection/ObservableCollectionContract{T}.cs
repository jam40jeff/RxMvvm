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
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IObservableCollection<>))]
    internal abstract class ObservableCollectionContract<T> : IObservableCollection<T>
    {
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        int IObservableCollection<T>.Count
        {
            get
            {
                return 0;
            }
        }

        int ICollection<T>.Count
        {
            get
            {
                return 0;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        int IReadOnlyCollection<T>.Count
        {
            get
            {
                return 0;
            }
        }

        T IObservableCollection<T>.this[int index]
        {
            get
            {
                return default(T);
            }

            set
            {
            }
        }

        T IList<T>.this[int index]
        {
            get
            {
                return default(T);
            }

            set
            {
            }
        }

        T IReadOnlyList<T>.this[int index]
        {
            get
            {
                return default(T);
            }
        }

        T IWritableObservableCollection<T>.this[int index]
        {
            set
            {
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return null;

            // ReSharper restore AssignNullToNotNullAttribute
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return null;

            // ReSharper restore AssignNullToNotNullAttribute
        }

        void IObservableCollection<T>.CopyTo(T[] array, int arrayIndex)
        {
        }

        bool IObservableCollection<T>.Remove(T item)
        {
            return false;
        }

        bool ICollection<T>.Remove(T item)
        {
            return false;
        }

        void IObservableCollection<T>.Insert(int index, T item)
        {
        }

        void IObservableCollection<T>.RemoveAt(int index)
        {
        }

        void IObservableCollection<T>.Add(T item)
        {
        }

        void IObservableCollection<T>.Clear()
        {
            Contract.Ensures(((IObservableCollection<T>)this).Count == 0);
        }

        bool IObservableCollection<T>.Contains(T item)
        {
            return false;
        }

        void IList<T>.RemoveAt(int index)
        {
        }

        int IWritableObservableCollection<T>.IndexOf(T item)
        {
            return 0;
        }

        int IObservableCollection<T>.IndexOf(T item)
        {
            Contract.Ensures(Contract.Result<int>() >= -1);
            Contract.Ensures(Contract.Result<int>() < ((IObservableCollection<T>)this).Count);
            return 0;
        }

        void IList<T>.Insert(int index, T item)
        {
        }

        int IList<T>.IndexOf(T item)
        {
            return 0;
        }

        void IWritableObservableCollection<T>.Insert(int index, T item)
        {
        }

        void IWritableObservableCollection<T>.RemoveAt(int index)
        {
        }

        void IWritableObservableCollection<T>.Add(T item)
        {
        }

        void ICollection<T>.Clear()
        {
        }

        bool ICollection<T>.Contains(T item)
        {
            return false;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
        }

        void ICollection<T>.Add(T item)
        {
        }

        void IWritableObservableCollection<T>.Clear()
        {
        }

        bool IWritableObservableCollection<T>.Contains(T item)
        {
            return false;
        }

        void IWritableObservableCollection<T>.CopyTo(T[] array, int arrayIndex)
        {
        }

        bool IWritableObservableCollection<T>.Remove(T item)
        {
            return false;
        }

        IDisposable IObservable<IObservableCollectionChanged<T>>.Subscribe(
            IObserver<IObservableCollectionChanged<T>> observer)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return null;

            // ReSharper restore AssignNullToNotNullAttribute
        }

        void IDisposable.Dispose()
        {
        }
    }
}