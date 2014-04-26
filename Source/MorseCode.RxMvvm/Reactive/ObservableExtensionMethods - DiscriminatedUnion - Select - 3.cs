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
    using MorseCode.RxMvvm.Common.StaticReflection;

    /// <summary>
    /// Provides <see langword="static"/> extension methods for <see cref="IObservable{T}"/>.
    /// </summary>
    public static partial class ObservableExtensionMethods
    {
        /// <summary>
        /// Transforms a discriminated union by applying a selector function to the first value.
        /// </summary>
        /// <param name="o">
        /// The discriminated union instance to flatten.
        /// </param>
        /// <param name="selector">
        /// The selector delegate.
        /// </param>
        /// <typeparam name="TCommon">
        /// The common type of all types allowed in the discriminated union.
        /// </typeparam>
        /// <typeparam name="T1">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T3">
        /// The third type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The resulting first type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// The transformed discriminated union.
        /// </returns>
        public static IDiscriminatedUnion<TCommon, TResult, T2, T3> SelectFirst<TCommon, T1, T2, T3, TResult>(
            this IDiscriminatedUnion<TCommon, T1, T2, T3> o, Func<T1, TResult> selector)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TResult : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Requires<ArgumentNullException>(selector != null, "selector");
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TCommon, TResult, T2, T3>>() != null);

            IDiscriminatedUnion<TCommon, TResult, T2, T3> observable =
                o.Switch(
                    v => DiscriminatedUnion.First<TCommon, TResult, T2, T3>(selector(v)),
                    DiscriminatedUnion.Second<TCommon, TResult, T2, T3>,
                    DiscriminatedUnion.Third<TCommon, TResult, T2, T3>);
            if (observable == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IDiscriminatedUnion<TCommon, T1, T2, T3>>.GetMethodInfo(
                        o2 => o2.Switch(null, null, null)).Name + " cannot be null.");
            }

            return observable;
        }

        /// <summary>
        /// Transforms a discriminated union by applying a selector function to the second value.
        /// </summary>
        /// <param name="o">
        /// The discriminated union instance to flatten.
        /// </param>
        /// <param name="selector">
        /// The selector delegate.
        /// </param>
        /// <typeparam name="TCommon">
        /// The common type of all types allowed in the discriminated union.
        /// </typeparam>
        /// <typeparam name="T1">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T3">
        /// The third type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The resulting second type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// The transformed discriminated union.
        /// </returns>
        public static IDiscriminatedUnion<TCommon, T1, TResult, T3> SelectSecond<TCommon, T1, T2, T3, TResult>(
            this IDiscriminatedUnion<TCommon, T1, T2, T3> o, Func<T2, TResult> selector)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TResult : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Requires<ArgumentNullException>(selector != null, "selector");
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TCommon, T1, TResult, T3>>() != null);

            IDiscriminatedUnion<TCommon, T1, TResult, T3> observable =
                o.Switch(
                    DiscriminatedUnion.First<TCommon, T1, TResult, T3>,
                    v => DiscriminatedUnion.Second<TCommon, T1, TResult, T3>(selector(v)),
                    DiscriminatedUnion.Third<TCommon, T1, TResult, T3>);
            if (observable == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IDiscriminatedUnion<TCommon, T1, T2, T3>>.GetMethodInfo(
                        o2 => o2.Switch(null, null, null)).Name + " cannot be null.");
            }

            return observable;
        }

        /// <summary>
        /// Transforms a discriminated union by applying a selector function to the third value.
        /// </summary>
        /// <param name="o">
        /// The discriminated union instance to flatten.
        /// </param>
        /// <param name="selector">
        /// The selector delegate.
        /// </param>
        /// <typeparam name="TCommon">
        /// The common type of all types allowed in the discriminated union.
        /// </typeparam>
        /// <typeparam name="T1">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T3">
        /// The third type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The resulting third type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// The transformed discriminated union.
        /// </returns>
        public static IDiscriminatedUnion<TCommon, T1, T2, TResult> SelectThird<TCommon, T1, T2, T3, TResult>(
            this IDiscriminatedUnion<TCommon, T1, T2, T3> o, Func<T3, TResult> selector)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TResult : TCommon
            where TCommon : class
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Requires<ArgumentNullException>(selector != null, "selector");
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TCommon, T1, T2, TResult>>() != null);

            IDiscriminatedUnion<TCommon, T1, T2, TResult> observable =
                o.Switch(
                    DiscriminatedUnion.First<TCommon, T1, T2, TResult>,
                    DiscriminatedUnion.Second<TCommon, T1, T2, TResult>,
                    v => DiscriminatedUnion.Third<TCommon, T1, T2, TResult>(selector(v)));
            if (observable == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IDiscriminatedUnion<TCommon, T1, T2, T3>>.GetMethodInfo(
                        o2 => o2.Switch(null, null, null)).Name + " cannot be null.");
            }

            return observable;
        }
    }
}