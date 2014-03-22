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
    /// Provides <see langword="static"/> factory methods for creating discriminating unions.
    /// </summary>
    public static partial class DiscriminatedUnion
    {
        /// <summary>
        /// Creates an instance of a class implementing <see cref="IDiscriminatedUnion{TCommon,T1,T2}"/> holding a value of type <typeparamref name="T1"/>.
        /// </summary>
        /// <param name="value">
        /// The value to hold in the discriminated union.
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
        /// An instance of a class implementing <see cref="IDiscriminatedUnion{TCommon,T1,T2}"/>.
        /// </returns>
        public static IDiscriminatedUnion<TCommon, T1, T2> First<TCommon, T1, T2>(T1 value)
            where T1 : TCommon
            where T2 : TCommon
            where TCommon : class
        {
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TCommon, T1, T2>>() != null);

            return new DiscriminatedUnionFirst<TCommon, T1, T2>(value);
        }

        /// <summary>
        /// Creates an instance of a class implementing <see cref="IDiscriminatedUnion{TCommon,T1,T2}"/> holding a value of type <typeparamref name="T2"/>.
        /// </summary>
        /// <param name="value">
        /// The value to hold in the discriminated union.
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
        /// An instance of a class implementing <see cref="IDiscriminatedUnion{TCommon,T1,T2}"/>.
        /// </returns>
        public static IDiscriminatedUnion<TCommon, T1, T2> Second<TCommon, T1, T2>(T2 value)
            where T1 : TCommon
            where T2 : TCommon
            where TCommon : class
        {
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TCommon, T1, T2>>() != null);

            return new DiscriminatedUnionSecond<TCommon, T1, T2>(value);
        }

        [Serializable]
        private class DiscriminatedUnionFirst<TCommon, T1, T2> : DiscriminatedUnion<TCommon, T1, T2>
            where T1 : TCommon
            where T2 : TCommon
            where TCommon : class
        {
            private readonly T1 value;

            /// <summary>
            /// Initializes a new instance of the <see cref="DiscriminatedUnionFirst{TCommon,T1,T2}"/> class.
            /// </summary>
            /// <param name="value">
            /// The value.
            /// </param>
            public DiscriminatedUnionFirst(T1 value)
            {
                this.value = value;
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="T1" />.
            /// </summary>
            public override bool IsFirst
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="T2" />.
            /// </summary>
            public override bool IsSecond
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="T1" /> if <see cref="IsFirst"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="T1" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override T1 First
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
                get
                {
                    return this.value;
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="T2" /> if <see cref="IsSecond"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="T2" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override T2 Second
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
                get
                {
                    return default(T2);
                }
            }

            /// <summary>
            /// Gets the value as <typeparamref name="TCommon" /> regardless of which of the two values are held in the discriminated union.
            /// </summary>
            public override TCommon Value
            {
                get
                {
                    return this.value;
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
            public override void Switch(Action<T1> first, Action<T2> second)
            {
                first(this.value);
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
            /// <typeparam name="TResult">
            /// The type of the result.
            /// </typeparam>
            /// <returns>
            /// The result of type <typeparamref name="TResult"/> of the function executed.
            /// </returns>
            public override TResult Switch<TResult>(Func<T1, TResult> first, Func<T2, TResult> second)
            {
                return first(this.value);
            }
        }

        [Serializable]
        private class DiscriminatedUnionSecond<TCommon, T1, T2> : DiscriminatedUnion<TCommon, T1, T2>
            where T1 : TCommon
            where T2 : TCommon
            where TCommon : class
        {
            private readonly T2 value;

            /// <summary>
            /// Initializes a new instance of the <see cref="DiscriminatedUnionSecond{TCommon,T1,T2}"/> class.
            /// </summary>
            /// <param name="value">
            /// The value.
            /// </param>
            public DiscriminatedUnionSecond(T2 value)
            {
                this.value = value;
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="T1" />.
            /// </summary>
            public override bool IsFirst
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="T2" />.
            /// </summary>
            public override bool IsSecond
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="T1" /> if <see cref="IsFirst"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="T1" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override T1 First
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
                get
                {
                    return default(T1);
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="T2" /> if <see cref="IsSecond"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="T2" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override T2 Second
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
                get
                {
                    return this.value;
                }
            }

            /// <summary>
            /// Gets the value as <typeparamref name="TCommon" /> regardless of which of the two values are held in the discriminated union.
            /// </summary>
            public override TCommon Value
            {
                get
                {
                    return this.value;
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
            public override void Switch(Action<T1> first, Action<T2> second)
            {
                second(this.value);
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
            /// <typeparam name="TResult">
            /// The type of the result.
            /// </typeparam>
            /// <returns>
            /// The result of type <typeparamref name="TResult"/> of the function executed.
            /// </returns>
            public override TResult Switch<TResult>(Func<T1, TResult> first, Func<T2, TResult> second)
            {
                return second(this.value);
            }
        }
    }
}