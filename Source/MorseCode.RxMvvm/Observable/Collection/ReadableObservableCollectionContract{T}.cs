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

    [ContractClassFor(typeof(IReadableObservableCollection<>))]
    internal abstract class ReadableObservableCollectionContract<T> : IReadableObservableCollection<T>
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

        int IReadOnlyCollection<T>.Count
        {
            get
            {
                return 0;
            }
        }

        T IReadOnlyList<T>.this[int index]
        {
            get
            {
                return default(T);
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