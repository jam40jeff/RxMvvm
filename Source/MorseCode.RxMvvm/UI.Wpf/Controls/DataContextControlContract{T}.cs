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

namespace MorseCode.RxMvvm.UI.Wpf.Controls
{
    using System;
    using System.Diagnostics.Contracts;

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Observable;

    [ContractClassFor(typeof(IDataContextControl<>))]
    internal abstract class DataContextControlContract<T> : IDataContextControl<T>
    {
        void IDisposable.Dispose()
        {
        }

        void IDataContextControl<T>.BindChainedDataContext<TDataContext>(
            IObservable<TDataContext> dataContext, 
            Func<TDataContext, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> getDataContext)
        {
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getDataContext != null, "getDataContext");
        }

        void IDataContextControl<T>.BindDataContext<TDataContext>(
            IObservable<TDataContext> dataContext, Func<TDataContext, IObservable<T>> getDataContext)
        {
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getDataContext != null, "getDataContext");
        }
    }
}