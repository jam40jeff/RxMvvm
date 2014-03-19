﻿#region License

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

namespace MorseCode.RxMvvm.Observable
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Common;

    /// <summary>
    /// A factory for creating observable properties.
    /// </summary>
    public static class ObservablePropertyFactory
    {
        /// <summary>
        /// Creates a read-only property.
        /// </summary>
        /// <param name="value">
        /// The value for the property.
        /// </param>
        /// <typeparam name="T">
        /// The type of the property.
        /// </typeparam>
        /// <returns>
        /// The read-only property as <see cref="IReadableObservableProperty{T}"/>.
        /// </returns>
        public static IReadableObservableProperty<T> CreateReadOnlyProperty<T>(T value)
        {
            Contract.Ensures(Contract.Result<IReadableObservableProperty<T>>() != null);

            return new ReadOnlyProperty<T>(value);
        }

        /// <summary>
        /// Creates an observable read-write property.
        /// </summary>
        /// <param name="initialValue">
        /// The initial value for the property.
        /// </param>
        /// <typeparam name="T">
        /// The type of the property.
        /// </typeparam>
        /// <returns>
        /// The observable read-write property as <see cref="IObservableProperty{T}"/>.
        /// </returns>
        public static IObservableProperty<T> CreateProperty<T>(T initialValue)
        {
            Contract.Ensures(Contract.Result<IObservableProperty<T>>() != null);

            return new ObservableProperty<T>(initialValue);
        }

        /// <summary>
        /// Creates a calculated property.
        /// </summary>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <typeparam name="TFirst">
        /// The type of the first property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of the calculation result.
        /// </typeparam>
        /// <returns>
        /// The calculated property.
        /// </returns>
        public static ICalculatedProperty<T> Create<TFirst, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            Func<TFirst, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<TFirst, IDiscriminatedUnion<T, Exception>> calculate = first =>
            {
                IDiscriminatedUnion<T, Exception> discriminatedUnion;
                try
                {
                    discriminatedUnion = DiscriminatedUnion.First<T, Exception>(calculateValue(first));
                }
                catch (Exception e)
                {
                    discriminatedUnion = DiscriminatedUnion.Second<T, Exception>(e);
                }

                return discriminatedUnion;
            };

            IObservable<IDiscriminatedUnion<T, Exception>> setOrExceptionObservable = firstProperty.Select(calculate);

            if (setOrExceptionObservable == null)
            {
                throw new InvalidOperationException("Result of CombineLatest cannot be null.");
            }

            IDiscriminatedUnion<T, Exception> initialValue = calculate(firstProperty.Value);

            if (initialValue == null)
            {
                throw new InvalidOperationException("Result of calculate cannot be null.");
            }

            return new CalculatedProperty<T>(setOrExceptionObservable, initialValue);
        }

        /// <summary>
        /// Creates a calculated property.
        /// </summary>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="secondProperty">
        /// The second property involved in the calculation.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <typeparam name="TFirst">
        /// The type of the first property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// The type of the second property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of the calculation result.
        /// </typeparam>
        /// <returns>
        /// The calculated property.
        /// </returns>
        public static ICalculatedProperty<T> Create<TFirst, TSecond, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            IReadableObservableProperty<TSecond> secondProperty,
            Func<TFirst, TSecond, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<TFirst, TSecond, IDiscriminatedUnion<T, Exception>> calculate = (first, second) =>
            {
                IDiscriminatedUnion<T, Exception> discriminatedUnion;
                try
                {
                    discriminatedUnion = DiscriminatedUnion.First<T, Exception>(calculateValue(first, second));
                }
                catch (Exception e)
                {
                    discriminatedUnion = DiscriminatedUnion.Second<T, Exception>(e);
                }

                return discriminatedUnion;
            };

            IObservable<IDiscriminatedUnion<T, Exception>> setOrExceptionObservable = firstProperty.CombineLatest(
                secondProperty, calculate);

            if (setOrExceptionObservable == null)
            {
                throw new InvalidOperationException("Result of CombineLatest cannot be null.");
            }

            IDiscriminatedUnion<T, Exception> initialValue = calculate(firstProperty.Value, secondProperty.Value);

            if (initialValue == null)
            {
                throw new InvalidOperationException("Result of calculate cannot be null.");
            }

            return new CalculatedProperty<T>(setOrExceptionObservable, initialValue);
        }

        /// <summary>
        /// Creates a calculated property.
        /// </summary>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="secondProperty">
        /// The second property involved in the calculation.
        /// </param>
        /// <param name="thirdProperty">
        /// The third property involved in the calculation.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <typeparam name="TFirst">
        /// The type of the first property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// The type of the second property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="TThird">
        /// The type of the third property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of the calculation result.
        /// </typeparam>
        /// <returns>
        /// The calculated property.
        /// </returns>
        public static ICalculatedProperty<T> Create<TFirst, TSecond, TThird, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            IReadableObservableProperty<TSecond> secondProperty,
            IReadableObservableProperty<TThird> thirdProperty,
            Func<TFirst, TSecond, TThird, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<TFirst, TSecond, TThird, IDiscriminatedUnion<T, Exception>> calculate = (first, second, third) =>
            {
                IDiscriminatedUnion<T, Exception> discriminatedUnion;
                try
                {
                    discriminatedUnion = DiscriminatedUnion.First<T, Exception>(calculateValue(first, second, third));
                }
                catch (Exception e)
                {
                    discriminatedUnion = DiscriminatedUnion.Second<T, Exception>(e);
                }

                return discriminatedUnion;
            };

            IObservable<IDiscriminatedUnion<T, Exception>> setOrExceptionObservable = firstProperty.CombineLatest(
                secondProperty, thirdProperty, calculate);

            if (setOrExceptionObservable == null)
            {
                throw new InvalidOperationException("Result of CombineLatest cannot be null.");
            }

            IDiscriminatedUnion<T, Exception> initialValue = calculate(firstProperty.Value, secondProperty.Value, thirdProperty.Value);

            if (initialValue == null)
            {
                throw new InvalidOperationException("Result of calculate cannot be null.");
            }

            return new CalculatedProperty<T>(setOrExceptionObservable, initialValue);
        }

        /// <summary>
        /// Creates a calculated property.
        /// </summary>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="secondProperty">
        /// The second property involved in the calculation.
        /// </param>
        /// <param name="thirdProperty">
        /// The third property involved in the calculation.
        /// </param>
        /// <param name="fourthProperty">
        /// The fourth property involved in the calculation.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <typeparam name="TFirst">
        /// The type of the first property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="TSecond">
        /// The type of the second property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="TThird">
        /// The type of the third property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="TFourth">
        /// The type of the fourth property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of the calculation result.
        /// </typeparam>
        /// <returns>
        /// The calculated property.
        /// </returns>
        public static ICalculatedProperty<T> Create<TFirst, TSecond, TThird, TFourth, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            IReadableObservableProperty<TSecond> secondProperty,
            IReadableObservableProperty<TThird> thirdProperty,
            IReadableObservableProperty<TFourth> fourthProperty,
            Func<TFirst, TSecond, TThird, TFourth, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(fourthProperty != null, "fourthProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<TFirst, TSecond, TThird, TFourth, IDiscriminatedUnion<T, Exception>> calculate = (first, second, third, fourth) =>
                {
                    Contract.Ensures(Contract.Result<IDiscriminatedUnion<T, Exception>>() != null);

                    IDiscriminatedUnion<T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion = DiscriminatedUnion.First<T, Exception>(calculateValue(first, second, third, fourth));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            IObservable<IDiscriminatedUnion<T, Exception>> setOrExceptionObservable = firstProperty.CombineLatest(
                secondProperty, thirdProperty, fourthProperty, calculate);

            if (setOrExceptionObservable == null)
            {
                throw new InvalidOperationException("Result of CombineLatest cannot be null.");
            }

            IDiscriminatedUnion<T, Exception> initialValue = calculate(firstProperty.Value, secondProperty.Value, thirdProperty.Value, fourthProperty.Value);

            if (initialValue == null)
            {
                throw new InvalidOperationException("Result of calculate cannot be null.");
            }

            return new CalculatedProperty<T>(setOrExceptionObservable, initialValue);
        }
    }
}