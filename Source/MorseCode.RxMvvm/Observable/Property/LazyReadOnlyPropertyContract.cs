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

    [ContractClassFor(typeof(ILazyReadOnlyProperty<>))]
    internal abstract class LazyReadOnlyPropertyContract<T> : ILazyReadOnlyProperty<T>
    {
        #region Explicit Interface Events

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        #endregion

        #region Explicit Interface Properties

        Exception ILazyReadOnlyProperty<T>.CalculationException
        {
            get
            {
                return null;
            }
        }

        bool ILazyReadOnlyProperty<T>.IsCalculated
        {
            get
            {
                return false;
            }
        }

        bool ILazyReadOnlyProperty<T>.IsCalculating
        {
            get
            {
                return false;
            }
        }

        IObservable<Exception> ILazyReadOnlyProperty<T>.OnCalculationException
        {
            get
            {
                Contract.Ensures(Contract.Result<IObservable<Exception>>() != null);

                return null;
            }
        }

        IObservable<IDiscriminatedUnion<object, T, Exception>>
            IReadableObservableProperty<IDiscriminatedUnion<object, T, Exception>>.OnChanged
        {
            get
            {
                return null;
            }
        }

        IObservable<bool> ILazyReadOnlyProperty<T>.OnIsCalculatedChanged
        {
            get
            {
                Contract.Ensures(Contract.Result<IObservable<bool>>() != null);

                return null;
            }
        }

        IObservable<bool> ILazyReadOnlyProperty<T>.OnIsCalculatingChanged
        {
            get
            {
                Contract.Ensures(Contract.Result<IObservable<bool>>() != null);

                return null;
            }
        }

        IObservable<IDiscriminatedUnion<object, T, Exception>>
            IReadableObservableProperty<IDiscriminatedUnion<object, T, Exception>>.OnSet
        {
            get
            {
                return null;
            }
        }

        IObservable<T> ILazyReadOnlyProperty<T>.OnValueOrDefaultChanged
        {
            get
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return null;
            }
        }

        IObservable<T> ILazyReadOnlyProperty<T>.OnValueOrDefaultSet
        {
            get
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return null;
            }
        }

        T ILazyReadOnlyProperty<T>.Value
        {
            get
            {
                return default(T);
            }
        }

        IDiscriminatedUnion<object, T, Exception>
            IReadableObservableProperty<IDiscriminatedUnion<object, T, Exception>>.Value
        {
            get
            {
                return null;
            }
        }

        IDiscriminatedUnion<object, T, Exception> ILazyReadOnlyProperty<T>.ValueOrException
        {
            get
            {
                Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                return null;
            }
        }

        #endregion

        #region Explicit Interface Methods

        void IDisposable.Dispose()
        {
        }

        T ILazyReadOnlyProperty<T>.GetValueOrThrowException()
        {
            return default(T);
        }

        void ILazyReadOnlyProperty<T>.EagerLoad()
        {
        }

        IDisposable IObservable<IDiscriminatedUnion<object, T, Exception>>.Subscribe(
            IObserver<IDiscriminatedUnion<object, T, Exception>> observer)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return null;
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion
    }
}