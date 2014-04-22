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
    using System;
    using System.Reactive.Disposables;

    using MorseCode.RxMvvm.Observable.Property;
    using MorseCode.RxMvvm.Samples.Calculator.ViewModels;
    using MorseCode.RxMvvm.UI.Wpf;
    using MorseCode.RxMvvm.UI.Wpf.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDisposable
    {
        private static readonly IBindingFactory<CalculatorViewModel> BindingFactory =
            BindingFactory<CalculatorViewModel>.Instance;

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        public MainWindow()
        {
            this.InitializeComponent();

            CalculatorViewModel viewModel = new CalculatorViewModel(false);
            CalculatorViewModel viewModel2 = new CalculatorViewModel(false);

            IObservableProperty<CalculatorViewModel> viewModelProperty =
                ObservablePropertyFactory.Instance.CreateProperty(viewModel);

            //FunctionDropDown.BindItemsForStructWithNoSelection(viewModelProperty, d => d.Operators, o => o.ToString(), "[Select an Item]", d => d.SelectedOperator, BindingFactory);
            this.compositeDisposable.Add(FunctionDropDown.BindItemsForStruct(
                viewModelProperty, d => d.Operators, o => o.ToString(), d => d.SelectedOperator, BindingFactory));
            this.compositeDisposable.Add(Operand1TextBox.BindText(viewModelProperty, o => o.Operand1, BindingFactory));
            this.compositeDisposable.Add(OperatorLabel.BindContent(viewModelProperty, o => o.SelectedOperatorString.OnValueOrDefaultChanged, BindingFactory));
            this.compositeDisposable.Add(Operand2TextBox.BindText(viewModelProperty, o => o.Operand2, BindingFactory));
            this.compositeDisposable.Add(ResultLabel.BindContent(viewModelProperty, o => o.Result.OnValueOrDefaultChanged, BindingFactory));

            SwitchDataContextButton.Click += (sender, args) => viewModelProperty.Value = viewModelProperty.Value == viewModel ? viewModel2 : viewModel;
            SwitchOperatorsButton.Click += (sender, args) => viewModelProperty.Value.SwitchOperators();
        }

        public void Dispose()
        {
            this.compositeDisposable.Dispose();
        }
    }
}