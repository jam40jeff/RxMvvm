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
    /// A factory for creating observable collections.
    /// </summary>
    public static class ObservableCollectionFactory
    {
        /// <summary>
        /// Creates an observable collection.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the items in the collection.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IObservableCollection{T}"/>.
        /// </returns>
        public static IObservableCollection<T> CreateObservableCollection<T>()
        {
            return new ObservableCollection<T>();
        }

        /// <summary>
        /// Creates an observable collection from an initial list of items.
        /// </summary>
        /// <param name="list">
        /// The initial list of items.
        /// </param>
        /// <typeparam name="T">
        /// The type of the items in the collection.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IObservableCollection{T}"/>.
        /// </returns>
        public static IObservableCollection<T> CreateObservableCollection<T>(IList<T> list)
        {
            return new ObservableCollection<T>(list);
        }
    }
}