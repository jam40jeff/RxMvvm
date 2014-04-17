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
        /// <returns>
        /// The new instance of a discriminated union.
        /// </returns>
        public static IDiscriminatedUnion<TCommon, T1, T2> CreateFirst<TCommon, T1, T2>(
            this IDiscriminatedUnion<TCommon, T1, T2> o, T1 value)
            where T1 : TCommon
            where T2 : TCommon
            where TCommon : class
        {
            return DiscriminatedUnion.First<TCommon, T1, T2>(value);
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
        /// <returns>
        /// The new instance of a discriminated union.
        /// </returns>
        public static IDiscriminatedUnion<TCommon, T1, T2> CreateSecond<TCommon, T1, T2>(
            this IDiscriminatedUnion<TCommon, T1, T2> o, T2 value)
            where T1 : TCommon
            where T2 : TCommon
            where TCommon : class
        {
            return DiscriminatedUnion.Second<TCommon, T1, T2>(value);
        }
    }
}