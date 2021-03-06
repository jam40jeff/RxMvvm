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
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IObservableProperty<>))]
    internal abstract class ObservablePropertyContract<T> : IObservableProperty<T>
    {
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        IObservable<T> IReadableObservableProperty<T>.OnChanged
        {
            get
            {
                return null;
            }
        }

        IObservable<T> IReadableObservableProperty<T>.OnSet
        {
            get
            {
                return null;
            }
        }

        T IObservableProperty<T>.Value
        {
            get
            {
                return default(T);
            }

            set
            {
            }
        }

        T IReadableObservableProperty<T>.Value
        {
            get
            {
                return default(T);
            }
        }

        T IWritableObservableProperty<T>.Value
        {
            set
            {
            }
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return null;

            // ReSharper restore AssignNullToNotNullAttribute
        }

        void IDisposable.Dispose()
        {
        }
    }
}