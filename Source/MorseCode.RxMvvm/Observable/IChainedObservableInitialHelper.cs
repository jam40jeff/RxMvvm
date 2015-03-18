﻿#region License

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
    using System.Diagnostics.Contracts;

    /// <summary>
    /// An interface providing methods for building chained observables.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value of the last observable in the chain.
    /// </typeparam>
    [ContractClass(typeof(ChainedObservableInitialHelperContract<>))]
    public interface IChainedObservableInitialHelper<out T> : IChainedObservableHelperBase<T>
    {
    }
}