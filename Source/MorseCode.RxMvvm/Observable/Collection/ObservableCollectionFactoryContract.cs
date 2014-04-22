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

namespace MorseCode.RxMvvm.Observable.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IObservableCollectionFactory))]
    internal abstract class ObservableCollectionFactoryContract : IObservableCollectionFactory
    {
        IObservableCollection<T> IObservableCollectionFactory.CreateObservableCollection<T>()
        {
            Contract.Ensures(Contract.Result<IObservableCollection<T>>() != null);

            return null;
        }

        IObservableCollection<T> IObservableCollectionFactory.CreateObservableCollection<T>(IEnumerable<T> list)
        {
            Contract.Requires<ArgumentNullException>(list != null, "list");
            Contract.Ensures(Contract.Result<IObservableCollection<T>>() != null);

            return null;
        }

        IReadableObservableCollection<T> IObservableCollectionFactory.CreateReadOnlyObservableCollection<T>(
            IEnumerable<T> list)
        {
            Contract.Requires<ArgumentNullException>(list != null, "list");
            Contract.Ensures(Contract.Result<IReadableObservableCollection<T>>() != null);

            return null;
        }
    }
}