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
    using System.Reactive;
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Common.DiscriminatedUnion;

    /// <summary>
    /// Provides <see langword="static"/> methods for <see cref="IObservable{T}"/>.
    /// </summary>
    public static partial class ObservableRxMvvm
    {
        /// <summary>
        /// Creates an observer that is capable of observing an observable with three notification channels using the specified actions.
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
        /// <param name="onNextFirst">
        /// Handler for notifications from the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// Handler for notifications from the second channel.
        /// </param>
        /// <param name="onNextThird">
        /// Handler for notifications from the third channel.
        /// </param>
        /// <returns>
        /// An observer capable of observing an observable with three notification channels.
        /// </returns>
        public static IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>> CreateDiscriminatedUnion<TCommon, T1, T2, T3>(
            Action<T1> onNextFirst, Action<T2> onNextSecond, Action<T3> onNextThird)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(onNextFirst != null, "onNextFirst");
            Contract.Requires<ArgumentNullException>(onNextSecond != null, "onNextSecond");
            Contract.Requires<ArgumentNullException>(onNextThird != null, "onNextThird");
            Contract.Ensures(Contract.Result<IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>>>() != null);

            return CreateDiscriminatedUnion<TCommon, T1, T2, T3>(
                onNextFirst,
                onNextSecond,
                onNextThird,
                ex => { throw ex; },
                () => { });
        }

        /// <summary>
        /// Creates an observer that is capable of observing an observable with three notification channels using the specified actions.
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
        /// <param name="onNextFirst">
        /// Handler for notifications from the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// Handler for notifications from the second channel.
        /// </param>
        /// <param name="onNextThird">
        /// Handler for notifications from the third channel.
        /// </param>
        /// <param name="onError">
        /// Handler for an error notification.
        /// </param>
        /// <returns>
        /// An observer capable of observing an observable with three notification channels.
        /// </returns>
        public static IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>> CreateDiscriminatedUnion<TCommon, T1, T2, T3>(
            Action<T1> onNextFirst, Action<T2> onNextSecond, Action<T3> onNextThird, Action<Exception> onError)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(onNextFirst != null, "onNextFirst");
            Contract.Requires<ArgumentNullException>(onNextSecond != null, "onNextSecond");
            Contract.Requires<ArgumentNullException>(onNextThird != null, "onNextThird");
            Contract.Requires<ArgumentNullException>(onError != null, "onError");
            Contract.Ensures(Contract.Result<IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>>>() != null);

            return CreateDiscriminatedUnion<TCommon, T1, T2, T3>(onNextFirst, onNextSecond, onNextThird, onError, () => { });
        }

        /// <summary>
        /// Creates an observer that is capable of observing an observable with three notification channels using the specified actions.
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
        /// <param name="onNextFirst">
        /// Handler for notifications from the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// Handler for notifications from the second channel.
        /// </param>
        /// <param name="onNextThird">
        /// Handler for notifications from the third channel.
        /// </param>
        /// <param name="onCompleted">
        /// Handler for a completed notification.
        /// </param>
        /// <returns>
        /// An observer capable of observing an observable with three notification channels.
        /// </returns>
        public static IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>> CreateDiscriminatedUnion<TCommon, T1, T2, T3>(
            Action<T1> onNextFirst, Action<T2> onNextSecond, Action<T3> onNextThird, Action onCompleted)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(onNextFirst != null, "onNextFirst");
            Contract.Requires<ArgumentNullException>(onNextSecond != null, "onNextSecond");
            Contract.Requires<ArgumentNullException>(onNextThird != null, "onNextThird");
            Contract.Requires<ArgumentNullException>(onCompleted != null, "onCompleted");
            Contract.Ensures(Contract.Result<IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>>>() != null);

            return CreateDiscriminatedUnion<TCommon, T1, T2, T3>(
                onNextFirst,
                onNextSecond,
                onNextThird,
                ex => { throw ex; },
                onCompleted);
        }

        /// <summary>
        /// Creates an observer that is capable of observing an observable with three notification channels using the specified actions.
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
        /// <param name="onNextFirst">
        /// Handler for notifications from the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// Handler for notifications from the second channel.
        /// </param>
        /// <param name="onNextThird">
        /// Handler for notifications from the third channel.
        /// </param>
        /// <param name="onError">
        /// Handler for an error notification.
        /// </param>
        /// <param name="onCompleted">
        /// Handler for a completed notification.
        /// </param>
        /// <returns>
        /// An observer capable of observing an observable with three notification channels.
        /// </returns>
        public static IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>> CreateDiscriminatedUnion<TCommon, T1, T2, T3>(
            Action<T1> onNextFirst, Action<T2> onNextSecond, Action<T3> onNextThird, Action<Exception> onError, Action onCompleted)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(onNextFirst != null, "onNextFirst");
            Contract.Requires<ArgumentNullException>(onNextSecond != null, "onNextSecond");
            Contract.Requires<ArgumentNullException>(onNextThird != null, "onNextThird");
            Contract.Requires<ArgumentNullException>(onError != null, "onError");
            Contract.Requires<ArgumentNullException>(onCompleted != null, "onCompleted");
            Contract.Ensures(Contract.Result<IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>>>() != null);

            return
                Observer.Create<IDiscriminatedUnion<TCommon, T1, T2, T3>>(
                    value => value.Switch(onNextFirst, onNextSecond, onNextThird), onError, onCompleted);
        }

        /// <summary>
        /// Creates an observable sequence with three notification channels from the <paramref name="subscribe"/> implementation.
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
        /// <param name="subscribe">
        /// Subscribes observers to the observable.
        /// </param>
        /// <returns>
        /// An observable with three notification channels that calls the specified <paramref name="subscribe"/> function 
        /// when an observer subscribes.
        /// </returns>
        public static IObservable<IDiscriminatedUnion<TCommon, T1, T2, T3>> CreateDiscriminatedUnion<TCommon, T1, T2, T3>(
            Func<IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>>, Action> subscribe)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(subscribe != null, "subscribe");
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<TCommon, T1, T2, T3>>>() != null);

            return Observable.Create(subscribe);
        }

        /// <summary>
        /// Creates an observable sequence with three notification channels from the <paramref name="subscribe"/> implementation.
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
        /// <param name="subscribe">
        /// Subscribes observers to the observable.
        /// </param>
        /// <returns>
        /// An observable with three notification channels that calls the specified <paramref name="subscribe"/> function
        /// when an observer subscribes.
        /// </returns>
        public static IObservable<IDiscriminatedUnion<TCommon, T1, T2, T3>> CreateDiscriminatedUnion<TCommon, T1, T2, T3>(
            Func<IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>>, IDisposable> subscribe)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(subscribe != null, "subscribe");
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<TCommon, T1, T2, T3>>>() != null);

            return Observable.Create(subscribe);
        }
    }
}