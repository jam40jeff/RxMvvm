﻿#region License

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

namespace MorseCode.RxMvvm.Observable.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// An interface representing an observable collection which may be read from.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the objects in the collection.
    /// </typeparam>
    [ContractClass(typeof(ReadableObservableCollectionContract<>))]
    public interface IReadableObservableCollection<out T> : IReadOnlyList<T>, 
                                                            IObservable<IObservableCollectionChanged<T>>, 
                                                            INotifyPropertyChanged, 
                                                            INotifyCollectionChanged, 
                                                            IDisposable
    {
    }
}