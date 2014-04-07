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
    using System.Diagnostics.Contracts;
    using System.Reactive.Disposables;

    /// <summary>
    /// A static class providing extension methods for subscribing to items in observable collections.  Subscriptions are automatically created and disposed of as items are added and removed from the observable collection.
    /// </summary>
    public static class CollectionObservable
    {
        /// <summary>
        /// Subscribes to items in observable collections.  Subscriptions are automatically created and disposed of as items are added and removed from the observable collection.
        /// </summary>
        /// <param name="observableCollection">
        /// The observable collection to subscribe to.
        /// </param>
        /// <param name="subscribe">
        /// A method taking a parameter of the item in the observable collection being subscribed to and expects the subscription <see cref="IDisposable"/> to be returned.
        /// </param>
        /// <typeparam name="T">
        /// The type of the items in the observable collection.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IDisposable"/> for removing all item subscriptions.
        /// </returns>
        public static IDisposable SubscribeItems<T>(
            this IReadableObservableCollection<T> observableCollection, Func<T, IDisposable> subscribe)
        {
            Contract.Requires(observableCollection != null);
            Contract.Requires(subscribe != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            CompositeDisposable disposable = new CompositeDisposable();

            object l = new object();

            lock (l)
            {
                Dictionary<T, IDisposable> disposablesByItem = new Dictionary<T, IDisposable>();
                disposable.Add(
                    observableCollection.Subscribe(
                        c =>
                        {
                            lock (l)
                            {
                                foreach (T newItem in c.NewItems)
                                {
                                    IDisposable d = subscribe(newItem);
                                    disposablesByItem.Add(newItem, d);
                                    disposable.Add(d);
                                }

                                foreach (T oldItem in c.OldItems)
                                {
                                    IDisposable d = disposablesByItem[oldItem];
                                    disposablesByItem.Remove(oldItem);
                                    disposable.Remove(d);
                                }
                            }
                        }));

                foreach (T item in observableCollection)
                {
                    IDisposable d = subscribe(item);
                    disposablesByItem.Add(item, d);
                    disposable.Add(d);
                }
            }

            if (disposable == null)
            {
                throw new InvalidOperationException("Disposable cannot be null.");
            }

            return disposable;
        }
    }
}