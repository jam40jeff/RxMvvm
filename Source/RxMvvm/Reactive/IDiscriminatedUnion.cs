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

    /// <summary>
    /// Interface representing the F# discriminated union with two possible types.  A value may only be specified for one of the types at a time.
    /// </summary>
    /// <typeparam name="TFirst">
    /// The first type of the discriminated union.
    /// </typeparam>
    /// <typeparam name="TSecond">
    /// The second type of the discriminated union.
    /// </typeparam>
    public interface IDiscriminatedUnion<out TFirst, out TSecond>
    {
        /// <summary>
        /// Gets a value indicating whether the discriminated union is holding a value of the type <see cref="TFirst"/>.
        /// </summary>
        bool IsFirst { get; }

        /// <summary>
        /// Gets a value indicating whether the discriminated union is holding a value of the type <see cref="TSecond"/>.
        /// </summary>
        bool IsSecond { get; }

        /// <summary>
        /// Gets the value of type <see cref="TFirst"/> if <see cref="IsFirst"/> is <value>true</value>, otherwise returns the default value for type <see cref="TFirst"/>.
        /// </summary>
        TFirst First { get; }

        /// <summary>
        /// Gets the value of type <see cref="TSecond"/> if <see cref="IsSecond"/> is <value>true</value>, otherwise returns the default value for type <see cref="TSecond"/>.
        /// </summary>
        TSecond Second { get; }

        /// <summary>
        /// Executes an action based on which value is contained in the discriminated union.
        /// </summary>
        /// <param name="first">
        /// The action to run if <see cref="IsFirst"/> is 
        /// <value>
        /// true
        /// </value>
        /// .
        /// </param>
        /// <param name="second">
        /// The action to run if <see cref="IsSecond"/> is 
        /// <value>
        /// true
        /// </value>
        /// .
        /// </param>
        void Switch(Action<TFirst> first, Action<TSecond> second);

        /// <summary>
        /// Executes a function based on which value is contained in the discriminated union.
        /// </summary>
        /// <param name="first">
        /// The function to run if <see cref="IsFirst"/> is 
        /// <value>
        /// true
        /// </value>
        /// .
        /// </param>
        /// <param name="second">
        /// The function to run if <see cref="IsSecond"/> is 
        /// <value>
        /// true
        /// </value>
        /// .
        /// </param>
        /// <typeparam name="TResult">
        /// The type of the result.
        /// </typeparam>
        /// <returns>
        /// The <see cref="TResult"/> of the function executed.
        /// </returns>
        TResult Switch<TResult>(Func<TFirst, TResult> first, Func<TSecond, TResult> second);
    }
}