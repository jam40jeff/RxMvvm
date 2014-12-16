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

namespace MorseCode.RxMvvm.Samples.Async.UI.Wpf
{
    using System;

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Samples.Async.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructors and Destructors

        public MainWindow()
        {
            IDiscriminatedUnion<object, string, int> d = DiscriminatedUnion.First<object, string, int>("abc");
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        protected override void BindControls(IObservable<MainViewModel> dataContext)
        {
            dataContext.Subscribe(v => this.DataContext = v);
        }

        #endregion
    }
}