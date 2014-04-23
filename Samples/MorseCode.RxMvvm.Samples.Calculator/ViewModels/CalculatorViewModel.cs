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

    [Serializable]
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

        private readonly IObservableProperty<Operator> selectedOperator;

        private readonly ICalculatedProperty<string> selectedOperatorString;

        private readonly IObservableProperty<string> operand2;

        private readonly ICalculatedProperty<string> result;

        public CalculatorViewModel(bool supportsAsync)
        {
            this.updateInRealTimeItems =
                ObservablePropertyFactory.Instance.CreateReadOnlyProperty(
                    ObservableCollectionFactory.Instance.CreateObservableCollection(new[] { true, false }));
            this.updateInRealTimeSelection = ObservablePropertyFactory.Instance.CreateProperty<bool?>(true);
            this.updateInRealTime =
                ObservablePropertyFactory.Instance.CreateCalculatedProperty(
                    this.updateInRealTimeSelection,
                    updateInRealTimeSelection => updateInRealTimeSelection.HasValue && updateInRealTimeSelection.Value);
            this.supportsAsync = ObservablePropertyFactory.Instance.CreateReadOnlyProperty(supportsAsync);
            this.simulateLatencyItems =
                ObservablePropertyFactory.Instance.CreateReadOnlyProperty(
                    ObservableCollectionFactory.Instance.CreateObservableCollection(new[] { true, false }));
            this.simulateLatencySelection = ObservablePropertyFactory.Instance.CreateProperty<bool?>(true);
            this.simulateLatency =
                ObservablePropertyFactory.Instance.CreateCalculatedProperty(
                    this.simulateLatencySelection,
                    simulateLatencySelection => simulateLatencySelection.HasValue && simulateLatencySelection.Value);
            this.operators =
                ObservablePropertyFactory.Instance.CreateReadOnlyProperty(
                    ObservableCollectionFactory.Instance.CreateObservableCollection(
                        new[] { Operator.Add, Operator.Subtract, Operator.Multiply, Operator.Divide }));
            this.operand1 = ObservablePropertyFactory.Instance.CreateProperty<string>(null);
            this.selectedOperator = ObservablePropertyFactory.Instance.CreateProperty(Operator.Add);
            this.selectedOperatorString =
                ObservablePropertyFactory.Instance.CreateCalculatedProperty(
                    this.selectedOperator,
                    selectedOperator =>
                        {
                            switch (selectedOperator)
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
                                        "Unknown enumeration value " + selectedOperator + ".");
                            }
                        });
            this.operand2 = ObservablePropertyFactory.Instance.CreateProperty<string>(null);
            this.result = ObservablePropertyFactory.Instance.CreateCalculatedProperty(
                this.Operand1,
                this.Operand2,
                this.SelectedOperator,
                (operand1, operand2, selectedOperator) =>
                    {
                        Func<double?, double?, double?> function;
                        switch (selectedOperator)
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
                                throw new NotSupportedException("Unknown enumeration value " + selectedOperator + ".");
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

        public IObservableProperty<Operator> SelectedOperator
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
            get
            {
                return this.result;
            }
        }

        public void SwitchOperators()
        {
            IObservableCollection<Operator> o = this.operators.Value;
            if (o.Contains(Operator.Multiply))
            {
                o.Remove(Operator.Multiply);
            }
            else
            {
                o.Add(Operator.Multiply);
            }
            if (o.Contains(Operator.Divide))
            {
                o.Remove(Operator.Divide);
            }
            else
            {
                o.Add(Operator.Divide);
            }

            if (!o.Contains(this.SelectedOperator.Value))
            {
                this.selectedOperator.Value = o[0];
            }
        }
    }
}