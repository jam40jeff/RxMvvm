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

    using MorseCode.RxMvvm.Common;

    /// <summary>
    /// Interface representing a property that is automatically calculated when its dependencies change.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the property.
    /// </typeparam>
    public interface ICalculatedProperty<out T> : IReadableObservableProperty<IDiscriminatedUnion<T, Exception>>
    {
        /// <summary>
        /// Gets an observable which notifies when a successful calculation results in a value change.
        /// </summary>
        IObservable<T> OnSuccessfulValueChanged { get; }

        /// <summary>
        /// Gets the on successful value changed.
        /// </summary>
        IObservable<T> OnSuccessfulValueSet { get; }

        /// <summary>
        /// Gets an observable which notifies when a calculation error occurs.
        /// </summary>
        IObservable<Exception> OnCalculationException { get; }

        /// <summary>
        /// Gets the latest value from a successful calculation.
        /// </summary>
        T LatestSuccessfulValue { get; }

        /// <summary>
        /// Gets the latest calculation exception.
        /// </summary>
        Exception LatestCalculationException { get; }

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
    }
}