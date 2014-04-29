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

namespace MorseCode.RxMvvm.UI.Wpf.Controls
{
    using System;
    using System.Diagnostics.Contracts;

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Observable;

    // ReSharper disable UnusedTypeParameter

    /// <summary>
    /// An interface representing a control which has a data context.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the data context.
    /// </typeparam>
    [ContractClass(typeof(DataContextControlContract<>))]
    public interface IDataContextControl<in T> : IDisposable
    {
        // ReSharper restore UnusedTypeParameter

        /// <summary>
        /// Binds the data context.
        /// </summary>
        /// <param name="dataContext">
        /// The data context to bind from.
        /// </param>
        /// <param name="getDataContext">
        /// A delegate to get the data context to bind to.
        /// </param>
        /// <typeparam name="TDataContext">
        /// The type of the data context to bind from.
        /// </typeparam>
        void BindChainedDataContext<TDataContext>(
            IObservable<TDataContext> dataContext, 
            Func<TDataContext, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> getDataContext)
            where TDataContext : class;

        /// <summary>
        /// Binds the data context.
        /// </summary>
        /// <param name="dataContext">
        /// The data context to bind from.
        /// </param>
        /// <param name="getDataContext">
        /// A delegate to get the data context to bind to.
        /// </param>
        /// <typeparam name="TDataContext">
        /// The type of the data context to bind from.
        /// </typeparam>
        void BindDataContext<TDataContext>(
            IObservable<TDataContext> dataContext, Func<TDataContext, IObservable<T>> getDataContext)
            where TDataContext : class;
    }
}