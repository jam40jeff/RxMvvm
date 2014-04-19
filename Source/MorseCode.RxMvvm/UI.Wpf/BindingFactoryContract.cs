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
    using System.Linq.Expressions;
    using System.Windows.Data;

    using MorseCode.RxMvvm.Observable.Property;

    [ContractClassFor(typeof(IBindingFactory<>))]
    internal abstract class BindingFactoryContract<T> : IBindingFactory<T>
    {
        Binding IBindingFactory<T>.CreateOneWayBinding<TProperty>(Expression<Func<T, IReadableObservableProperty<TProperty>>> getPropertyName)
        {
            Contract.Requires<ArgumentNullException>(getPropertyName != null, "getPropertyName");
            Contract.Ensures(Contract.Result<Binding>() != null);

            return null;
        }

        Binding IBindingFactory<T>.CreateOneWayToSourceBinding<TProperty>(Expression<Func<T, IWritableObservableProperty<TProperty>>> getPropertyName)
        {
            Contract.Requires<ArgumentNullException>(getPropertyName != null, "getPropertyName");
            Contract.Ensures(Contract.Result<Binding>() != null);

            return null;
        }

        Binding IBindingFactory<T>.CreateTwoWayBinding<TProperty>(Expression<Func<T, IObservableProperty<TProperty>>> getPropertyName)
        {
            Contract.Requires<ArgumentNullException>(getPropertyName != null, "getPropertyName");
            Contract.Ensures(Contract.Result<Binding>() != null);

            return null;
        }

        Binding IBindingFactory<T>.CreateCalculatedBinding<TProperty>(Expression<Func<T, ICalculatedProperty<TProperty>>> getPropertyName)
        {
            Contract.Requires<ArgumentNullException>(getPropertyName != null, "getPropertyName");
            Contract.Ensures(Contract.Result<Binding>() != null);

            return null;
        }
    }
}