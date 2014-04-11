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

namespace MorseCode.RxMvvm.Observable.Property.NotifyPropertyChanged
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Observable.Collection;

    internal class ReadOnlyNotifyPropertyChangedProperty<T> : IReadableNotifyPropertyChangedProperty<T>
    {
        private readonly IDisposable subscription;

        private T lastValue;

        internal ReadOnlyNotifyPropertyChangedProperty(IObservable<T> observable, IScheduler scheduler)
        {
            Contract.Requires<ArgumentNullException>(observable != null, "observable");
            Contract.Requires<ArgumentNullException>(scheduler != null, "scheduler");
            Contract.Ensures(this.subscription != null);

            this.subscription = observable.ObserveOn(scheduler).Subscribe(
                v =>
                    {
                        this.lastValue = v;
                        OnValueChanged();
                    });

            if (this.subscription == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IObservable<T>>.GetMemberInfo(
                        o => o.Subscribe(null)).Name + " cannot be null.");
            }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                this.PropertyChanged += value;
            }

            remove
            {
                this.PropertyChanged -= value;
            }
        }

        private event PropertyChangedEventHandler PropertyChanged;

        T IReadableNotifyPropertyChangedProperty<T>.Value
        {
            get
            {
                return this.Value;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        protected T Value
        {
            get
            {
                return this.lastValue;
            }
        }

        void IDisposable.Dispose()
        {
            this.subscription.Dispose();
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the <see cref="IReadableNotifyPropertyChangedProperty{T}.Value"/> property.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            this.OnPropertyChanged(
                new PropertyChangedEventArgs(ReadableNotifyPropertyChangedPropertyUtility.ValuePropertyName));
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.subscription != null);
        }
    }
}