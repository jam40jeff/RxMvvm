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

namespace MorseCode.RxMvvm.UI.Wpf
{
    using System.ComponentModel;
    using System.Reactive.Concurrency;
    using System.Windows;

    using MorseCode.RxMvvm.Common;

    internal static class RxMvvmApplicationUtility
    {
        #region Public Methods and Operators

        public static void SetupRxMvvmConfiguration(Application application)
        {
            RxMvvmConfiguration.SetIsInDesignModeFunc(
                () =>
                    {
                        Window firstWindow = application.Windows.Count > 0 ? application.Windows[0] : null;
                        return firstWindow != null && DesignerProperties.GetIsInDesignMode(firstWindow);
                    });
            IScheduler scheduler = new DispatcherScheduler(application.Dispatcher);
            RxMvvmConfiguration.SetNotifyPropertyChangedSchedulerFactory(() => scheduler);
        }

        #endregion
    }
}