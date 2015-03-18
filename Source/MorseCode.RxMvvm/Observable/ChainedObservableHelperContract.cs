#region License

// Copyright 2015 MorseCode Software
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

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;

    [ContractClassFor(typeof(IChainedObservableHelper<>))]
    internal abstract class ChainedObservableHelperContract<T> : IChainedObservableHelper<T>
    {
        #region Explicit Interface Methods

        IChainedObservableHelper<TNew> IChainedObservableHelperBase<T>.Add<TNew>(Func<T, IObservable<TNew>> getObservable)
        {
            return null;
        }

        IObservable<IDiscriminatedUnion<object, TNew, NonComputable>> IChainedObservableHelperBase<T>.AddLeafAndCompleteWithoutEvaluation<TNew>(Func<T, TNew> getObservable)
        {
            return null;
        }

        IObservable<IDiscriminatedUnion<object, T, NonComputable>> IChainedObservableHelper<T>.Complete()
        {
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>() != null);

            return null;
        }

        IObservable<IDiscriminatedUnion<object, T, NonComputable>> IChainedObservableHelper<T>.Complete(bool notifyLeafOnly)
        {
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>() != null);

            return null;
        }

        IObservable<IDiscriminatedUnion<object, T, NonComputable>> IChainedObservableHelper<T>.Complete(bool notifyLeafOnly, bool provideAllNotifications)
        {
            Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>() != null);

            return null;
        }

        IObservable<T> IChainedObservableHelper<T>.CompleteWithDefaultIfNotComputable()
        {
            Contract.Ensures(Contract.Result<IObservable<T>>() != null);

            return null;
        }

        IObservable<T> IChainedObservableHelper<T>.CompleteWithDefaultIfNotComputable(bool notifyLeafOnly)
        {
            Contract.Ensures(Contract.Result<IObservable<T>>() != null);

            return null;
        }

        IObservable<T> IChainedObservableHelper<T>.CompleteWithDefaultIfNotComputable(bool notifyLeafOnly, bool provideAllNotifications)
        {
            Contract.Ensures(Contract.Result<IObservable<T>>() != null);

            return null;
        }

        #endregion
    }
}