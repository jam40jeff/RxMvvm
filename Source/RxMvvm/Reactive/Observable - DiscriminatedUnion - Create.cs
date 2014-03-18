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

    /// <summary>
    /// Provides <see langword="static"/> methods for <see cref="IObservable{T}"/>.
    /// </summary>
    public static class ObservableRxMvvm
    {
        /// <summary>
        /// Creates an observer that is capable of observing an observable with two notification channels using the specified actions.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="onNextFirst">
        /// Handler for notifications from the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// Handler for notifications from the second channel.
        /// </param>
        /// <returns>
        /// An observer capable of observing an observable with two notification channels.
        /// </returns>
        public static IObserver<IDiscriminatedUnion<TFirst, TSecond>> CreateDiscriminatedUnion<TFirst, TSecond>(
            Action<TFirst> onNextFirst, Action<TSecond> onNextSecond)
        {
            Contract.Requires(onNextFirst != null);
            Contract.Requires(onNextSecond != null);
            Contract.Ensures(Contract.Result<IObserver<IDiscriminatedUnion<TFirst, TSecond>>>() != null);

            return CreateDiscriminatedUnion(
                onNextFirst, 
                onNextSecond, 
                ex => { throw ex; /*.PrepareForRethrow(); changed to internal in Rx 1.1.10425 */ }, 
                () => { });
        }

        /// <summary>
        /// Creates an observer that is capable of observing an observable with two notification channels using the specified actions.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="onNextFirst">
        /// Handler for notifications from the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// Handler for notifications from the second channel.
        /// </param>
        /// <param name="onError">
        /// Handler for an error notification.
        /// </param>
        /// <returns>
        /// An observer capable of observing an observable with two notification channels.
        /// </returns>
        public static IObserver<IDiscriminatedUnion<TFirst, TSecond>> CreateDiscriminatedUnion<TFirst, TSecond>(
            Action<TFirst> onNextFirst, Action<TSecond> onNextSecond, Action<Exception> onError)
        {
            Contract.Requires(onNextFirst != null);
            Contract.Requires(onNextSecond != null);
            Contract.Requires(onError != null);
            Contract.Ensures(Contract.Result<IObserver<IDiscriminatedUnion<TFirst, TSecond>>>() != null);

            return CreateDiscriminatedUnion(onNextFirst, onNextSecond, onError, () => { });
        }

        /// <summary>
        /// Creates an observer that is capable of observing an observable with two notification channels using the specified actions.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="onNextFirst">
        /// Handler for notifications from the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// Handler for notifications from the second channel.
        /// </param>
        /// <param name="onCompleted">
        /// Handler for a completed notification.
        /// </param>
        /// <returns>
        /// An observer capable of observing an observable with two notification channels.
        /// </returns>
        public static IObserver<IDiscriminatedUnion<TFirst, TSecond>> CreateDiscriminatedUnion<TFirst, TSecond>(
            Action<TFirst> onNextFirst, Action<TSecond> onNextSecond, Action onCompleted)
        {
            Contract.Requires(onNextFirst != null);
            Contract.Requires(onNextSecond != null);
            Contract.Requires(onCompleted != null);
            Contract.Ensures(Contract.Result<IObserver<IDiscriminatedUnion<TFirst, TSecond>>>() != null);

            return CreateDiscriminatedUnion(
                onNextFirst, 
                onNextSecond, 
                ex => { throw ex; /*.PrepareForRethrow(); changed to internal in Rx 1.1.10425 */ }, 
                onCompleted);
        }

        /// <summary>
        /// Creates an observer that is capable of observing an observable with two notification channels using the specified actions.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="onNextFirst">
        /// Handler for notifications from the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// Handler for notifications from the second channel.
        /// </param>
        /// <param name="onError">
        /// Handler for an error notification.
        /// </param>
        /// <param name="onCompleted">
        /// Handler for a completed notification.
        /// </param>
        /// <returns>
        /// An observer capable of observing an observable with two notification channels.
        /// </returns>
        public static IObserver<IDiscriminatedUnion<TFirst, TSecond>> CreateDiscriminatedUnion<TFirst, TSecond>(
            Action<TFirst> onNextFirst, Action<TSecond> onNextSecond, Action<Exception> onError, Action onCompleted)
        {
            Contract.Requires(onNextFirst != null);
            Contract.Requires(onNextSecond != null);
            Contract.Requires(onError != null);
            Contract.Requires(onCompleted != null);
            Contract.Ensures(Contract.Result<IObserver<IDiscriminatedUnion<TFirst, TSecond>>>() != null);

            return
                Observer.Create<IDiscriminatedUnion<TFirst, TSecond>>(
                    value => value.Switch(onNextFirst, onNextSecond), onError, onCompleted);
        }

        /// <summary>
        /// Creates an observable sequence with two notification channels from the <paramref name="subscribe"/> implementation.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="subscribe">
        /// Subscribes observers to the observable.
        /// </param>
        /// <returns>
        /// An observable with two notification channels that calls the specified <paramref name="subscribe"/> function 
        /// when an observer subscribes.
        /// </returns>
        public static IObservable<IDiscriminatedUnion<TFirst, TSecond>> CreateDiscriminatedUnion<TFirst, TSecond>(
            Func<IObserver<IDiscriminatedUnion<TFirst, TSecond>>, Action> subscribe)
        {
            Contract.Requires(subscribe != null);
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<TFirst, TSecond>>>() != null);

            return Observable.Create(subscribe);
        }

        /// <summary>
        /// Creates an observable sequence with two notification channels from the <paramref name="subscribe"/> implementation.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="subscribe">
        /// Subscribes observers to the observable.
        /// </param>
        /// <returns>
        /// An observable with two notification channels that calls the specified <paramref name="subscribe"/> function
        /// when an observer subscribes.
        /// </returns>
        public static IObservable<IDiscriminatedUnion<TFirst, TSecond>> CreateDiscriminatedUnion<TFirst, TSecond>(
            Func<IObserver<IDiscriminatedUnion<TFirst, TSecond>>, IDisposable> subscribe)
        {
            Contract.Requires(subscribe != null);
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<TFirst, TSecond>>>() != null);

            return Observable.Create(subscribe);
        }
    }
}