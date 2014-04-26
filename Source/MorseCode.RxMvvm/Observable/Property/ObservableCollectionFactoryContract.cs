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

    [ContractClassFor(typeof(IObservablePropertyFactory))]
    internal abstract class ObservablePropertyFactoryContract : IObservablePropertyFactory
    {
        IReadOnlyProperty<T> IObservablePropertyFactory.CreateReadOnlyProperty<T>(T value)
        {
            Contract.Ensures(Contract.Result<IReadOnlyProperty<T>>() != null);

            return null;
        }

        IReadOnlyProperty<T> IObservablePropertyFactory.CreateReadOnlyProperty<T>(Lazy<T> value)
        {
            Contract.Requires<ArgumentNullException>(value != null, "value");
            Contract.Ensures(Contract.Result<IReadOnlyProperty<T>>() != null);

            return null;
        }

        IObservableProperty<T> IObservablePropertyFactory.CreateProperty<T>(T initialValue)
        {
            Contract.Ensures(Contract.Result<IObservableProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedProperty<TFirst, T>(
            IObservable<TFirst> firstProperty, Func<TFirst, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedProperty<TFirst, TSecond, T>(
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            Func<TFirst, TSecond, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedProperty<TFirst, TSecond, TThird, T>(
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            IObservable<TThird> thirdProperty, 
            Func<TFirst, TSecond, TThird, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            IObservable<TThird> thirdProperty, 
            IObservable<TFourth> fourthProperty, 
            Func<TFirst, TSecond, TThird, TFourth, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(fourthProperty != null, "fourthProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedPropertyWithContext<TContext, TFirst, T>(
            TContext context, IObservable<TFirst> firstProperty, Func<TContext, TFirst, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedPropertyWithContext<TContext, TFirst, TSecond, T>(
            TContext context, 
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            Func<TContext, TFirst, TSecond, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, T>(
            TContext context, 
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            IObservable<TThird> thirdProperty, 
            Func<TContext, TFirst, TSecond, TThird, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T>(
            TContext context, 
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            IObservable<TThird> thirdProperty, 
            IObservable<TFourth> fourthProperty, 
            Func<TContext, TFirst, TSecond, TThird, TFourth, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(fourthProperty != null, "fourthProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedProperty<TFirst, T>(
            IObservable<TFirst> firstProperty, TimeSpan throttleTime, Func<TFirst, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedProperty<TFirst, TSecond, T>(
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            TimeSpan throttleTime, 
            Func<TFirst, TSecond, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedProperty<TFirst, TSecond, TThird, T>(
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            IObservable<TThird> thirdProperty, 
            TimeSpan throttleTime, 
            Func<TFirst, TSecond, TThird, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            IObservable<TThird> thirdProperty, 
            IObservable<TFourth> fourthProperty, 
            TimeSpan throttleTime, 
            Func<TFirst, TSecond, TThird, TFourth, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(fourthProperty != null, "fourthProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedPropertyWithContext<TContext, TFirst, T>(
            TContext context, 
            IObservable<TFirst> firstProperty, 
            TimeSpan throttleTime, 
            Func<TContext, TFirst, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, T>(
            TContext context, 
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            TimeSpan throttleTime, 
            Func<TContext, TFirst, TSecond, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, T>(
            TContext context, 
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            IObservable<TThird> thirdProperty, 
            TimeSpan throttleTime, 
            Func<TContext, TFirst, TSecond, TThird, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T>(
            TContext context, 
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            IObservable<TThird> thirdProperty, 
            IObservable<TFourth> fourthProperty, 
            TimeSpan throttleTime, 
            Func<TContext, TFirst, TSecond, TThird, TFourth, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(fourthProperty != null, "fourthProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedProperty<TFirst, T>(
            IObservable<TFirst> firstProperty, 
            TimeSpan throttleTime, 
            Func<AsyncCalculationHelper, TFirst, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedProperty<TFirst, TSecond, T>(
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            TimeSpan throttleTime, 
            Func<AsyncCalculationHelper, TFirst, TSecond, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedProperty<TFirst, TSecond, TThird, T>(
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            IObservable<TThird> thirdProperty, 
            TimeSpan throttleTime, 
            Func<AsyncCalculationHelper, TFirst, TSecond, TThird, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            IObservable<TThird> thirdProperty, 
            IObservable<TFourth> fourthProperty, 
            TimeSpan throttleTime, 
            Func<AsyncCalculationHelper, TFirst, TSecond, TThird, TFourth, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(fourthProperty != null, "fourthProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, T>(
            TContext context, 
            IObservable<TFirst> firstProperty, 
            TimeSpan throttleTime, 
            Func<AsyncCalculationHelper, TContext, TFirst, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, T>(
            TContext context, 
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            TimeSpan throttleTime, 
            Func<AsyncCalculationHelper, TContext, TFirst, TSecond, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, T>(
            TContext context, 
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            IObservable<TThird> thirdProperty, 
            TimeSpan throttleTime, 
            Func<AsyncCalculationHelper, TContext, TFirst, TSecond, TThird, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }

        ICalculatedProperty<T> IObservablePropertyFactory.CreateCancellableAsyncCalculatedPropertyWithContext<TContext, TFirst, TSecond, TThird, TFourth, T>(
            TContext context, 
            IObservable<TFirst> firstProperty, 
            IObservable<TSecond> secondProperty, 
            IObservable<TThird> thirdProperty, 
            IObservable<TFourth> fourthProperty, 
            TimeSpan throttleTime, 
            Func<AsyncCalculationHelper, TContext, TFirst, TSecond, TThird, TFourth, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(fourthProperty != null, "fourthProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            return null;
        }
    }
}