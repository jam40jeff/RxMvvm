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

namespace MorseCode.RxMvvm.Samples.Async.ViewModels
{
    using System;

    using MorseCode.RxMvvm.ViewModel;

    public class AsyncApplicationViewModel : ApplicationViewModelBase, IAsyncApplicationViewModel
    {
        #region Static Fields

        private static readonly Lazy<AsyncApplicationViewModel> InstanceLazy =
            new Lazy<AsyncApplicationViewModel>(() => new AsyncApplicationViewModel());

        #endregion

        #region Constructors and Destructors

        private AsyncApplicationViewModel()
        {
        }

        #endregion

        #region Public Properties

        public static IAsyncApplicationViewModel Instance
        {
            get
            {
                return InstanceLazy.Value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void NavigateToMainPage()
        {
            this.CurrentViewModelInternal.Value = new MainViewModel();
        }

        #endregion

        #region Methods

        protected override void Initialize()
        {
            this.NavigateToMainPage();
        }

        #endregion
    }
}