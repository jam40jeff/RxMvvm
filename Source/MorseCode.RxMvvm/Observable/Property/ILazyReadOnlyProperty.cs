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

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;

    /// <summary>
    /// Interface representing a read-only property whose value is lazily evaluated.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the property.
    /// </typeparam>
    [ContractClass(typeof(LazyReadOnlyPropertyContract<>))]
    public interface ILazyReadOnlyProperty<out T> : IReadableObservableProperty<IDiscriminatedUnion<object, T, Exception>>
    {
        /// <summary>
        /// Gets an observable which notifies when a successful calculation or an error (which returns <value>default(<typeparamref name="T"/>)</value>) results in a value change.
        /// </summary>
        IObservable<T> OnValueOrDefaultChanged { get; }

        /// <summary>
        /// Gets an observable which notifies on a successful calculation or an error (which returns <value>default(<typeparamref name="T"/>)</value>).
        /// </summary>
        IObservable<T> OnValueOrDefaultSet { get; }

        /// <summary>
        /// Gets an observable which notifies when a calculation error occurs.
        /// </summary>
        IObservable<Exception> OnCalculationException { get; }

        /// <summary>
        /// Gets an observable which notifies when the IsCalculated state changes.
        /// </summary>
        IObservable<bool> OnIsCalculatedChanged { get; }

        /// <summary>
        /// Gets an observable which notifies when the IsCalculating state changes.
        /// </summary>
        IObservable<bool> OnIsCalculatingChanged { get; }

        /// <summary>
        /// Gets a value indicating whether or not the value of the property has been calculated.
        /// </summary>
        bool IsCalculated { get; }

        /// <summary>
        /// Gets a value indicating whether or not the value of the property is currently being calculated.
        /// </summary>
        bool IsCalculating { get; }

        /// <summary>
        /// Gets the latest calculation exception.
        /// </summary>
        Exception CalculationException { get; }

        /// <summary>
        /// Gets the value or <value>default(<typeparamref name="T"/>)</value> if the latest calculation resulted in an error.
        /// </summary>
        new T Value { get; }

        /// <summary>
        /// Gets the value or exception if the latest calculation resulted in an error.
        /// </summary>
        IDiscriminatedUnion<object, T, Exception> ValueOrException { get; }

        /// <summary>
        /// Gets the latest successful value or throws an exception if the latest calculation resulted in an error.
        /// </summary>
        /// <returns>
        /// The latest successful value.
        /// </returns>
        /// <exception cref="Exception">
        /// The latest calculation exception.
        /// </exception>
        T GetSuccessfulValueOrThrowException();

        /// <summary>
        /// Forces the value of the property to be eagerly loaded.
        /// </summary>
        void EagerLoad();
    }
}