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
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reactive.Concurrency;

    /// <summary>
    /// An interface representing a factory for creating collections implementing <see cref="INotifyCollectionChanged"/> and <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    [ContractClass(typeof(NotifyCollectionChangedCollectionFactoryContract))]
    public interface INotifyCollectionChangedCollectionFactory
    {
        /// <summary>
        /// Creates a collection implementing <see cref="INotifyCollectionChanged"/> and <see cref="INotifyPropertyChanged"/>.
        /// </summary>
        /// <param name="observableCollection">
        /// The observable collection to create the collection from.
        /// </param>
        /// <param name="scheduler">
        /// The scheduler to run the notifications on.
        /// </param>
        /// <typeparam name="T">
        /// The type of the items in the collection.
        /// </typeparam>
        /// <returns>
        /// The <see cref="INotifyCollectionChangedCollection{T}"/>.
        /// </returns>
        INotifyCollectionChangedCollection<T> CreateNotifyCollectionChangedCollection<T>(
            IObservableCollection<T> observableCollection, IScheduler scheduler);

        /// <summary>
        /// Creates a read-only collection implementing <see cref="INotifyCollectionChanged"/> and <see cref="INotifyPropertyChanged"/>.
        /// </summary>
        /// <param name="observableCollection">
        /// The observable collection to create the collection from.
        /// </param>
        /// <param name="scheduler">
        /// The scheduler to run the notifications on.
        /// </param>
        /// <typeparam name="T">
        /// The type of the items in the collection.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IReadableNotifyCollectionChangedCollection{T}"/>.
        /// </returns>
        IReadableNotifyCollectionChangedCollection<T> CreateReadOnlyNotifyCollectionChangedCollection<T>(
            IReadableObservableCollection<T> observableCollection, IScheduler scheduler);
    }
}