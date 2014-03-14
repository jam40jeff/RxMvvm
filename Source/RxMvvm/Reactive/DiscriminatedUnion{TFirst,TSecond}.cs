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

    internal abstract class DiscriminatedUnion<TFirst, TSecond> : IDiscriminatedUnion<TFirst, TSecond>
    {
        /// <summary>
        /// Gets a value indicating whether is first.
        /// </summary>
        public abstract bool IsFirst { get; }

        /// <summary>
        /// Gets a value indicating whether is second.
        /// </summary>
        public abstract bool IsSecond { get; }

        /// <summary>
        /// Gets the first.
        /// </summary>
        public abstract TFirst First { get; }

        /// <summary>
        /// Gets the second.
        /// </summary>
        public abstract TSecond Second { get; }

        /// <summary>
        /// The switch.
        /// </summary>
        /// <param name="first">
        /// The first.
        /// </param>
        /// <param name="second">
        /// The second.
        /// </param>
        public abstract void Switch(Action<TFirst> first, Action<TSecond> second);

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
        public abstract TResult Switch<TResult>(Func<TFirst, TResult> first, Func<TSecond, TResult> second);

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            if (this.IsFirst)
            {
                return "{First:" + (ReferenceEquals(this.First, null) ? null : this.First.ToString()) + '}';
            }

            return "{Second:" + (ReferenceEquals(this.Second, null) ? null : this.Second.ToString()) + '}';
        }
    }
}