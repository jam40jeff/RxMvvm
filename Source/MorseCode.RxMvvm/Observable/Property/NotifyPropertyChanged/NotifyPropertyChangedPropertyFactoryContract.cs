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

namespace MorseCode.RxMvvm.Observable.Property.NotifyPropertyChanged
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive.Concurrency;

    [ContractClassFor(typeof(INotifyPropertyChangedPropertyFactory))]
    internal abstract class NotifyPropertyChangedPropertyFactoryContract : INotifyPropertyChangedPropertyFactory
    {
        INotifyPropertyChangedProperty<T> INotifyPropertyChangedPropertyFactory.CreateNotifyPropertyChangedProperty<T>(
            IObservableProperty<T> observableProperty, IScheduler scheduler)
        {
            Contract.Requires<ArgumentNullException>(observableProperty != null, "observableProperty");
            Contract.Requires<ArgumentNullException>(scheduler != null, "scheduler");
            Contract.Ensures(Contract.Result<INotifyPropertyChangedProperty<T>>() != null);

            return null;
        }

        IReadableNotifyPropertyChangedProperty<T> INotifyPropertyChangedPropertyFactory.CreateReadOnlyNotifyPropertyChangedProperty<T>(
            IObservable<T> observable, IScheduler scheduler)
        {
            Contract.Requires<ArgumentNullException>(observable != null, "observable");
            Contract.Requires<ArgumentNullException>(scheduler != null, "scheduler");
            Contract.Ensures(Contract.Result<IReadableNotifyPropertyChangedProperty<T>>() != null);

            return null;
        }
    }
}