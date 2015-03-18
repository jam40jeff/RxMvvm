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

namespace MorseCode.RxMvvm.Observable
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Common.StaticReflection;
    using MorseCode.RxMvvm.Reactive;

    /// <summary>
    /// A static class providing extension methods for creating chained observables.
    /// Chained observables automatically re-subscribe the rest of the chain when any of the intermediate property values change.
    /// </summary>
    public static class ChainedObservable
    {
        #region Public Methods and Operators

        /// <summary>
        /// Adds an observable to a chained observable flattening the previous value and propagates any previous exceptions.
        /// </summary>
        /// <param name="helper">
        /// The helper to which to add a new observable.
        /// </param>
        /// <param name="getObservable">
        /// A function to get the next observable in the chain from the last observable's non-exceptional value.
        /// </param>
        /// <typeparam name="T">
        /// The type of the non-exceptional value of the previous observable.
        /// </typeparam>
        /// <typeparam name="TNew">
        /// The type of the non-exceptional value of the observable to add.
        /// </typeparam>
        /// <returns>
        /// A <see cref="IChainedObservableHelper{TNewHelper}"/> (where 
        /// <code>
        /// TNewHelper
        /// </code>
        /// is
        /// <see cref="IDiscriminatedUnion{TDiscriminatedUnion1,TDiscriminatedUnion2,TDiscriminatedUnion3}"/> where 
        /// <code>
        /// TDiscriminatedUnion1
        /// </code>
        /// is
        /// <see cref="object"/> and 
        /// <code>
        /// TDiscriminatedUnion2
        /// </code>
        /// is <typeparamref name="TNew"/> and 
        /// <code>
        /// TDiscriminatedUnion3
        /// </code>
        /// is <see cref="Exception"/>) for
        /// building the chained observable.
        /// </returns>
        public static IChainedObservableHelperBase<IDiscriminatedUnion<object, TNew, Exception>> AddByFlatteningAndPropagatingException<T, TNew>(this IChainedObservableHelperBase<IDiscriminatedUnion<object, IDiscriminatedUnion<object, T, Exception>, Exception>> helper, Func<T, IObservable<TNew>> getObservable)
        {
            Contract.Requires<ArgumentNullException>(helper != null, "helper");
            Contract.Requires<ArgumentNullException>(getObservable != null, "getObservable");
            Contract.Ensures(Contract.Result<IChainedObservableHelperBase<IDiscriminatedUnion<object, TNew, Exception>>>() != null);

            return helper.Add(v => v.Flatten().Switch(x => getObservable(x).Select(DiscriminatedUnion.First<object, TNew, Exception>), e => Observable.Return(DiscriminatedUnion.Second<object, TNew, Exception>(e))));
        }

        /// <summary>
        /// Adds an observable to a chained observable and propagates any previous exceptions.
        /// </summary>
        /// <param name="helper">
        /// The helper to which to add a new observable.
        /// </param>
        /// <param name="getObservable">
        /// A function to get the next observable in the chain from the last observable's non-exceptional value.
        /// </param>
        /// <typeparam name="T">
        /// The type of the non-exceptional value of the previous observable.
        /// </typeparam>
        /// <typeparam name="TNew">
        /// The type of the non-exceptional value of the observable to add.
        /// </typeparam>
        /// <returns>
        /// A <see cref="IChainedObservableHelper{TNewHelper}"/> (where 
        /// <code>
        /// TNewHelper
        /// </code>
        /// is
        /// <see cref="IDiscriminatedUnion{TDiscriminatedUnion1,TDiscriminatedUnion2,TDiscriminatedUnion3}"/> where 
        /// <code>
        /// TDiscriminatedUnion1
        /// </code>
        /// is
        /// <see cref="object"/> and 
        /// <code>
        /// TDiscriminatedUnion2
        /// </code>
        /// is <typeparamref name="TNew"/> and 
        /// <code>
        /// TDiscriminatedUnion3
        /// </code>
        /// is <see cref="Exception"/>) for
        /// building the chained observable.
        /// </returns>
        public static IChainedObservableHelperBase<IDiscriminatedUnion<object, TNew, Exception>> AddByPropagatingException<T, TNew>(this IChainedObservableHelperBase<IDiscriminatedUnion<object, T, Exception>> helper, Func<T, IObservable<TNew>> getObservable)
        {
            Contract.Requires<ArgumentNullException>(helper != null, "helper");
            Contract.Requires<ArgumentNullException>(getObservable != null, "getObservable");
            Contract.Ensures(Contract.Result<IChainedObservableHelperBase<IDiscriminatedUnion<object, TNew, Exception>>>() != null);

            return helper.Add(v => v.Switch(x => getObservable(x).Select(DiscriminatedUnion.First<object, TNew, Exception>), e => Observable.Return(DiscriminatedUnion.Second<object, TNew, Exception>(e))));
        }

        /// <summary>
        /// Begins a chained observable.
        /// </summary>
        /// <param name="observable">
        /// The first observable in the chain.
        /// </param>
        /// <typeparam name="T">
        /// The type of the value of the first observable.
        /// </typeparam>
        /// <returns>
        /// A <see cref="IChainedObservableInitialHelper{T}"/> for building the chained observable.
        /// </returns>
        public static IChainedObservableInitialHelper<T> BeginChain<T>(this IObservable<T> observable)
        {
            Contract.Requires<ArgumentNullException>(observable != null, "observable");
            Contract.Ensures(Contract.Result<IChainedObservableInitialHelper<T>>() != null);

            return new ChainedObservableInitialHelper<T>(skipFirst => skipFirst ? observable.Skip(1) : observable, skipFirst => skipFirst ? observable.Select(DiscriminatedUnion.First<object, T, NonComputable>).Skip(1) : observable.Select(DiscriminatedUnion.First<object, T, NonComputable>));
        }

        #endregion

        internal class ChainedObservableHelper<T> : ChainedObservableHelperBase<T>, IChainedObservableHelper<T>
        {
            #region Constructors and Destructors

            internal ChainedObservableHelper(Func<bool, IObservable<T>> setupPreviousObservable, Func<bool, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> setupPreviousNonComputableObservable)
                : base(setupPreviousObservable, setupPreviousNonComputableObservable)
            {
                Contract.Requires<ArgumentNullException>(setupPreviousObservable != null, "setupPreviousObservable");
                Contract.Requires<ArgumentNullException>(setupPreviousNonComputableObservable != null, "setupPreviousNonComputableObservable");
            }

            #endregion

            #region Explicit Interface Methods

            IObservable<IDiscriminatedUnion<object, T, NonComputable>> IChainedObservableHelper<T>.Complete()
            {
                return this.CompleteInternal();
            }

            IObservable<IDiscriminatedUnion<object, T, NonComputable>> IChainedObservableHelper<T>.Complete(bool notifyLeafOnly)
            {
                return this.CompleteInternal(notifyLeafOnly);
            }

            IObservable<IDiscriminatedUnion<object, T, NonComputable>> IChainedObservableHelper<T>.Complete(bool notifyLeafOnly, bool provideAllNotifications)
            {
                return this.CompleteInternal(notifyLeafOnly, provideAllNotifications);
            }

            IObservable<T> IChainedObservableHelper<T>.CompleteWithDefaultIfNotComputable()
            {
                return this.CompleteWithDefaultIfNotComputableInternal();
            }

            IObservable<T> IChainedObservableHelper<T>.CompleteWithDefaultIfNotComputable(bool notifyLeafOnly)
            {
                return this.CompleteWithDefaultIfNotComputableInternal(notifyLeafOnly);
            }

            IObservable<T> IChainedObservableHelper<T>.CompleteWithDefaultIfNotComputable(bool notifyLeafOnly, bool provideAllNotifications)
            {
                return this.CompleteWithDefaultIfNotComputableInternal(notifyLeafOnly, provideAllNotifications);
            }

            #endregion
        }

        internal abstract class ChainedObservableHelperBase<T> : IChainedObservableHelperBase<T>
        {
            #region Fields

            private readonly Func<bool, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> setupPreviousNonComputableObservable;

            private readonly Func<bool, IObservable<T>> setupPreviousObservable;

            #endregion

            #region Constructors and Destructors

            internal ChainedObservableHelperBase(Func<bool, IObservable<T>> setupPreviousObservable, Func<bool, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> setupPreviousNonComputableObservable)
            {
                Contract.Requires<ArgumentNullException>(setupPreviousObservable != null, "setupPreviousObservable");
                Contract.Requires<ArgumentNullException>(setupPreviousNonComputableObservable != null, "setupPreviousNonComputableObservable");

                this.setupPreviousObservable = setupPreviousObservable;
                this.setupPreviousNonComputableObservable = setupPreviousNonComputableObservable;
            }

            #endregion

            #region Explicit Interface Methods

            IChainedObservableHelper<TNew> IChainedObservableHelperBase<T>.Add<TNew>(Func<T, IObservable<TNew>> getObservable)
            {
                return new ChainedObservableHelper<TNew>(
                    skipFirst => this.setupPreviousNonComputableObservable(false).Select(
                        v =>
                        {
                            IObservable<TNew> o = this.GetInnerObservable(getObservable, x => x, () => default(TNew))(v);
                            if (skipFirst)
                            {
                                o = o.Skip(1);
                            }

                            return o;
                        }).Switch(),
                    skipFirst => this.setupPreviousNonComputableObservable(false).Select(
                        v =>
                        {
                            IObservable<IDiscriminatedUnion<object, TNew, NonComputable>> o = this.GetInnerObservable(getObservable, DiscriminatedUnion.First<object, TNew, NonComputable>, () => DiscriminatedUnion.Second<object, TNew, NonComputable>(NonComputable.Value))(v);
                            if (skipFirst)
                            {
                                o = o.Skip(1);
                            }

                            return o;
                        }).Switch());
            }

            IObservable<IDiscriminatedUnion<object, TNew, NonComputable>> IChainedObservableHelperBase<T>.AddLeafAndCompleteWithoutEvaluation<TNew>(Func<T, TNew> getObservable)
            {
                IObservable<IDiscriminatedUnion<object, TNew, NonComputable>> observable = this.setupPreviousNonComputableObservable(false).Select(
                    o => o.Switch(
                        v =>
                        {
                            if (ReferenceEquals(v, null))
                            {
                                return DiscriminatedUnion.Second<object, TNew, NonComputable>(NonComputable.Value);
                            }

                            TNew o2 = getObservable(v);
                            if (o2 == null)
                            {
                                return DiscriminatedUnion.Second<object, TNew, NonComputable>(NonComputable.Value);
                            }

                            return DiscriminatedUnion.First<object, TNew, NonComputable>(o2);
                        },
                        v => DiscriminatedUnion.Second<object, TNew, NonComputable>(NonComputable.Value)));
                if (observable == null)
                {
                    throw new InvalidOperationException("Result of " + StaticReflection<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>.GetMethodInfo(o2 => o2.Select((Func<IDiscriminatedUnion<object, T, NonComputable>, IDiscriminatedUnion<object, TNew, NonComputable>>)null)).Name + " cannot be null.");
                }

                return observable;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Completes the observable chain and returns an observable which will produce the latest value.
            /// </summary>
            /// <returns>
            /// The chained <see cref="IObservable{TObservableDiscriminatedUnion}"/> (where <code>TObservableDiscriminatedUnion</code> is <see cref="IDiscriminatedUnion{TDiscriminatedUnion1,TDiscriminatedUnion2,TDiscriminatedUnion3}"/> where
            /// <code>TDiscriminatedUnion1</code> is <see cref="Object"/> and <code>TDiscriminatedUnion2</code> is <typeparamref name="T"/> and <code>TDiscriminatedUnion3</code> is
            /// <see cref="NonComputable"/>).
            /// </returns>
            /// <remarks>
            /// This overload is equivalent to calling <see cref="CompleteInternal(bool)"/> passing <value>false</value> such that the observable chain will be notified on intermediate property changes.
            /// </remarks>
            protected IObservable<IDiscriminatedUnion<object, T, NonComputable>> CompleteInternal()
            {
                Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>() != null);

                return this.CompleteInternal(false);
            }

            /// <summary>
            /// Completes the observable chain and returns an observable which will produce the latest value.
            /// </summary>
            /// <param name="notifyLeafOnly">
            /// If 
            /// <value>
            /// true
            /// </value>
            /// , the chained observable will only be notified when the last property changes, otherwise it will be notified when intermediate properties change as well.
            /// </param>
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
            /// is <typeparamref name="T"/> and 
            /// <code>
            /// TDiscriminatedUnion3
            /// </code>
            /// is
            /// </returns>
            /// <remarks>
            /// This overload is equivalent to calling <see cref="CompleteInternal(bool,bool)"/> passing <paramref name="notifyLeafOnly"/> and 
            /// <value>
            /// false
            /// </value>
            /// such that the observable chain will send through all
            /// notifications rather than just changes.
            /// </remarks>
            protected IObservable<IDiscriminatedUnion<object, T, NonComputable>> CompleteInternal(bool notifyLeafOnly)
            {
                Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>() != null);

                return this.CompleteInternal(notifyLeafOnly, false);
            }

            /// <summary>
            /// Completes the observable chain and returns an observable which will produce the latest value.
            /// </summary>
            /// <param name="notifyLeafOnly">
            /// If 
            /// <value>
            /// true
            /// </value>
            /// , the chained observable will only be notified when the last property changes, otherwise it will be notified when intermediate properties change as well.
            /// </param>
            /// <param name="provideAllNotifications">
            /// If 
            /// <value>
            /// true
            /// </value>
            /// , the observable chain will send through all notifications, otherwise it will send through only changes.
            /// </param>
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
            /// is <typeparamref name="T"/> and 
            /// <code>
            /// TDiscriminatedUnion3
            /// </code>
            /// is <see cref="NonComputable"/>.
            /// </returns>
            protected IObservable<IDiscriminatedUnion<object, T, NonComputable>> CompleteInternal(bool notifyLeafOnly, bool provideAllNotifications)
            {
                Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>() != null);

                IObservable<IDiscriminatedUnion<object, T, NonComputable>> o = this.setupPreviousNonComputableObservable(notifyLeafOnly);
                if (o == null)
                {
                    throw new InvalidOperationException("Result of " + StaticReflection.GetInScopeMemberInfo(() => this.setupPreviousNonComputableObservable).Name + " cannot be null.");
                }

                if (!provideAllNotifications)
                {
                    o = o.DistinctUntilChanged();
                    if (o == null)
                    {
                        throw new InvalidOperationException("Result of " + StaticReflection<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>.GetMethodInfo(o2 => o2.DistinctUntilChanged()).Name + " cannot be null.");
                    }
                }

                return o;
            }

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
            /// This overload is equivalent to calling <see cref="CompleteInternal(bool)"/> passing <value>false</value> such that the observable chain will be notified on intermediate property changes.
            /// </remarks>
            protected IObservable<T> CompleteWithDefaultIfNotComputableInternal()
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return this.CompleteWithDefaultIfNotComputableInternal(false);
            }

            /// <summary>
            /// Completes the observable chain and returns an observable which will produce the latest value or
            /// <value>
            /// default(T)
            /// </value>
            /// if the chain is not computable.
            /// </summary>
            /// <param name="notifyLeafOnly">
            /// If 
            /// <value>
            /// true
            /// </value>
            /// , the chained observable will only be notified when the last property changes, otherwise it will be notified when intermediate properties change as well.
            /// </param>
            /// <returns>
            /// The chained <see cref="IObservable{T}"/> which will produce 
            /// <value>
            /// default(T)
            /// </value>
            /// if the chain is not computable.
            /// </returns>
            /// <remarks>
            /// This overload is equivalent to calling <see cref="CompleteInternal(bool,bool)"/> passing <paramref name="notifyLeafOnly"/> and 
            /// <value>
            /// false
            /// </value>
            /// such that the observable chain will send through all
            /// notifications rather than just changes.
            /// </remarks>
            protected IObservable<T> CompleteWithDefaultIfNotComputableInternal(bool notifyLeafOnly)
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return this.CompleteWithDefaultIfNotComputableInternal(notifyLeafOnly, false);
            }

            /// <summary>
            /// Completes the observable chain and returns an observable which will produce the latest value or
            /// <value>
            /// default(T)
            /// </value>
            /// if the chain is not computable.
            /// </summary>
            /// <param name="notifyLeafOnly">
            /// If 
            /// <value>
            /// true
            /// </value>
            /// , the chained observable will only be notified when the last property changes, otherwise it will be notified when intermediate properties change as well.
            /// </param>
            /// <param name="provideAllNotifications">
            /// If 
            /// <value>
            /// true
            /// </value>
            /// , the observable chain will send through all notifications, otherwise it will send through only changes.
            /// </param>
            /// <returns>
            /// The chained <see cref="IObservable{T}"/> which will produce 
            /// <value>
            /// default(T)
            /// </value>
            /// if the chain is not computable.
            /// </returns>
            protected IObservable<T> CompleteWithDefaultIfNotComputableInternal(bool notifyLeafOnly, bool provideAllNotifications)
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                IObservable<T> o = this.setupPreviousObservable(notifyLeafOnly);
                if (o == null)
                {
                    throw new InvalidOperationException("Result of " + StaticReflection.GetInScopeMemberInfo(() => this.setupPreviousObservable).Name + " cannot be null.");
                }

                if (!provideAllNotifications)
                {
                    o = o.DistinctUntilChanged();
                    if (o == null)
                    {
                        throw new InvalidOperationException("Result of " + StaticReflection<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>.GetMethodInfo(o2 => o2.DistinctUntilChanged()).Name + " cannot be null.");
                    }
                }

                return o;
            }

            [ContractInvariantMethod]
            private void CodeContractsInvariants()
            {
                Contract.Invariant(this.setupPreviousObservable != null);
                Contract.Invariant(this.setupPreviousNonComputableObservable != null);
            }

            private Func<IDiscriminatedUnion<object, T, NonComputable>, IObservable<TInner>> GetInnerObservable<TInner, TNew>(Func<T, IObservable<TNew>> getObservable, Func<TNew, TInner> getInnerValue, Func<TInner> getNonComputableInnerValue)
            {
                Contract.Requires<ArgumentNullException>(getObservable != null, "getObservable");
                Contract.Requires<ArgumentNullException>(getInnerValue != null, "getInnerValue");
                Contract.Requires<ArgumentNullException>(getNonComputableInnerValue != null, "getNonComputableInnerValue");
                Contract.Ensures(Contract.Result<Func<IDiscriminatedUnion<object, T, NonComputable>, IObservable<TInner>>>() != null);

                return o => o.Switch(
                    v =>
                    {
                        if (ReferenceEquals(v, null))
                        {
                            return Observable.Return(getNonComputableInnerValue());
                        }

                        IObservable<TNew> o2 = getObservable(v);
                        return o2 == null ? Observable.Return(getNonComputableInnerValue()) : o2.Select(getInnerValue);
                    },
                    v => Observable.Return(getNonComputableInnerValue()));
            }

            #endregion
        }

        internal class ChainedObservableInitialHelper<T> : ChainedObservableHelperBase<T>, IChainedObservableInitialHelper<T>
        {
            #region Constructors and Destructors

            internal ChainedObservableInitialHelper(Func<bool, IObservable<T>> setupPreviousObservable, Func<bool, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> setupPreviousNonComputableObservable)
                : base(setupPreviousObservable, setupPreviousNonComputableObservable)
            {
                Contract.Requires<ArgumentNullException>(setupPreviousObservable != null, "setupPreviousObservable");
                Contract.Requires<ArgumentNullException>(setupPreviousNonComputableObservable != null, "setupPreviousNonComputableObservable");
            }

            #endregion
        }
    }
}