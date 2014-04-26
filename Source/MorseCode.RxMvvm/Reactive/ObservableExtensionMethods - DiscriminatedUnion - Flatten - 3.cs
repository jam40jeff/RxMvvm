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

            IDiscriminatedUnion<object, T1, T2, T3> observable = o.Switch(o2 => o2.Switch(v => DiscriminatedUnion.First<object, T1, T2, T3>(v), DiscriminatedUnion.Second<object, T1, T2, T3>, DiscriminatedUnion.Third<object, T1, T2, T3>), DiscriminatedUnion.Second<object, T1, T2, T3>, DiscriminatedUnion.Third<object, T1, T2, T3>);
            if (observable == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IDiscriminatedUnion<object, IDiscriminatedUnion<object, T1, T2, T3>, T2, T3>>
                          .GetMethodInfo(o2 => o2.Switch(null, null, null)).Name + " cannot be null.");
            }

            return observable;
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

            IDiscriminatedUnion<object, T1, T2, T3> observable = o.Switch(DiscriminatedUnion.First<object, T1, T2, T3>, o2 => o2.Switch(v => DiscriminatedUnion.First<object, T1, T2, T3>(v), DiscriminatedUnion.Second<object, T1, T2, T3>, DiscriminatedUnion.Third<object, T1, T2, T3>), DiscriminatedUnion.Third<object, T1, T2, T3>);
            if (observable == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IDiscriminatedUnion<object, T1, IDiscriminatedUnion<object, T1, T2, T3>, T3>>
                          .GetMethodInfo(o2 => o2.Switch(null, null, null)).Name + " cannot be null.");
            }

            return observable;
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

            IDiscriminatedUnion<object, T1, T2, T3> observable = o.Switch(DiscriminatedUnion.First<object, T1, T2, T3>, DiscriminatedUnion.Second<object, T1, T2, T3>, o2 => o2.Switch(v => DiscriminatedUnion.First<object, T1, T2, T3>(v), DiscriminatedUnion.Second<object, T1, T2, T3>, DiscriminatedUnion.Third<object, T1, T2, T3>));
            if (observable == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IDiscriminatedUnion<object, T1, T2, IDiscriminatedUnion<object, T1, T2, T3>>>
                          .GetMethodInfo(o2 => o2.Switch(null, null, null)).Name + " cannot be null.");
            }

            return observable;
        }
    }
}