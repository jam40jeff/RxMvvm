﻿#region License

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
    using System.Reactive;
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Common.StaticReflection;

    /// <summary>
    /// A static class providing extension methods for observable collections.
    /// </summary>
    public static class ObservableCollectionExtensionMethods
    {
        /// <summary>
        /// Merges an observable of an observable collection with the observable collection itself by creating a collection changed notification when the entire collection changes.
        /// </summary>
        /// <param name="observableCollectionObservable">
        /// The observable collection observable.
        /// </param>
        /// <param name="observableCollectionObservableForTypeInference">
        /// The observable collection observable specified a second time for type inference purposes.
        /// </param>
        /// <typeparam name="TCollection">
        /// The type of the observable collection.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of the items in the observable collection.
        /// </typeparam>
        /// <returns>
        /// An observable which is the result of an observable of an observable collection merged with the observable collection itself by creating a collection changed notification when the entire collection changes.
        /// </returns>
        public static IObservable<IMergedCollectionChanged<TCollection, T>> MergeCollectionPropertyWithChanges<TCollection, T>(
            this IObservable<TCollection> observableCollectionObservable,
            IObservable<IReadableObservableCollection<T>> observableCollectionObservableForTypeInference)
            where TCollection : class, IReadableObservableCollection<T>
        {
            if (!ReferenceEquals(observableCollectionObservable, observableCollectionObservableForTypeInference))
            {
                throw new InvalidOperationException(
                    "Parameters " + StaticReflection.GetInScopeMemberInfo(() => observableCollectionObservable)
                    + " and "
                    + StaticReflection.GetInScopeMemberInfo(() => observableCollectionObservableForTypeInference)
                    + " must refer to the same object.");
            }

            return
                observableCollectionObservable.StartWith(new TCollection[] { null })
                                              .Buffer(2, 1)
                                              .Select(
                                                  collections =>
                                                  new MergedCollectionChanged<TCollection, T>(
                                                      collections.Count < 2 ? collections[0] : collections[1],
                                                      new ObservableCollectionChanged<T>(
                                                      collections.Count < 2 ? null : collections[0],
                                                      collections.Count < 2 ? collections[0] : collections[1])))
                                              .Merge(
                                                  observableCollectionObservable.Join(
                                                      observableCollectionObservable.Where(c => c != null).Switch(),
                                                      c => c == null ? Observable.Empty<TCollection>() : observableCollectionObservable.Skip(1),
                                                      n => Observable.Empty<Unit>(),
                                                      (c, n) => new MergedCollectionChanged<TCollection, T>(c, n)));
        }
    }
}