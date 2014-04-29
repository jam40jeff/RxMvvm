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

namespace MorseCode.RxMvvm.Samples.Calculator.ViewModels
{
    using MorseCode.RxMvvm.Observable.Property;

    public class MainViewModel
    {
        private readonly CalculatorViewModel calculator1;

        private readonly CalculatorViewModel calculator2;

        private readonly IObservableProperty<CalculatorViewModel> currentCalculator;

        public MainViewModel()
        {
            this.calculator1 = new CalculatorViewModel(false);
            this.calculator2 = new CalculatorViewModel(false);

            this.currentCalculator = ObservablePropertyFactory.Instance.CreateProperty(this.calculator1);
        }

        public IObservableProperty<CalculatorViewModel> CurrentCalculator
        {
            get
            {
                return this.currentCalculator;
            }
        }

        public void SwitchCalculators()
        {
            this.currentCalculator.Value = this.currentCalculator.Value == this.calculator1
                                               ? this.calculator2
                                               : this.calculator1;
        }
    }
}