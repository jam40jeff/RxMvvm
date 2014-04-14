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
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reactive.Concurrency;

    /// <summary>
    /// A factory for creating properties implementing <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public class NotifyPropertyChangedPropertyFactory : INotifyPropertyChangedPropertyFactory
    {
        INotifyPropertyChangedProperty<T>
            INotifyPropertyChangedPropertyFactory.CreateNotifyCollectionChangedCollection<T>(
            IObservableProperty<T> observableProperty, IScheduler scheduler)
        {
            return new NotifyPropertyChangedProperty<T>(observableProperty, scheduler);
        }

        IReadableNotifyPropertyChangedProperty<T>
            INotifyPropertyChangedPropertyFactory.CreateReadOnlyNotifyCollectionChangedCollection<T>(
            IObservable<T> observable, IScheduler scheduler)
        {
            return new ReadOnlyNotifyPropertyChangedProperty<T>(observable, scheduler);
        }
    }
}