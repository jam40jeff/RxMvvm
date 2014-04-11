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
        /// The observable from which values are taken.
        /// </param>
        /// <returns>
        /// An observable of values from the first notification channel.
        /// </returns>
        public static IObservable<T1> TakeFirst<TCommon, T1, T2, T3>(
            this IObservable<IDiscriminatedUnion<TCommon, T1, T2, T3>> source)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");
            Contract.Ensures(Contract.Result<IObservable<T1>>() != null);

            return Observable.Create<T1>(
                observer =>
                {
                    // ReSharper disable ConvertToLambdaExpression
                    return source.SubscribeDiscriminatedUnion(
                        // ReSharper restore ConvertToLambdaExpression
                        observer.OnNext, second => { }, third => { }, observer.OnError, observer.OnCompleted);
                });
        }

        /// <summary>
        /// Returns an observable that contains only the values from the second notification channel.
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
        /// The observable from which values are taken.
        /// </param>
        /// <returns>
        /// An observable of values from the second notification channel.
        /// </returns>
        public static IObservable<T2> TakeSecond<TCommon, T1, T2, T3>(
            this IObservable<IDiscriminatedUnion<TCommon, T1, T2, T3>> source)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");
            Contract.Ensures(Contract.Result<IObservable<T2>>() != null);

            return Observable.Create<T2>(
                observer =>
                {
                    // ReSharper disable ConvertToLambdaExpression
                    return source.SubscribeDiscriminatedUnion(
                        // ReSharper restore ConvertToLambdaExpression
                        first => { }, observer.OnNext, third => { }, observer.OnError, observer.OnCompleted);
                });
        }

        /// <summary>
        /// Returns an observable that contains only the values from the third notification channel.
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
        /// The observable from which values are taken.
        /// </param>
        /// <returns>
        /// An observable of values from the third notification channel.
        /// </returns>
        public static IObservable<T3> TakeThird<TCommon, T1, T2, T3>(
            this IObservable<IDiscriminatedUnion<TCommon, T1, T2, T3>> source)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");
            Contract.Ensures(Contract.Result<IObservable<T3>>() != null);

            return Observable.Create<T3>(
                observer =>
                {
                    // ReSharper disable ConvertToLambdaExpression
                    return source.SubscribeDiscriminatedUnion(
                        // ReSharper restore ConvertToLambdaExpression
                        first => { }, second => { }, observer.OnNext, observer.OnError, observer.OnCompleted);
                });
        }
    }
}