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

    [ContractClassFor(typeof(IDiscriminatedUnion<,,>))]
    internal abstract class DiscriminatedUnionInterfaceContract<TFirst, TSecond, TThird> :
        IDiscriminatedUnion<TFirst, TSecond, TThird>
    {
        /// <summary>
        /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TFirst" />.
        /// </summary>
        public bool IsFirst
        {
            get
            {
                Contract.Ensures(this.IsFirst ^ (this.IsSecond || this.IsThird));

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TSecond" />.
        /// </summary>
        public bool IsSecond
        {
            get
            {
                Contract.Ensures(this.IsSecond ^ (this.IsFirst || this.IsThird));

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TThird" />.
        /// </summary>
        public bool IsThird
        {
            get
            {
                Contract.Ensures(this.IsThird ^ (this.IsFirst || this.IsSecond));

                return false;
            }
        }

        /// <summary>
        /// Gets the value of type <typeparamref name="TFirst" /> if <see cref="IsFirst"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TFirst" />.
        /// </summary>
        public TFirst First
        {
            get
            {
                Contract.Requires(this.IsFirst);

                return default(TFirst);
            }
        }

        /// <summary>
        /// Gets the value of type <typeparamref name="TSecond" /> if <see cref="IsSecond"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TSecond" />.
        /// </summary>
        public TSecond Second
        {
            get
            {
                Contract.Requires(this.IsSecond);

                return default(TSecond);
            }
        }

        /// <summary>
        /// Gets the value of type <typeparamref name="TThird" /> if <see cref="IsThird"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TThird" />.
        /// </summary>
        public TThird Third
        {
            get
            {
                Contract.Requires(this.IsThird);

                return default(TThird);
            }
        }

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
        public void Switch(Action<TFirst> first, Action<TSecond> second, Action<TThird> third)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);
            Contract.Requires(third != null);
        }

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
        public TResult Switch<TResult>(Func<TFirst, TResult> first, Func<TSecond, TResult> second, Func<TThird, TResult> third)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);
            Contract.Requires(third != null);
            return default(TResult);
        }
    }
}