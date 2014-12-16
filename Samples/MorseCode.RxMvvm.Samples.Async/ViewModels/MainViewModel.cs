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

namespace MorseCode.RxMvvm.Samples.Async.ViewModels
{
    using System;
    using System.Threading.Tasks;

    using MorseCode.RxMvvm.Observable.Property;

    public class MainViewModel
    {
        #region Fields

        private readonly ICalculatedProperty<Tuple<string, int>> calculated;

        private readonly ICalculatedProperty<string> calculatedText;

        private readonly ICalculatedProperty<int> calculatedTime;

        private readonly IObservableProperty<string> text;

        #endregion

        #region Constructors and Destructors

        public MainViewModel()
        {
            this.text = ObservablePropertyFactory.Instance.CreateProperty(string.Empty);

            Random r = new Random();
            this.calculated = ObservablePropertyFactory.Instance.CreateCancellableAsyncCalculatedPropertyWithContext(
                r,
                this.text,
                TimeSpan.FromSeconds(0.1),
                async (helper, context, text) =>
                {
                    int millisecondsToWait = context.Next(5000);
                    await Task.Delay(millisecondsToWait, helper.Token).ConfigureAwait(false);
                    await helper.CheckCancellationTokenAndYield().ConfigureAwait(false);
                    return Tuple.Create(text + " [CALCULATED]", millisecondsToWait);
                });

            this.calculatedText = ObservablePropertyFactory.Instance.CreateCalculatedProperty(
                this.calculated, calculated => calculated.Switch(v => v == null ? null : v.Item1, e => "[ERROR]"));

            this.calculatedTime = ObservablePropertyFactory.Instance.CreateCalculatedProperty(
                this.calculated, calculated => calculated.Switch(v => v == null ? 0 : v.Item2, e => -1));
        }

        #endregion

        #region Public Properties

        public ICalculatedProperty<string> CalculatedText
        {
            get
            {
                return this.calculatedText;
            }
        }

        public ICalculatedProperty<int> CalculatedTime
        {
            get
            {
                return this.calculatedTime;
            }
        }

        public IObservableProperty<string> Text
        {
            get
            {
                return this.text;
            }
        }

        #endregion
    }
}