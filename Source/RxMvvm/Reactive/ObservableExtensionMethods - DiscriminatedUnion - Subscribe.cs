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

namespace MorseCode.RxMvvm.Reactive
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides <see langword="static"/> extension methods for <see cref="IObservable{T}"/>.
    /// </summary>
    public static partial class ObservableExtensionMethods
    {
        /// <summary>
        /// Notifies the observable that an observer is to receive notifications.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="source">
        /// The observable for which a subscription is created.
        /// </param>
        /// <param name="onNextFirst">
        /// The handler of notifications in the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// The handler of notifications in the second channel.
        /// </param>
        /// <returns>
        /// The observer's interface that enables cancelation of the subscription so that it stops receiving notifications.
        /// </returns>
        public static IDisposable SubscribeDiscriminatedUnion<TFirst, TSecond>(
            this IObservable<IDiscriminatedUnion<TFirst, TSecond>> source, 
            Action<TFirst> onNextFirst, 
            Action<TSecond> onNextSecond)
        {
            Contract.Requires(source != null);
            Contract.Requires(onNextFirst != null);
            Contract.Requires(onNextSecond != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return source.Subscribe(DiscriminatedUnion.CreateDiscriminatedUnion(onNextFirst, onNextSecond));
        }

        /// <summary>
        /// Notifies the observable that an observer is to receive notifications.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="source">
        /// The observable for which a subscription is created.
        /// </param>
        /// <param name="onNextFirst">
        /// The handler of notifications in the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// The handler of notifications in the second channel.
        /// </param>
        /// <param name="onError">
        /// The handler of an error notification.
        /// </param>
        /// <returns>
        /// The observer's interface that enables cancelation of the subscription so that it stops receiving notifications.
        /// </returns>
        public static IDisposable SubscribeDiscriminatedUnion<TFirst, TSecond>(
            this IObservable<IDiscriminatedUnion<TFirst, TSecond>> source, 
            Action<TFirst> onNextFirst, 
            Action<TSecond> onNextSecond, 
            Action<Exception> onError)
        {
            Contract.Requires(source != null);
            Contract.Requires(onNextFirst != null);
            Contract.Requires(onNextSecond != null);
            Contract.Requires(onError != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return source.Subscribe(DiscriminatedUnion.CreateDiscriminatedUnion(onNextFirst, onNextSecond, onError));
        }

        /// <summary>
        /// Notifies the observable that an observer is to receive notifications.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="source">
        /// The observable for which a subscription is created.
        /// </param>
        /// <param name="onNextFirst">
        /// The handler of notifications in the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// The handler of notifications in the second channel.
        /// </param>
        /// <param name="onCompleted">
        /// The handler of a completion notification.
        /// </param>
        /// <returns>
        /// The observer's interface that enables cancelation of the subscription so that it stops receiving notifications.
        /// </returns>
        public static IDisposable SubscribeDiscriminatedUnion<TFirst, TSecond>(
            this IObservable<IDiscriminatedUnion<TFirst, TSecond>> source, 
            Action<TFirst> onNextFirst, 
            Action<TSecond> onNextSecond, 
            Action onCompleted)
        {
            Contract.Requires(source != null);
            Contract.Requires(onNextFirst != null);
            Contract.Requires(onNextSecond != null);
            Contract.Requires(onCompleted != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return source.Subscribe(DiscriminatedUnion.CreateDiscriminatedUnion(onNextFirst, onNextSecond, onCompleted));
        }

        /// <summary>
        /// Notifies the observable that an observer is to receive notifications.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="source">
        /// The observable for which a subscription is created.
        /// </param>
        /// <param name="onNextFirst">
        /// The handler of notifications in the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// The handler of notifications in the second channel.
        /// </param>
        /// <param name="onError">
        /// The handler of an error notification.
        /// </param>
        /// <param name="onCompleted">
        /// The handler of a completion notification.
        /// </param>
        /// <returns>
        /// The observer's interface that enables cancelation of the subscription so that it stops receiving notifications.
        /// </returns>
        public static IDisposable SubscribeDiscriminatedUnion<TFirst, TSecond>(
            this IObservable<IDiscriminatedUnion<TFirst, TSecond>> source, 
            Action<TFirst> onNextFirst, 
            Action<TSecond> onNextSecond, 
            Action<Exception> onError, 
            Action onCompleted)
        {
            Contract.Requires(source != null);
            Contract.Requires(onNextFirst != null);
            Contract.Requires(onNextSecond != null);
            Contract.Requires(onError != null);
            Contract.Requires(onCompleted != null);
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return
                source.Subscribe(
                    DiscriminatedUnion.CreateDiscriminatedUnion(onNextFirst, onNextSecond, onError, onCompleted));
        }
    }
}