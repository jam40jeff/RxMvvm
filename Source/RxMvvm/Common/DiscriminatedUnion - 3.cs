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
        /// Creates an instance of a class implementing <see cref="IDiscriminatedUnion{TFirst,TSecond,TThird}"/> holding a value of type <typeparamref name="TFirst"/>.
        /// </summary>
        /// <param name="value">
        /// The value to hold in the discriminated union.
        /// </param>
        /// <typeparam name="TFirst">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="TThird">
        /// The third type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// An instance of a class implementing <see cref="IDiscriminatedUnion{TFirst,TSecond,TThird}"/>.
        /// </returns>
        public static IDiscriminatedUnion<TFirst, TSecond, TThird> First<TFirst, TSecond, TThird>(TFirst value)
        {
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TFirst, TSecond, TThird>>() != null);

            return new DiscriminatedUnionFirst<TFirst, TSecond, TThird>(value);
        }

        /// <summary>
        /// Creates an instance of a class implementing <see cref="IDiscriminatedUnion{TFirst,TSecond,TThird}"/> holding a value of type <typeparamref name="TSecond"/>.
        /// </summary>
        /// <param name="value">
        /// The value to hold in the discriminated union.
        /// </param>
        /// <typeparam name="TFirst">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="TThird">
        /// The third type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// An instance of a class implementing <see cref="IDiscriminatedUnion{TFirst,TSecond,TThird}"/>.
        /// </returns>
        public static IDiscriminatedUnion<TFirst, TSecond, TThird> Second<TFirst, TSecond, TThird>(TSecond value)
        {
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TFirst, TSecond, TThird>>() != null);

            return new DiscriminatedUnionSecond<TFirst, TSecond, TThird>(value);
        }

        /// <summary>
        /// Creates an instance of a class implementing <see cref="IDiscriminatedUnion{TFirst,TSecond,TThird}"/> holding a value of type <typeparamref name="TThird"/>.
        /// </summary>
        /// <param name="value">
        /// The value to hold in the discriminated union.
        /// </param>
        /// <typeparam name="TFirst">
        /// The first type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// The second type of the discriminated union.
        /// </typeparam>
        /// <typeparam name="TThird">
        /// The third type of the discriminated union.
        /// </typeparam>
        /// <returns>
        /// An instance of a class implementing <see cref="IDiscriminatedUnion{TFirst,TSecond,TThird}"/>.
        /// </returns>
        public static IDiscriminatedUnion<TFirst, TSecond, TThird> Third<TFirst, TSecond, TThird>(TThird value)
        {
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TFirst, TSecond, TThird>>() != null);

            return new DiscriminatedUnionThird<TFirst, TSecond, TThird>(value);
        }

        private class DiscriminatedUnionFirst<TFirst, TSecond, TThird> : DiscriminatedUnion<TFirst, TSecond, TThird>
        {
            private readonly TFirst value;

            /// <summary>
            /// Initializes a new instance of the <see cref="DiscriminatedUnionFirst{TFirst,TSecond,TThird}"/> class.
            /// </summary>
            /// <param name="value">
            /// The value.
            /// </param>
            public DiscriminatedUnionFirst(TFirst value)
            {
                this.value = value;
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TFirst" />.
            /// </summary>
            public override bool IsFirst
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TSecond" />.
            /// </summary>
            public override bool IsSecond
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TThird" />.
            /// </summary>
            public override bool IsThird
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="TFirst" /> if <see cref="IsFirst"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TFirst" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override TFirst First
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
                get
                {
                    return this.value;
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="TSecond" /> if <see cref="IsSecond"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TSecond" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override TSecond Second
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
                get
                {
                    return default(TSecond);
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="TThird" /> if <see cref="IsThird"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TThird" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override TThird Third
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
                get
                {
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
            public override void Switch(Action<TFirst> first, Action<TSecond> second, Action<TThird> third)
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
            /// <param name="third">
            /// The function to run if <see cref="IsThird"/> is <c>true</c>.
            /// </param>
            /// <typeparam name="TResult">
            /// The type of the result.
            /// </typeparam>
            /// <returns>
            /// The result of type <typeparamref name="TResult"/> of the function executed.
            /// </returns>
            public override TResult Switch<TResult>(Func<TFirst, TResult> first, Func<TSecond, TResult> second, Func<TThird, TResult> third)
            {
                return first(this.value);
            }
        }

        private class DiscriminatedUnionSecond<TFirst, TSecond, TThird> : DiscriminatedUnion<TFirst, TSecond, TThird>
        {
            private readonly TSecond value;

            /// <summary>
            /// Initializes a new instance of the <see cref="DiscriminatedUnionSecond{TFirst,TSecond,TThird}"/> class.
            /// </summary>
            /// <param name="value">
            /// The value.
            /// </param>
            public DiscriminatedUnionSecond(TSecond value)
            {
                this.value = value;
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TFirst" />.
            /// </summary>
            public override bool IsFirst
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TSecond" />.
            /// </summary>
            public override bool IsSecond
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TThird" />.
            /// </summary>
            public override bool IsThird
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="TFirst" /> if <see cref="IsFirst"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TFirst" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override TFirst First
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
                get
                {
                    return default(TFirst);
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="TSecond" /> if <see cref="IsSecond"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TSecond" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override TSecond Second
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
                get
                {
                    return this.value;
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="TThird" /> if <see cref="IsThird"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TThird" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override TThird Third
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
                get
                {
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
            public override void Switch(Action<TFirst> first, Action<TSecond> second, Action<TThird> third)
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
            /// <param name="third">
            /// The function to run if <see cref="IsThird"/> is <c>true</c>.
            /// </param>
            /// <typeparam name="TResult">
            /// The type of the result.
            /// </typeparam>
            /// <returns>
            /// The result of type <typeparamref name="TResult"/> of the function executed.
            /// </returns>
            public override TResult Switch<TResult>(Func<TFirst, TResult> first, Func<TSecond, TResult> second, Func<TThird, TResult> third)
            {
                return second(this.value);
            }
        }

        private class DiscriminatedUnionThird<TFirst, TSecond, TThird> : DiscriminatedUnion<TFirst, TSecond, TThird>
        {
            private readonly TThird value;

            /// <summary>
            /// Initializes a new instance of the <see cref="DiscriminatedUnionThird{TFirst,TSecond,TThird}"/> class.
            /// </summary>
            /// <param name="value">
            /// The value.
            /// </param>
            public DiscriminatedUnionThird(TThird value)
            {
                this.value = value;
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TFirst" />.
            /// </summary>
            public override bool IsFirst
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TSecond" />.
            /// </summary>
            public override bool IsSecond
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the discriminated union is holding a value of the type <typeparamref name="TThird" />.
            /// </summary>
            public override bool IsThird
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="TFirst" /> if <see cref="IsFirst"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TFirst" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override TFirst First
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
                get
                {
                    return default(TFirst);
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="TSecond" /> if <see cref="IsSecond"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TSecond" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override TSecond Second
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
                get
                {
                    return default(TSecond);
                }
            }

            /// <summary>
            /// Gets the value of type <typeparamref name="TThird" /> if <see cref="IsThird"/> is <c>true</c>, otherwise returns the default value for type <typeparamref name="TThird" />.
            /// </summary>
            // ReSharper disable MemberHidesStaticFromOuterClass
            public override TThird Third
            {
                // ReSharper restore MemberHidesStaticFromOuterClass
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
            /// <param name="third">
            /// The action to run if <see cref="IsThird"/> is <c>true</c>.
            /// </param>
            public override void Switch(Action<TFirst> first, Action<TSecond> second, Action<TThird> third)
            {
                third(this.value);
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
            public override TResult Switch<TResult>(Func<TFirst, TResult> first, Func<TSecond, TResult> second, Func<TThird, TResult> third)
            {
                return third(this.value);
            }
        }
    }
}