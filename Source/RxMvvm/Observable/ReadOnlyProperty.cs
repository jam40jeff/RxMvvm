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
    using System.Reactive.Linq;

    internal class ReadOnlyProperty<T> : IReadableObservableProperty<T>
    {
        private readonly T value;

        private readonly IObservable<T> observable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyProperty{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public ReadOnlyProperty(T value)
        {
            this.value = value;
            this.observable = Observable.Return(value);
        }

        /// <summary>
        /// Gets the on changed.
        /// </summary>
        public IObservable<T> OnChanged
        {
            get
            {
                return this.observable;
            }
        }

        /// <summary>
        /// Gets the on set.
        /// </summary>
        public IObservable<T> OnSet
        {
            get
            {
                return this.observable;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value
        {
            get
            {
                return this.value;
            }
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            return this.observable.Subscribe(observer);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
        }
    }
}