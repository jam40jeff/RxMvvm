#region License

// Copyright 2014 MorseCode Software
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

namespace MorseCode.RxMvvm.Samples.Calculator.UI.Wpf
{
    using MorseCode.RxMvvm.Samples.Calculator.ViewModels;
    using MorseCode.RxMvvm.ViewModel;

    public partial class App
    {
        protected override IApplicationViewModel CreateApplicationViewModel()
        {
            return CalculatorApplicationViewModel.Instance;
        }

        protected override void RegisterViews(ViewRegistrationHelper viewRegistrationHelper)
        {
            viewRegistrationHelper.RegisterView(() => new MainWindow())
                                  .WithBinding<CalculatorViewModel>(
                                      (v, d) => v.BindDataContext(d));

            viewRegistrationHelper.RegisterView(() => new MainWindow2())
                                  .WithBinding<MainViewModel>(
                                      (v, d) => v.BindDataContext(d));

            viewRegistrationHelper.RegisterView(() => new MainWindow3())
                                  .WithBinding<Main3ViewModel>(
                                      (v, d) => v.BindDataContext(d));
        }
    }
}