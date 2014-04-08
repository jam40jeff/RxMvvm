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
    using System.ComponentModel;

    /// <summary>
    /// Interface representing a readable property.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the property.
    /// </typeparam>
    public interface IReadableObservableProperty<out T> : IObservable<T>, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Gets an observable which notifies when a value change occurs.
        /// </summary>
        IObservable<T> OnChanged { get; }

        /// <summary>
        /// Gets an observable which notifies when a value set occurs.
        /// </summary>
        IObservable<T> OnSet { get; }

        /// <summary>
        /// Gets the latest value.
        /// </summary>
        T Value { get; }
    }
}