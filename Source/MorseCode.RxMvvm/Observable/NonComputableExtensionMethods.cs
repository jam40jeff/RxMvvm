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

namespace MorseCode.RxMvvm.Observable
{
    using System;
    using System.Diagnostics.Contracts;

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Common.StaticReflection;

    /// <summary>
    /// Non computable extension methods.
    /// </summary>
    public static class NonComputableExtensionMethods
    {
        /// <summary>
        /// Combines two non computable discriminated unions.
        /// </summary>
        /// <param name="o">
        /// The non computable discriminated union to combine.
        /// </param>
        /// <param name="discriminatedUnion">
        /// The non computable discriminated union to combine with.
        /// </param>
        /// <param name="selector">
        /// The selector delegate.
        /// </param>
        /// <typeparam name="T1">
        /// The first type of the first discriminated union.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The first type of the second discriminated union.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The first type of the resulting discriminated union.
        /// </typeparam>
        /// <returns>
        /// The combined discriminated union.
        /// </returns>
        public static IDiscriminatedUnion<object, TResult, NonComputable> CombineWith<T1, T2, TResult>(
            this IDiscriminatedUnion<object, T1, NonComputable> o, 
            IDiscriminatedUnion<object, T2, NonComputable> discriminatedUnion, 
            Func<T1, T2, TResult> selector)
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Requires<ArgumentNullException>(discriminatedUnion != null, "discriminatedUnion");
            Contract.Requires<ArgumentNullException>(selector != null, "selector");
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, TResult, NonComputable>>() != null);

            IDiscriminatedUnion<object, TResult, NonComputable> observable =
                o.Switch(
                    v1 =>
                    discriminatedUnion.Switch(
                        v2 => DiscriminatedUnion.First<object, TResult, NonComputable>(selector(v1, v2)), 
                        _ => DiscriminatedUnion.Second<object, TResult, NonComputable>(NonComputable.Value)), 
                    _ => DiscriminatedUnion.Second<object, TResult, NonComputable>(NonComputable.Value));
            if (observable == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IDiscriminatedUnion<object, T1, NonComputable>>.GetMethodInfo(
                        o2 => o2.Switch(null, null)).Name + " cannot be null.");
            }

            return observable;
        }
    }
}