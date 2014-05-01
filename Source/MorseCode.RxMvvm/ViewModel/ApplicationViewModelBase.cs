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
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    using MorseCode.RxMvvm.Observable.Property;

    /// <summary>
    /// A base class for an application view model.
    /// </summary>
    public abstract class ApplicationViewModelBase : IApplicationViewModel
    {
        protected readonly IObservableProperty<object> CurrentViewModelInternal =
            ObservablePropertyFactory.Instance.CreateProperty<object>(null);

        private readonly Subject<Exception> unhandledErrorSubject = new Subject<Exception>();

        private readonly IObservable<Exception> unhandledErrorObservable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationViewModelBase"/> class.
        /// </summary>
        protected ApplicationViewModelBase()
        {
            this.unhandledErrorObservable = this.unhandledErrorSubject.AsObservable();
        }

        IReadableObservableProperty<object> IApplicationViewModel.CurrentViewModel
        {
            get
            {
                return this.CurrentViewModelInternal;
            }
        }

        IObservable<Exception> IApplicationViewModel.OnUnhandledError
        {
            get
            {
                return this.unhandledErrorObservable;
            }
        }

        void IApplicationViewModel.Initialize()
        {
            this.Initialize();
        }

        /// <summary>
        /// Fires an error notification.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void FireUnhandledError(Exception e)
        {
            this.unhandledErrorSubject.OnNext(e);
        }

        /// <summary>
        /// Initializes the application view model.
        /// </summary>
        protected abstract void Initialize();
    }
}