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

    using MorseCode.RxMvvm.Common;

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
        /// The type of the first observable.
        /// </typeparam>
        /// <returns>
        /// A <see cref="ChainedObservableHelper{T}"/> for building the chained observable.
        /// </returns>
        public static ChainedObservableHelper<T> BeginChain<T>(this IObservable<T> observable)
        {
            Contract.Requires<ArgumentNullException>(observable != null, "observable");
            Contract.Ensures(Contract.Result<ChainedObservableHelper<T>>() != null);

            return new ChainedObservableHelper<T>(
                skipFirst => skipFirst ? observable.Skip(1) : observable,
                skipFirst =>
                skipFirst
                    ? observable.Select(DiscriminatedUnion.First<object, T, NonComputable>).Skip(1)
                    : observable.Select(DiscriminatedUnion.First<object, T, NonComputable>));
        }

        /// <summary>
        /// A helper class for building chained observables.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the last observable in the chain.
        /// </typeparam>
        public class ChainedObservableHelper<T>
        {
            private readonly Func<bool, IObservable<T>> setupPreviousObservable;

            private readonly Func<bool, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> setupPreviousNonComputableObservable;

            internal ChainedObservableHelper(
                Func<bool, IObservable<T>> setupPreviousObservable,
                Func<bool, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> setupPreviousNonComputableObservable)
            {
                Contract.Requires<ArgumentNullException>(setupPreviousObservable != null, "setupPreviousObservable");
                Contract.Requires<ArgumentNullException>(setupPreviousNonComputableObservable != null, "setupPreviousNonComputableObservable");

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
            /// The type of the observable to add.
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
                    skipFirst =>
                    {
                        Func<IDiscriminatedUnion<object, T, NonComputable>, IObservable<TNew>> innerObservable;
                        if (skipFirst)
                        {
                            innerObservable =
                                o =>
                                o.Switch(
                                    v =>
                                    ReferenceEquals(v, null) ? Observable.Return(default(TNew)) : getObservable(v),
                                    v => Observable.Return(default(TNew))).Skip(1);
                        }
                        else
                        {
                            innerObservable =
                                o =>
                                o.Switch(
                                    v =>
                                    ReferenceEquals(v, null) ? Observable.Return(default(TNew)) : getObservable(v),
                                    v => Observable.Return(default(TNew)));
                        }

                        return this.setupPreviousNonComputableObservable(false).Select(
                            v =>
                            {
                                IObservable<TNew> o = innerObservable(v);
                                if (o == null)
                                {
                                    throw new ArgumentException(
                                        "The function specified for parameter "
                                        + StaticReflection.GetInScopeMemberInfo(() => getObservable).Name
                                        + " cannot return null.");
                                }

                                return o;
                            }).Switch();
                    },
                    skipFirst =>
                    {
                        Func<IDiscriminatedUnion<object, T, NonComputable>, IObservable<IDiscriminatedUnion<object, TNew, NonComputable>>> innerObservable;
                        if (skipFirst)
                        {
                            innerObservable = o => o.Switch(
                                v =>
                                {
                                    if (ReferenceEquals(v, null))
                                    {
                                        return
                                            Observable.Return(
                                                DiscriminatedUnion.Second<object, TNew, NonComputable>(
                                                    NonComputable.Value));
                                    }

                                    IObservable<TNew> o2 = getObservable(v);
                                    if (o2 == null)
                                    {
                                        throw new ArgumentException(
                                            "The function specified for parameter "
                                            + StaticReflection.GetInScopeMemberInfo(() => getObservable).Name
                                            + " cannot return null.");
                                    }

                                    return o2.Select(DiscriminatedUnion.First<object, TNew, NonComputable>);
                                },
                                v =>
                                Observable.Return(
                                    DiscriminatedUnion.Second<object, TNew, NonComputable>(NonComputable.Value)))
                                                    .Skip(1);
                        }
                        else
                        {
                            innerObservable = o => o.Switch(
                                v =>
                                {
                                    if (ReferenceEquals(v, null))
                                    {
                                        return
                                            Observable.Return(
                                                DiscriminatedUnion.Second<object, TNew, NonComputable>(
                                                    NonComputable.Value));
                                    }

                                    IObservable<TNew> o2 = getObservable(v);
                                    if (o2 == null)
                                    {
                                        throw new ArgumentException(
                                            "The function specified for parameter "
                                            + StaticReflection.GetInScopeMemberInfo(() => getObservable).Name
                                            + " cannot return null.");
                                    }

                                    return o2.Select(DiscriminatedUnion.First<object, TNew, NonComputable>);
                                },
                                v =>
                                Observable.Return(
                                    DiscriminatedUnion.Second<object, TNew, NonComputable>(NonComputable.Value)));
                        }

                        return this.setupPreviousNonComputableObservable(false).Select(innerObservable).Switch();
                    });
            }

            /// <summary>
            /// Completes the observable chain.
            /// </summary>
            /// <returns>
            /// The chained <see cref="IObservable{TObservable}"/> (where <code>TObservable</code> is <see cref="IDiscriminatedUnion{TDiscriminatedUnion1,TDiscriminatedUnion2,TDiscriminatedUnion3}"/> where
            /// <code>TDiscriminatedUnion1</code> is <see cref="Object"/> and <code>TDiscriminatedUnion2</code> is <typeparamref name="T"/> and <code>TDiscriminatedUnion3</code> is
            /// <see cref="NonComputable"/>).
            /// </returns>
            /// <remarks>
            /// This overload will call through to <see cref="Complete(bool)"/> passing <value>false</value> such that the observable chain will be notified on intermediate property changes.
            /// </remarks>
            public IObservable<IDiscriminatedUnion<object, T, NonComputable>> Complete()
            {
                Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>() != null);

                return this.Complete(false);
            }

            /// <summary>
            /// Completes the observable chain.
            /// </summary>
            /// <param name="notifyLeafOnly">
            /// If 
            /// <value>
            /// true
            /// </value>
            /// , the chained observable will only be notified when the last property changes, otherwise it will be notified when intermediate properties change as well.
            /// </param>
            /// <returns>
            /// The chained <see cref="IObservable{TObservable}"/> (where 
            /// <code>
            /// TObservable
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
            /// This overload will call through to <see cref="Complete(bool,bool)"/> passing <paramref name="notifyLeafOnly"/> and 
            /// <value>
            /// false
            /// </value>
            /// such that the observable chain will send through all
            /// notifications rather than just changes.
            /// </remarks>
            public IObservable<IDiscriminatedUnion<object, T, NonComputable>> Complete(bool notifyLeafOnly)
            {
                Contract.Ensures(Contract.Result<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>() != null);

                return this.Complete(notifyLeafOnly, false);
            }

            /// <summary>
            /// Completes the observable chain.
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
            /// The chained <see cref="IObservable{TObservable}"/> (where 
            /// <code>
            /// TObservable
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
            public IObservable<IDiscriminatedUnion<object, T, NonComputable>> Complete(
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
                            + StaticReflection<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>.GetMethodInfo(o2 => o2.DistinctUntilChanged()).Name + " cannot be null.");
                    }
                }

                return o;
            }

            /// <summary>
            /// Completes the observable chain and returns an observable which will produce <value>default(T)</value> if the chain is not computable.
            /// </summary>
            /// <returns>
            /// The chained <see cref="IObservable{T}"/> which will produce <value>default(T)</value> if the chain is not computable.
            /// </returns>
            /// <remarks>
            /// This overload will call through to <see cref="Complete(bool)"/> passing <value>false</value> such that the observable chain will be notified on intermediate property changes.
            /// </remarks>
            public IObservable<T> CompleteWithDefaultIfNotComputable()
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return this.CompleteWithDefaultIfNotComputable(false);
            }

            /// <summary>
            /// Completes the observable chain and returns an observable which will produce 
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
            /// This overload will call through to <see cref="Complete(bool,bool)"/> passing <paramref name="notifyLeafOnly"/> and 
            /// <value>
            /// false
            /// </value>
            /// such that the observable chain will send through all
            /// notifications rather than just changes.
            /// </remarks>
            public IObservable<T> CompleteWithDefaultIfNotComputable(bool notifyLeafOnly)
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return this.CompleteWithDefaultIfNotComputable(notifyLeafOnly, false);
            }

            /// <summary>
            /// Completes the observable chain and returns an observable which will produce 
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
                            + StaticReflection<IObservable<IDiscriminatedUnion<object, T, NonComputable>>>.GetMethodInfo(o2 => o2.DistinctUntilChanged()).Name + " cannot be null.");
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
        }
    }
}