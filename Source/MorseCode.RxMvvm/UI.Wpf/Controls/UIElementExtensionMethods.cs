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
    using System.Reactive.Linq;
    using System.Windows;

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Observable;
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
        /// <param name="getVisible">
        /// A delegate to get whether the control should be visible.
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
            Func<T, IObservable<bool>> getVisible,
            IBindingFactory<T> bindingFactory,
            bool useHiddenWhenNotVisible = false) where T : class
        {
            Contract.Requires<ArgumentNullException>(uiElement != null, "uiElement");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getVisible != null, "getVisible");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            return uiElement.BindVisibility(
                dataContext,
                d => getVisible(d).Select(DiscriminatedUnion.First<object, bool, NonComputable>),
                bindingFactory,
                useHiddenWhenNotVisible);
        }

        /// <summary>
        /// Binds the <see cref="UIElement.Visibility"/> property of a <see cref="UIElement"/>.
        /// </summary>
        /// <param name="uiElement">
        /// The UI element.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getVisible">
        /// A delegate to get whether the control should be visible.
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
            Func<T, IObservable<IDiscriminatedUnion<object, bool, NonComputable>>> getVisible,
            IBindingFactory<T> bindingFactory,
            bool useHiddenWhenNotVisible = false) where T : class
        {
            Contract.Requires<ArgumentNullException>(uiElement != null, "uiElement");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getVisible != null, "getVisible");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            Visibility notVisibleVisiblity = useHiddenWhenNotVisible ? Visibility.Hidden : Visibility.Collapsed;
            return bindingFactory.CreateChainedOneWayBinding(
                dataContext,
                getVisible,
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
        /// <param name="getEnabled">
        /// A delegate to get whether the control should be enabled.
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
            Func<T, IObservableProperty<bool>> getEnabled,
            IBindingFactory<T> bindingFactory) where T : class
        {
            Contract.Requires<ArgumentNullException>(uiElement != null, "uiElement");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getEnabled != null, "getEnabled");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            return uiElement.BindIsEnabled(
                dataContext,
                d => getEnabled(d).Select(DiscriminatedUnion.First<object, bool, NonComputable>),
                bindingFactory);
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
        /// <param name="getEnabled">
        /// A delegate to get whether the control should be enabled.
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
            Func<T, IObservable<IDiscriminatedUnion<object, bool, NonComputable>>> getEnabled,
            IBindingFactory<T> bindingFactory) where T : class
        {
            Contract.Requires<ArgumentNullException>(uiElement != null, "uiElement");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getEnabled != null, "getEnabled");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            return bindingFactory.CreateChainedOneWayBinding(dataContext, getEnabled, v => uiElement.IsEnabled = v);
        }
    }
}