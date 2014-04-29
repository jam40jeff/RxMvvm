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
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows;

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Observable;

    /// <summary>
    /// A base class for a window.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the data context.
    /// </typeparam>
    public abstract class WindowBase<T> : Window, IDataContextControl<T>
        where T : class
    {
        private static readonly IBindingFactory<T> BindingFactoryInternal = BindingFactory<T>.Instance;

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        /// <summary>
        /// Gets the binding factory.
        /// </summary>
        protected IBindingFactory<T> BindingFactory
        {
            get
            {
                Contract.Ensures(Contract.Result<IBindingFactory<T>>() != null);

                return BindingFactoryInternal;
            }
        }

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
        public void BindChainedDataContext<TDataContext>(
            IObservable<TDataContext> dataContext,
            Func<TDataContext, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> getDataContext)
            where TDataContext : class
        {
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getDataContext != null, "getDataContext");

            this.BindDataContextInternal(dataContext, d => getDataContext(d).Select(u => u.Switch(v => v, _ => null)));
        }

        void IDataContextControl<T>.BindChainedDataContext<TDataContext>(
            IObservable<TDataContext> dataContext,
            Func<TDataContext, IObservable<IDiscriminatedUnion<object, T, NonComputable>>> getDataContext)
        {
            this.BindDataContextInternal(dataContext, d => getDataContext(d).Select(u => u.Switch(v => v, _ => null)));
        }

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
        public void BindDataContext<TDataContext>(
            IObservable<TDataContext> dataContext, Func<TDataContext, IObservable<T>> getDataContext)
            where TDataContext : class
        {
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getDataContext != null, "getDataContext");

            this.BindDataContextInternal(dataContext, getDataContext);
        }

        void IDataContextControl<T>.BindDataContext<TDataContext>(
            IObservable<TDataContext> dataContext, Func<TDataContext, IObservable<T>> getDataContext)
        {
            this.BindDataContextInternal(dataContext, getDataContext);
        }

        void IDisposable.Dispose()
        {
            this.compositeDisposable.Dispose();

            this.OnDispose();
        }

        /// <summary>
        /// A method which must be overridden to bind the child controls.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        protected abstract void BindControls(IObservable<T> dataContext);

        /// <summary>
        /// Adds a disposable to be disposed of when this window is disposed.
        /// </summary>
        /// <param name="disposable">
        /// The disposable.
        /// </param>
        protected void AddDisposable(IDisposable disposable)
        {
            this.compositeDisposable.Add(disposable);
        }

        /// <summary>
        /// A method which may be overridden to handle disposal.
        /// </summary>
        protected virtual void OnDispose()
        {
        }

        private void BindDataContextInternal<TDataContext>(
            IObservable<TDataContext> dataContext, Func<TDataContext, IObservable<T>> getDataContext)
        {
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getDataContext != null, "getDataContext");

            this.BindControls(dataContext.BeginChain().Add(getDataContext).CompleteWithDefaultIfNotComputable());
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(BindingFactoryInternal != null);
            Contract.Invariant(this.compositeDisposable != null);
        }
    }
}