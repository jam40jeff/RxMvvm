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
    /// Provides <see langword="static"/> extension methods for <see cref="IObserver{T}"/>.
    /// </summary>
    public static class ObserverExtensionMethods
    {
        /// <summary>
        /// Provides the observer with new data in the first notification channel.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="observer">
        /// The object to be notified.
        /// </param>
        /// <param name="first">
        /// The current first notification information.
        /// </param>
        public static void OnNextFirst<TFirst, TSecond>(
            this IObserver<IDiscriminatedUnion<TFirst, TSecond>> observer, TFirst first)
        {
            Contract.Requires(observer != null);

            observer.OnNext(DiscriminatedUnion.First<TFirst, TSecond>(first));
        }

        /// <summary>
        /// Provides the observer with new data in the second notification channel.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="observer">
        /// The object to be notified.
        /// </param>
        /// <param name="second">
        /// The current second notification information.
        /// </param>
        public static void OnNextSecond<TFirst, TSecond>(
            this IObserver<IDiscriminatedUnion<TFirst, TSecond>> observer, TSecond second)
        {
            Contract.Requires(observer != null);

            observer.OnNext(DiscriminatedUnion.Second<TFirst, TSecond>(second));
        }
    }
}