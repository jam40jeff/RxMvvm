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

    using MorseCode.RxMvvm.Observable.Property.Internal;

    /// <summary>
    /// A factory for creating observable properties.
    /// </summary>
    public class ObservablePropertyFactory : IObservablePropertyFactory
    {
        #region Static Fields

        private static readonly Lazy<ObservablePropertyFactory> InstanceLazy =
            new Lazy<ObservablePropertyFactory>(() => new ObservablePropertyFactory());

        #endregion

        #region Constructors and Destructors

        private ObservablePropertyFactory()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the singleton instance of an <see cref="ObservablePropertyFactory"/>.
        /// </summary>
        [ContractVerification(false)]
        public static IObservablePropertyFactory Instance
        {
            get
            {
                Contract.Ensures(Contract.Result<IObservablePropertyFactory>() != null);

                return InstanceLazy.Value;
            }
        }

        #endregion

        #region Explicit Interface Methods

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedProperty<TFirst, T>(
            IObservable<TFirst> firstProperty,
            TimeSpan throttleTime,
            Func<TFirst, T> calculateValue,
            bool isLongRunningCalculation)
        {
            return new AsyncCalculatedProperty<TFirst, T>(firstProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedProperty<TFirst, TSecond, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            TimeSpan throttleTime,
            Func<TFirst, TSecond, T> calculateValue,
            bool isLongRunningCalculation)
        {
            return new AsyncCalculatedProperty<TFirst, TSecond, T>(
                firstProperty, secondProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedProperty<TFirst, TSecond, TThird, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            TimeSpan throttleTime,
            Func<TFirst, TSecond, TThird, T> calculateValue,
            bool isLongRunningCalculation)
        {
            return new AsyncCalculatedProperty<TFirst, TSecond, TThird, T>(
                firstProperty, secondProperty, thirdProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            TimeSpan throttleTime,
            Func<TFirst, TSecond, TThird, TFourth, T> calculateValue,
            bool isLongRunningCalculation)
        {
            return new AsyncCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
                firstProperty, secondProperty, thirdProperty, fourthProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedPropertyWithContext<TContext, TFirst, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            TimeSpan throttleTime,
            Func<TContext, TFirst, T> calculateValue,
            bool isLongRunningCalculation)
        {
            return new AsyncCalculatedPropertyWithContext<TContext, TFirst, T>(
                context, firstProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            TimeSpan throttleTime,
            Func<TContext, TFirst, TSecond, T> calculateValue,
            bool isLongRunningCalculation)
        {
            return new AsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, T>(
                context, firstProperty, secondProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            TimeSpan throttleTime,
            Func<TContext, TFirst, TSecond, TThird, T> calculateValue,
            bool isLongRunningCalculation)
        {
            return new AsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, T>(
                context, firstProperty, secondProperty, thirdProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            TimeSpan throttleTime,
            Func<TContext, TFirst, TSecond, TThird, TFourth, T> calculateValue,
            bool isLongRunningCalculation)
        {
            return new AsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T>(
                context, firstProperty, secondProperty, thirdProperty, fourthProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedProperty<TFirst, T>(
            IObservable<TFirst> firstProperty, Func<TFirst, T> calculateValue)
        {
            return new CalculatedProperty<TFirst, T>(firstProperty, calculateValue);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedProperty<TFirst, TSecond, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            Func<TFirst, TSecond, T> calculateValue)
        {
            return new CalculatedProperty<TFirst, TSecond, T>(firstProperty, secondProperty, calculateValue);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedProperty<TFirst, TSecond, TThird, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            Func<TFirst, TSecond, TThird, T> calculateValue)
        {
            return new CalculatedProperty<TFirst, TSecond, TThird, T>(
                firstProperty, secondProperty, thirdProperty, calculateValue);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            Func<TFirst, TSecond, TThird, TFourth, T> calculateValue)
        {
            return new CalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
                firstProperty, secondProperty, thirdProperty, fourthProperty, calculateValue);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedPropertyWithContext<TContext, TFirst, T>(
            TContext context, IObservable<TFirst> firstProperty, Func<TContext, TFirst, T> calculateValue)
        {
            return new CalculatedPropertyWithContext<TContext, TFirst, T>(context, firstProperty, calculateValue);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedPropertyWithContext<TContext, TFirst, TSecond, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            Func<TContext, TFirst, TSecond, T> calculateValue)
        {
            return new CalculatedPropertyWithContext<TContext, TFirst, TSecond, T>(
                context, firstProperty, secondProperty, calculateValue);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            Func<TContext, TFirst, TSecond, TThird, T> calculateValue)
        {
            return new CalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, T>(
                context, firstProperty, secondProperty, thirdProperty, calculateValue);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            Func<TContext, TFirst, TSecond, TThird, TFourth, T> calculateValue)
        {
            return new CalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T>(
                context, firstProperty, secondProperty, thirdProperty, fourthProperty, calculateValue);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedProperty<TFirst, T>(
            IObservable<TFirst> firstProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, Task<T>> calculateValue,
            bool isLongRunningCalculation)
        {
            return new CancellableAsyncCalculatedProperty<TFirst, T>(firstProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedProperty<TFirst, TSecond, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, TSecond, Task<T>> calculateValue,
            bool isLongRunningCalculation)
        {
            return new CancellableAsyncCalculatedProperty<TFirst, TSecond, T>(
                firstProperty, secondProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedProperty<TFirst, TSecond, TThird, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, TSecond, TThird, Task<T>> calculateValue,
            bool isLongRunningCalculation)
        {
            return new CancellableAsyncCalculatedProperty<TFirst, TSecond, TThird, T>(
                firstProperty, secondProperty, thirdProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, TSecond, TThird, TFourth, Task<T>> calculateValue,
            bool isLongRunningCalculation)
        {
            return new CancellableAsyncCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
                firstProperty, secondProperty, thirdProperty, fourthProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TContext, TFirst, Task<T>> calculateValue,
            bool isLongRunningCalculation)
        {
            return new CancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, T>(
                context, firstProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TContext, TFirst, TSecond, Task<T>> calculateValue,
            bool isLongRunningCalculation)
        {
            return new CancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, T>(
                context, firstProperty, secondProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TContext, TFirst, TSecond, TThird, Task<T>> calculateValue,
            bool isLongRunningCalculation)
        {
            return new CancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, T>(
                context, firstProperty, secondProperty, thirdProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T>(
            TContext context,
            IObservable<TFirst> firstProperty,
            IObservable<TSecond> secondProperty,
            IObservable<TThird> thirdProperty,
            IObservable<TFourth> fourthProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TContext, TFirst, TSecond, TThird, TFourth, Task<T>> calculateValue,
            bool isLongRunningCalculation)
        {
            return
                new CancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T>(
                    context, firstProperty, secondProperty, thirdProperty, fourthProperty, throttleTime, calculateValue, isLongRunningCalculation);
        }

        IObservableProperty<T> IObservablePropertyFactory.CreateProperty<T>(T initialValue)
        {
            return new ObservableProperty<T>(initialValue);
        }

        IReadOnlyProperty<T> IObservablePropertyFactory.CreateReadOnlyProperty<T>(T value)
        {
            return new ReadOnlyProperty<T>(value);
        }

        ILazyReadOnlyProperty<T> IObservablePropertyFactory.CreateLazyReadOnlyProperty<T>(Func<T> valueFactory, bool isLongRunningCalculation)
        {
            return new LazyReadOnlyProperty<T>(valueFactory, isLongRunningCalculation);
        }

        #endregion
    }
}