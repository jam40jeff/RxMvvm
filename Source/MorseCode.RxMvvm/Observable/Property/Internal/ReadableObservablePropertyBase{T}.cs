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

namespace MorseCode.RxMvvm.Observable.Property.Internal
{
    using System;
    using System.ComponentModel;

    [Serializable]
    internal abstract class ReadableObservablePropertyBase<T> : IReadableObservableProperty<T>
    {
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

        IObservable<T> IReadableObservableProperty<T>.OnChanged
        {
            get
            {
                return this.OnChanged;
            }
        }

        IObservable<T> IReadableObservableProperty<T>.OnSet
        {
            get
            {
                return this.OnSet;
            }
        }

        T IReadableObservableProperty<T>.Value
        {
            get
            {
                return this.Value;
            }
        }

        public T Value
        {
            get
            {
                return this.GetValue();
            }
        }

        /// <summary>
        /// Gets the on changed observable.
        /// </summary>
        protected abstract IObservable<T> OnChanged { get; }

        /// <summary>
        /// Gets the on set observable.
        /// </summary>
        protected abstract IObservable<T> OnSet { get; }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            return this.OnChanged.Subscribe(observer);
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <returns>
        /// The value of the property.
        /// </returns>
        protected abstract T GetValue();

        /// <summary>
        /// Disposes of the property.
        /// </summary>
        protected virtual void Dispose()
        {
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
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the <see cref="IReadableObservableProperty{T}.Value"/> property.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            this.OnPropertyChanged(
                new PropertyChangedEventArgs(ReadableObservablePropertyUtility.ValuePropertyName));
        }
    }
}