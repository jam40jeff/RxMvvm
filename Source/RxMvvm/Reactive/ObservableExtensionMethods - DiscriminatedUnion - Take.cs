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
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Common;

    /// <summary>
    /// Provides <see langword="static"/> extension methods for <see cref="IObservable{T}"/>.
    /// </summary>
    public static partial class ObservableExtensionMethods
    {
        /// <summary>
        /// Returns an observable that contains only the values from the first notification channel.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="source">
        /// The observable from which values are taken.
        /// </param>
        /// <returns>
        /// An observable of values from the first notification channel.
        /// </returns>
        public static IObservable<TFirst> TakeFirst<TFirst, TSecond>(
            this IObservable<IDiscriminatedUnion<TFirst, TSecond>> source)
        {
            Contract.Requires(source != null);
            Contract.Ensures(Contract.Result<IObservable<TFirst>>() != null);

            return Observable.Create<TFirst>(
                observer =>
                    {
                        // ReSharper disable ConvertToLambdaExpression
                        return source.SubscribeDiscriminatedUnion(
                            // ReSharper restore ConvertToLambdaExpression
                            observer.OnNext, second => { }, observer.OnError, observer.OnCompleted);
                    });
        }

        /// <summary>
        /// Returns an observable that contains only the values from the second notification channel.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="source">
        /// The observable from which values are taken.
        /// </param>
        /// <returns>
        /// An observable of values from the second notification channel.
        /// </returns>
        public static IObservable<TSecond> TakeSecond<TFirst, TSecond>(
            this IObservable<IDiscriminatedUnion<TFirst, TSecond>> source)
        {
            Contract.Requires(source != null);
            Contract.Ensures(Contract.Result<IObservable<TSecond>>() != null);

            return Observable.Create<TSecond>(
                observer =>
                    {
                        // ReSharper disable ConvertToLambdaExpression
                        return source.SubscribeDiscriminatedUnion(
                            // ReSharper restore ConvertToLambdaExpression
                            first => { }, observer.OnNext, observer.OnError, observer.OnCompleted);
                    });
        }
    }
}