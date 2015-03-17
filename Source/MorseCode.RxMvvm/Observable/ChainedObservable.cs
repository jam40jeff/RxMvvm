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
    /// A static class providing extension methods for creating chained observables.  Chained observables automatically re-subscribe the rest of the chain when any of the intermediate property values change.
    /// </summary>
    public static class ChainedObservable
    {
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
        /// A <see cref="ChainedObservableInitialHelper{T}"/> for building the chained observable.
        /// </returns>
        public static ChainedObservableInitialHelper<T> BeginChain<T>(this IObservable<T> observable)
        {
            Contract.Requires<ArgumentNullException>(observable != null, "observable");
            Contract.Ensures(Contract.Result<ChainedObservableInitialHelper<T>>() != null);

            return new ChainedObservableInitialHelper<T>(
                skipFirst => skipFirst ? observable.Skip(1) : observable,
                skipFirst =>
                skipFirst
                    ? observable.Select(DiscriminatedUnion.First<object, T, NonComputable>).Skip(1)
                    : observable.Select(DiscriminatedUnion.First<object, T, NonComputable>));
        }

        /// <summary>
        /// A base class for helper classes for building chained observables.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value of the last observable in the chain.
        /// </typeparam>
        public abstract class ChainedObservableHelperBase<T>
        {
            private readonly Func<bool, IObservable<T>> setupPreviousObservable;

            private readonly Func<bool, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> setupPreviousNonComputableObservable;

            internal ChainedObservableHelperBase(
                Func<bool, IObservable<T>> setupPreviousObservable,
                Func<bool, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> setupPreviousNonComputableObservable)
            {
                Contract.Requires<ArgumentNullException>(setupPreviousObservable != null, "setupPreviousObservable");
                Contract.Requires<ArgumentNullException>(
                    setupPreviousNonComputableObservable != null, "setupPreviousNonComputableObservable");

                this.setupPreviousObservable = setupPreviousObservable;
                this.setupPreviousNonComputableObservable = setupPreviousNonComputableObservable;
            }

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
            /// The <see cref="ChainedObservableHelper{TNew}"/>.
            /// A <see cref="ChainedObservableHelper{TNew}"/> for building the chained observable.
            /// </returns>
            public ChainedObservableHelper<TNew> Add<TNew>(Func<T, IObservable<TNew>> getObservable)
            {
                Contract.Requires<ArgumentNullException>(getObservable != null, "getObservable");
                Contract.Ensures(Contract.Result<ChainedObservableHelper<TNew>>() != null);

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

            /// <summary>
            /// Adds an observable to a chained observable and completes the observable chain and returns an observable which will produce the latest leaf observable.
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
            public IObservable<IDiscriminatedUnion<object, TNew, NonComputable>> AddLeafAndCompleteWithoutEvaluation<TNew>(Func<T, TNew> getObservable)
                where TNew : class
            {
                Contract.Requires<ArgumentNullException>(getObservable != null, "getObservable");
                Contract.Ensures(
                    Contract.Result<IObservable<IDiscriminatedUnion<object, TNew, NonComputable>>>() != null);

                IObservable<IDiscriminatedUnion<object, TNew, NonComputable>> observable =
                    this.setupPreviousNonComputableObservable(false).Select(
                        o => o.Switch(
                            v =>
                            {
                                if (ReferenceEquals(v, null))
                                {
                                    return
                                        DiscriminatedUnion.Second<object, TNew, NonComputable>(
                                            NonComputable.Value);
                                }

                                TNew o2 = getObservable(v);
                                if (o2 == null)
                                {
                                    return
                                        DiscriminatedUnion.Second<object, TNew, NonComputable>(
                                            NonComputable.Value);
                                }

                                return DiscriminatedUnion.First<object, TNew, NonComputable>(o2);
                            },
                            v => DiscriminatedUnion.Second<object, TNew, NonComputable>(NonComputable.Value)));
                if (observable == null)
                {
                    throw new InvalidOperationException(
                        "Result of "
                        + StaticReflection<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>.GetMethodInfo(
                            o2 =>
                            o2.Select(
                                (Func<IDiscriminatedUnion<object, T, NonComputable>, IDiscriminatedUnion<object, TNew, NonComputable>>)null)).Name
                        + " cannot be null.");
                }

                return observable;
            }

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
            protected IObservable<IDiscriminatedUnion<object, T, NonComputable>> CompleteInternal(
                bool notifyLeafOnly, bool provideAllNotifications)
            {
                Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>() != null);

                IObservable<IDiscriminatedUnion<object, T, NonComputable>> o =
                    this.setupPreviousNonComputableObservable(notifyLeafOnly);
                if (o == null)
                {
                    throw new InvalidOperationException(
                        "Result of "
                        + StaticReflection.GetInScopeMemberInfo(() => this.setupPreviousNonComputableObservable).Name
                        + " cannot be null.");
                }

                if (!provideAllNotifications)
                {
                    o = o.DistinctUntilChanged();
                    if (o == null)
                    {
                        throw new InvalidOperationException(
                            "Result of "
                            + StaticReflection<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>.GetMethodInfo(
                            o2 => o2.DistinctUntilChanged()).Name + " cannot be null.");
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
            protected IObservable<T> CompleteWithDefaultIfNotComputableInternal(
                bool notifyLeafOnly, bool provideAllNotifications)
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                IObservable<T> o = this.setupPreviousObservable(notifyLeafOnly);
                if (o == null)
                {
                    throw new InvalidOperationException(
                        "Result of " + StaticReflection.GetInScopeMemberInfo(() => this.setupPreviousObservable).Name
                        + " cannot be null.");
                }

                if (!provideAllNotifications)
                {
                    o = o.DistinctUntilChanged();
                    if (o == null)
                    {
                        throw new InvalidOperationException(
                            "Result of "
                            + StaticReflection<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>.GetMethodInfo(
                            o2 => o2.DistinctUntilChanged()).Name + " cannot be null.");
                    }
                }

                return o;
            }

            private Func<IDiscriminatedUnion<object, T, NonComputable>, IObservable<TInner>> GetInnerObservable<TInner, TNew>(
                Func<T, IObservable<TNew>> getObservable,
                Func<TNew, TInner> getInnerValue,
                Func<TInner> getNonComputableInnerValue)
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
                            return ObservableRxMvvm.Always(getNonComputableInnerValue());
                        }

                        IObservable<TNew> o2 = getObservable(v);
                        return o2 == null ? ObservableRxMvvm.Always(getNonComputableInnerValue()) : o2.Select(getInnerValue);
                    },
                    v => ObservableRxMvvm.Always(getNonComputableInnerValue()));
            }

            [ContractInvariantMethod]
            private void CodeContractsInvariants()
            {
                Contract.Invariant(this.setupPreviousObservable != null);
                Contract.Invariant(this.setupPreviousNonComputableObservable != null);
            }
        }

        /// <summary>
        /// A helper class for building chained observables.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value of the last observable in the chain.
        /// </typeparam>
        public class ChainedObservableInitialHelper<T> : ChainedObservableHelperBase<T>
        {
            internal ChainedObservableInitialHelper(
                Func<bool, IObservable<T>> setupPreviousObservable,
                Func<bool, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> setupPreviousNonComputableObservable)
                : base(setupPreviousObservable, setupPreviousNonComputableObservable)
            {
                Contract.Requires<ArgumentNullException>(setupPreviousObservable != null, "setupPreviousObservable");
                Contract.Requires<ArgumentNullException>(
                    setupPreviousNonComputableObservable != null, "setupPreviousNonComputableObservable");
            }
        }

        /// <summary>
        /// A helper class for building chained observables.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value of the last observable in the chain.
        /// </typeparam>
        public class ChainedObservableHelper<T> : ChainedObservableHelperBase<T>
        {
            internal ChainedObservableHelper(
                Func<bool, IObservable<T>> setupPreviousObservable,
                Func<bool, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> setupPreviousNonComputableObservable)
                : base(setupPreviousObservable, setupPreviousNonComputableObservable)
            {
                Contract.Requires<ArgumentNullException>(setupPreviousObservable != null, "setupPreviousObservable");
                Contract.Requires<ArgumentNullException>(
                    setupPreviousNonComputableObservable != null, "setupPreviousNonComputableObservable");
            }

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
            public IObservable<IDiscriminatedUnion<object, T, NonComputable>> Complete()
            {
                Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>() != null);

                return this.CompleteInternal();
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
            /// This overload is equivalent to calling <see cref="Complete(bool,bool)"/> passing <paramref name="notifyLeafOnly"/> and 
            /// <value>
            /// false
            /// </value>
            /// such that the observable chain will send through all
            /// notifications rather than just changes.
            /// </remarks>
            public IObservable<IDiscriminatedUnion<object, T, NonComputable>> Complete(bool notifyLeafOnly)
            {
                Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>() != null);

                return this.CompleteInternal(notifyLeafOnly);
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
            public IObservable<IDiscriminatedUnion<object, T, NonComputable>> Complete(
                bool notifyLeafOnly, bool provideAllNotifications)
            {
                Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>() != null);

                return this.CompleteInternal(notifyLeafOnly, provideAllNotifications);
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
            /// This overload is equivalent to calling <see cref="Complete(bool)"/> passing <value>false</value> such that the observable chain will be notified on intermediate property changes.
            /// </remarks>
            public IObservable<T> CompleteWithDefaultIfNotComputable()
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return this.CompleteWithDefaultIfNotComputableInternal();
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
            /// This overload is equivalent to calling <see cref="Complete(bool,bool)"/> passing <paramref name="notifyLeafOnly"/> and 
            /// <value>
            /// false
            /// </value>
            /// such that the observable chain will send through all
            /// notifications rather than just changes.
            /// </remarks>
            public IObservable<T> CompleteWithDefaultIfNotComputable(bool notifyLeafOnly)
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return this.CompleteWithDefaultIfNotComputableInternal(notifyLeafOnly);
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
            public IObservable<T> CompleteWithDefaultIfNotComputable(bool notifyLeafOnly, bool provideAllNotifications)
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return this.CompleteWithDefaultIfNotComputableInternal(notifyLeafOnly, provideAllNotifications);
            }
        }
    }
}