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

namespace MorseCode.RxMvvm.UI.Wpf.Controls
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// An interface representing a view.
    /// </summary>
    [ContractClass(typeof(ViewContract))]
    public interface IView : IDisposable
    {
        /// <summary>
        /// Shows the view by replacing another view.
        /// </summary>
        /// <param name="oldView">
        /// The old view to replace.
        /// </param>
        void ShowReplacing(IView oldView);

        /// <summary>
        /// Gets the view's top position.
        /// </summary>
        /// <returns>
        /// The view's top position.
        /// </returns>
        double GetViewPositionTop();

        /// <summary>
        /// Gets the view's left position.
        /// </summary>
        /// <returns>
        /// The view's left position.
        /// </returns>
        double GetViewPositionLeft();

        /// <summary>
        /// Gets the view's width.
        /// </summary>
        /// <returns>
        /// The view's width.
        /// </returns>
        double GetViewWidth();

        /// <summary>
        /// Gets the view's height.
        /// </summary>
        /// <returns>
        /// The view's height.
        /// </returns>
        double GetViewHeight();
    }
}