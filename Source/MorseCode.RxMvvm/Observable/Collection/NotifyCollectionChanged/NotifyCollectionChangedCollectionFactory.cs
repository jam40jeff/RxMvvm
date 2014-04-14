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
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Reactive.Concurrency;

    /// <summary>
    /// A factory for creating collections implementing <see cref="INotifyCollectionChanged"/> and <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public class NotifyCollectionChangedCollectionFactory : INotifyCollectionChangedCollectionFactory
    {
        private static readonly Lazy<NotifyCollectionChangedCollectionFactory> InstanceLazy =
            new Lazy<NotifyCollectionChangedCollectionFactory>(() => new NotifyCollectionChangedCollectionFactory());

        private NotifyCollectionChangedCollectionFactory()
        {
        }

        /// <summary>
        /// Gets the singleton instance of an <see cref="NotifyCollectionChangedCollectionFactory"/>.
        /// </summary>
        public static INotifyCollectionChangedCollectionFactory Instance
        {
            get
            {
                return InstanceLazy.Value;
            }
        }

        INotifyCollectionChangedCollection<T> INotifyCollectionChangedCollectionFactory.CreateNotifyCollectionChangedCollection<T>(
            IObservableCollection<T> observableCollection, IScheduler scheduler)
        {
            return new NotifyCollectionChangedCollection<T>(observableCollection, scheduler);
        }

        IReadableNotifyCollectionChangedCollection<T> INotifyCollectionChangedCollectionFactory.CreateReadOnlyNotifyCollectionChangedCollection<T>(
            IReadableObservableCollection<T> observableCollection, IScheduler scheduler)
        {
            return new ReadOnlyNotifyCollectionChangedCollection<T>(observableCollection, scheduler);
        }
    }
}