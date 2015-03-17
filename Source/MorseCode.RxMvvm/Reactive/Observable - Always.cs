#region License

// Copyright 2015 MorseCode Software
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
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    /// <summary>
    /// Provides <see langword="static"/> methods for <see cref="IObservable{T}"/>.
    /// </summary>
    public static partial class ObservableRxMvvm
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns an observable which will return a single value for each subscription, providing the equivalent of a constant time-based value.
        /// </summary>
        /// <param name="value">The value to return.</param>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>An observable which will return <paramref name="value"/> for each subscription.</returns>
        public static IObservable<T> Always<T>(T value)
        {
            return Observable.Create<T>(o =>
                {
                    o.OnNext(value);
                    return Disposable.Empty;
                });
        }

        #endregion
    }
}