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
    using System.Linq.Expressions;
    using System.Windows.Controls;
    using System.Windows.Data;

    using MorseCode.RxMvvm.Observable.Property;

    /// <summary>
    /// Provides text box extension methods for binding.
    /// </summary>
    public static class TextBoxExtensionMethods
    {
        /// <summary>
        /// Binds the <see cref="TextBox.Text"/> property of a <see cref="TextBox"/>.
        /// </summary>
        /// <param name="textBox">
        /// The text box.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getTextProperty">
        /// The text property.
        /// </param>
        /// <param name="bindingFactory">
        /// The binding factory.
        /// </param>
        /// <typeparam name="T">
        /// The type of the data context.
        /// </typeparam>
        public static void BindText<T>(
            this TextBox textBox, 
            IReadableObservableProperty<T> dataContext, 
            Expression<Func<T, IObservableProperty<string>>> getTextProperty, 
            IBindingFactory<T> bindingFactory)
        {
            Contract.Requires<ArgumentNullException>(textBox != null, "textBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getTextProperty != null, "getTextProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");

            Binding textBinding = bindingFactory.CreateTwoWayBinding(getTextProperty);
            textBox.SetBinding(TextBox.TextProperty, textBinding);
        }
    }
}