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
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IWritableObservableCollection<>))]
    internal abstract class WritableObservableCollectionContract<T> : IWritableObservableCollection<T>
    {
        T IWritableObservableCollection<T>.this[int index]
        {
            set
            {
            }
        }

        int IWritableObservableCollection<T>.IndexOf(T item)
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
    }
}