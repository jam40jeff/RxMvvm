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

    /// <summary>
    /// An interface providing methods for building chained observables.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value of the last observable in the chain.
    /// </typeparam>
    [ContractClass(typeof(ChainedObservableHelperBaseContract<>))]
    public interface IChainedObservableHelperBase<out T>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Adds an observable to a chained observable.
        /// </summary>
        /// <param name="getObservable">
        /// A function to get the next observable in the chain from the last observable.
        /// </param>
        /// <typeparam name="TNew">
        /// The type of the value of the observable to add.
        /// </typeparam>
        /// <returns>
        /// A <see cref="IChainedObservableHelper{TNew}"/> for building the chained observable.
        /// </returns>
        IChainedObservableHelper<TNew> Add<TNew>(Func<T, IObservable<TNew>> getObservable);

        /// <summary>
        /// Adds an observable to a chained observable, completes the observable chain, and returns an observable which will produce the latest leaf observable.
        /// </summary>
        /// <param name="getObservable">
        /// A function to get the next observable in the chain from the last observable.
        /// </param>
        /// <typeparam name="TNew">
        /// The type of the observable to add.
        /// </typeparam>
        /// <returns>
        /// The chained <see cref="IObservable{TObservableDiscriminatedUnion}"/> (where 
        /// <code>
        /// TObservableDiscriminatedUnion
        /// </code>
        /// is <see cref="IDiscriminatedUnion{TDiscriminatedUnion1,TDiscriminatedUnion2,TDiscriminatedUnion3}"/> where
        /// <code>
        /// TDiscriminatedUnion1
        /// </code>
        /// is <see cref="Object"/> and 
        /// <code>
        /// TDiscriminatedUnion2
        /// </code>
        /// is <typeparamref name="TNew"/> and 
        /// <code>
        /// TDiscriminatedUnion3
        /// </code>
        /// is <see cref="NonComputable"/>.
        /// </returns>
        IObservable<IDiscriminatedUnion<object, TNew, NonComputable>> AddLeafAndCompleteWithoutEvaluation<TNew>(Func<T, TNew> getObservable) where TNew : class;

        #endregion
    }
}