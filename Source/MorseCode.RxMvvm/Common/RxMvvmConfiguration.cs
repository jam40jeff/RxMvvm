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

namespace MorseCode.RxMvvm.Common
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reactive.Concurrency;

    /// <summary>
    /// A class containing global configuration.
    /// </summary>
    public static class RxMvvmConfiguration
    {
        private static readonly Func<IScheduler> DefaultGetCalculationScheduler;

        private static readonly Func<IScheduler> DefaultGetLongRunningCalculationScheduler;

        private static Func<IScheduler> getNotifyPropertyChangedScheduler;

        private static Func<IScheduler> getCalculationScheduler;

        private static Func<IScheduler> getLongRunningCalculationScheduler;

        /// <summary>
        /// Initializes static members of the <see cref="RxMvvmConfiguration"/> class.
        /// </summary>
        static RxMvvmConfiguration()
        {
            Contract.Ensures(DefaultGetCalculationScheduler != null);
            Contract.Ensures(DefaultGetLongRunningCalculationScheduler != null);

            DefaultGetCalculationScheduler = () => Scheduler.Default;
            DefaultGetLongRunningCalculationScheduler = () => NewThreadScheduler.Default;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use serialization.
        /// </summary>
        public static bool UseSerialization { get; set; }

        /// <summary>
        /// Ensures the delegate is safe to be serialized if using serialization.
        /// </summary>
        /// <param name="d">
        /// The delegate.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Throw if the delegate is not safe to be serialized.
        /// </exception>
        public static void EnsureSerializableDelegateIfUsingSerialization(Delegate d)
        {
            Contract.Requires<ArgumentNullException>(d != null, "d");

            if (UseSerialization)
            {
                if (!d.Method.IsStatic)
                {
                    throw new InvalidOperationException(
                        "Delegates must be static in order for them to be guaranteed to be safe to be serialized.");
                }
            }
        }

        /// <summary>
        /// Gets an <see cref="IScheduler"/> to use to fire <see cref="INotifyPropertyChanged"/> notifications.
        /// If <value>null</value> is returned, <see cref="INotifyPropertyChanged"/> notifications should not be fired.
        /// </summary>
        /// <returns>
        /// An <see cref="IScheduler"/> to use to fire <see cref="INotifyPropertyChanged"/> notifications.
        /// </returns>
        public static IScheduler GetNotifyPropertyChangedScheduler()
        {
            if (getNotifyPropertyChangedScheduler == null)
            {
                return null;
            }

            return getNotifyPropertyChangedScheduler();
        }

        /// <summary>
        /// Sets a function returning an <see cref="IScheduler"/> to use to fire <see cref="INotifyPropertyChanged"/> notifications.
        /// The set notify property changed scheduler factory.
        /// </summary>
        /// <param name="f">
        /// A function returning an <see cref="IScheduler"/> to use to fire <see cref="INotifyPropertyChanged"/> notifications.
        /// </param>
        public static void SetNotifyPropertyChangedSchedulerFactory(Func<IScheduler> f)
        {
            getNotifyPropertyChangedScheduler = f;
        }

        /// <summary>
        /// Gets an <see cref="IScheduler"/> to use for calculations.  The return value will not be <value>null</value>.
        /// </summary>
        /// <returns>
        /// An <see cref="IScheduler"/> to use for calculations.  The return value will not be <value>null</value>.
        /// </returns>
        public static IScheduler GetCalculationScheduler()
        {
            Contract.Ensures(Contract.Result<IScheduler>() != null);

            if (getCalculationScheduler == null)
            {
                return DefaultGetCalculationScheduler();
            }

            IScheduler scheduler = getCalculationScheduler() ?? DefaultGetCalculationScheduler();

            if (scheduler == null)
            {
                throw new InvalidOperationException(
                    StaticReflection.StaticReflection.GetInScopeMemberInfo(() => DefaultGetCalculationScheduler).Name
                    + " must not return null.");
            }

            return scheduler;
        }

        /// <summary>
        /// Sets a function returning an <see cref="IScheduler"/> to use for calculations.
        /// </summary>
        /// <param name="f">
        /// A function returning an <see cref="IScheduler"/> to use for calculations.
        /// If 
        /// <value>
        /// null
        /// </value>
        /// is specified (or returned from the function specified), a default scheduler will be used.
        /// </param>
        public static void SetCalculationSchedulerFactory(Func<IScheduler> f)
        {
            getCalculationScheduler = f;
        }

        /// <summary>
        /// Gets an <see cref="IScheduler"/> to use for long-running calculations.  The return value will not be <value>null</value>.
        /// </summary>
        /// <returns>
        /// An <see cref="IScheduler"/> to use for long-running calculations.  The return value will not be <value>null</value>.
        /// </returns>
        public static IScheduler GetLongRunningCalculationScheduler()
        {
            Contract.Ensures(Contract.Result<IScheduler>() != null);

            if (getLongRunningCalculationScheduler == null)
            {
                return DefaultGetLongRunningCalculationScheduler();
            }

            IScheduler scheduler = getLongRunningCalculationScheduler() ?? DefaultGetLongRunningCalculationScheduler();

            if (scheduler == null)
            {
                throw new InvalidOperationException(
                    StaticReflection.StaticReflection.GetInScopeMemberInfo(
                        () => DefaultGetLongRunningCalculationScheduler).Name + " must not return null.");
            }

            return scheduler;
        }

        /// <summary>
        /// Sets a function returning an <see cref="IScheduler"/> to use for long-running calculations.
        /// </summary>
        /// <param name="f">
        /// A function returning an <see cref="IScheduler"/> to use for long-running calculations.
        /// If 
        /// <value>
        /// null
        /// </value>
        /// is specified (or returned from the function specified), a default scheduler will be used.
        /// </param>
        public static void SetLongRunningCalculationSchedulerFactory(Func<IScheduler> f)
        {
            getLongRunningCalculationScheduler = f;
        }

        [ContractInvariantMethod]
        private static void CodeContractsInvariants()
        {
            Contract.Invariant(DefaultGetCalculationScheduler != null);
            Contract.Invariant(DefaultGetLongRunningCalculationScheduler != null);
        }
    }
}