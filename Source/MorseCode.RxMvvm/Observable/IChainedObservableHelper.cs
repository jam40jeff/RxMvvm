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
    [ContractClass(typeof(ChainedObservableHelperContract<>))]
    public interface IChainedObservableHelper<out T> : IChainedObservableHelperBase<T>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Completes the observable chain and returns an observable which will produce the latest value.
        /// </summary>
        /// <returns>
        /// The chained <see cref="IObservable{TObservableDiscriminatedUnion}"/> (where <code>TObservableDiscriminatedUnion</code> is <see cref="IDiscriminatedUnion{TDiscriminatedUnion1,TDiscriminatedUnion2,TDiscriminatedUnion3}"/> where
        /// <code>TDiscriminatedUnion1</code> is <see cref="Object"/> and <code>TDiscriminatedUnion2</code> is <typeparamref name="T"/> and <code>TDiscriminatedUnion3</code> is
        /// <see cref="NonComputable"/>).
        /// </returns>
        /// <remarks>
        /// This overload is equivalent to calling <see cref="Complete(bool)"/> passing <value>false</value> such that the observable chain will be notified on intermediate property changes.
        /// </remarks>
        IObservable<IDiscriminatedUnion<object, T, NonComputable>> Complete();

        /// <summary>
        /// Completes the observable chain and returns an observable which will produce the latest value.
        /// </summary>
        /// <param name="notifyLeafOnly">
        /// If <value>true</value>, the chained observable will only be notified when the last property changes, otherwise it will be notified when intermediate properties change as well.
        /// </param>
        /// <returns>
        /// The chained <see cref="IObservable{TObservableDiscriminatedUnion}"/> (where <code>TObservableDiscriminatedUnion</code> is
        /// <see cref="IDiscriminatedUnion{TDiscriminatedUnion1,TDiscriminatedUnion2,TDiscriminatedUnion3}"/> where <code>TDiscriminatedUnion1</code> is
        /// <see cref="Object"/> and <code>TDiscriminatedUnion2</code> is <typeparamref name="T"/> and <code>TDiscriminatedUnion3</code> is <see cref="NonComputable"/>).
        /// </returns>
        /// <remarks>
        /// This overload is equivalent to calling <see cref="Complete(bool,bool)"/> passing <paramref name="notifyLeafOnly"/> and <value>false</value> such that the
        /// observable chain will send through all notifications rather than just changes.
        /// </remarks>
        IObservable<IDiscriminatedUnion<object, T, NonComputable>> Complete(bool notifyLeafOnly);

        /// <summary>
        /// Completes the observable chain and returns an observable which will produce the latest value.
        /// </summary>
        /// <param name="notifyLeafOnly">
        /// If <value>true</value>, the chained observable will only be notified when the last property changes, otherwise it will be notified when intermediate properties
        /// change as well.
        /// </param>
        /// <param name="provideAllNotifications">
        /// If <value>true</value>, the observable chain will send through all notifications, otherwise it will send through only changes.
        /// </param>
        /// <returns>
        /// The chained <see cref="IObservable{TObservableDiscriminatedUnion}"/> (where <code>TObservableDiscriminatedUnion</code> is
        /// <see cref="IDiscriminatedUnion{TDiscriminatedUnion1,TDiscriminatedUnion2,TDiscriminatedUnion3}"/> where <code>TDiscriminatedUnion1</code> is
        /// <see cref="Object"/> and <code>TDiscriminatedUnion2</code> is <typeparamref name="T"/> and <code>TDiscriminatedUnion3</code> is <see cref="NonComputable"/>.
        /// </returns>
        IObservable<IDiscriminatedUnion<object, T, NonComputable>> Complete(bool notifyLeafOnly, bool provideAllNotifications);

        /// <summary>
        /// Completes the observable chain and returns an observable which will produce the latest value or
        /// <value>
        /// default(T)
        /// </value>
        /// if the chain is not computable.
        /// </summary>
        /// <returns>
        /// The chained <see cref="IObservable{T}"/> which will produce <value>default(T)</value> if the chain is not computable.
        /// </returns>
        /// <remarks>
        /// This overload is equivalent to calling <see cref="Complete(bool)"/> passing <value>false</value> such that the observable chain will be notified on intermediate property changes.
        /// </remarks>
        IObservable<T> CompleteWithDefaultIfNotComputable();

        /// <summary>
        /// Completes the observable chain and returns an observable which will produce the latest value or <value>default(T)</value> if the chain is not computable.
        /// </summary>
        /// <param name="notifyLeafOnly">
        /// If <value>true</value>, the chained observable will only be notified when the last property changes, otherwise it will be notified when intermediate properties
        /// change as well.
        /// </param>
        /// <returns>
        /// The chained <see cref="IObservable{T}"/> which will produce <value>default(T)</value> if the chain is not computable.
        /// </returns>
        /// <remarks>
        /// This overload is equivalent to calling <see cref="Complete(bool,bool)"/> passing <paramref name="notifyLeafOnly"/> and <value>false</value> such that the
        /// observable chain will send through all notifications rather than just changes.
        /// </remarks>
        IObservable<T> CompleteWithDefaultIfNotComputable(bool notifyLeafOnly);

        /// <summary>
        /// Completes the observable chain and returns an observable which will produce the latest value or <value>default(T)</value> if the chain is not computable.
        /// </summary>
        /// <param name="notifyLeafOnly">
        /// If <value>true</value>, the chained observable will only be notified when the last property changes, otherwise it will be notified when intermediate properties
        /// change as well.
        /// </param>
        /// <param name="provideAllNotifications">
        /// If <value>true</value>, the observable chain will send through all notifications, otherwise it will send through only changes.
        /// </param>
        /// <returns>
        /// The chained <see cref="IObservable{T}"/> which will produce <value>default(T)</value> if the chain is not computable.
        /// </returns>
        IObservable<T> CompleteWithDefaultIfNotComputable(bool notifyLeafOnly, bool provideAllNotifications);

        #endregion
    }
}