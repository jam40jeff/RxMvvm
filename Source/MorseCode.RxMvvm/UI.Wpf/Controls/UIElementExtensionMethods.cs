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
    using System.Windows;

    using MorseCode.RxMvvm.Observable.Property;

    /// <summary>
    /// Provides UI element extension methods for binding.
    /// </summary>
    // ReSharper disable InconsistentNaming
    public static class UIElementExtensionMethods
    {
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Binds the <see cref="UIElement.Visibility"/> property of a <see cref="UIElement"/>.
        /// </summary>
        /// <param name="uiElement">
        /// The UI element.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getVisibleProperty">
        /// A delegate to get the visible property.
        /// </param>
        /// <param name="bindingFactory">
        /// The binding factory.
        /// </param>
        /// <param name="useHiddenWhenNotVisible">
        /// If 
        /// <value>
        /// false
        /// </value>
        /// , the <see cref="UIElement.Visibility"/> property will be set to <see cref="Visibility.Collapsed"/> when the control is not visible.
        /// If 
        /// <value>
        /// true
        /// </value>
        /// , the <see cref="UIElement.Visibility"/> property will be set to <see cref="Visibility.Hidden"/> when the control is not visible.
        /// </param>
        /// <typeparam name="T">
        /// The type of the data context.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IDisposable"/> which will clean up the bindings when disposed.
        /// </returns>
        public static IBinding BindVisibility<T>(
            this UIElement uiElement, 
            IObservable<T> dataContext, 
            Func<T, IObservableProperty<bool>> getVisibleProperty, 
            IBindingFactory<T> bindingFactory, 
            bool useHiddenWhenNotVisible = false) where T : class
        {
            Contract.Requires<ArgumentNullException>(uiElement != null, "uiElement");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getVisibleProperty != null, "getVisibleProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            Visibility notVisibleVisiblity = useHiddenWhenNotVisible ? Visibility.Hidden : Visibility.Collapsed;
            return bindingFactory.CreateOneWayBinding(
                dataContext, 
                getVisibleProperty, 
                v => uiElement.Visibility = v ? Visibility.Visible : notVisibleVisiblity);
        }

        /// <summary>
        /// Binds the <see cref="UIElement.IsEnabled"/> property of a <see cref="UIElement"/>.
        /// </summary>
        /// <param name="uiElement">
        /// The UI element.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getEnabledProperty">
        /// A delegate to get the enabled property.
        /// </param>
        /// <param name="bindingFactory">
        /// The binding factory.
        /// </param>
        /// <typeparam name="T">
        /// The type of the data context.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IDisposable"/> which will clean up the bindings when disposed.
        /// </returns>
        public static IBinding BindIsEnabled<T>(
            this UIElement uiElement, 
            IObservable<T> dataContext, 
            Func<T, IObservableProperty<bool>> getEnabledProperty, 
            IBindingFactory<T> bindingFactory) where T : class
        {
            Contract.Requires<ArgumentNullException>(uiElement != null, "uiElement");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getEnabledProperty != null, "getVisibleProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            return bindingFactory.CreateOneWayBinding(dataContext, getEnabledProperty, v => uiElement.IsEnabled = v);
        }
    }
}