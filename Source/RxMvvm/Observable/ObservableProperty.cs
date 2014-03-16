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

namespace MorseCode.RxMvvm.Observable
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    using MorseCode.RxMvvm.Common;

    internal class ObservableProperty<T> : IObservableProperty<T>
    {
        private readonly BehaviorSubject<T> behaviorSubject;

        private readonly IObservable<T> allNotificationsObservable;

        private readonly IObservable<T> changeObservable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableProperty{T}"/> class.
        /// </summary>
        /// <param name="initialValue">
        /// The initial value.
        /// </param>
        public ObservableProperty(T initialValue)
        {
            Contract.Ensures(this.behaviorSubject != null);
            Contract.Ensures(this.changeObservable != null);
            Contract.Ensures(this.allNotificationsObservable != null);

            this.behaviorSubject = new BehaviorSubject<T>(initialValue);

            this.changeObservable = this.behaviorSubject.DistinctUntilChanged();
            if (this.changeObservable == null)
            {
                throw new InvalidOperationException(
                    StaticReflection.GetInScopeMemberInfo(() => this.changeObservable).Name + " may not be null.");
            }

            this.allNotificationsObservable = this.behaviorSubject.AsObservable();
            if (this.allNotificationsObservable == null)
            {
                throw new InvalidOperationException(
                    StaticReflection.GetInScopeMemberInfo(() => this.allNotificationsObservable).Name + " may not be null.");
            }
        }

        /// <summary>
        /// Gets the on changed.
        /// </summary>
        public IObservable<T> OnChanged
        {
            get
            {
                return this.changeObservable;
            }
        }

        /// <summary>
        /// Gets the on set.
        /// </summary>
        public IObservable<T> OnSet
        {
            get
            {
                return this.allNotificationsObservable;
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public T Value
        {
            get
            {
                return this.behaviorSubject.FirstAsync().Wait();
            }

            set
            {
                this.behaviorSubject.OnNext(value);
            }
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            return this.changeObservable.Subscribe(observer);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.behaviorSubject.Dispose();
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.behaviorSubject != null);
            Contract.Invariant(this.changeObservable != null);
            Contract.Invariant(this.allNotificationsObservable != null);
        }
    }
}