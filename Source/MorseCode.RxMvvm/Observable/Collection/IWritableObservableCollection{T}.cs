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

    /// <summary>
    /// An interface representing an observable collection which may be read from.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the objects in the collection.
    /// </typeparam>
    [ContractClass(typeof(WritableObservableCollectionContract<>))]
    public interface IWritableObservableCollection<in T>
    {
        /// <summary>
        /// Sets the element at the specified index in the observable collection.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to set.
        /// </param>
        /// <returns>
        /// The element at the specified index in the observable collection.
        /// </returns>
        T this[int index] { set; }

        /// <summary>
        /// Gets the index of a specific item in the observable collection.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the observable collection.
        /// </param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        int IndexOf(T item);

        /// <summary>
        /// Inserts an item to the observable collection at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be inserted.
        /// </param>
        /// <param name="item">
        /// The object to insert into the observable collection.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in the observable collection.
        /// </exception>
        void Insert(int index, T item);

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index"/> is not a valid index in the observable collection.
        /// </exception>
        void RemoveAt(int index);

        /// <summary>
        /// Adds an item to the observable collection.
        /// </summary>
        /// <param name="item">
        /// The object to add to the observable collection.
        /// </param>
        void Add(T item);

        /// <summary>
        /// Removes all items from the observable collection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets whether the observable collection contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the observable collection.
        /// </param>
        /// <returns>
        /// <value>true</value> if <paramref name="item"/> is found in the observable collection, otherwise <value>false</value>.
        /// </returns>
        bool Contains(T item);

        /// <summary>
        /// Copies the elements of the observable collection to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from observable collection. The <see cref="T:System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// The number of elements in the source observable collection is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        /// </exception>
        void CopyTo(T[] array, int arrayIndex);

        /// <summary>
        /// Removes the first occurrence of a specific object from the observable collection.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the observable collection.
        /// </param>
        /// <returns>
        /// <value>true</value> if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>, otherwise <value>false</value>. This method also returns <value>false</value> if <paramref name="item"/> is not found in the original observable collection.
        /// </returns>
        bool Remove(T item);
    }
}