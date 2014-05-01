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

namespace MorseCode.RxMvvm.ViewModel
{
    using System;

    using MorseCode.RxMvvm.Observable.Property;

    /// <summary>
    /// An interface representing an application view model.
    /// </summary>
    public interface IApplicationViewModel
    {
        /// <summary>
        /// Gets the current view model.
        /// </summary>
        IReadableObservableProperty<object> CurrentViewModel { get; }

        /// <summary>
        /// Gets the on error observable.
        /// </summary>
        IObservable<Exception> OnUnhandledError { get; }

        /// <summary>
        /// Initializes the application view model.
        /// </summary>
        void Initialize();
    }
}