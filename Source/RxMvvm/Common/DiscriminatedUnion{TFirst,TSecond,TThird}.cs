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

    [ContractClass(typeof(DiscriminatedUnionContract<,,>))]
    internal abstract class DiscriminatedUnion<TFirst, TSecond, TThird> : IDiscriminatedUnion<TFirst, TSecond, TThird>
    {
        /// <summary>
        /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TFirst" />.
        /// </summary>
        public abstract bool IsFirst { get; }

        /// <summary>
        /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TSecond" />.
        /// </summary>
        public abstract bool IsSecond { get; }

        /// <summary>
        /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TThird" />.
        /// </summary>
        public abstract bool IsThird { get; }

        /// <summary>
        /// Gets the value of type <typeparamref name="TFirst" /> if <see cref="IsFirst"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TFirst" />.
        /// </summary>
        public abstract TFirst First { get; }

        /// <summary>
        /// Gets the value of type <typeparamref name="TSecond" /> if <see cref="IsSecond"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TSecond" />.
        /// </summary>
        public abstract TSecond Second { get; }

        /// <summary>
        /// Gets the value of type <typeparamref name="TThird" /> if <see cref="IsThird"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TThird" />.
        /// </summary>
        public abstract TThird Third { get; }

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
        public abstract void Switch(Action<TFirst> first, Action<TSecond> second, Action<TThird> third);

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
        public abstract TResult Switch<TResult>(
            Func<TFirst, TResult> first, Func<TSecond, TResult> second, Func<TThird, TResult> third);

        /// <summary>
        /// Override of the <see cref="ToString()"/> method.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/> representation of the discriminated union.
        /// </returns>
        public override string ToString()
        {
            if (this.IsFirst)
            {
                return "{First:" + (ReferenceEquals(this.First, null) ? null : this.First.ToString()) + '}';
            }

            if (this.IsSecond)
            {
                return "{Second:" + (ReferenceEquals(this.Second, null) ? null : this.Second.ToString()) + '}';
            }

            if (this.IsThird)
            {
                return "{Third:" + (ReferenceEquals(this.Third, null) ? null : this.Third.ToString()) + '}';
            }

            throw new InvalidOperationException("IsFirst, IsSecond, or IsThird must be true.");
        }
    }
}