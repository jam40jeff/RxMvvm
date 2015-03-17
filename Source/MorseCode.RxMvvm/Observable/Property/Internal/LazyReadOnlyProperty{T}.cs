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

namespace MorseCode.RxMvvm.Observable.Property.Internal
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Threading;
    using System.Threading.Tasks;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Common.StaticReflection;
    using MorseCode.RxMvvm.Reactive;

    [Serializable]
    internal class LazyReadOnlyProperty<T> : ReadableObservablePropertyBase<IDiscriminatedUnion<object, T, Exception>>, ILazyReadOnlyProperty<T>, ISerializable
    {
        #region Fields

        private readonly IObservable<T> changeObservable;

        private readonly IObservable<IDiscriminatedUnion<object, T, Exception>> changeOrExceptionObservable;

        private readonly IObservable<Exception> exceptionObservable;

        private readonly object hasLoadBeenCalledLock = new object();

        private readonly IObservable<bool> isCalculatedObservable;

        private readonly BehaviorSubject<bool> isCalculatedSubject;

        private readonly IObservable<bool> isCalculatingObservable;

        private readonly BehaviorSubject<bool> isCalculatingSubject;

        private readonly bool isLongRunningCalculation;

        private readonly IObservable<T> setObservable;

        private readonly Lazy<Task<T>> value;

        private readonly Func<Task<T>> valueFactory;

        private readonly IObservable<IDiscriminatedUnion<object, T, Exception>> valueObservable;

        private readonly BehaviorSubject<IDiscriminatedUnion<object, T, Exception>> valueSubject;

        private bool hasLoadBeenCalled;

        #endregion

        #region Constructors and Destructors

        internal LazyReadOnlyProperty(Func<Task<T>> valueFactory, bool isLongRunningCalculation)
        {
            Contract.Requires<ArgumentNullException>(valueFactory != null, "valueFactory");
            Contract.Ensures(this.value != null);
            Contract.Ensures(this.valueFactory != null);
            Contract.Ensures(this.changeObservable != null);
            Contract.Ensures(this.changeOrExceptionObservable != null);
            Contract.Ensures(this.exceptionObservable != null);
            Contract.Ensures(this.isCalculatedObservable != null);
            Contract.Ensures(this.isCalculatedSubject != null);
            Contract.Ensures(this.isCalculatingObservable != null);
            Contract.Ensures(this.isCalculatingSubject != null);
            Contract.Ensures(this.setObservable != null);
            Contract.Ensures(this.valueObservable != null);
            Contract.Ensures(this.valueSubject != null);

            this.valueFactory = valueFactory;
            this.isLongRunningCalculation = isLongRunningCalculation;

            this.isCalculatedSubject = new BehaviorSubject<bool>(false);
            this.isCalculatingSubject = new BehaviorSubject<bool>(false);

            this.valueSubject = new BehaviorSubject<IDiscriminatedUnion<object, T, Exception>>(DiscriminatedUnion.First<object, T, Exception>(default(T)));

            this.value = new Lazy<Task<T>>(valueFactory);

            this.valueObservable = this.valueSubject.Do(_ => this.Load());

            if (this.valueObservable == null)
            {
                throw new InvalidOperationException("Result of " + StaticReflection.GetInScopeMethodInfo(() => Observable.Do(null, (Action<object>)null)).Name + " cannot be null.");
            }

            this.isCalculatedObservable = this.isCalculatedSubject.DistinctUntilChanged();

            if (this.isCalculatedObservable == null)
            {
                throw new InvalidOperationException("Result of " + StaticReflection<IObservable<T>>.GetMethodInfo(o => o.DistinctUntilChanged()).Name + " cannot be null.");
            }

            this.isCalculatingObservable = this.isCalculatingSubject.DistinctUntilChanged();

            if (this.isCalculatingObservable == null)
            {
                throw new InvalidOperationException("Result of " + StaticReflection<IObservable<T>>.GetMethodInfo(o => o.DistinctUntilChanged()).Name + " cannot be null.");
            }

            this.setObservable = this.valueObservable.TakeFirst();

            this.changeObservable = this.setObservable.DistinctUntilChanged();

            if (this.changeObservable == null)
            {
                throw new InvalidOperationException("Result of " + StaticReflection<IObservable<T>>.GetMethodInfo(o => o.DistinctUntilChanged()).Name + " cannot be null.");
            }

            this.exceptionObservable = this.valueObservable.TakeSecond();
            this.changeOrExceptionObservable = this.changeObservable.Select(DiscriminatedUnion.First<object, T, Exception>).Merge(this.exceptionObservable.Select(DiscriminatedUnion.Second<object, T, Exception>));

            if (this.changeOrExceptionObservable == null)
            {
                throw new InvalidOperationException("Result of " + StaticReflection<IObservable<IDiscriminatedUnion<object, T, Exception>>>.GetMethodInfo(o => o.Merge(null)).Name + " cannot be null.");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyReadOnlyProperty{T}"/> class from serialized data.
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The serialization context.
        /// </param>
        [ContractVerification(false)]
        // ReSharper disable UnusedParameter.Local
        protected LazyReadOnlyProperty(SerializationInfo info, StreamingContext context) // ReSharper restore UnusedParameter.Local
            : this((Func<Task<T>>)(info.GetValue("v", typeof(Func<Task<T>>)) ?? default(T)), (bool)(info.GetValue("l", typeof(bool)) ?? default(T)))
        {
        }

        #endregion

        #region Public Properties

        public Exception CalculationException
        {
            get
            {
                return this.GetValue().Switch(v => null, e => e);
            }
        }

        public bool IsCalculated
        {
            get
            {
                return this.isCalculatedSubject.Value;
            }
        }

        public bool IsCalculating
        {
            get
            {
                return this.isCalculatingSubject.Value;
            }
        }

        public new T Value
        {
            get
            {
                return this.GetValue().Switch(v => v, e => default(T));
            }
        }

        public IDiscriminatedUnion<object, T, Exception> ValueOrException
        {
            get
            {
                return this.GetValue();
            }
        }

        #endregion

        #region Explicit Interface Properties

        Exception ILazyReadOnlyProperty<T>.CalculationException
        {
            get
            {
                return this.CalculationException;
            }
        }

        bool ILazyReadOnlyProperty<T>.IsCalculated
        {
            get
            {
                return this.IsCalculated;
            }
        }

        bool ILazyReadOnlyProperty<T>.IsCalculating
        {
            get
            {
                return this.IsCalculating;
            }
        }

        IObservable<Exception> ILazyReadOnlyProperty<T>.OnCalculationException
        {
            get
            {
                return this.exceptionObservable;
            }
        }

        IObservable<bool> ILazyReadOnlyProperty<T>.OnIsCalculatedChanged
        {
            get
            {
                return this.isCalculatedObservable;
            }
        }

        IObservable<bool> ILazyReadOnlyProperty<T>.OnIsCalculatingChanged
        {
            get
            {
                return this.isCalculatingObservable;
            }
        }

        IObservable<T> ILazyReadOnlyProperty<T>.OnValueOrDefaultChanged
        {
            get
            {
                IObservable<T> result = this.GetValueOrDefault(this.OnChanged).DistinctUntilChanged();

                if (result == null)
                {
                    throw new InvalidOperationException("Result of " + StaticReflection<IObservable<T>>.GetMethodInfo(o => o.DistinctUntilChanged()).Name + " cannot be null.");
                }

                return result;
            }
        }

        IObservable<T> ILazyReadOnlyProperty<T>.OnValueOrDefaultSet
        {
            get
            {
                return this.GetValueOrDefault(this.OnSet);
            }
        }

        T ILazyReadOnlyProperty<T>.Value
        {
            get
            {
                return this.Value;
            }
        }

        IDiscriminatedUnion<object, T, Exception> ILazyReadOnlyProperty<T>.ValueOrException
        {
            get
            {
                return this.ValueOrException;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the on changed observable.
        /// </summary>
        protected override IObservable<IDiscriminatedUnion<object, T, Exception>> OnChanged
        {
            get
            {
                return this.changeOrExceptionObservable;
            }
        }

        /// <summary>
        /// Gets the on set observable.
        /// </summary>
        protected override IObservable<IDiscriminatedUnion<object, T, Exception>> OnSet
        {
            get
            {
                return this.valueObservable;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the object data to serialize.
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The serialization context.
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("v", this.valueFactory);
            info.AddValue("l", this.isLongRunningCalculation);
        }

        #endregion

        #region Explicit Interface Methods

        void ILazyReadOnlyProperty<T>.EagerLoad()
        {
            this.Load();
        }

        T ILazyReadOnlyProperty<T>.GetValueOrThrowException()
        {
            if (this.GetValue() == null)
            {
                throw new InvalidOperationException("Latest value or exception discriminated union cannot be null.");
            }

            return this.GetValue().GetValueOrThrowException();
        }

        #endregion

        #region Methods

        protected override void Dispose()
        {
            base.Dispose();

            this.valueSubject.Dispose();
            this.isCalculatedSubject.Dispose();
            this.isCalculatingSubject.Dispose();
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <returns>
        /// The value of the property.
        /// </returns>
        protected override IDiscriminatedUnion<object, T, Exception> GetValue()
        {
            Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

            if (!this.value.IsValueCreated)
            {
                this.Load();
                if (RxMvvmConfiguration.IsInDesignMode())
                {
                    while (!this.IsCalculated)
                    {
                        Thread.Sleep(20);
                    }
                }
            }

            if (this.valueSubject.Value == null)
            {
                throw new InvalidOperationException("Latest value or exception discriminated union cannot be null.");
            }

            return this.valueSubject.Value;
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the <see cref="ILazyReadOnlyProperty{T}.CalculationException"/> property.
        /// </summary>
        protected virtual void OnCalculationExceptionChanged()
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(LazyReadOnlyPropertyUtility.CalculationExceptionPropertyName));
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the <see cref="ILazyReadOnlyProperty{T}.IsCalculated"/> property.
        /// </summary>
        protected virtual void OnIsCalculatedChanged()
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(LazyReadOnlyPropertyUtility.IsCalculatedPropertyName));
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the <see cref="ILazyReadOnlyProperty{T}.IsCalculating"/> property.
        /// </summary>
        protected virtual void OnIsCalculatingChanged()
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(LazyReadOnlyPropertyUtility.IsCalculatingPropertyName));
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the <see cref="ILazyReadOnlyProperty{T}.ValueOrException"/> property.
        /// </summary>
        protected virtual void OnValueOrExceptionChanged()
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(LazyReadOnlyPropertyUtility.ValueOrExceptionPropertyName));
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.value != null);
            Contract.Invariant(this.valueFactory != null);
            Contract.Invariant(this.changeObservable != null);
            Contract.Invariant(this.changeOrExceptionObservable != null);
            Contract.Invariant(this.exceptionObservable != null);
            Contract.Invariant(this.isCalculatedObservable != null);
            Contract.Invariant(this.isCalculatedSubject != null);
            Contract.Invariant(this.isCalculatingObservable != null);
            Contract.Invariant(this.isCalculatingSubject != null);
            Contract.Invariant(this.setObservable != null);
            Contract.Invariant(this.valueObservable != null);
            Contract.Invariant(this.valueSubject != null);
        }

        private IObservable<T> GetValueOrDefault(IObservable<IDiscriminatedUnion<object, T, Exception>> o)
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Ensures(Contract.Result<IObservable<T>>() != null);

            IObservable<T> result = o.Select(d => d.Switch(v => v, e => default(T)));

            if (result == null)
            {
                throw new InvalidOperationException("Result of " + StaticReflection<IObservable<T>>.GetMethodInfo(o2 => o2.Select<T, object>(o3 => null)).Name + " cannot be null.");
            }

            return result;
        }

        private void Load()
        {
            if (!this.value.IsValueCreated)
            {
                lock (this.hasLoadBeenCalledLock)
                {
                    if (this.hasLoadBeenCalled)
                    {
                        return;
                    }

                    this.hasLoadBeenCalled = true;
                }

                IScheduler scheduler = this.isLongRunningCalculation ? RxMvvmConfiguration.GetLongRunningCalculationScheduler() : RxMvvmConfiguration.GetCalculationScheduler();
                scheduler.Schedule(async () =>
                    {
                        IScheduler notifyPropertyChangedScheduler = RxMvvmConfiguration.GetNotifyPropertyChangedScheduler();

                        this.isCalculatingSubject.OnNext(true);
                        if (notifyPropertyChangedScheduler != null)
                        {
                            notifyPropertyChangedScheduler.Schedule(this.OnIsCalculatingChanged);
                        }

                        IDiscriminatedUnion<object, T, Exception> v;
                        try
                        {
                            v = DiscriminatedUnion.First<object, T, Exception>(await this.value.Value.ConfigureAwait(false));
                        }
                        catch (Exception e)
                        {
                            v = DiscriminatedUnion.Second<object, T, Exception>(e);
                        }

                        this.valueSubject.OnNext(v);

                        if (notifyPropertyChangedScheduler != null)
                        {
                            v.Switch(
                                o =>
                                {
                                    if (!ReferenceEquals(o, null) && !o.Equals(default(T)))
                                    {
                                        notifyPropertyChangedScheduler.Schedule(this.OnValueOrExceptionChanged);
                                        notifyPropertyChangedScheduler.Schedule(this.OnValueChanged);
                                    }
                                },
                                e =>
                                {
                                    notifyPropertyChangedScheduler.Schedule(this.OnValueOrExceptionChanged);
                                    notifyPropertyChangedScheduler.Schedule(this.OnCalculationExceptionChanged);
                                });
                        }

                        this.isCalculatedSubject.OnNext(true);
                        if (notifyPropertyChangedScheduler != null)
                        {
                            notifyPropertyChangedScheduler.Schedule(this.OnIsCalculatedChanged);
                        }

                        this.isCalculatingSubject.OnNext(false);
                        if (notifyPropertyChangedScheduler != null)
                        {
                            notifyPropertyChangedScheduler.Schedule(this.OnIsCalculatingChanged);
                        }
                    });
            }
        }

        #endregion
    }
}