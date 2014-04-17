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

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Common.DiscriminatedUnion;

    /// <summary>
    /// Provides <see langword="static"/> extension methods for <see cref="IObserver{T}"/>.
    /// </summary>
    public static partial class ObserverExtensionMethods
    {
        /// <summary>
        /// Provides the observer with new data in the first notification channel.
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
        /// <param name="observer">
        /// The object to be notified.
        /// </param>
        /// <param name="first">
        /// The current first notification information.
        /// </param>
        public static void OnNextFirst<TCommon, T1, T2, T3>(
            this IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>> observer, T1 first)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(observer != null, "observer");

            observer.OnNext(DiscriminatedUnion.First<TCommon, T1, T2, T3>(first));
        }

        /// <summary>
        /// Provides the observer with new data in the second notification channel.
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
        /// <param name="observer">
        /// The object to be notified.
        /// </param>
        /// <param name="second">
        /// The current second notification information.
        /// </param>
        public static void OnNextFirst<TCommon, T1, T2, T3>(
            this IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>> observer, T2 second)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(observer != null, "observer");

            observer.OnNext(DiscriminatedUnion.Second<TCommon, T1, T2, T3>(second));
        }

        /// <summary>
        /// Provides the observer with new data in the third notification channel.
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
        /// <param name="observer">
        /// The object to be notified.
        /// </param>
        /// <param name="third">
        /// The current third notification information.
        /// </param>
        public static void OnNextFirst<TCommon, T1, T2, T3>(
            this IObserver<IDiscriminatedUnion<TCommon, T1, T2, T3>> observer, T3 third)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(observer != null, "observer");

            observer.OnNext(DiscriminatedUnion.Third<TCommon, T1, T2, T3>(third));
        }
    }
}