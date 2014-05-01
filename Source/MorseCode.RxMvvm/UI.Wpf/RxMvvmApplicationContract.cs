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

namespace MorseCode.RxMvvm.UI.Wpf
{
    using System;
    using System.Diagnostics.Contracts;

    using MorseCode.RxMvvm.ViewModel;

    [ContractClassFor(typeof(RxMvvmApplicationBase))]
    internal abstract class RxMvvmApplicationContract : RxMvvmApplicationBase
    {
        /// <summary>
        /// Creates the application view model.
        /// </summary>
        /// <returns>
        /// The application view model.
        /// </returns>
        protected override IApplicationViewModel CreateApplicationViewModel()
        {
            Contract.Ensures(Contract.Result<IApplicationViewModel>() != null);

            return null;
        }

        /// <summary>
        /// Registers the views.
        /// </summary>
        /// <param name="viewRegistrationHelper">
        /// The view registration helper.
        /// </param>
        protected override void RegisterViews(ViewRegistrationHelper viewRegistrationHelper)
        {
            Contract.Requires<ArgumentNullException>(viewRegistrationHelper != null);
        }
    }
}