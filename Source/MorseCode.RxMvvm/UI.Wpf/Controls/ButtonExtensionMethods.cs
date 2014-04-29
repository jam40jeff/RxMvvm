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
    using System.Windows.Controls.Primitives;

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Observable;

    /// <summary>
    /// Provides button extension methods for binding.
    /// </summary>
    public static class ButtonExtensionMethods
    {
        /// <summary>
        /// Binds the <see cref="UIElement.Visibility"/> property of a <see cref="UIElement"/>.
        /// </summary>
        /// <param name="button">
        /// The button.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getClickAction">
        /// A delegate to get the click action.
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
        public static IBinding BindClick<T>(
            this ButtonBase button, 
            IObservable<T> dataContext, 
            Func<T, Action> getClickAction, 
            IBindingFactory<T> bindingFactory) where T : class
        {
            Contract.Requires<ArgumentNullException>(button != null, "button");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getClickAction != null, "getClickAction");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            return button.BindClick(
                dataContext, 
                d => Observable.Return(DiscriminatedUnion.First<object, Action, NonComputable>(getClickAction(d))), 
                bindingFactory);
        }

        /// <summary>
        /// Binds the <see cref="UIElement.Visibility"/> property of a <see cref="UIElement"/>.
        /// </summary>
        /// <param name="button">
        /// The button.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getClickAction">
        /// A delegate to get the click action.
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
        public static IBinding BindClick<T>(
            this ButtonBase button, 
            IObservable<T> dataContext, 
            Func<T, IObservable<IDiscriminatedUnion<object, Action, NonComputable>>> getClickAction, 
            IBindingFactory<T> bindingFactory) where T : class
        {
            Contract.Requires<ArgumentNullException>(button != null, "button");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getClickAction != null, "getClickAction");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IBinding>() != null);

            return bindingFactory.CreateActionBinding(
                dataContext, 
                getClickAction, 
                binding => Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    h => (sender, args) =>
                        {
                            if (!binding.IsUpdatingControl)
                            {
                                h(sender, args);
                            }
                        }, 
                    h => button.Click += h, 
                    h => button.Click -= h));
        }
    }
}