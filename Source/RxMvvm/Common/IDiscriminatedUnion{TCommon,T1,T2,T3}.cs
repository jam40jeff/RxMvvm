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

namespace MorseCode.RxMvvm.Common
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Interface representing the F# discriminated union with two possible types.  A value may only be specified for one of the types at a time.
    /// </summary>
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
    [ContractClass(typeof(DiscriminatedUnionInterfaceContract<,,,>))]
    public interface IDiscriminatedUnion<out TCommon, out T1, out T2, out T3>
        where T1 : TCommon
        where T2 : TCommon
        where T3 : TCommon
        where TCommon : class
    {
        /// <summary>
        /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="T1" />.
        /// </summary>
        bool IsFirst { get; }

        /// <summary>
        /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="T2" />.
        /// </summary>
        bool IsSecond { get; }

        /// <summary>
        /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="T3" />.
        /// </summary>
        bool IsThird { get; }

        /// <summary>
        /// Gets the value of type <typeparamref name="T1" /> if <see cref="IsFirst"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="T1" />.
        /// </summary>
        T1 First { get; }

        /// <summary>
        /// Gets the value of type <typeparamref name="T2" /> if <see cref="IsSecond"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="T2" />.
        /// </summary>
        T2 Second { get; }

        /// <summary>
        /// Gets the value of type <typeparamref name="T3" /> if <see cref="IsThird"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="T3" />.
        /// </summary>
        T3 Third { get; }

        /// <summary>
        /// Gets the value as <typeparamref name="TCommon" /> regardless of which of the three values are held in the discriminated union.
        /// </summary>
        TCommon Value { get; }

        /// <summary>
        /// Executes an action based on which value is contained in the discriminated union.
        /// </summary>
        /// <param name="first">
        /// The action to run if <see cref="IsFirst"/> is <c>true</c>.
        /// </param>
        /// <param name="second">
        /// The action to run if <see cref="IsSecond"/> is <c>true</c>.
        /// </param>
        /// <param name="third">
        /// The action to run if <see cref="IsThird"/> is <c>true</c>.
        /// </param>
        void Switch(Action<T1> first, Action<T2> second, Action<T3> third);

        /// <summary>
        /// Executes a function based on which value is contained in the discriminated union.
        /// </summary>
        /// <param name="first">
        /// The function to run if <see cref="IsFirst"/> is <c>true</c>.
        /// </param>
        /// <param name="second">
        /// The function to run if <see cref="IsSecond"/> is <c>true</c>.
        /// </param>
        /// <param name="third">
        /// The function to run if <see cref="IsThird"/> is <c>true</c>.
        /// </param>
        /// <typeparam name="TResult">
        /// The type of the result.
        /// </typeparam>
        /// <returns>
        /// The result of type <typeparamref name="TResult"/> of the function executed.
        /// </returns>
        TResult Switch<TResult>(Func<T1, TResult> first, Func<T2, TResult> second, Func<T3, TResult> third);
    }
}