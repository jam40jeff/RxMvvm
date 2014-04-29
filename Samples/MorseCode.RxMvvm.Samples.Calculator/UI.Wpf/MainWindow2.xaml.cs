﻿#region License

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
    using System;
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Observable.Property;
    using MorseCode.RxMvvm.Samples.Calculator.ViewModels;
    using MorseCode.RxMvvm.UI.Wpf.Controls;

    /// <summary>
    /// Interaction logic for MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2
    {
        private readonly IReadOnlyProperty<MainViewModel> viewModelProperty;

        public MainWindow2()
        {
            this.InitializeComponent();

            this.viewModelProperty = ObservablePropertyFactory.Instance.CreateReadOnlyProperty(new MainViewModel());

            IDataContextControl<MainViewModel> c = this;
            c.BindDataContext(Observable.Return(new MainViewModel()), d => this.viewModelProperty);
        }

        protected override void BindControls(IObservable<MainViewModel> dataContext)
        {
            CalculatorUserControl.BindDataContext(dataContext, d => d.CurrentCalculator);
            AddDisposable(SwitchCalculatorsButton.BindClick(dataContext, d => d.SwitchCalculators, BindingFactory));
        }
    }
}