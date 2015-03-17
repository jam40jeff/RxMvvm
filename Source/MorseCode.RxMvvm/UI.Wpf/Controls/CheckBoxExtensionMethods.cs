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
    using System.Windows.Controls;

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Observable;
    using MorseCode.RxMvvm.Observable.Property;
    using MorseCode.RxMvvm.Reactive;

    /// <summary>
    /// Provides check box extension methods for binding.
    /// </summary>
    public static class CheckBoxExtensionMethods
    {
        /// <summary>
        /// Binds the <see cref="CheckBox.IsChecked"/> property of a <see cref="CheckBox"/>.
        /// </summary>
        /// <param name="checkBox">
        /// The check box.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getCheckedProperty">
        /// A delegate to get the checked property.
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
        public static IBinding BindIsChecked<T>(
            this CheckBox checkBox, 
            IObservable<T> dataContext, 
            Func<T, IObservableProperty<bool>> getCheckedProperty, 
            IBindingFactory<T> bindingFactory) where T : class
        {
            Contract.Requires<ArgumentNullException>(checkBox != null, "checkBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getCheckedProperty != null, "getCheckedProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            return checkBox.BindIsChecked(
                dataContext, 
                d =>
                ObservableRxMvvm.Always(
                    DiscriminatedUnion.First<object, IObservableProperty<bool>, NonComputable>(getCheckedProperty(d))), 
                bindingFactory);
        }

        /// <summary>
        /// Binds the <see cref="CheckBox.IsChecked"/> property of a <see cref="CheckBox"/>.
        /// </summary>
        /// <param name="checkBox">
        /// The check box.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getCheckedProperty">
        /// A delegate to get the checked property.
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
        public static IBinding BindIsChecked<T>(
            this CheckBox checkBox, 
            IObservable<T> dataContext, 
            Func<T, IObservable<IDiscriminatedUnion<object, IObservableProperty<bool>, NonComputable>>> getCheckedProperty, 
            IBindingFactory<T> bindingFactory) where T : class
        {
            Contract.Requires<ArgumentNullException>(checkBox != null, "checkBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getCheckedProperty != null, "getCheckedProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            checkBox.IsThreeState = false;
            return bindingFactory.CreateChainedTwoWayBinding(
                dataContext, 
                getCheckedProperty, 
                binding => Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    h => (sender, args) =>
                        {
                            if (!binding.IsUpdatingControl)
                            {
                                h(sender, args);
                            }
                        }, 
                    h => checkBox.Checked += h, 
                    h => checkBox.Checked -= h), 
                v => checkBox.IsChecked = v, 
                () => checkBox.IsChecked ?? false);
        }

        /// <summary>
        /// Binds the <see cref="CheckBox.IsChecked"/> property of a three state <see cref="CheckBox"/>.
        /// </summary>
        /// <param name="checkBox">
        /// The check box.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getCheckedProperty">
        /// A delegate to get the checked property.
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
        public static IBinding BindThreeStateIsChecked<T>(
            this CheckBox checkBox, 
            IObservable<T> dataContext, 
            Func<T, IObservableProperty<bool?>> getCheckedProperty, 
            IBindingFactory<T> bindingFactory) where T : class
        {
            Contract.Requires<ArgumentNullException>(checkBox != null, "checkBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getCheckedProperty != null, "getCheckedProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            return checkBox.BindThreeStateIsChecked(
                dataContext, 
                d =>
                ObservableRxMvvm.Always(
                    DiscriminatedUnion.First<object, IObservableProperty<bool?>, NonComputable>(getCheckedProperty(d))), 
                bindingFactory);
        }

        /// <summary>
        /// Binds the <see cref="CheckBox.IsChecked"/> property of a three state <see cref="CheckBox"/>.
        /// </summary>
        /// <param name="checkBox">
        /// The check box.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getCheckedProperty">
        /// A delegate to get the checked property.
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
        public static IBinding BindThreeStateIsChecked<T>(
            this CheckBox checkBox, 
            IObservable<T> dataContext, 
            Func<T, IObservable<IDiscriminatedUnion<object, IObservableProperty<bool?>, NonComputable>>> getCheckedProperty, 
            IBindingFactory<T> bindingFactory) where T : class
        {
            Contract.Requires<ArgumentNullException>(checkBox != null, "checkBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getCheckedProperty != null, "getCheckedProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            checkBox.IsThreeState = true;
            return bindingFactory.CreateChainedTwoWayBinding(
                dataContext, 
                getCheckedProperty, 
                binding => Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    h => (sender, args) =>
                        {
                            if (!binding.IsUpdatingControl)
                            {
                                h(sender, args);
                            }
                        }, 
                    h => checkBox.Checked += h, 
                    h => checkBox.Checked -= h), 
                v => checkBox.IsChecked = v, 
                () => checkBox.IsChecked);
        }
    }
}