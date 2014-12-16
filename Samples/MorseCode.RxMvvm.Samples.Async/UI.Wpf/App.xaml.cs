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

namespace UI.Wpf
{
    using MorseCode.RxMvvm.Samples.Async.UI.Wpf;
    using MorseCode.RxMvvm.Samples.Async.ViewModels;
    using MorseCode.RxMvvm.ViewModel;

    public partial class App
    {
        #region Methods

        protected override IApplicationViewModel CreateApplicationViewModel()
        {
            return AsyncApplicationViewModel.Instance;
        }

        protected override void RegisterViews(ViewRegistrationHelper viewRegistrationHelper)
        {
            viewRegistrationHelper.RegisterView(() => new MainWindow())
                                  .WithBinding<MainViewModel>((v, d) => v.BindDataContext(d));
        }

        #endregion
    }
}