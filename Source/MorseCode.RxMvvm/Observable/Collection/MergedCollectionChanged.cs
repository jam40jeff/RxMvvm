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
    internal class MergedCollectionChanged<TCollection, T> : IMergedCollectionChanged<TCollection, T>
        where TCollection : class, IReadableObservableCollection<T>
    {
        private readonly TCollection collection;

        private readonly IObservableCollectionChanged<T> collectionChanged;

        internal MergedCollectionChanged(
            TCollection collection, IObservableCollectionChanged<T> collectionChanged)
        {
            this.collection = collection;
            this.collectionChanged = collectionChanged;
        }

        TCollection IMergedCollectionChanged<TCollection, T>.Collection
        {
            get
            {
                return this.collection;
            }
        }

        IObservableCollectionChanged<T> IMergedCollectionChanged<TCollection, T>.CollectionChanged
        {
            get
            {
                return this.collectionChanged;
            }
        }
    }
}