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
    using System.Reactive.Subjects;

    /// <summary>
    /// Provides <see langword="static"/> extension methods for <see cref="IObservable{T}"/>.
    /// </summary>
    public static partial class ObservableExtensionMethods
    {
        #region Public Methods and Operators

        /// <summary>
        /// Converts the observable into a behavior observable.  This method should only be called on
        /// observables which always produces a value on subscription and does not complete.
        /// </summary>
        /// <param name="o">The observable to convert.</param>
        /// <typeparam name="T">The type of the object that provides notification information.</typeparam>
        /// <returns>A behavior observable version of <paramref name="o"/>.</returns>
        /// <remarks>Behavior observables are meant to represent observables on time-varied values, such as <see cref="BehaviorSubject{T}"/>.
        /// This method should not be called on an observable which does not exhibit this behavior.</remarks>
        public static IBehaviorObservable<T> ToBehaviorObservable<T>(this IObservable<T> o)
        {
            return new BehaviorObservable<T>(o);
        }

        #endregion
    }
}