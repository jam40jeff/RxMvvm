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

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;

    /// <summary>
    /// Provides <see langword="static"/> extension methods for <see cref="IObservable{T}"/>.
    /// </summary>
    public static partial class ObservableExtensionMethods
    {
        /// <summary>
        /// Notifies the observable that an observer is to receive notifications.
        /// </summary>
        /// <typeparam name="TCommon">
        /// Common type of the notification channels.
        /// </typeparam>
        /// <typeparam name="T1">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="T2">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <typeparam name="T3">
        /// Type of the third notification channel.
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
        /// <param name="onNextThird">
        /// The handler of notifications in the third channel.
        /// </param>
        /// <returns>
        /// The observer's interface that enables cancellation of the subscription so that it stops receiving notifications.
        /// </returns>
        public static IDisposable SubscribeDiscriminatedUnion<TCommon, T1, T2, T3>(
            this IObservable<IDiscriminatedUnion<TCommon, T1, T2, T3>> source,
            Action<T1> onNextFirst,
            Action<T2> onNextSecond,
            Action<T3> onNextThird)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");
            Contract.Requires<ArgumentNullException>(onNextFirst != null, "onNextFirst");
            Contract.Requires<ArgumentNullException>(onNextSecond != null, "onNextSecond");
            Contract.Requires<ArgumentNullException>(onNextThird != null, "onNextThird");
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return source.Subscribe(ObservableRxMvvm.CreateDiscriminatedUnion<TCommon, T1, T2, T3>(onNextFirst, onNextSecond, onNextThird));
        }

        /// <summary>
        /// Notifies the observable that an observer is to receive notifications.
        /// </summary>
        /// <typeparam name="TCommon">
        /// Common type of the notification channels.
        /// </typeparam>
        /// <typeparam name="T1">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="T2">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <typeparam name="T3">
        /// Type of the third notification channel.
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
        /// <param name="onNextThird">
        /// The handler of notifications in the third channel.
        /// </param>
        /// <param name="onError">
        /// The handler of an error notification.
        /// </param>
        /// <returns>
        /// The observer's interface that enables cancellation of the subscription so that it stops receiving notifications.
        /// </returns>
        public static IDisposable SubscribeDiscriminatedUnion<TCommon, T1, T2, T3>(
            this IObservable<IDiscriminatedUnion<TCommon, T1, T2, T3>> source,
            Action<T1> onNextFirst,
            Action<T2> onNextSecond,
            Action<T3> onNextThird,
            Action<Exception> onError)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");
            Contract.Requires<ArgumentNullException>(onNextFirst != null, "onNextFirst");
            Contract.Requires<ArgumentNullException>(onNextSecond != null, "onNextSecond");
            Contract.Requires<ArgumentNullException>(onNextThird != null, "onNextThird");
            Contract.Requires<ArgumentNullException>(onError != null, "onError");
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return source.Subscribe(ObservableRxMvvm.CreateDiscriminatedUnion<TCommon, T1, T2, T3>(onNextFirst, onNextSecond, onNextThird, onError));
        }

        /// <summary>
        /// Notifies the observable that an observer is to receive notifications.
        /// </summary>
        /// <typeparam name="TCommon">
        /// Common type of the notification channels.
        /// </typeparam>
        /// <typeparam name="T1">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="T2">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <typeparam name="T3">
        /// Type of the third notification channel.
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
        /// <param name="onNextThird">
        /// The handler of notifications in the third channel.
        /// </param>
        /// <param name="onCompleted">
        /// The handler of a completion notification.
        /// </param>
        /// <returns>
        /// The observer's interface that enables cancellation of the subscription so that it stops receiving notifications.
        /// </returns>
        public static IDisposable SubscribeDiscriminatedUnion<TCommon, T1, T2, T3>(
            this IObservable<IDiscriminatedUnion<TCommon, T1, T2, T3>> source,
            Action<T1> onNextFirst,
            Action<T2> onNextSecond,
            Action<T3> onNextThird,
            Action onCompleted)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");
            Contract.Requires<ArgumentNullException>(onNextFirst != null, "onNextFirst");
            Contract.Requires<ArgumentNullException>(onNextSecond != null, "onNextSecond");
            Contract.Requires<ArgumentNullException>(onNextThird != null, "onNextThird");
            Contract.Requires<ArgumentNullException>(onCompleted != null, "onCompleted");
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return source.Subscribe(ObservableRxMvvm.CreateDiscriminatedUnion<TCommon, T1, T2, T3>(onNextFirst, onNextSecond, onNextThird, onCompleted));
        }

        /// <summary>
        /// Notifies the observable that an observer is to receive notifications.
        /// </summary>
        /// <typeparam name="TCommon">
        /// Common type of the notification channels.
        /// </typeparam>
        /// <typeparam name="T1">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="T2">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <typeparam name="T3">
        /// Type of the third notification channel.
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
        /// <param name="onNextThird">
        /// The handler of notifications in the third channel.
        /// </param>
        /// <param name="onError">
        /// The handler of an error notification.
        /// </param>
        /// <param name="onCompleted">
        /// The handler of a completion notification.
        /// </param>
        /// <returns>
        /// The observer's interface that enables cancellation of the subscription so that it stops receiving notifications.
        /// </returns>
        public static IDisposable SubscribeDiscriminatedUnion<TCommon, T1, T2, T3>(
            this IObservable<IDiscriminatedUnion<TCommon, T1, T2, T3>> source,
            Action<T1> onNextFirst,
            Action<T2> onNextSecond,
            Action<T3> onNextThird,
            Action<Exception> onError,
            Action onCompleted)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");
            Contract.Requires<ArgumentNullException>(onNextFirst != null, "onNextFirst");
            Contract.Requires<ArgumentNullException>(onNextSecond != null, "onNextSecond");
            Contract.Requires<ArgumentNullException>(onNextThird != null, "onNextThird");
            Contract.Requires<ArgumentNullException>(onError != null, "onError");
            Contract.Requires<ArgumentNullException>(onCompleted != null, "onCompleted");
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            return
                source.Subscribe(
                    ObservableRxMvvm.CreateDiscriminatedUnion<TCommon, T1, T2, T3>(onNextFirst, onNextSecond, onNextThird, onError, onCompleted));
        }
    }
}