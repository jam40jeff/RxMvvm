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
    using System;

    using MorseCode.RxMvvm.Observable.Collection;
    using MorseCode.RxMvvm.Observable.Property;

    public class CalculatorViewModel
    {
        private readonly IReadOnlyProperty<IObservableCollection<bool>> updateInRealTimeItems;

        private readonly IObservableProperty<bool?> updateInRealTimeSelection;

        private readonly ICalculatedProperty<bool> updateInRealTime;

        private readonly IReadOnlyProperty<bool> supportsAsync;

        private readonly IReadOnlyProperty<IObservableCollection<bool>> simulateLatencyItems;

        private readonly IObservableProperty<bool?> simulateLatencySelection;

        private readonly ICalculatedProperty<bool> simulateLatency;

        private readonly IReadOnlyProperty<IObservableCollection<Operator>> operators;

        private readonly IObservableProperty<string> operand1;

        private readonly IObservableProperty<Operator?> selectedOperator;

        private readonly ICalculatedProperty<string> selectedOperatorString;

        private readonly IObservableProperty<string> operand2;

        private readonly ICalculatedProperty<string> result;

        public CalculatorViewModel(bool supportsAsync)
        {
            this.updateInRealTimeItems = ObservablePropertyFactory.CreateReadOnlyProperty(ObservableCollectionFactory.CreateObservableCollection(new[] { true, false }));
            this.updateInRealTimeSelection = ObservablePropertyFactory.CreateProperty<bool?>(true);
            this.updateInRealTime = ObservablePropertyFactory.CreateCalculatedProperty(
                this.updateInRealTimeSelection,
                updateInRealTimeSelection => updateInRealTimeSelection.HasValue && updateInRealTimeSelection.Value);
            this.supportsAsync = ObservablePropertyFactory.CreateReadOnlyProperty(supportsAsync);
            this.simulateLatencyItems = ObservablePropertyFactory.CreateReadOnlyProperty(ObservableCollectionFactory.CreateObservableCollection(new[] { true, false }));
            this.simulateLatencySelection = ObservablePropertyFactory.CreateProperty<bool?>(true);
            this.simulateLatency = ObservablePropertyFactory.CreateCalculatedProperty(
                this.simulateLatencySelection,
                simulateLatencySelection => simulateLatencySelection.HasValue && simulateLatencySelection.Value);
            this.operators =
                ObservablePropertyFactory.CreateReadOnlyProperty(ObservableCollectionFactory.CreateObservableCollection(
                    new[] { Operator.Add, Operator.Subtract, Operator.Multiply, Operator.Divide }));
            this.operand1 = ObservablePropertyFactory.CreateProperty<string>(null);
            this.selectedOperator = ObservablePropertyFactory.CreateProperty<Operator?>(Operator.Add);
            this.selectedOperatorString = ObservablePropertyFactory.CreateCalculatedProperty(
                this.selectedOperator,
                selectedOperator =>
                {
                    if (selectedOperator == null)
                    {
                        return null;
                    }

                    switch (selectedOperator.Value)
                    {
                        case Operator.Add:
                            return "+";
                        case Operator.Subtract:
                            return "-";
                        case Operator.Multiply:
                            return "*";
                        case Operator.Divide:
                            return "/";
                        default:
                            throw new NotSupportedException(
                                "Unknown enumeration value " + selectedOperator.Value + ".");
                    }
                });
            this.operand2 = ObservablePropertyFactory.CreateProperty<string>(null);
            this.result = ObservablePropertyFactory.CreateCalculatedProperty(this.Operand1, this.Operand2, this.SelectedOperator,
                                                        (operand1, operand2, selectedOperator) =>
                                                        {
                                                            if (selectedOperator == null)
                                                            {
                                                                return null;
                                                            }

                                                            Func<double?, double?, double?> function;
                                                            switch (selectedOperator.Value)
                                                            {
                                                                case Operator.Add:
                                                                    function = (x, y) => x + y;
                                                                    break;
                                                                case Operator.Subtract:
                                                                    function = (x, y) => x - y;
                                                                    break;
                                                                case Operator.Multiply:
                                                                    function = (x, y) => x * y;
                                                                    break;
                                                                case Operator.Divide:
                                                                    function = (x, y) => x / y;
                                                                    break;
                                                                default:
                                                                    throw new NotSupportedException(
                                                                        "Unknown enumeration value " +
                                                                        selectedOperator.Value + ".");
                                                            }

                                                            double operand1Value;
                                                            double.TryParse(operand1, out operand1Value);
                                                            double operand2Value;
                                                            double.TryParse(operand2, out operand2Value);
                                                            double? r = function(operand1Value, operand2Value);
                                                            return r == null ? null : r.ToString();
                                                        });
        }

        public IReadOnlyProperty<IObservableCollection<bool>> UpdateInRealTimeItems
        {
            get
            {
                return this.updateInRealTimeItems;
            }
        }

        public IObservableProperty<bool?> UpdateInRealTimeSelection
        {
            get
            {
                return this.updateInRealTimeSelection;
            }
        }

        public ICalculatedProperty<bool> UpdateInRealTime
        {
            get
            {
                return this.updateInRealTime;
            }
        }

        public IReadOnlyProperty<IObservableCollection<bool>> SimulateLatencyItems
        {
            get
            {
                return this.simulateLatencyItems;
            }
        }

        public IObservableProperty<bool?> SimulateLatencySelection
        {
            get
            {
                return this.simulateLatencySelection;
            }
        }

        public ICalculatedProperty<bool> SimulateLatency
        {
            get
            {
                return this.simulateLatency;
            }
        }

        public IReadOnlyProperty<IObservableCollection<Operator>> Operators
        {
            get
            {
                return this.operators;
            }
        }

        public IObservableProperty<string> Operand1
        {
            get
            {
                return this.operand1;
            }
        }

        public IObservableProperty<Operator?> SelectedOperator
        {
            get
            {
                return this.selectedOperator;
            }
        }

        public ICalculatedProperty<string> SelectedOperatorString
        {
            get
            {
                return this.selectedOperatorString;
            }
        }

        public IObservableProperty<string> Operand2
        {
            get
            {
                return this.operand2;
            }
        }

        public IReadableObservableProperty<bool> SupportsAsync
        {
            get
            {
                return this.supportsAsync;
            }
        }

		public ICalculatedProperty<string> Result
		{
			get { return this.result; }
		}
    }

    public enum Operator
    {
        Add,

        Subtract,

        Multiply,

        Divide
    }
}