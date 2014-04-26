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
    /// <summary>
    /// An interface representing a collection and a collection change notification.
    /// </summary>
    /// <typeparam name="TCollection">
    /// The type of the observable collection.
    /// </typeparam>
    /// <typeparam name="T">
    /// The type of the items in the observable collection.
    /// </typeparam>
    public interface IMergedCollectionChanged<out TCollection, out T>
        where TCollection : class, IReadableObservableCollection<T>
    {
        /// <summary>
        /// Gets the collection.
        /// </summary>
        TCollection Collection { get; }

        /// <summary>
        /// Gets the collection changed.
        /// </summary>
        IObservableCollectionChanged<T> CollectionChanged { get; }
    }
}