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

namespace MorseCode.RxMvvm.Observable.Property
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    /// <summary>
    /// An interface representing a factory for creating observable properties.
    /// </summary>
    [ContractClass(typeof(ObservablePropertyFactoryContract))]
    public interface IObservablePropertyFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates an asynchronously calculated property.
        /// </summary>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
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
        ICalculatedProperty<T> CreateAsyncCalculatedProperty<TFirst, T>(
            IObservable<TFirst> firstProperty,
            TimeSpan throttleTime,
            Func<TFirst, T> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates an asynchronously calculated property.
        /// </summary>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="secondProperty">
        /// The second property involved in the calculation.
        /// </param>
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
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
        ICalculatedProperty<T> CreateAsyncCalculatedProperty<TFirst, TSecond, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            TimeSpan throttleTime,
            Func<TFirst, TSecond, T> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates an asynchronously calculated property.
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
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
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
        ICalculatedProperty<T> CreateAsyncCalculatedProperty<TFirst, TSecond, TThird, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            TimeSpan throttleTime,
            Func<TFirst, TSecond, TThird, T> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates an asynchronously calculated property.
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
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
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
        ICalculatedProperty<T> CreateAsyncCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            TimeSpan throttleTime,
            Func<TFirst, TSecond, TThird, TFourth, T> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates an asynchronously calculated property.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
        /// <typeparam name="TFirst">
        /// The type of the first property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of the calculation result.
        /// </typeparam>
        /// <returns>
        /// The calculated property.
        /// </returns>
        ICalculatedProperty<T> CreateAsyncCalculatedPropertyWithContext<TContext, TFirst, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            TimeSpan throttleTime,
            Func<TContext, TFirst, T> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates an asynchronously calculated property.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="secondProperty">
        /// The second property involved in the calculation.
        /// </param>
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
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
        ICalculatedProperty<T> CreateAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            TimeSpan throttleTime,
            Func<TContext, TFirst, TSecond, T> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates an asynchronously calculated property.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="secondProperty">
        /// The second property involved in the calculation.
        /// </param>
        /// <param name="thirdProperty">
        /// The third property involved in the calculation.
        /// </param>
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
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
        ICalculatedProperty<T> CreateAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            TimeSpan throttleTime,
            Func<TContext, TFirst, TSecond, TThird, T> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates an asynchronously calculated property.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
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
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
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
        ICalculatedProperty<T> CreateAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            TimeSpan throttleTime,
            Func<TContext, TFirst, TSecond, TThird, TFourth, T> calculateValue,
            bool isLongRunningCalculation = false);

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
        ICalculatedProperty<T> CreateCalculatedProperty<TFirst, T>(
            IObservable<TFirst> firstProperty, Func<TFirst, T> calculateValue);

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
        ICalculatedProperty<T> CreateCalculatedProperty<TFirst, TSecond, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            Func<TFirst, TSecond, T> calculateValue);

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
        ICalculatedProperty<T> CreateCalculatedProperty<TFirst, TSecond, TThird, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            Func<TFirst, TSecond, TThird, T> calculateValue);

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
        ICalculatedProperty<T> CreateCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            Func<TFirst, TSecond, TThird, TFourth, T> calculateValue);

        /// <summary>
        /// Creates a calculated property.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
        /// <typeparam name="TFirst">
        /// The type of the first property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of the calculation result.
        /// </typeparam>
        /// <returns>
        /// The calculated property.
        /// </returns>
        ICalculatedProperty<T> CreateCalculatedPropertyWithContext<TContext, TFirst, T>(
            TContext context, IObservable<TFirst> firstProperty, Func<TContext, TFirst, T> calculateValue);

        /// <summary>
        /// Creates a calculated property.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="secondProperty">
        /// The second property involved in the calculation.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
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
        ICalculatedProperty<T> CreateCalculatedPropertyWithContext<TContext, TFirst, TSecond, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            Func<TContext, TFirst, TSecond, T> calculateValue);

        /// <summary>
        /// Creates a calculated property.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
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
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
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
        ICalculatedProperty<T> CreateCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            Func<TContext, TFirst, TSecond, TThird, T> calculateValue);

        /// <summary>
        /// Creates a calculated property.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
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
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
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
        ICalculatedProperty<T> CreateCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            Func<TContext, TFirst, TSecond, TThird, TFourth, T> calculateValue);

        /// <summary>
        /// Creates a cancellable asynchronously calculated property.
        /// </summary>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
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
        ICalculatedProperty<T> CreateCancellableAsyncCalculatedProperty<TFirst, T>(
            IObservable<TFirst> firstProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, Task<T>> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates a cancellable asynchronously calculated property.
        /// </summary>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="secondProperty">
        /// The second property involved in the calculation.
        /// </param>
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
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
        ICalculatedProperty<T> CreateCancellableAsyncCalculatedProperty<TFirst, TSecond, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, TSecond, Task<T>> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates a cancellable asynchronously calculated property.
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
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
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
        ICalculatedProperty<T> CreateCancellableAsyncCalculatedProperty<TFirst, TSecond, TThird, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, TSecond, TThird, Task<T>> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates a cancellable asynchronously calculated property.
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
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
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
        ICalculatedProperty<T> CreateCancellableAsyncCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, TSecond, TThird, TFourth, Task<T>> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates a cancellable asynchronously calculated property.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
        /// <typeparam name="TFirst">
        /// The type of the first property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of the calculation result.
        /// </typeparam>
        /// <returns>
        /// The calculated property.
        /// </returns>
        ICalculatedProperty<T> CreateCancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TContext, TFirst, Task<T>> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates a cancellable asynchronously calculated property.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="secondProperty">
        /// The second property involved in the calculation.
        /// </param>
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
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
        ICalculatedProperty<T> CreateCancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TContext, TFirst, TSecond, Task<T>> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates a cancellable asynchronously calculated property.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="firstProperty">
        /// The first property involved in the calculation.
        /// </param>
        /// <param name="secondProperty">
        /// The second property involved in the calculation.
        /// </param>
        /// <param name="thirdProperty">
        /// The third property involved in the calculation.
        /// </param>
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
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
        ICalculatedProperty<T> CreateCancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TContext, TFirst, TSecond, TThird, Task<T>> calculateValue,
            bool isLongRunningCalculation = false);

        /// <summary>
        /// Creates a cancellable asynchronously calculated property.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
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
        /// <param name="throttleTime">
        /// The amount of time to throttle between calculations.  Set to <see cref="TimeSpan.Zero"/> to suppress throttling.
        /// </param>
        /// <param name="calculateValue">
        /// The method to calculate the value.
        /// </param>
        /// <param name="isLongRunningCalculation">
        /// Whether or not the calculation is expected to take longer than 50 milliseconds to complete.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
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
        ICalculatedProperty<T> CreateCancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TContext, TFirst, TSecond, TThird, TFourth, Task<T>> calculateValue,
            bool isLongRunningCalculation = false);

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
        IObservableProperty<T> CreateProperty<T>(T initialValue);

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
        /// The read-only property as <see cref="IReadOnlyProperty{T}"/>.
        /// </returns>
        IReadOnlyProperty<T> CreateReadOnlyProperty<T>(T value);

        /// <summary>
        /// Creates a read-only property whose value is lazily evaluated.
        /// </summary>
        /// <param name="value">
        /// The lazily evaluated value for the property.
        /// </param>
        /// <typeparam name="T">
        /// The type of the property.
        /// </typeparam>
        /// <returns>
        /// The read-only property as <see cref="IReadOnlyProperty{T}"/>.
        /// </returns>
        IReadOnlyProperty<T> CreateReadOnlyProperty<T>(Lazy<T> value);

        #endregion
    }
}