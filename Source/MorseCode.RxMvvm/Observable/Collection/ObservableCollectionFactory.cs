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

namespace MorseCode.RxMvvm.Observable.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A factory for creating observable collections.
    /// </summary>
    public class ObservableCollectionFactory : IObservableCollectionFactory
    {
        private static readonly Lazy<ObservableCollectionFactory> InstanceLazy =
            new Lazy<ObservableCollectionFactory>(() => new ObservableCollectionFactory());

        private ObservableCollectionFactory()
        {
        }

        /// <summary>
        /// Gets the singleton instance of an <see cref="ObservableCollectionFactory"/>.
        /// </summary>
        public static IObservableCollectionFactory Instance
        {
            get
            {
                return InstanceLazy.Value;
            }
        }

        IObservableCollection<T> IObservableCollectionFactory.CreateObservableCollection<T>()
        {
            return new ObservableCollection<T>();
        }

        IObservableCollection<T> IObservableCollectionFactory.CreateObservableCollection<T>(IEnumerable<T> list)
        {
            return new ObservableCollection<T>(list.ToList());
        }

        IReadableObservableCollection<T> IObservableCollectionFactory.CreateReadOnlyObservableCollection<T>(IEnumerable<T> list)
        {
            return new ReadOnlyObservableCollection<T>(list.ToList());
        }
    }
}