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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Contains extension methods for interface <see cref="IObservableCollectionChanged{T}"/>.
    /// </summary>
    public static class ObservableCollectionChanged
    {
        /// <summary>
        /// Flattens multiple instances of <see cref="IObservableCollectionChanged{T}"/> into one by concatenating their corresponding
        /// <see cref="IObservableCollectionChanged{T}.OldItems"/> and <see cref="IObservableCollectionChanged{T}.NewItems"/> lists.
        /// </summary>
        /// <param name="o">
        /// The enumerable of <see cref="IObservableCollectionChanged{T}"/> instances to flatten.
        /// </param>
        /// <typeparam name="T">
        /// The type of the objects in the collection.
        /// </typeparam>
        /// <returns>
        /// The flattened <see cref="IObservableCollectionChanged{T}"/> instance.
        /// </returns>
        public static IObservableCollectionChanged<T> Flatten<T>(this IEnumerable<IObservableCollectionChanged<T>> o)
        {
            Tuple<IEnumerable<T>, IEnumerable<T>> items =
                o.Aggregate(
                    Tuple.Create(Enumerable.Empty<T>(), Enumerable.Empty<T>()),
                    (t, i) => Tuple.Create(t.Item1.Concat(i.OldItems), t.Item2.Concat(i.NewItems)));

            if (items == null)
            {
                throw new InvalidOperationException("Result of Aggregate cannot be null.");
            }

            if (items.Item1 == null || items.Item2 == null)
            {
                throw new InvalidOperationException("Aggregate should have produced a Tuple with two non-null items.");
            }

            return new ObservableCollectionChanged<T>(items.Item1.ToList(), items.Item2.ToList());
        }
    }
}