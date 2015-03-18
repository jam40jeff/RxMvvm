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

    [ContractClassFor(typeof(IChainedObservableInitialHelper<>))]
    internal abstract class ChainedObservableInitialHelperContract<T> : IChainedObservableInitialHelper<T>
    {
        #region Public Methods and Operators

        public IChainedObservableHelper<TNew> Add<TNew>(Func<T, IObservable<TNew>> getObservable)
        {
            return null;
        }

        public IObservable<IDiscriminatedUnion<object, TNew, NonComputable>> AddLeafAndCompleteWithoutEvaluation<TNew>(Func<T, TNew> getObservable) where TNew : class
        {
            return null;
        }

        #endregion
    }
}