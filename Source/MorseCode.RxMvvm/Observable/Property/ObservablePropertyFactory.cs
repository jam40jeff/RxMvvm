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
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

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
        public static IReadOnlyProperty<T> CreateReadOnlyProperty<T>(T value)
        {
            Contract.Ensures(Contract.Result<IReadableObservableProperty<T>>() != null);

            return new ReadOnlyProperty<T>(new Lazy<T>(() => value));
        }

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
        /// The read-only property as <see cref="IReadableObservableProperty{T}"/>.
        /// </returns>
        public static IReadOnlyProperty<T> CreateReadOnlyProperty<T>(Lazy<T> value)
        {
            Contract.Requires<ArgumentNullException>(value != null, "value");
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
        public static ICalculatedProperty<T> CreateCalculatedProperty<TFirst, T>(
            IReadableObservableProperty<TFirst> firstProperty, Func<TFirst, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<TFirst, IDiscriminatedUnion<object, T, Exception>> calculate = first =>
                {
                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion = DiscriminatedUnion.First<object, T, Exception>(calculateValue(first));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            return new CalculatedProperty<T>((resultSubject, isCalculatingSubject) =>
            {
                resultSubject.OnNext(calculate(firstProperty.Value));

                return firstProperty.Subscribe(
                        v =>
                        {
                            isCalculatingSubject.OnNext(true);

                            try
                            {
                                resultSubject.OnNext(calculate(v));
                            }
                            catch (Exception e)
                            {
                                resultSubject.OnNext(
                                    DiscriminatedUnion.Second<object, T, Exception>(e));
                            }

                            isCalculatingSubject.OnNext(false);
                        });
            });
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
        public static ICalculatedProperty<T> CreateCalculatedProperty<TFirst, TSecond, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            IReadableObservableProperty<TSecond> secondProperty,
            Func<TFirst, TSecond, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<TFirst, TSecond, IDiscriminatedUnion<object, T, Exception>> calculate = (first, second) =>
                {
                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion = DiscriminatedUnion.First<object, T, Exception>(calculateValue(first, second));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            return new CalculatedProperty<T>((resultSubject, isCalculatingSubject) =>
            {
                resultSubject.OnNext(calculate(firstProperty.Value, secondProperty.Value));

                IObservable<Tuple<TFirst, TSecond>> o = firstProperty.CombineLatest(secondProperty, Tuple.Create);
                return o.Subscribe(
                        v =>
                        {
                            isCalculatingSubject.OnNext(true);

                            try
                            {
                                resultSubject.OnNext(calculate(v.Item1, v.Item2));
                            }
                            catch (Exception e)
                            {
                                resultSubject.OnNext(
                                    DiscriminatedUnion.Second<object, T, Exception>(e));
                            }

                            isCalculatingSubject.OnNext(false);
                        });
            });
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
        public static ICalculatedProperty<T> CreateCalculatedProperty<TFirst, TSecond, TThird, T>(
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

            Func<TFirst, TSecond, TThird, IDiscriminatedUnion<object, T, Exception>> calculate = (first, second, third) =>
                {
                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion = DiscriminatedUnion.First<object, T, Exception>(
                            calculateValue(first, second, third));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            return new CalculatedProperty<T>((resultSubject, isCalculatingSubject) =>
            {
                resultSubject.OnNext(calculate(firstProperty.Value, secondProperty.Value, thirdProperty.Value));

                IObservable<Tuple<TFirst, TSecond, TThird>> o = firstProperty.CombineLatest(secondProperty, thirdProperty, Tuple.Create);
                return o.Subscribe(
                        v =>
                        {
                            isCalculatingSubject.OnNext(true);

                            try
                            {
                                resultSubject.OnNext(calculate(v.Item1, v.Item2, v.Item3));
                            }
                            catch (Exception e)
                            {
                                resultSubject.OnNext(
                                    DiscriminatedUnion.Second<object, T, Exception>(e));
                            }

                            isCalculatingSubject.OnNext(false);
                        });
            });
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
        public static ICalculatedProperty<T> CreateCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
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

            Func<TFirst, TSecond, TThird, TFourth, IDiscriminatedUnion<object, T, Exception>> calculate =
                (first, second, third, fourth) =>
                {
                    Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(calculateValue(first, second, third, fourth));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            return new CalculatedProperty<T>((resultSubject, isCalculatingSubject) =>
            {
                resultSubject.OnNext(calculate(firstProperty.Value, secondProperty.Value, thirdProperty.Value, fourthProperty.Value));

                IObservable<Tuple<TFirst, TSecond, TThird, TFourth>> o = firstProperty.CombineLatest(
                    secondProperty, thirdProperty, fourthProperty, Tuple.Create);
                return o.Subscribe(
                        v =>
                        {
                            isCalculatingSubject.OnNext(true);

                            try
                            {
                                resultSubject.OnNext(calculate(v.Item1, v.Item2, v.Item3, v.Item4));
                            }
                            catch (Exception e)
                            {
                                resultSubject.OnNext(
                                    DiscriminatedUnion.Second<object, T, Exception>(e));
                            }

                            isCalculatingSubject.OnNext(false);
                        });
            });
        }

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
        /// <typeparam name="TFirst">
        /// The type of the first property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of the calculation result.
        /// </typeparam>
        /// <returns>
        /// The calculated property.
        /// </returns>
        public static ICalculatedProperty<T> CreateAsyncCalculatedProperty<TFirst, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            TimeSpan throttleTime,
            Func<TFirst, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<TFirst, IDiscriminatedUnion<object, T, Exception>> calculate =
                first =>
                {
                    Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(calculateValue(first));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            // TODO: pick a better scheduler
            return new CalculatedProperty<T>((resultSubject, isCalculatingSubject) =>
            {
                CompositeDisposable d = new CompositeDisposable();
                IScheduler scheduler = Scheduler.Default;

                IDisposable scheduledTask = scheduler.ScheduleAsync(async (s, t) => resultSubject.OnNext(await Task.FromResult(calculate(firstProperty.Value))));
                d.Add(scheduledTask);

                IObservable<TFirst> o = throttleTime > TimeSpan.Zero ? firstProperty.Throttle(throttleTime, scheduler) : firstProperty.ObserveOn(scheduler);
                d.Add(
                    o.Subscribe(
                        v =>
                        {
                            using (scheduledTask)
                            {
                            }

                            isCalculatingSubject.OnNext(true);

                            scheduledTask = scheduler.ScheduleAsync(
                                async (s, t) =>
                                {
                                    try
                                    {
                                        resultSubject.OnNext(
                                            await Task.FromResult(calculate(v)));
                                    }
                                    catch (Exception e)
                                    {
                                        resultSubject.OnNext(
                                            DiscriminatedUnion.Second<object, T, Exception>(e));
                                    }

                                    isCalculatingSubject.OnNext(false);
                                });
                        }));

                return d;
            });
        }

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
        public static ICalculatedProperty<T> CreateAsyncCalculatedProperty<TFirst, TSecond, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            IReadableObservableProperty<TSecond> secondProperty,
            TimeSpan throttleTime,
            Func<TFirst, TSecond, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<TFirst, TSecond, IDiscriminatedUnion<object, T, Exception>> calculate =
                (first, second) =>
                {
                    Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(calculateValue(first, second));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            // TODO: pick a better scheduler
            return new CalculatedProperty<T>((resultSubject, isCalculatingSubject) =>
            {
                CompositeDisposable d = new CompositeDisposable();
                IScheduler scheduler = Scheduler.Default;

                IDisposable scheduledTask = scheduler.ScheduleAsync(async (s, t) => resultSubject.OnNext(await Task.FromResult(calculate(firstProperty.Value, secondProperty.Value))));
                d.Add(scheduledTask);

                IObservable<Tuple<TFirst, TSecond>> o = firstProperty.CombineLatest(
                    secondProperty, Tuple.Create);
                o = throttleTime > TimeSpan.Zero ? o.Throttle(throttleTime, scheduler) : o.ObserveOn(scheduler);
                d.Add(
                    o.Subscribe(
                        v =>
                        {
                            using (scheduledTask)
                            {
                            }

                            isCalculatingSubject.OnNext(true);

                            scheduledTask = scheduler.ScheduleAsync(
                                async (s, t) =>
                                {
                                    try
                                    {
                                        resultSubject.OnNext(
                                            await Task.FromResult(calculate(v.Item1, v.Item2)));
                                    }
                                    catch (Exception e)
                                    {
                                        resultSubject.OnNext(
                                            DiscriminatedUnion.Second<object, T, Exception>(e));
                                    }

                                    isCalculatingSubject.OnNext(false);
                                });
                        }));

                return d;
            });
        }

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
        public static ICalculatedProperty<T> CreateAsyncCalculatedProperty<TFirst, TSecond, TThird, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            IReadableObservableProperty<TSecond> secondProperty,
            IReadableObservableProperty<TThird> thirdProperty,
            TimeSpan throttleTime,
            Func<TFirst, TSecond, TThird, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<TFirst, TSecond, TThird, IDiscriminatedUnion<object, T, Exception>> calculate =
                (first, second, third) =>
                {
                    Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(calculateValue(first, second, third));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            // TODO: pick a better scheduler
            return new CalculatedProperty<T>((resultSubject, isCalculatingSubject) =>
            {
                CompositeDisposable d = new CompositeDisposable();
                IScheduler scheduler = Scheduler.Default;

                IDisposable scheduledTask = scheduler.ScheduleAsync(async (s, t) => resultSubject.OnNext(await Task.FromResult(calculate(firstProperty.Value, secondProperty.Value, thirdProperty.Value))));
                d.Add(scheduledTask);

                IObservable<Tuple<TFirst, TSecond, TThird>> o = firstProperty.CombineLatest(
                    secondProperty, thirdProperty, Tuple.Create);
                o = throttleTime > TimeSpan.Zero ? o.Throttle(throttleTime, scheduler) : o.ObserveOn(scheduler);
                d.Add(
                    o.Subscribe(
                        v =>
                        {
                            using (scheduledTask)
                            {
                            }

                            isCalculatingSubject.OnNext(true);

                            scheduledTask = scheduler.ScheduleAsync(
                                async (s, t) =>
                                {
                                    try
                                    {
                                        resultSubject.OnNext(
                                            await Task.FromResult(calculate(v.Item1, v.Item2, v.Item3)));
                                    }
                                    catch (Exception e)
                                    {
                                        resultSubject.OnNext(
                                            DiscriminatedUnion.Second<object, T, Exception>(e));
                                    }

                                    isCalculatingSubject.OnNext(false);
                                });
                        }));

                return d;
            });
        }

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
        public static ICalculatedProperty<T> CreateAsyncCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            IReadableObservableProperty<TSecond> secondProperty,
            IReadableObservableProperty<TThird> thirdProperty,
            IReadableObservableProperty<TFourth> fourthProperty,
            TimeSpan throttleTime,
            Func<TFirst, TSecond, TThird, TFourth, T> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(fourthProperty != null, "fourthProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<TFirst, TSecond, TThird, TFourth, IDiscriminatedUnion<object, T, Exception>> calculate =
                (first, second, third, fourth) =>
                {
                    Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(calculateValue(first, second, third, fourth));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            // TODO: pick a better scheduler
            return new CalculatedProperty<T>((resultSubject, isCalculatingSubject) =>
                {
                    CompositeDisposable d = new CompositeDisposable();
                    IScheduler scheduler = Scheduler.Default;

                    IDisposable scheduledTask = scheduler.ScheduleAsync(async (s, t) => resultSubject.OnNext(await Task.FromResult(calculate(firstProperty.Value, secondProperty.Value, thirdProperty.Value, fourthProperty.Value))));
                    d.Add(scheduledTask);

                    IObservable<Tuple<TFirst, TSecond, TThird, TFourth>> o = firstProperty.CombineLatest(
                        secondProperty, thirdProperty, fourthProperty, Tuple.Create);
                    o = throttleTime > TimeSpan.Zero ? o.Throttle(throttleTime, scheduler) : o.ObserveOn(scheduler);
                    d.Add(
                        o.Subscribe(
                            v =>
                            {
                                using (scheduledTask)
                                {
                                }

                                isCalculatingSubject.OnNext(true);

                                scheduledTask = scheduler.ScheduleAsync(
                                    async (s, t) =>
                                    {
                                        try
                                        {
                                            resultSubject.OnNext(
                                                await Task.FromResult(calculate(v.Item1, v.Item2, v.Item3, v.Item4)));
                                        }
                                        catch (Exception e)
                                        {
                                            resultSubject.OnNext(
                                                DiscriminatedUnion.Second<object, T, Exception>(e));
                                        }

                                        isCalculatingSubject.OnNext(false);
                                    });
                            }));

                    return d;
                });
        }

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
        /// <typeparam name="TFirst">
        /// The type of the first property involved in the calculation.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of the calculation result.
        /// </typeparam>
        /// <returns>
        /// The calculated property.
        /// </returns>
        public static ICalculatedProperty<T> CreateCancellableAsyncCalculatedProperty<TFirst, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<AsyncCalculationHelper, TFirst, Task<IDiscriminatedUnion<object, T, Exception>>> calculate =
                async (helper, first) =>
                {
                    Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(await calculateValue(helper, first));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            // TODO: pick a better scheduler
            return new CalculatedProperty<T>((resultSubject, isCalculatingSubject) =>
            {
                CompositeDisposable d = new CompositeDisposable();
                IScheduler scheduler = Scheduler.Default;

                IDisposable scheduledTask = scheduler.ScheduleAsync(
                    async (s, t) =>
                    {
                        await s.Yield();
                        resultSubject.OnNext(await calculate(new AsyncCalculationHelper(s, t), firstProperty.Value));
                    });
                d.Add(scheduledTask);

                IObservable<TFirst> o = throttleTime > TimeSpan.Zero ? firstProperty.Throttle(throttleTime, scheduler) : firstProperty.ObserveOn(scheduler);
                d.Add(
                    o.Subscribe(
                        v =>
                        {
                            using (scheduledTask)
                            {
                            }

                            isCalculatingSubject.OnNext(true);

                            scheduledTask = scheduler.ScheduleAsync(
                                async (s, t) =>
                                {
                                    try
                                    {
                                        await s.Yield();
                                        resultSubject.OnNext(await calculate(new AsyncCalculationHelper(s, t), v));
                                    }
                                    catch (Exception e)
                                    {
                                        resultSubject.OnNext(
                                            DiscriminatedUnion.Second<object, T, Exception>(e));
                                    }

                                    isCalculatingSubject.OnNext(false);
                                });
                        }));

                return d;
            });
        }

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
        public static ICalculatedProperty<T> CreateCancellableAsyncCalculatedProperty<TFirst, TSecond, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            IReadableObservableProperty<TSecond> secondProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, TSecond, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<AsyncCalculationHelper, TFirst, TSecond, Task<IDiscriminatedUnion<object, T, Exception>>> calculate =
                async (helper, first, second) =>
                {
                    Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(await calculateValue(helper, first, second));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            // TODO: pick a better scheduler
            return new CalculatedProperty<T>((resultSubject, isCalculatingSubject) =>
            {
                CompositeDisposable d = new CompositeDisposable();
                IScheduler scheduler = Scheduler.Default;

                IDisposable scheduledTask = scheduler.ScheduleAsync(
                    async (s, t) =>
                    {
                        await s.Yield();
                        resultSubject.OnNext(await calculate(new AsyncCalculationHelper(s, t), firstProperty.Value, secondProperty.Value));
                    });
                d.Add(scheduledTask);

                IObservable<Tuple<TFirst, TSecond>> o = firstProperty.CombineLatest(secondProperty, Tuple.Create);
                o = throttleTime > TimeSpan.Zero ? o.Throttle(throttleTime, scheduler) : o.ObserveOn(scheduler);
                d.Add(
                    o.Subscribe(
                        v =>
                        {
                            using (scheduledTask)
                            {
                            }

                            isCalculatingSubject.OnNext(true);

                            scheduledTask = scheduler.ScheduleAsync(
                                async (s, t) =>
                                {
                                    try
                                    {
                                        await s.Yield();
                                        resultSubject.OnNext(await calculate(new AsyncCalculationHelper(s, t), v.Item1, v.Item2));
                                    }
                                    catch (Exception e)
                                    {
                                        resultSubject.OnNext(
                                            DiscriminatedUnion.Second<object, T, Exception>(e));
                                    }

                                    isCalculatingSubject.OnNext(false);
                                });
                        }));

                return d;
            });
        }

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
        public static ICalculatedProperty<T> CreateCancellableAsyncCalculatedProperty<TFirst, TSecond, TThird, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            IReadableObservableProperty<TSecond> secondProperty,
            IReadableObservableProperty<TThird> thirdProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, TSecond, TThird, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<AsyncCalculationHelper, TFirst, TSecond, TThird, Task<IDiscriminatedUnion<object, T, Exception>>> calculate =
                async (helper, first, second, third) =>
                {
                    Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(await calculateValue(helper, first, second, third));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            // TODO: pick a better scheduler
            return new CalculatedProperty<T>((resultSubject, isCalculatingSubject) =>
            {
                CompositeDisposable d = new CompositeDisposable();
                IScheduler scheduler = Scheduler.Default;

                IDisposable scheduledTask = scheduler.ScheduleAsync(
                    async (s, t) =>
                    {
                        await s.Yield();
                        resultSubject.OnNext(await calculate(new AsyncCalculationHelper(s, t), firstProperty.Value, secondProperty.Value, thirdProperty.Value));
                    });
                d.Add(scheduledTask);

                IObservable<Tuple<TFirst, TSecond, TThird>> o = firstProperty.CombineLatest(
                    secondProperty, thirdProperty, Tuple.Create);
                o = throttleTime > TimeSpan.Zero ? o.Throttle(throttleTime, scheduler) : o.ObserveOn(scheduler);
                d.Add(
                    o.Subscribe(
                        v =>
                        {
                            using (scheduledTask)
                            {
                            }

                            isCalculatingSubject.OnNext(true);

                            scheduledTask = scheduler.ScheduleAsync(
                                async (s, t) =>
                                {
                                    try
                                    {
                                        await s.Yield();
                                        resultSubject.OnNext(await calculate(new AsyncCalculationHelper(s, t), v.Item1, v.Item2, v.Item3));
                                    }
                                    catch (Exception e)
                                    {
                                        resultSubject.OnNext(
                                            DiscriminatedUnion.Second<object, T, Exception>(e));
                                    }

                                    isCalculatingSubject.OnNext(false);
                                });
                        }));

                return d;
            });
        }

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
        public static ICalculatedProperty<T> CreateCancellableAsyncCalculatedProperty<TFirst, TSecond, TThird, TFourth, T>(
            IReadableObservableProperty<TFirst> firstProperty,
            IReadableObservableProperty<TSecond> secondProperty,
            IReadableObservableProperty<TThird> thirdProperty,
            IReadableObservableProperty<TFourth> fourthProperty,
            TimeSpan throttleTime,
            Func<AsyncCalculationHelper, TFirst, TSecond, TThird, TFourth, Task<T>> calculateValue)
        {
            Contract.Requires<ArgumentNullException>(firstProperty != null, "firstProperty");
            Contract.Requires<ArgumentNullException>(secondProperty != null, "secondProperty");
            Contract.Requires<ArgumentNullException>(thirdProperty != null, "thirdProperty");
            Contract.Requires<ArgumentNullException>(fourthProperty != null, "fourthProperty");
            Contract.Requires<ArgumentNullException>(calculateValue != null, "calculateValue");
            Contract.Ensures(Contract.Result<ICalculatedProperty<T>>() != null);

            Func<AsyncCalculationHelper, TFirst, TSecond, TThird, TFourth, Task<IDiscriminatedUnion<object, T, Exception>>> calculate =
                async (helper, first, second, third, fourth) =>
                {
                    Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                    IDiscriminatedUnion<object, T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion =
                            DiscriminatedUnion.First<object, T, Exception>(await calculateValue(helper, first, second, third, fourth));
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<object, T, Exception>(e);
                    }

                    return discriminatedUnion;
                };

            // TODO: pick a better scheduler
            return new CalculatedProperty<T>((resultSubject, isCalculatingSubject) =>
            {
                CompositeDisposable d = new CompositeDisposable();
                IScheduler scheduler = Scheduler.Default;

                IDisposable scheduledTask = scheduler.ScheduleAsync(
                    async (s, t) =>
                    {
                        await s.Yield();
                        resultSubject.OnNext(await calculate(new AsyncCalculationHelper(s, t), firstProperty.Value, secondProperty.Value, thirdProperty.Value, fourthProperty.Value));
                    });
                d.Add(scheduledTask);

                IObservable<Tuple<TFirst, TSecond, TThird, TFourth>> o = firstProperty.CombineLatest(
                    secondProperty, thirdProperty, fourthProperty, Tuple.Create);
                o = throttleTime > TimeSpan.Zero ? o.Throttle(throttleTime, scheduler) : o.ObserveOn(scheduler);
                d.Add(
                    o.Subscribe(
                        v =>
                        {
                            using (scheduledTask)
                            {
                            }

                            isCalculatingSubject.OnNext(true);

                            scheduledTask = scheduler.ScheduleAsync(
                                async (s, t) =>
                                {
                                    try
                                    {
                                        await s.Yield();
                                        resultSubject.OnNext(await calculate(new AsyncCalculationHelper(s, t), v.Item1, v.Item2, v.Item3, v.Item4));
                                    }
                                    catch (Exception e)
                                    {
                                        resultSubject.OnNext(
                                            DiscriminatedUnion.Second<object, T, Exception>(e));
                                    }

                                    isCalculatingSubject.OnNext(false);
                                });
                        }));

                return d;
            });
        }
    }
}