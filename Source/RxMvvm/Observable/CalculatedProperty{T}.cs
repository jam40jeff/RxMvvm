namespace MorseCode.RxMvvm.Observable
{
    using System;
    using System.Diagnostics.Contracts;
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
        private readonly BehaviorSubject<IDiscriminatedUnion<T, Exception>> valueOrExceptionSubject;

        private readonly BehaviorSubject<T> valueSubject;

        private readonly BehaviorSubject<Exception> exceptionSubject;

        private readonly IObservable<T> allNotificationsObservable;

        private readonly IObservable<T> changeObservable;

        private readonly IObservable<Exception> exceptionObservable;

        private readonly IObservable<IDiscriminatedUnion<T, Exception>> setOrExceptionObservable;

        private readonly IObservable<IDiscriminatedUnion<T, Exception>> changeOrExceptionObservable;

        private readonly IDisposable valueOrExceptionSubjectSubscription;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedProperty{T}"/> class. 
        /// </summary>
        /// <param name="setOrExceptionObservable">
        /// The observable that is the result of the calculation.
        /// </param>
        /// <param name="initialValue">
        /// The initial Value.
        /// </param>
        public CalculatedProperty(
            IObservable<IDiscriminatedUnion<T, Exception>> setOrExceptionObservable,
            IDiscriminatedUnion<T, Exception> initialValue)
        {
            Contract.Requires<ArgumentNullException>(setOrExceptionObservable != null, "setOrExceptionObservable");
            Contract.Requires<ArgumentNullException>(initialValue != null, "initialValue");
            Contract.Ensures(this.valueOrExceptionSubject != null);
            Contract.Ensures(this.valueSubject != null);
            Contract.Ensures(this.exceptionSubject != null);
            Contract.Ensures(this.allNotificationsObservable != null);
            Contract.Ensures(this.changeObservable != null);
            Contract.Ensures(this.exceptionObservable != null);
            Contract.Ensures(this.setOrExceptionObservable != null);
            Contract.Ensures(this.changeOrExceptionObservable != null);
            Contract.Ensures(this.valueOrExceptionSubjectSubscription != null);

            this.setOrExceptionObservable = setOrExceptionObservable;
            this.valueOrExceptionSubject = new BehaviorSubject<IDiscriminatedUnion<T, Exception>>(initialValue);
            this.valueSubject = new BehaviorSubject<T>(initialValue.IsFirst ? initialValue.First : default(T));
            this.exceptionSubject = new BehaviorSubject<Exception>(initialValue.IsSecond ? initialValue.Second : null);
            this.valueOrExceptionSubjectSubscription = this.setOrExceptionObservable.Subscribe(
                v =>
                {
                    this.valueOrExceptionSubject.OnNext(v);
                    v.Switch(this.valueSubject.OnNext, this.exceptionSubject.OnNext);
                });

            if (this.valueOrExceptionSubjectSubscription == null)
            {
                throw new InvalidOperationException("Result of Subscribe cannot be null.");
            }

            this.allNotificationsObservable = this.setOrExceptionObservable.TakeFirst();
            this.changeObservable = this.allNotificationsObservable.DistinctUntilChanged();

            if (this.changeObservable == null)
            {
                throw new InvalidOperationException("Result of DistinctUntilChanged cannot be null.");
            }

            this.exceptionObservable = this.setOrExceptionObservable.TakeSecond();
            this.changeOrExceptionObservable =
                this.changeObservable.Select(DiscriminatedUnion.First<T, Exception>)
                    .Merge(this.exceptionObservable.Select(DiscriminatedUnion.Second<T, Exception>));

            if (this.changeOrExceptionObservable == null)
            {
                throw new InvalidOperationException("Result of Merge cannot be null.");
            }
        }

        /// <summary>
        /// Gets an observable which notifies when a value change occurs.
        /// </summary>
        public IObservable<IDiscriminatedUnion<T, Exception>> OnChanged
        {
            get
            {
                return this.changeOrExceptionObservable;
            }
        }

        /// <summary>
        /// Gets an observable which notifies when a value set occurs.
        /// </summary>
        public IObservable<IDiscriminatedUnion<T, Exception>> OnSet
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
                return this.allNotificationsObservable;
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
        public IDiscriminatedUnion<T, Exception> Value
        {
            get
            {
                if (this.valueOrExceptionSubject.Value == null)
                {
                    throw new InvalidOperationException("Latest value or exception discriminated union cannot be null.");
                }

                return this.valueOrExceptionSubject.Value;
            }
        }

        IDisposable IObservable<IDiscriminatedUnion<T, Exception>>.Subscribe(
            IObserver<IDiscriminatedUnion<T, Exception>> observer)
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

        /// <summary>
        /// Disposes of the object.
        /// </summary>
        public void Dispose()
        {
            this.valueOrExceptionSubject.Dispose();
            this.valueOrExceptionSubjectSubscription.Dispose();
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.valueOrExceptionSubject != null);
            Contract.Invariant(this.valueSubject != null);
            Contract.Invariant(this.exceptionSubject != null);
            Contract.Invariant(this.allNotificationsObservable != null);
            Contract.Invariant(this.changeObservable != null);
            Contract.Invariant(this.exceptionObservable != null);
            Contract.Invariant(this.setOrExceptionObservable != null);
            Contract.Invariant(this.changeOrExceptionObservable != null);
            Contract.Invariant(this.valueOrExceptionSubjectSubscription != null);
        }
    }
}