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

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Common.StaticReflection;

    /// <summary>
    /// Provides <see langword="static"/> extension methods for <see cref="IObservable{T}"/>.
    /// </summary>
    public static partial class ObservableExtensionMethods
    {
        /// <summary>
        /// Flattens a nested observable discriminated union to produce a single observable discriminated union.
        /// </summary>
        /// <param name="o">
        /// The observable discriminated union instance to flatten.
        /// </param>
        /// <typeparam name="T1">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// The flattened observable discriminated union.
        /// </returns>
        public static IObservable<IDiscriminatedUnion<object, T1, T2>> Flatten<T1, T2>(
            this IObservable<IDiscriminatedUnion<object, IObservable<IDiscriminatedUnion<object, T1, T2>>, T2>> o)
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T1, T2>>>() != null);

            IObservable<IDiscriminatedUnion<object, T1, T2>> observable =
                o.Select(o2 => o2.Switch(v => v, v => ObservableRxMvvm.Always(DiscriminatedUnion.Second<object, T1, T2>(v))))
                 .Switch();
            if (observable == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IObservable<IDiscriminatedUnion<object, IObservable<IDiscriminatedUnion<object, T1, T2>>, T2>>>.GetMethodInfo(
                              o2 =>
                              o2.Select(
                                  (Func<IDiscriminatedUnion<object, IObservable<IDiscriminatedUnion<object, T1, T2>>, T2>, IDiscriminatedUnion<object, T1, T2>>)null)).Name + " cannot be null.");
            }

            return observable;
        }

        /// <summary>
        /// Flattens a nested observable discriminated union to produce a single observable discriminated union.
        /// </summary>
        /// <param name="o">
        /// The observable discriminated union instance to flatten.
        /// </param>
        /// <typeparam name="T1">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// The flattened observable discriminated union.
        /// </returns>
        public static IObservable<IDiscriminatedUnion<object, T1, T2>> Flatten<T1, T2>(
            this IObservable<IDiscriminatedUnion<object, T1, IObservable<IDiscriminatedUnion<object, T1, T2>>>> o)
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T1, T2>>>() != null);

            IObservable<IDiscriminatedUnion<object, T1, T2>> observable =
                o.Select(o2 => o2.Switch(v => ObservableRxMvvm.Always(DiscriminatedUnion.First<object, T1, T2>(v)), v => v))
                 .Switch();
            if (observable == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IObservable<IDiscriminatedUnion<object, IObservable<IDiscriminatedUnion<object, T1, T2>>, T2>>>.GetMethodInfo(
                              o2 =>
                              o2.Select(
                                  (Func<IDiscriminatedUnion<object, IObservable<IDiscriminatedUnion<object, T1, T2>>, T2>, IDiscriminatedUnion<object, T1, T2>>)null)).Name + " cannot be null.");
            }

            return observable;
        }

        /// <summary>
        /// Flattens a nested observable discriminated union to produce a single observable discriminated union.
        /// </summary>
        /// <param name="o">
        /// The observable discriminated union instance to flatten.
        /// </param>
        /// <typeparam name="T1">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// The flattened observable discriminated union.
        /// </returns>
        public static IObservable<IDiscriminatedUnion<object, T1, T2>> Flatten<T1, T2>(
            this IObservable<IDiscriminatedUnion<object, IObservable<T1>, T2>> o)
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T1, T2>>>() != null);

            IObservable<IDiscriminatedUnion<object, T1, T2>> observable =
                o.Select(o2 => o2.Switch(v => v.Select(DiscriminatedUnion.First<object, T1, T2>), v => ObservableRxMvvm.Always(DiscriminatedUnion.Second<object, T1, T2>(v))))
                 .Switch();
            if (observable == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IObservable<IDiscriminatedUnion<object, IObservable<IDiscriminatedUnion<object, T1, T2>>, T2>>>.GetMethodInfo(
                              o2 =>
                              o2.Select(
                                  (Func<IDiscriminatedUnion<object, IObservable<IDiscriminatedUnion<object, T1, T2>>, T2>, IDiscriminatedUnion<object, T1, T2>>)null)).Name + " cannot be null.");
            }

            return observable;
        }

        /// <summary>
        /// Flattens a nested observable discriminated union to produce a single observable discriminated union.
        /// </summary>
        /// <param name="o">
        /// The observable discriminated union instance to flatten.
        /// </param>
        /// <typeparam name="T1">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// The flattened observable discriminated union.
        /// </returns>
        public static IObservable<IDiscriminatedUnion<object, T1, T2>> Flatten<T1, T2>(
            this IObservable<IDiscriminatedUnion<object, T1, IObservable<T2>>> o)
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T1, T2>>>() != null);

            IObservable<IDiscriminatedUnion<object, T1, T2>> observable =
                o.Select(o2 => o2.Switch(v => ObservableRxMvvm.Always(DiscriminatedUnion.First<object, T1, T2>(v)), v => v.Select(DiscriminatedUnion.Second<object, T1, T2>)))
                 .Switch();
            if (observable == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IObservable<IDiscriminatedUnion<object, IObservable<IDiscriminatedUnion<object, T1, T2>>, T2>>>.GetMethodInfo(
                              o2 =>
                              o2.Select(
                                  (Func<IDiscriminatedUnion<object, IObservable<IDiscriminatedUnion<object, T1, T2>>, T2>, IDiscriminatedUnion<object, T1, T2>>)null)).Name + " cannot be null.");
            }

            return observable;
        }
    }
}