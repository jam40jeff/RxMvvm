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

namespace MorseCode.RxMvvm.Samples.Calculator.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive.Disposables;

    public class ViewModelBase : IViewModel
    {
        #region Fields

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        #endregion

        #region Explicit Interface Methods

        void IDisposable.Dispose()
        {
            this.compositeDisposable.Dispose();

            this.OnDispose();
        }

        #endregion

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

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.compositeDisposable != null);
        }
    }
}