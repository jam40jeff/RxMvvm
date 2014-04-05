namespace MorseCode.RxMvvm.Observable.Property
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Reactive;

    /// <summary>
    /// Class representing a property that is automatically calculated when its dependencies change.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the property.
    /// </typeparam>
    public class CalculatedProperty<T> : ICalculatedProperty<T>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedProperty{T}"/> class. 
        /// </summary>
        /// <param name="registerCalculation">
        /// Registers the calculation.
        /// </param>
        public CalculatedProperty(Func<BehaviorSubject<IDiscriminatedUnion<object, T, Exception>>, BehaviorSubject<bool>, IDisposable> registerCalculation)
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
                throw new InvalidOperationException("Result of AsObservable cannot be null.");
            }

            this.setObservable = this.setOrExceptionObservable.TakeFirst();

            if (this.setObservable == null)
            {
                throw new InvalidOperationException("Result of TakeFirst cannot be null.");
            }

            this.changeObservable = this.setObservable.DistinctUntilChanged();

            if (this.changeObservable == null)
            {
                throw new InvalidOperationException("Result of DistinctUntilChanged cannot be null.");
            }

            this.exceptionObservable = this.setOrExceptionObservable.TakeSecond();
            this.changeOrExceptionObservable =
                this.changeObservable.Select(DiscriminatedUnion.First<object, T, Exception>)
                    .Merge(this.exceptionObservable.Select(DiscriminatedUnion.Second<object, T, Exception>));

            if (this.changeOrExceptionObservable == null)
            {
                throw new InvalidOperationException("Result of Merge cannot be null.");
            }
        }

        /// <summary>
        /// Gets an observable which notifies when a value change occurs.
        /// </summary>
        public IObservable<IDiscriminatedUnion<object, T, Exception>> OnChanged
        {
            get
            {
                return this.changeOrExceptionObservable;
            }
        }

        /// <summary>
        /// Gets an observable which notifies when a value set occurs.
        /// </summary>
        public IObservable<IDiscriminatedUnion<object, T, Exception>> OnSet
        {
            get
            {
                return this.setOrExceptionObservable;
            }
        }

        /// <summary>
        /// Gets an observable which notifies when a successful calculation results in a value change.
        /// </summary>
        public IObservable<T> OnSuccessfulValueChanged
        {
            get
            {
                return this.changeObservable;
            }
        }

        /// <summary>
        /// Gets the on successful value changed.
        /// </summary>
        public IObservable<T> OnSuccessfulValueSet
        {
            get
            {
                return this.setObservable;
            }
        }

        /// <summary>
        /// Gets an observable which notifies when a calculation error occurs.
        /// </summary>
        public IObservable<Exception> OnCalculationException
        {
            get
            {
                return this.exceptionObservable;
            }
        }

        /// <summary>
        /// Gets the latest value from a successful calculation.
        /// </summary>
        public T LatestSuccessfulValue
        {
            get
            {
                return this.valueSubject.Value;
            }
        }

        /// <summary>
        /// Gets the latest calculation exception.
        /// </summary>
        public Exception LatestCalculationException
        {
            get
            {
                return this.exceptionSubject.Value;
            }
        }

        /// <summary>
        /// Gets the latest successful value or throws an exception if the latest calculation resulted in an error.
        /// </summary>
        /// <returns>
        /// The latest successful value.
        /// </returns>
        public IDiscriminatedUnion<object, T, Exception> Value
        {
            get
            {
                Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                if (this.valueOrExceptionSubject.Value == null)
                {
                    throw new InvalidOperationException("Latest value or exception discriminated union cannot be null.");
                }

                return this.valueOrExceptionSubject.Value;
            }
        }

        IDisposable IObservable<IDiscriminatedUnion<object, T, Exception>>.Subscribe(
            IObserver<IDiscriminatedUnion<object, T, Exception>> observer)
        {
            return this.changeOrExceptionObservable.Subscribe(observer);
        }

        /// <summary>
        /// The get value or throw exception.
        /// </summary>
        /// <returns>
        /// The latest successful value.
        /// </returns>
        /// <exception cref="Exception">
        /// The latest calculation exception.
        /// </exception>
        public T GetSuccessfulValueOrThrowException()
        {
            if (this.valueOrExceptionSubject.Value == null)
            {
                throw new InvalidOperationException("Latest value or exception discriminated union cannot be null.");
            }

            return this.valueOrExceptionSubject.Value.Switch(v => v, e => { throw e; });
        }

        void IDisposable.Dispose()
        {
            this.valueOrExceptionSubject.Dispose();
            this.isCalculatingSubject.Dispose();
            this.valueSubject.Dispose();
            this.exceptionSubject.Dispose();
            this.subscriptionsDisposable.Dispose();
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