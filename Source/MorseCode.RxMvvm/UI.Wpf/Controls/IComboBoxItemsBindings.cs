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

    /// <summary>
    /// An interface representing the bindings resulting from binding the check box items.
    /// </summary>
    public interface IComboBoxItemsBindings : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the control's items are being updated.
        /// </summary>
        bool IsUpdatingControlItems { get; }

        /// <summary>
        /// Gets a value indicating whether the control's selected item is being updated.
        /// </summary>
        bool IsUpdatingControlSelectedItem { get; }
    }
}