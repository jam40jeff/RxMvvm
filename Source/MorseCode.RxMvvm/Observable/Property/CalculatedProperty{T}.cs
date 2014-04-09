namespace MorseCode.RxMvvm.Observable.Property
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Reactive;

    internal class CalculatedProperty<T> : ReadableObservablePropertyBase<IDiscriminatedUnion<object, T, Exception>>, ICalculatedProperty<T>
    {
        private readonly BehaviorSubject<IDiscriminatedUnion<object, T, Exception>> valueOrExceptionSubject;

        private readonly BehaviorSubject<bool> isCalculatingSubject;

        private readonly BehaviorSubject<T> valueSubject;

        private readonly BehaviorSubject<Exception> exceptionSubject;

        private readonly IObservable<T> setObservable;

        private readonly IObservable<T> changeObservable;

        private readonly IObservable<Exception> exceptionObservable;

        private readonly IObservable<IDiscriminatedUnion<object, T, Exception>> setOrExceptionObservable;

        private readonly IObservable<IDiscriminatedUnion<object, T, Exception>> changeOrExceptionObservable;

        private readonly CompositeDisposable subscriptionsDisposable = new CompositeDisposable();

        internal CalculatedProperty(Func<BehaviorSubject<IDiscriminatedUnion<object, T, Exception>>, BehaviorSubject<bool>, IDisposable> registerCalculation)
        {
            Contract.Requires<ArgumentNullException>(registerCalculation != null, "registerCalculation");
            Contract.Ensures(this.valueOrExceptionSubject != null);
            Contract.Ensures(this.isCalculatingSubject != null);
            Contract.Ensures(this.valueSubject != null);
            Contract.Ensures(this.exceptionSubject != null);
            Contract.Ensures(this.setObservable != null);
            Contract.Ensures(this.changeObservable != null);
            Contract.Ensures(this.exceptionObservable != null);
            Contract.Ensures(this.setOrExceptionObservable != null);
            Contract.Ensures(this.changeOrExceptionObservable != null);

            this.valueOrExceptionSubject = new BehaviorSubject<IDiscriminatedUnion<object, T, Exception>>(DiscriminatedUnion.First<object, T, Exception>(default(T)));
            this.valueSubject = new BehaviorSubject<T>(default(T));
            this.exceptionSubject = new BehaviorSubject<Exception>(null);
            this.isCalculatingSubject = new BehaviorSubject<bool>(false);

            this.subscriptionsDisposable.Add(this.valueOrExceptionSubject.Subscribe(v => v.Switch(this.valueSubject.OnNext, this.exceptionSubject.OnNext)));

            this.subscriptionsDisposable.Add(registerCalculation(this.valueOrExceptionSubject, this.isCalculatingSubject));

            this.setOrExceptionObservable = this.valueOrExceptionSubject.AsObservable();

            if (this.setOrExceptionObservable == null)
            {
                throw new InvalidOperationException("Result of " + StaticReflection<BehaviorSubject<IDiscriminatedUnion<object, T, Exception>>>.GetMethodInfo(o => o.AsObservable()).Name + " cannot be null.");
            }

            this.setObservable = this.setOrExceptionObservable.TakeFirst();

            if (this.setObservable == null)
            {
                throw new InvalidOperationException("Result of " + StaticReflection<BehaviorSubject<IDiscriminatedUnion<object, T, Exception>>>.GetMethodInfo(o => o.TakeFirst()).Name + " cannot be null.");
            }

            this.changeObservable = this.setObservable.DistinctUntilChanged();

            if (this.changeObservable == null)
            {
                throw new InvalidOperationException("Result of " + StaticReflection<IObservable<T>>.GetMethodInfo(o => o.DistinctUntilChanged()).Name + " cannot be null.");
            }

            this.exceptionObservable = this.setOrExceptionObservable.TakeSecond();
            this.changeOrExceptionObservable =
                this.changeObservable.Select(DiscriminatedUnion.First<object, T, Exception>)
                    .Merge(this.exceptionObservable.Select(DiscriminatedUnion.Second<object, T, Exception>));

            if (this.changeOrExceptionObservable == null)
            {
                throw new InvalidOperationException("Result of " + StaticReflection<IObservable<IDiscriminatedUnion<object, T, Exception>>>.GetMethodInfo(o => o.Merge(null)).Name + " cannot be null.");
            }

            // TODO: does this subscription need to happen on the UI thread?
            this.subscriptionsDisposable.Add(this.valueOrExceptionSubject.Skip(1).Subscribe(v => this.OnValueChanged()));
            this.subscriptionsDisposable.Add(this.changeObservable.Skip(1).Subscribe(v => this.OnLatestSuccessfulValueChanged()));
            this.subscriptionsDisposable.Add(this.exceptionObservable.Skip(1).Subscribe(v => this.OnLatestCalculationExceptionChanged()));
        }

        IObservable<T> ICalculatedProperty<T>.OnSuccessfulValueChanged
        {
            get
            {
                return this.changeObservable;
            }
        }

        IObservable<T> ICalculatedProperty<T>.OnSuccessfulValueSet
        {
            get
            {
                return this.setObservable;
            }
        }

        IObservable<Exception> ICalculatedProperty<T>.OnCalculationException
        {
            get
            {
                return this.exceptionObservable;
            }
        }

        T ICalculatedProperty<T>.LatestSuccessfulValue
        {
            get
            {
                return this.valueSubject.Value;
            }
        }

        Exception ICalculatedProperty<T>.LatestCalculationException
        {
            get
            {
                return this.exceptionSubject.Value;
            }
        }

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
                return this.setOrExceptionObservable;
            }
        }

        T ICalculatedProperty<T>.GetSuccessfulValueOrThrowException()
        {
            if (this.valueOrExceptionSubject.Value == null)
            {
                throw new InvalidOperationException("Latest value or exception discriminated union cannot be null.");
            }

            return this.valueOrExceptionSubject.Value.Switch(v => v, e => { throw e; });
        }

        /// <summary>
        /// Disposes of the property.
        /// </summary>
        protected override void Dispose()
        {
            base.Dispose();

            this.valueOrExceptionSubject.Dispose();
            this.isCalculatingSubject.Dispose();
            this.valueSubject.Dispose();
            this.exceptionSubject.Dispose();
            this.subscriptionsDisposable.Dispose();
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

            if (this.valueOrExceptionSubject.Value == null)
            {
                throw new InvalidOperationException("Latest value or exception discriminated union cannot be null.");
            }

            return this.valueOrExceptionSubject.Value;
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the <see cref="ICalculatedProperty{T}.LatestSuccessfulValue"/> property.
        /// </summary>
        protected virtual void OnLatestSuccessfulValueChanged()
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(CalculatedPropertyUtility.LatestSuccessfulValuePropertyName));
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for the <see cref="ICalculatedProperty{T}.LatestCalculationException"/> property.
        /// </summary>
        protected virtual void OnLatestCalculationExceptionChanged()
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(CalculatedPropertyUtility.LatestCalculationExceptionPropertyName));
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.valueOrExceptionSubject != null);
            Contract.Invariant(this.isCalculatingSubject != null);
            Contract.Invariant(this.valueSubject != null);
            Contract.Invariant(this.exceptionSubject != null);
            Contract.Invariant(this.setObservable != null);
            Contract.Invariant(this.changeObservable != null);
            Contract.Invariant(this.exceptionObservable != null);
            Contract.Invariant(this.setOrExceptionObservable != null);
            Contract.Invariant(this.changeOrExceptionObservable != null);
            Contract.Invariant(this.subscriptionsDisposable != null);
        }
    }
}