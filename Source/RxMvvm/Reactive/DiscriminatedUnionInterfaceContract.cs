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

    [ContractClassFor(typeof(IDiscriminatedUnion<,>))]
    internal abstract class DiscriminatedUnionInterfaceContract<TFirst, TSecond> : IDiscriminatedUnion<TFirst, TSecond>
    {
        /// <summary>
        /// Gets a value indicating whether is first.
        /// </summary>
        public bool IsFirst
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is second.
        /// </summary>
        public bool IsSecond
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the first.
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
        /// Gets the second.
        /// </summary>
        public TSecond Second
        {
            get
            {
                Contract.Requires(!this.IsSecond);

                return default(TSecond);
            }
        }

        /// <summary>
        /// The switch.
        /// </summary>
        /// <param name="first">
        /// The first.
        /// </param>
        /// <param name="second">
        /// The second.
        /// </param>
        public void Switch(Action<TFirst> first, Action<TSecond> second)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);
        }

        /// <summary>
        /// The switch.
        /// </summary>
        /// <param name="first">
        /// The first.
        /// </param>
        /// <param name="second">
        /// The second.
        /// </param>
        /// <typeparam name="TResult">
        /// </typeparam>
        /// <returns>
        /// The <see cref="TResult"/>.
        /// </returns>
        public TResult Switch<TResult>(Func<TFirst, TResult> first, Func<TSecond, TResult> second)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);
            return default(TResult);
        }
    }
}