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
    using System.Reactive;
    using System.Reactive.Linq;

    /// <summary>
    /// Provides <see langword="static"/> extension methods for <see cref="IDiscriminatedUnion{TFirst,TSecond}"/>.
    /// </summary>
    public static class DiscriminatedUnion
    {
        /// <summary>
        /// Creates an observer that is capable of observing an observable with two notification channels using the specified actions.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="onNextFirst">
        /// Handler for notifications from the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// Handler for notifications from the second channel.
        /// </param>
        /// <returns>
        /// An observer capable of observing an observable with two notification channels.
        /// </returns>
        public static IObserver<IDiscriminatedUnion<TFirst, TSecond>> CreateDiscriminatedUnion<TFirst, TSecond>(
            Action<TFirst> onNextFirst, Action<TSecond> onNextSecond)
        {
            Contract.Requires(onNextFirst != null);
            Contract.Requires(onNextSecond != null);
            Contract.Ensures(Contract.Result<IObserver<IDiscriminatedUnion<TFirst, TSecond>>>() != null);

            return CreateDiscriminatedUnion(
                onNextFirst, 
                onNextSecond, 
                ex => { throw ex; /*.PrepareForRethrow(); changed to internal in Rx 1.1.10425 */ }, 
                () => { });
        }

        /// <summary>
        /// Creates an observer that is capable of observing an observable with two notification channels using the specified actions.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="onNextFirst">
        /// Handler for notifications from the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// Handler for notifications from the second channel.
        /// </param>
        /// <param name="onError">
        /// Handler for an error notification.
        /// </param>
        /// <returns>
        /// An observer capable of observing an observable with two notification channels.
        /// </returns>
        public static IObserver<IDiscriminatedUnion<TFirst, TSecond>> CreateDiscriminatedUnion<TFirst, TSecond>(
            Action<TFirst> onNextFirst, Action<TSecond> onNextSecond, Action<Exception> onError)
        {
            Contract.Requires(onNextFirst != null);
            Contract.Requires(onNextSecond != null);
            Contract.Requires(onError != null);
            Contract.Ensures(Contract.Result<IObserver<IDiscriminatedUnion<TFirst, TSecond>>>() != null);

            return CreateDiscriminatedUnion(onNextFirst, onNextSecond, onError, () => { });
        }

        /// <summary>
        /// Creates an observer that is capable of observing an observable with two notification channels using the specified actions.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="onNextFirst">
        /// Handler for notifications from the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// Handler for notifications from the second channel.
        /// </param>
        /// <param name="onCompleted">
        /// Handler for a completed notification.
        /// </param>
        /// <returns>
        /// An observer capable of observing an observable with two notification channels.
        /// </returns>
        public static IObserver<IDiscriminatedUnion<TFirst, TSecond>> CreateDiscriminatedUnion<TFirst, TSecond>(
            Action<TFirst> onNextFirst, Action<TSecond> onNextSecond, Action onCompleted)
        {
            Contract.Requires(onNextFirst != null);
            Contract.Requires(onNextSecond != null);
            Contract.Requires(onCompleted != null);
            Contract.Ensures(Contract.Result<IObserver<IDiscriminatedUnion<TFirst, TSecond>>>() != null);

            return CreateDiscriminatedUnion(
                onNextFirst, 
                onNextSecond, 
                ex => { throw ex; /*.PrepareForRethrow(); changed to internal in Rx 1.1.10425 */ }, 
                onCompleted);
        }

        /// <summary>
        /// Creates an observer that is capable of observing an observable with two notification channels using the specified actions.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="onNextFirst">
        /// Handler for notifications from the first channel.
        /// </param>
        /// <param name="onNextSecond">
        /// Handler for notifications from the second channel.
        /// </param>
        /// <param name="onError">
        /// Handler for an error notification.
        /// </param>
        /// <param name="onCompleted">
        /// Handler for a completed notification.
        /// </param>
        /// <returns>
        /// An observer capable of observing an observable with two notification channels.
        /// </returns>
        public static IObserver<IDiscriminatedUnion<TFirst, TSecond>> CreateDiscriminatedUnion<TFirst, TSecond>(
            Action<TFirst> onNextFirst, Action<TSecond> onNextSecond, Action<Exception> onError, Action onCompleted)
        {
            Contract.Requires(onNextFirst != null);
            Contract.Requires(onNextSecond != null);
            Contract.Requires(onError != null);
            Contract.Requires(onCompleted != null);
            Contract.Ensures(Contract.Result<IObserver<IDiscriminatedUnion<TFirst, TSecond>>>() != null);

            return
                Observer.Create<IDiscriminatedUnion<TFirst, TSecond>>(
                    value => value.Switch(onNextFirst, onNextSecond), onError, onCompleted);
        }

        /// <summary>
        /// Creates an observable sequence with two notification channels from the <paramref name="subscribe"/> implementation.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="subscribe">
        /// Subscribes observers to the observable.
        /// </param>
        /// <returns>
        /// An observable with two notification channels that calls the specified <paramref name="subscribe"/> function 
        /// when an observer subscribes.
        /// </returns>
        public static IObservable<IDiscriminatedUnion<TFirst, TSecond>> CreateDiscriminatedUnion<TFirst, TSecond>(
            Func<IObserver<IDiscriminatedUnion<TFirst, TSecond>>, Action> subscribe)
        {
            Contract.Requires(subscribe != null);
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<TFirst, TSecond>>>() != null);

            return Observable.Create(subscribe);
        }

        /// <summary>
        /// Creates an observable sequence with two notification channels from the <paramref name="subscribe"/> implementation.
        /// </summary>
        /// <typeparam name="TFirst">
        /// Type of the first notification channel.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// Type of the second notification channel.
        /// </typeparam>
        /// <param name="subscribe">
        /// Subscribes observers to the observable.
        /// </param>
        /// <returns>
        /// An observable with two notification channels that calls the specified <paramref name="subscribe"/> function
        /// when an observer subscribes.
        /// </returns>
        public static IObservable<IDiscriminatedUnion<TFirst, TSecond>> CreateDiscriminatedUnion<TFirst, TSecond>(
            Func<IObserver<IDiscriminatedUnion<TFirst, TSecond>>, IDisposable> subscribe)
        {
            Contract.Requires(subscribe != null);
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<TFirst, TSecond>>>() != null);

            return Observable.Create(subscribe);
        }

        /// <summary>
        /// Creates an instance of a class implementing <see cref="IDiscriminatedUnion{TFirst,TSecond}"/> holding a value of type <typeparamref name="TFirst"/>.
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
        /// <returns>
        /// An instance of a class implementing <see cref="IDiscriminatedUnion{TFirst,TSecond}"/>.
        /// </returns>
        public static IDiscriminatedUnion<TFirst, TSecond> First<TFirst, TSecond>(TFirst value)
        {
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TFirst, TSecond>>() != null);

            return new DiscriminatedUnionFirst<TFirst, TSecond>(value);
        }

        /// <summary>
        /// Creates an instance of a class implementing <see cref="IDiscriminatedUnion{TFirst,TSecond}"/> holding a value of type <typeparamref name="TSecond"/>.
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
        /// <returns>
        /// An instance of a class implementing <see cref="IDiscriminatedUnion{TFirst,TSecond}"/>.
        /// </returns>
        public static IDiscriminatedUnion<TFirst, TSecond> Second<TFirst, TSecond>(TSecond value)
        {
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<TFirst, TSecond>>() != null);

            return new DiscriminatedUnionSecond<TFirst, TSecond>(value);
        }

        private class DiscriminatedUnionFirst<TFirst, TSecond> : DiscriminatedUnion<TFirst, TSecond>
        {
            private readonly TFirst value;

            /// <summary>
            /// Initializes a new instance of the <see cref="DiscriminatedUnionFirst{TFirst,TSecond}"/> class.
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
            /// Executes an action based on which value is contained in the discriminated union.
            /// </summary>
            /// <param name="first">
            /// The action to run if <see cref="IsFirst"/> is <c>true</c>.
            /// </param>
            /// <param name="second">
            /// The action to run if <see cref="IsSecond"/> is <c>true</c>.
            /// </param>
            public override void Switch(Action<TFirst> first, Action<TSecond> second)
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
            public override TResult Switch<TResult>(Func<TFirst, TResult> first, Func<TSecond, TResult> second)
            {
                return first(this.value);
            }
        }

        private class DiscriminatedUnionSecond<TFirst, TSecond> : DiscriminatedUnion<TFirst, TSecond>
        {
            private readonly TSecond value;

            /// <summary>
            /// Initializes a new instance of the <see cref="DiscriminatedUnionSecond{TFirst,TSecond}"/> class.
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
            /// Executes an action based on which value is contained in the discriminated union.
            /// </summary>
            /// <param name="first">
            /// The action to run if <see cref="IsFirst"/> is <c>true</c>.
            /// </param>
            /// <param name="second">
            /// The action to run if <see cref="IsSecond"/> is <c>true</c>.
            /// </param>
            public override void Switch(Action<TFirst> first, Action<TSecond> second)
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
            public override TResult Switch<TResult>(Func<TFirst, TResult> first, Func<TSecond, TResult> second)
            {
                return second(this.value);
            }
        }
    }
}