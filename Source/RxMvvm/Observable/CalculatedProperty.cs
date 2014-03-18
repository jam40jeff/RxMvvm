namespace MorseCode.RxMvvm.Observable
{
    using System;
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

        private readonly IObservable<IDiscriminatedUnion<T, Exception>> changeOrExceptionObservable;

        private readonly IDisposable combineLatestSubscription;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedProperty{T}"/> class. 
        /// </summary>
        /// <param name="calculateValue">
        /// Function to calculate the value.
        /// </param>
        /// <param name="combineLatest">
        /// Function to combine latest.
        /// </param>
        public CalculatedProperty(
            Func<T> calculateValue, 
            Func<Func<T, IDiscriminatedUnion<T, Exception>>, IObservable<IDiscriminatedUnion<T, Exception>>> combineLatest)
        {
            Func<T, IDiscriminatedUnion<T, Exception>> calculate = var =>
                {
                    IDiscriminatedUnion<T, Exception> discriminatedUnion;
                    try
                    {
                        discriminatedUnion = DiscriminatedUnion.First<T, Exception>(calculateValue());
                    }
                    catch (Exception e)
                    {
                        discriminatedUnion = DiscriminatedUnion.Second<T, Exception>(e);
                    }

                    return discriminatedUnion;
                };
            this.changeOrExceptionObservable = combineLatest(calculate);
            this.allNotificationsObservable = this.changeOrExceptionObservable.TakeFirst();
            this.valueOrExceptionSubject =
                new BehaviorSubject<IDiscriminatedUnion<T, Exception>>(calculate(calculateValue()));
            this.combineLatestSubscription =
                this.changeOrExceptionObservable.Subscribe(this.valueOrExceptionSubject.OnNext);

            this.allNotificationsObservable = this.changeOrExceptionObservable.TakeFirst();
            this.changeObservable = this.allNotificationsObservable.DistinctUntilChanged();
            this.exceptionObservable = this.changeOrExceptionObservable.TakeSecond();
        }

        /// <summary>
        /// Gets an observable which notifies when a value change occurs.
        /// </summary>
        public IObservable<T> OnChanged
        {
            get
            {
                return this.changeObservable;
            }
        }

        /// <summary>
        /// Gets an observable which notifies when a value set occurs.
        /// </summary>
        public IObservable<T> OnSet
        {
            get
            {
                return this.allNotificationsObservable;
            }
        }

        /// <summary>
        /// Gets an observable which notifies when an error occurs.
        /// </summary>
        public IObservable<Exception> OnCalculationError
        {
            get
            {
                return this.exceptionObservable;
            }
        }

        /// <summary>
        /// Gets the latest value.
        /// </summary>
        public Either<T, Exception> Value
        {
            get
            {
                return this.behaviorSubject.FirstAsync().Wait();
            }
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            return this.changeObservable.Subscribe(observer);
        }

        /// <summary>
        /// Disposes of the object.
        /// </summary>
        public void Dispose()
        {
            this.valueOrExceptionSubject.Dispose();
            this.combineLatestSubscription.Dispose();
        }
    }
}