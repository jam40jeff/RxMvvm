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
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Common.StaticReflection;
    using MorseCode.RxMvvm.Reactive;

    [Serializable]
    internal abstract class CalculatedPropertyBase<T> :
        ReadableObservablePropertyBase<IDiscriminatedUnion<object, T, Exception>>,
        ICalculatedProperty<T>
    {
        private readonly CompositeDisposable subscriptionsDisposable = new CompositeDisposable();

        private CalculatedPropertyHelper helper;

        IObservable<T> ICalculatedProperty<T>.OnSuccessfulValueChanged
        {
            get
            {
                return this.Helper.OnSuccessfulValueChanged;
            }
        }

        IObservable<T> ICalculatedProperty<T>.OnSuccessfulValueSet
        {
            get
            {
                return this.Helper.OnSuccessfulValueSet;
            }
        }

        IObservable<T> ICalculatedProperty<T>.OnValueOrDefaultChanged
        {
            get
            {
                return this.GetValueOrDefault(this.helper.OnChanged);
            }
        }

        IObservable<T> ICalculatedProperty<T>.OnValueOrDefaultSet
        {
            get
            {
                return this.GetValueOrDefault(this.helper.OnSet);
            }
        }

        IObservable<Exception> ICalculatedProperty<T>.OnCalculationException
        {
            get
            {
                return this.Helper.OnCalculationException;
            }
        }

        T ICalculatedProperty<T>.LatestSuccessfulValue
        {
            get
            {
                return this.Helper.LatestSuccessfulValue;
            }
        }

        Exception ICalculatedProperty<T>.LatestCalculationException
        {
            get
            {
                return this.Helper.LatestCalculationException;
            }
        }

        /// <summary>
        /// Gets the on changed observable.
        /// </summary>
        protected override IObservable<IDiscriminatedUnion<object, T, Exception>> OnChanged
        {
            get
            {
                return this.Helper.OnChanged;
            }
        }

        /// <summary>
        /// Gets the on set observable.
        /// </summary>
        protected override IObservable<IDiscriminatedUnion<object, T, Exception>> OnSet
        {
            get
            {
                return this.Helper.OnSet;
            }
        }

        private CalculatedPropertyHelper Helper
        {
            get
            {
                Contract.Ensures(Contract.Result<CalculatedPropertyHelper>() != null);

                if (this.helper == null)
                {
                    throw new InvalidOperationException(
                        StaticReflection.GetInScopeMethodInfo(() => this.SetHelper(null)).Name
                        + " must be called before " + StaticReflection.GetInScopeMemberInfo(() => this.Helper).Name
                        + " may be accessed.");
                }

                return this.helper;
            }
        }

        T ICalculatedProperty<T>.GetSuccessfulValueOrThrowException()
        {
            return this.Helper.GetSuccessfulValueOrThrowException();
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <returns>
        /// The value of the property.
        /// </returns>
        protected override IDiscriminatedUnion<object, T, Exception> GetValue()
        {
            return this.Helper.GetValue();
        }

        /// <summary>
        /// Disposes of the property.
        /// </summary>
        protected override void Dispose()
        {
            base.Dispose();

            this.subscriptionsDisposable.Dispose();

            using (this.helper)
            {
            }
        }

        /// <summary>
        /// Sets the calculated property helper.
        /// </summary>
        /// <param name="calculatedPropertyHelper">
        /// The calculated property helper.
        /// </param>
        protected void SetHelper(CalculatedPropertyHelper calculatedPropertyHelper)
        {
            Contract.Requires<ArgumentNullException>(calculatedPropertyHelper != null, "calculatedPropertyHelper");
            Contract.Ensures(this.helper != null);

            if (this.helper != null)
            {
                throw new InvalidOperationException(
                    StaticReflection.GetInScopeMethodInfo(() => this.SetHelper(null)).Name + " may only be called once.");
            }

            IScheduler notifyPropertyChangedScheduler = RxMvvmConfiguration.GetNotifyPropertyChangedScheduler();
            if (notifyPropertyChangedScheduler != null)
            {
                this.subscriptionsDisposable.Add(
                    calculatedPropertyHelper.OnChanged.Skip(1).ObserveOn(notifyPropertyChangedScheduler).Subscribe(v => this.OnValueChanged()));
                this.subscriptionsDisposable.Add(
                    calculatedPropertyHelper.OnSuccessfulValueChanged.Skip(1).ObserveOn(notifyPropertyChangedScheduler).Subscribe(v => this.OnLatestSuccessfulValueChanged()));
                this.subscriptionsDisposable.Add(
                    calculatedPropertyHelper.OnCalculationException.Skip(1).ObserveOn(notifyPropertyChangedScheduler).Subscribe(v => this.OnLatestCalculationExceptionChanged()));
            }

            this.helper = calculatedPropertyHelper;
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

        private IObservable<T> GetValueOrDefault(IObservable<IDiscriminatedUnion<object, T, Exception>> o)
        {
            Contract.Requires<ArgumentNullException>(o != null, "o");
            Contract.Ensures(Contract.Result<IObservable<T>>() != null);

            IObservable<T> result = o.Select(d => d.Switch(v => v, e => default(T)));

            if (result == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IObservable<T>>.GetMethodInfo(o2 => o2.Select<T, object>(o3 => null)).Name
                    + " cannot be null.");
            }

            return result;
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.subscriptionsDisposable != null);
        }

        /// <summary>
        /// A helper class for calculated properties.
        /// </summary>
        protected class CalculatedPropertyHelper : IDisposable
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

            internal CalculatedPropertyHelper(
                Func<BehaviorSubject<IDiscriminatedUnion<object, T, Exception>>, BehaviorSubject<bool>, IDisposable> registerCalculation)
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

                this.valueOrExceptionSubject =
                    new BehaviorSubject<IDiscriminatedUnion<object, T, Exception>>(
                        DiscriminatedUnion.First<object, T, Exception>(default(T)));
                this.valueSubject = new BehaviorSubject<T>(default(T));
                this.exceptionSubject = new BehaviorSubject<Exception>(null);
                this.isCalculatingSubject = new BehaviorSubject<bool>(false);

                this.subscriptionsDisposable.Add(
                    this.valueOrExceptionSubject.Subscribe(
                        v => v.Switch(this.valueSubject.OnNext, this.exceptionSubject.OnNext)));

                this.subscriptionsDisposable.Add(
                    registerCalculation(this.valueOrExceptionSubject, this.isCalculatingSubject));

                this.setOrExceptionObservable = this.valueOrExceptionSubject.AsObservable();

                if (this.setOrExceptionObservable == null)
                {
                    throw new InvalidOperationException(
                        "Result of "
                        + StaticReflection<BehaviorSubject<IDiscriminatedUnion<object, T, Exception>>>.GetMethodInfo(
                            o => o.AsObservable()).Name + " cannot be null.");
                }

                this.setObservable = this.setOrExceptionObservable.TakeFirst();

                if (this.setObservable == null)
                {
                    throw new InvalidOperationException(
                        "Result of "
                        + StaticReflection<BehaviorSubject<IDiscriminatedUnion<object, T, Exception>>>.GetMethodInfo(
                            o => o.TakeFirst()).Name + " cannot be null.");
                }

                this.changeObservable = this.setObservable.DistinctUntilChanged();

                if (this.changeObservable == null)
                {
                    throw new InvalidOperationException(
                        "Result of "
                        + StaticReflection<IObservable<T>>.GetMethodInfo(o => o.DistinctUntilChanged()).Name
                        + " cannot be null.");
                }

                this.exceptionObservable = this.setOrExceptionObservable.TakeSecond();
                this.changeOrExceptionObservable =
                    this.changeObservable.Select(DiscriminatedUnion.First<object, T, Exception>)
                        .Merge(this.exceptionObservable.Select(DiscriminatedUnion.Second<object, T, Exception>));

                if (this.changeOrExceptionObservable == null)
                {
                    throw new InvalidOperationException(
                        "Result of "
                        + StaticReflection<IObservable<IDiscriminatedUnion<object, T, Exception>>>.GetMethodInfo(
                            o => o.Merge(null)).Name + " cannot be null.");
                }
            }

            internal IObservable<T> OnSuccessfulValueChanged
            {
                get
                {
                    return this.changeObservable;
                }
            }

            internal IObservable<T> OnSuccessfulValueSet
            {
                get
                {
                    return this.setObservable;
                }
            }

            internal IObservable<Exception> OnCalculationException
            {
                get
                {
                    return this.exceptionObservable;
                }
            }

            internal T LatestSuccessfulValue
            {
                get
                {
                    return this.valueSubject.Value;
                }
            }

            internal Exception LatestCalculationException
            {
                get
                {
                    return this.exceptionSubject.Value;
                }
            }

            internal IObservable<IDiscriminatedUnion<object, T, Exception>> OnChanged
            {
                get
                {
                    return this.changeOrExceptionObservable;
                }
            }

            internal IObservable<IDiscriminatedUnion<object, T, Exception>> OnSet
            {
                get
                {
                    return this.setOrExceptionObservable;
                }
            }

            void IDisposable.Dispose()
            {
                this.valueOrExceptionSubject.Dispose();
                this.isCalculatingSubject.Dispose();
                this.valueSubject.Dispose();
                this.exceptionSubject.Dispose();
                this.subscriptionsDisposable.Dispose();
            }

            internal T GetSuccessfulValueOrThrowException()
            {
                if (this.valueOrExceptionSubject.Value == null)
                {
                    throw new InvalidOperationException("Latest value or exception discriminated union cannot be null.");
                }

                return this.valueOrExceptionSubject.Value.Switch(v => v, e => { throw e; });
            }

            internal IDiscriminatedUnion<object, T, Exception> GetValue()
            {
                Contract.Ensures(Contract.Result<IDiscriminatedUnion<object, T, Exception>>() != null);

                if (this.valueOrExceptionSubject.Value == null)
                {
                    throw new InvalidOperationException("Latest value or exception discriminated union cannot be null.");
                }

                return this.valueOrExceptionSubject.Value;
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
}