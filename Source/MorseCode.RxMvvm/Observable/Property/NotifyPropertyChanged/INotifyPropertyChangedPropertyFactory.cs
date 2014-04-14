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

namespace MorseCode.RxMvvm.Observable.Property.NotifyPropertyChanged
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reactive.Concurrency;

    /// <summary>
    /// An interface representing a factory for creating properties implementing <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    [ContractClass(typeof(NotifyPropertyChangedPropertyFactoryContract))]
    public interface INotifyPropertyChangedPropertyFactory
    {
        /// <summary>
        /// Creates a property implementing <see cref="INotifyPropertyChanged"/>.
        /// </summary>
        /// <param name="observableProperty">
        /// The observable property to create the property from.
        /// </param>
        /// <param name="scheduler">
        /// The scheduler to run the notifications on.
        /// </param>
        /// <typeparam name="T">
        /// The type of the property.
        /// </typeparam>
        /// <returns>
        /// The <see cref="INotifyPropertyChangedProperty{T}"/>.
        /// </returns>
        INotifyPropertyChangedProperty<T> CreateNotifyCollectionChangedCollection<T>(
            IObservableProperty<T> observableProperty, IScheduler scheduler);

        /// <summary>
        /// Creates a read-only property implementing <see cref="INotifyPropertyChanged"/>.
        /// </summary>
        /// <param name="observable">
        /// The observable to create the property from.
        /// </param>
        /// <param name="scheduler">
        /// The scheduler to run the notifications on.
        /// </param>
        /// <typeparam name="T">
        /// The type of the property.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IReadableNotifyPropertyChangedProperty{T}"/>.
        /// </returns>
        IReadableNotifyPropertyChangedProperty<T> CreateReadOnlyNotifyCollectionChangedCollection<T>(
            IObservable<T> observable, IScheduler scheduler);
    }
}