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
    using System.Diagnostics.Contracts;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Common.StaticReflection;

    [Serializable]
    internal class ObservableProperty<T> : ReadableObservablePropertyBase<T>, IObservableProperty<T>, ISerializable
    {
        private readonly BehaviorSubject<T> behaviorSubject;

        private readonly IObservable<T> allNotificationsObservable;

        private readonly IObservable<T> changeObservable;

        private readonly IDisposable onChangedSubscription;

        internal ObservableProperty(T initialValue)
        {
            Contract.Ensures(this.behaviorSubject != null);
            Contract.Ensures(this.allNotificationsObservable != null);
            Contract.Ensures(this.changeObservable != null);

            this.behaviorSubject = new BehaviorSubject<T>(initialValue);

            this.changeObservable = this.behaviorSubject.DistinctUntilChanged();
            if (this.changeObservable == null)
            {
                throw new InvalidOperationException(
                    StaticReflection.GetInScopeMemberInfo(() => this.changeObservable).Name + " may not be null.");
            }

            this.allNotificationsObservable = this.behaviorSubject.AsObservable();
            if (this.allNotificationsObservable == null)
            {
                throw new InvalidOperationException(
                    StaticReflection.GetInScopeMemberInfo(() => this.allNotificationsObservable).Name
                    + " may not be null.");
            }

            IScheduler notifyPropertyChangedScheduler = RxMvvmConfiguration.GetNotifyPropertyChangedScheduler();
            if (notifyPropertyChangedScheduler != null)
            {
                this.onChangedSubscription = this.changeObservable.Skip(1).ObserveOn(notifyPropertyChangedScheduler).Subscribe(v => this.OnValueChanged());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableProperty{T}"/> class from serialized data.
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The serialization context.
        /// </param>
        [ContractVerification(false)]
        // ReSharper disable UnusedParameter.Local
        protected ObservableProperty(SerializationInfo info, StreamingContext context)
            // ReSharper restore UnusedParameter.Local
            : this((T)(info.GetValue("v", typeof(T)) ?? default(T)))
        {
        }

        T IWritableObservableProperty<T>.Value
        {
            set
            {
                this.Value = value;
            }
        }

        T IObservableProperty<T>.Value
        {
            get
            {
                return this.Value;
            }

            set
            {
                this.Value = value;
            }
        }

        public new T Value
        {
            get
            {
                return this.GetValue();
            }

            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// Gets the on changed observable.
        /// </summary>
        protected override IObservable<T> OnChanged
        {
            get
            {
                return this.changeObservable;
            }
        }

        /// <summary>
        /// Gets the on set observable.
        /// </summary>
        protected override IObservable<T> OnSet
        {
            get
            {
                return this.allNotificationsObservable;
            }
        }

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
            info.AddValue("v", this.behaviorSubject.Value);
        }

        /// <summary>
        /// Disposes of the property.
        /// </summary>
        protected override void Dispose()
        {
            base.Dispose();

            this.behaviorSubject.Dispose();

            using (this.onChangedSubscription)
            {
            }
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <returns>
        /// The value of the property.
        /// </returns>
        protected override T GetValue()
        {
            return this.behaviorSubject.Value;
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="value">
        /// The value to set the property to.
        /// </param>
        protected virtual void SetValue(T value)
        {
            this.behaviorSubject.OnNext(value);
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.behaviorSubject != null);
            Contract.Invariant(this.allNotificationsObservable != null);
            Contract.Invariant(this.changeObservable != null);
        }
    }
}