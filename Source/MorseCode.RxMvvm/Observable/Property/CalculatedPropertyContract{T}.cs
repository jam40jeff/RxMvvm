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

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;

    [ContractClassFor(typeof(ICalculatedProperty<>))]
    internal abstract class CalculatedPropertyContract<T> : ICalculatedProperty<T>
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

        IObservable<IDiscriminatedUnion<object, T, Exception>> IReadableObservableProperty<IDiscriminatedUnion<object, T, Exception>>.OnChanged
        {
            get
            {
                return null;
            }
        }

        IObservable<IDiscriminatedUnion<object, T, Exception>> IReadableObservableProperty<IDiscriminatedUnion<object, T, Exception>>.OnSet
        {
            get
            {
                return null;
            }
        }

        IDiscriminatedUnion<object, T, Exception> IReadableObservableProperty<IDiscriminatedUnion<object, T, Exception>>.Value
        {
            get
            {
                return null;
            }
        }

        IObservable<T> ICalculatedProperty<T>.OnSuccessfulValueChanged
        {
            get
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return null;
            }
        }

        IObservable<T> ICalculatedProperty<T>.OnSuccessfulValueSet
        {
            get
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return null;
            }
        }

        IObservable<Exception> ICalculatedProperty<T>.OnCalculationException
        {
            get
            {
                Contract.Ensures(Contract.Result<IObservable<Exception>>() != null);

                return null;
            }
        }

        T ICalculatedProperty<T>.LatestSuccessfulValue
        {
            get
            {
                return default(T);
            }
        }

        Exception ICalculatedProperty<T>.LatestCalculationException
        {
            get
            {
                return null;
            }
        }

        IDisposable IObservable<IDiscriminatedUnion<object, T, Exception>>.Subscribe(
            IObserver<IDiscriminatedUnion<object, T, Exception>> observer)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return null;

            // ReSharper restore AssignNullToNotNullAttribute
        }

        void IDisposable.Dispose()
        {
        }

        T ICalculatedProperty<T>.GetSuccessfulValueOrThrowException()
        {
            return default(T);
        }
    }
}