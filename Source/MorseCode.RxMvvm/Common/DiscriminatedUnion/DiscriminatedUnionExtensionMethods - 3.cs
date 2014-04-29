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

namespace MorseCode.RxMvvm.Common.DiscriminatedUnion
{
    using System;
    using System.Diagnostics.Contracts;

    using MorseCode.RxMvvm.Common.StaticReflection;

    /// <summary>
    /// Discriminated union extension methods.
    /// </summary>
    public static partial class DiscriminatedUnionExtensionMethods
    {
        /// <summary>
        /// Creates a new instance of a discriminated union with a value of type <typeparamref name="T1"/>.
        /// </summary>
        /// <param name="o">
        /// The discriminated union instance from which to infer the generic type parameters.
        /// </param>
        /// <param name="value">
        /// The value.
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
        /// <returns>
        /// The new instance of a discriminated union.
        /// </returns>
        public static IDiscriminatedUnion<TCommon, T1, T2, T3> CreateFirst<TCommon, T1, T2, T3>(
            this IDiscriminatedUnion<TCommon, T1, T2, T3> o, T1 value)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TCommon, T1, T2, T3>>() != null);

            return DiscriminatedUnion.First<TCommon, T1, T2, T3>(value);
        }

        /// <summary>
        /// Creates a new instance of a discriminated union with a value of type <typeparamref name="T2"/>.
        /// </summary>
        /// <param name="o">
        /// The discriminated union instance from which to infer the generic type parameters.
        /// </param>
        /// <param name="value">
        /// The value.
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
        /// <returns>
        /// The new instance of a discriminated union.
        /// </returns>
        public static IDiscriminatedUnion<TCommon, T1, T2, T3> CreateSecond<TCommon, T1, T2, T3>(
            this IDiscriminatedUnion<TCommon, T1, T2, T3> o, T2 value)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TCommon, T1, T2, T3>>() != null);

            return DiscriminatedUnion.Second<TCommon, T1, T2, T3>(value);
        }

        /// <summary>
        /// Creates a new instance of a discriminated union with a value of type <typeparamref name="T3"/>.
        /// </summary>
        /// <param name="o">
        /// The discriminated union instance from which to infer the generic type parameters.
        /// </param>
        /// <param name="value">
        /// The value.
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
        /// <returns>
        /// The new instance of a discriminated union.
        /// </returns>
        public static IDiscriminatedUnion<TCommon, T1, T2, T3> CreateThird<TCommon, T1, T2, T3>(
            this IDiscriminatedUnion<TCommon, T1, T2, T3> o, T3 value)
            where T1 : TCommon
            where T2 : TCommon
            where T3 : TCommon
            where TCommon : class
        {
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TCommon, T1, T2, T3>>() != null);

            return DiscriminatedUnion.Third<TCommon, T1, T2, T3>(value);
        }

        /// <summary>
        /// Flattens a nested discriminated union to produce a single discriminated union.
        /// </summary>
        /// <param name="o">
        /// The discriminated union instance to flatten.
        /// </param>
        /// <typeparam name="T1">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T3">
        /// The third type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// The flattened discriminated union.
        /// </returns>
        public static IDiscriminatedUnion<object, T1, T2, T3> Flatten<T1, T2, T3>(
            this IDiscriminatedUnion<object, IDiscriminatedUnion<object, T1, T2, T3>, T2, T3> o)
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T1, T2, T3>>() != null);

            IDiscriminatedUnion<object, T1, T2, T3> discriminatedUnion = o.Switch(u => u.Switch(v => DiscriminatedUnion.First<object, T1, T2, T3>(v), DiscriminatedUnion.Second<object, T1, T2, T3>, DiscriminatedUnion.Third<object, T1, T2, T3>), DiscriminatedUnion.Second<object, T1, T2, T3>, DiscriminatedUnion.Third<object, T1, T2, T3>);
            if (discriminatedUnion == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IDiscriminatedUnion<object, IDiscriminatedUnion<object, T1, T2, T3>, T2, T3>>
                          .GetMethodInfo(o2 => o2.Switch(null, null, null)).Name + " cannot be null.");
            }

            return discriminatedUnion;
        }

        /// <summary>
        /// Flattens a nested discriminated union to produce a single discriminated union.
        /// </summary>
        /// <param name="o">
        /// The discriminated union instance to flatten.
        /// </param>
        /// <typeparam name="T1">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T3">
        /// The third type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// The flattened discriminated union.
        /// </returns>
        public static IDiscriminatedUnion<object, T1, T2, T3> Flatten<T1, T2, T3>(
            this IDiscriminatedUnion<object, T1, IDiscriminatedUnion<object, T1, T2, T3>, T3> o)
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T1, T2, T3>>() != null);

            IDiscriminatedUnion<object, T1, T2, T3> discriminatedUnion = o.Switch(DiscriminatedUnion.First<object, T1, T2, T3>, u => u.Switch(v => DiscriminatedUnion.First<object, T1, T2, T3>(v), DiscriminatedUnion.Second<object, T1, T2, T3>, DiscriminatedUnion.Third<object, T1, T2, T3>), DiscriminatedUnion.Third<object, T1, T2, T3>);
            if (discriminatedUnion == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IDiscriminatedUnion<object, T1, IDiscriminatedUnion<object, T1, T2, T3>, T3>>
                          .GetMethodInfo(o2 => o2.Switch(null, null, null)).Name + " cannot be null.");
            }

            return discriminatedUnion;
        }

        /// <summary>
        /// Flattens a nested discriminated union to produce a single discriminated union.
        /// </summary>
        /// <param name="o">
        /// The discriminated union instance to flatten.
        /// </param>
        /// <typeparam name="T1">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T3">
        /// The third type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// The flattened discriminated union.
        /// </returns>
        public static IDiscriminatedUnion<object, T1, T2, T3> Flatten<T1, T2, T3>(
            this IDiscriminatedUnion<object, T1, T2, IDiscriminatedUnion<object, T1, T2, T3>> o)
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T1, T2, T3>>() != null);

            IDiscriminatedUnion<object, T1, T2, T3> discriminatedUnion = o.Switch(DiscriminatedUnion.First<object, T1, T2, T3>, DiscriminatedUnion.Second<object, T1, T2, T3>, u => u.Switch(v => DiscriminatedUnion.First<object, T1, T2, T3>(v), DiscriminatedUnion.Second<object, T1, T2, T3>, DiscriminatedUnion.Third<object, T1, T2, T3>));
            if (discriminatedUnion == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IDiscriminatedUnion<object, T1, T2, IDiscriminatedUnion<object, T1, T2, T3>>>
                          .GetMethodInfo(o2 => o2.Switch(null, null, null)).Name + " cannot be null.");
            }

            return discriminatedUnion;
        }
    }
}