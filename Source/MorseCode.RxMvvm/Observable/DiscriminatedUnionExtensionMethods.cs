#region License

// Copyright 2015 MorseCode Software
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

    /// <summary>
    /// Provides extension methods for discriminated unions.
    /// </summary>
    public static class DiscriminatedUnionExtensionMethods
    {
        #region Public Methods and Operators

        /// <summary>
        /// Given a discriminated union holding a value or an exception, either gets the value or throws the exception.
        /// </summary>
        /// <param name="o">The discriminated union holding a value or an exception.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>The value in the discriminated union if there is one.</returns>
        /// <exception cref="Exception">Throws the exception held in the discriminated union if there is one.</exception>
        public static T GetValueOrThrowException<T>(this IDiscriminatedUnion<object, T, Exception> o)
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");

            return o.Switch(v => v, e => { throw e; });
        }

        #endregion
    }
}