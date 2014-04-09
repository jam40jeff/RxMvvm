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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    using MorseCode.RxMvvm.Samples.Calculator.ViewModels;
    using MorseCode.RxMvvm.UI.Wpf;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            CalculatorViewModel viewModel = new CalculatorViewModel(false);
            DataContext = viewModel;

            FunctionDropDown.SetBinding(
                ItemsControl.ItemsSourceProperty,
                BindingFactory<CalculatorViewModel>.CreateOneWayBinding(o => o.Operators));

            FunctionDropDown.SetBinding(
                Selector.SelectedItemProperty,
                BindingFactory<CalculatorViewModel>.CreateTwoWayBinding(o => o.SelectedOperator));

            Operand1TextBox.SetBinding(
                TextBox.TextProperty, BindingFactory<CalculatorViewModel>.CreateTwoWayBinding(o => o.Operand1));

            OperatorLabel.SetBinding(
                ContentProperty,
                BindingFactory<CalculatorViewModel>.CreateCalculatedBinding(o => o.SelectedOperatorString));

            Operand2TextBox.SetBinding(
                TextBox.TextProperty, BindingFactory<CalculatorViewModel>.CreateTwoWayBinding(o => o.Operand2));

            ResultLabel.SetBinding(
                ContentProperty, BindingFactory<CalculatorViewModel>.CreateCalculatedBinding(o => o.Result));
        }
    }
}