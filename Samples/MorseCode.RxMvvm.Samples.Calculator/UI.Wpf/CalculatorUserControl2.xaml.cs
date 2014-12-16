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

namespace MorseCode.RxMvvm.Samples.Calculator.UI.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    using MorseCode.RxMvvm.Samples.Calculator.ViewModels;

    public partial class CalculatorUserControl2 : UserControl
    {
        #region Constructors and Destructors

        public CalculatorUserControl2()
        {
            this.InitializeComponent();

            Tuple<RadioButton, Operator>[] radioButtonsAndOperators = new[]
                                                                          {
                                                                              Tuple.Create(
                                                                                  this.AddRadioButton, Operator.Add),
                                                                              Tuple.Create(
                                                                                  this.SubtractRadioButton,
                                                                                  Operator.Subtract),
                                                                              Tuple.Create(
                                                                                  this.MultiplyRadioButton, Operator.Multiply)
                                                                              ,
                                                                              Tuple.Create(
                                                                                  this.DivideRadioButton,
                                                                                  Operator.Divide)
                                                                          };

            foreach (Tuple<RadioButton, Operator> radioButtonAndOperator in radioButtonsAndOperators)
            {
                Binding isCheckedBinding = new Binding("SelectedOperator.Value");
                isCheckedBinding.Converter = new IsCheckedOperatorConverter(radioButtonAndOperator.Item2);
                radioButtonAndOperator.Item1.SetBinding(ToggleButton.IsCheckedProperty, isCheckedBinding);

                radioButtonAndOperator.Item1.Content = radioButtonAndOperator.Item2.ToString();
            }
        }

        #endregion

        #region Methods

        private void SwitchOperatorsButtonClick(object sender, RoutedEventArgs e)
        {
            ((CalculatorViewModel)this.DataContext).SwitchOperators();
        }

        #endregion

        private class IsCheckedOperatorConverter : IValueConverter
        {
            #region Fields

            private readonly Operator @operator;

            #endregion

            #region Constructors and Destructors

            public IsCheckedOperatorConverter(Operator @operator)
            {
                this.@operator = @operator;
            }

            #endregion

            #region Public Methods and Operators

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Operator? @operator = value as Operator?;
                return @operator != null && @operator.Value == this.@operator;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                bool? v = value as bool?;
                return v != null && v.Value ? this.@operator : Binding.DoNothing;
            }

            #endregion
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? v = value as bool?;
            return v != null && v.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}