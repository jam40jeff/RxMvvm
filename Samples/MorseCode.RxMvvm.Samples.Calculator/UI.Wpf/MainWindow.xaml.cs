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
    using MorseCode.RxMvvm.Observable.Property;
    using MorseCode.RxMvvm.Samples.Calculator.ViewModels;
    using MorseCode.RxMvvm.UI.Wpf;
    using MorseCode.RxMvvm.UI.Wpf.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static readonly IBindingFactory<CalculatorViewModel> BindingFactory =
            BindingFactory<CalculatorViewModel>.Instance;

        public MainWindow()
        {
            this.InitializeComponent();

            CalculatorViewModel viewModel = new CalculatorViewModel(false);
            DataContext = viewModel;

            IReadableObservableProperty<CalculatorViewModel> viewModelProperty =
                ObservablePropertyFactory.Instance.CreateReadOnlyProperty(viewModel);

            //FunctionDropDown.BindItemsForStructWithNoSelection(viewModelProperty, d => d.Operators, o => o.ToString(), "[Select an Item]", d => d.SelectedOperator, BindingFactory);
            FunctionDropDown.BindItemsForStruct(viewModelProperty, d => d.Operators, o => o.ToString(), d => d.SelectedOperator, BindingFactory);
            Operand1TextBox.BindText(viewModelProperty, o => o.Operand1, BindingFactory);
            OperatorLabel.BindContent(
                viewModelProperty, o => o.SelectedOperatorString, BindingFactory);
            Operand2TextBox.BindText(viewModelProperty, o => o.Operand2, BindingFactory);
            ResultLabel.BindContent(viewModelProperty, o => o.Result, BindingFactory);
        }
    }
}