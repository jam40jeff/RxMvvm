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
    using System.Collections.Generic;

    /// <summary>
    /// An interface containing the items changed in an observable collection.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the objects in the collection.
    /// </typeparam>
    public interface IObservableCollectionChanged<out T>
    {
        /// <summary>
        /// Gets the old items which were removed from the collection.
        /// </summary>
        IReadOnlyList<T> OldItems { get; }

        /// <summary>
        /// Gets the new items which were added to the collection.
        /// </summary>
        IReadOnlyList<T> NewItems { get; }
    }
}