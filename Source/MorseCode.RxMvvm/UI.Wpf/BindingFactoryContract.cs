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

namespace MorseCode.RxMvvm.UI.Wpf
{
    using System;
    using System.Diagnostics.Contracts;

    using MorseCode.RxMvvm.Observable.Property;

    [ContractClassFor(typeof(IBindingFactory<>))]
    internal abstract class BindingFactoryContract<T> : IBindingFactory<T>
        where T : class
    {
        IBinding IBindingFactory<T>.CreateOneWayBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IObservable<TProperty>> getDataContextValue, 
            Action<TProperty> setControlValue)
        {
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getDataContextValue != null, "getDataContextValue");
            Contract.Requires<ArgumentNullException>(setControlValue != null, "setControlValue");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            return null;
        }

        IBinding IBindingFactory<T>.CreateOneWayToSourceBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IWritableObservableProperty<TProperty>> getDataContextProperty, 
            Func<IBinding, IObservable<object>> createUiObservable, 
            Func<TProperty> getControlValue)
        {
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getDataContextProperty != null, "getDataContextProperty");
            Contract.Requires<ArgumentNullException>(createUiObservable != null, "createUiObservable");
            Contract.Requires<ArgumentNullException>(getControlValue != null, "getControlValue");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            return null;
        }

        IBinding IBindingFactory<T>.CreateTwoWayBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IObservableProperty<TProperty>> getDataContextProperty, 
            Func<IBinding, IObservable<object>> createUiObservable, 
            Action<TProperty> setControlValue, 
            Func<TProperty> getControlValue)
        {
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getDataContextProperty != null, "getDataContextProperty");
            Contract.Requires<ArgumentNullException>(createUiObservable != null, "createUiObservable");
            Contract.Requires<ArgumentNullException>(setControlValue != null, "setControlValue");
            Contract.Requires<ArgumentNullException>(getControlValue != null, "getControlValue");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            return null;
        }
    }
}