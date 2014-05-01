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
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Observable.Property;
    using MorseCode.RxMvvm.Samples.Calculator.ViewModels;
    using MorseCode.RxMvvm.UI.Wpf.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IObservableProperty<CalculatorViewModel> viewModelProperty;

        private readonly CalculatorViewModel viewModel1;

        private readonly CalculatorViewModel viewModel2;

        public MainWindow()
        {
            this.InitializeComponent();

            this.viewModel1 = new CalculatorViewModel(false);
            this.viewModel2 = new CalculatorViewModel(false);

            this.viewModelProperty = ObservablePropertyFactory.Instance.CreateProperty(this.viewModel1);
        }

        protected override void BindControls(IObservable<CalculatorViewModel> dataContext)
        {
            //FunctionDropDown.BindItemsForStructWithNoSelection(dataContext, d => d.Operators, o => o.ToString(), "[Select an Item]", d => d.SelectedOperator, BindingFactory);
            AddDisposable(
                FunctionDropDown.BindItemsForStruct(
                    dataContext, d => d.Operators, o => o.ToString(), d => d.SelectedOperator));
            AddDisposable(Operand1TextBox.BindText(dataContext, o => o.Operand1, BindingFactory));
            AddDisposable(
                OperatorLabel.BindContent(
                    dataContext, o => o.SelectedOperatorString.OnValueOrDefaultChanged, BindingFactory));
            AddDisposable(Operand2TextBox.BindText(dataContext, o => o.Operand2, BindingFactory));
            AddDisposable(ResultLabel.BindContent(dataContext, o => o.Result.OnValueOrDefaultChanged, BindingFactory));
            AddDisposable(
                RadioButtonBindingUtility.BindGroup(
                    dataContext, d => d.SelectedOperator, o => o.ToString(), BindingFactory)
                                         .Add(AddRadioButton, d => Observable.Return(Operator.Add))
                                         .Add(SubtractRadioButton, d => Observable.Return(Operator.Subtract))
                                         .Add(MultiplyRadioButton, d => Observable.Return(Operator.Multiply))
                                         .Add(DivideRadioButton, d => Observable.Return(Operator.Divide))
                                         .Complete());
            AddDisposable(
                MultiplyRadioButton.BindVisibility(
                    dataContext, d => d.ShowMultiply.OnSuccessfulValueChanged, BindingFactory));
            AddDisposable(
                DivideRadioButton.BindVisibility(
                    dataContext, d => d.ShowDivide.OnSuccessfulValueChanged, BindingFactory));

            AddDisposable(
                SwitchDataContextButton.BindClick(
                    dataContext,
                    d => d.SwitchDataContext,
                    BindingFactory));
            AddDisposable(SwitchOperatorsButton.BindClick(dataContext, d => d.SwitchOperators, BindingFactory));
        }
    }
}