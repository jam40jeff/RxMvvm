//#region License

//// Copyright 2014 MorseCode Software
//// Licensed under the Apache License, Version 2.0 (the "License");
//// you may not use this file except in compliance with the License.
//// You may obtain a copy of the License at
////     http://www.apache.org/licenses/LICENSE-2.0
//// Unless required by applicable law or agreed to in writing, software
//// distributed under the License is distributed on an "AS IS" BASIS,
//// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//// See the License for the specific language governing permissions and
//// limitations under the License.
//#endregion

//namespace MorseCode.RxMvvm.UI.Wpf.Controls
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Diagnostics.Contracts;
//    using System.Reactive.Linq;
//    using System.Windows;
//    using System.Windows.Controls;

//    using MorseCode.RxMvvm.Observable;
//    using MorseCode.RxMvvm.Observable.Property;

//    /// <summary>
//    /// Provides methods for binding radio buttons.
//    /// </summary>
//    public static class RadioButtonBindingUtility
//    {
//        /// <summary>
//        /// Binds the <see cref="CheckBox.IsChecked"/> property of a <see cref="CheckBox"/>.
//        /// </summary>
//        /// <param name="dataContext">
//        /// The data context.
//        /// </param>
//        /// <param name="getSelectedItemProperty">
//        /// A delegate to get the selected item property.
//        /// </param>
//        /// <param name="getItemText">
//        /// A delegate to get the text to display for an item.
//        /// </param>
//        /// <param name="bindingFactory">
//        /// The binding factory.
//        /// </param>
//        /// <typeparam name="T">
//        /// The type of the data context.
//        /// </typeparam>
//        /// <typeparam name="TItem">
//        /// The type of the items.
//        /// </typeparam>
//        /// <returns>
//        /// A <see cref="GroupHelper{T,TItem}"/> for adding radio buttons to the group.
//        /// </returns>
//        public static GroupHelper<T, TItem> BindGroup<T, TItem>(
//            IObservable<T> dataContext,
//            Func<T, IObservableProperty<TItem>> getSelectedItemProperty,
//            Func<TItem, string> getItemText,
//            IBindingFactory<T> bindingFactory) where T : class
//        {
//            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
//            Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
//            Contract.Requires<ArgumentNullException>(getItemText != null, "getItemText");
//            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
//            Contract.Ensures(Contract.Result<IBinding>() != null);

//            return new GroupHelper<T, TItem>(dataContext, getSelectedItemProperty, getItemText, bindingFactory);
//        }

//        /// <summary>
//        /// Provides methods for binding a group of radio buttons.
//        /// </summary>
//        /// <typeparam name="T">
//        /// The type of the data context.
//        /// </typeparam>
//        /// <typeparam name="TItem">
//        /// The type of the items.
//        /// </typeparam>
//        public class GroupHelper<T, TItem>
//            where T : class
//        {
//            private readonly IObservable<T> dataContext;

//            private readonly Func<T, IObservableProperty<TItem>> getSelectedItemProperty;

//            private readonly Func<TItem, string> getItemText;

//            private readonly IBindingFactory<T> bindingFactory;

//            private readonly string groupName;

//            private readonly List<object> radioButtonInformationList = new List<object>();

//            internal GroupHelper(
//                IObservable<T> dataContext,
//                Func<T, IObservableProperty<TItem>> getSelectedItemProperty,
//                Func<TItem, string> getItemText,
//                IBindingFactory<T> bindingFactory)
//            {
//                Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
//                Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
//                Contract.Requires<ArgumentNullException>(getItemText != null, "getItemText");
//                Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
//                Contract.Ensures(this.dataContext != null);
//                Contract.Ensures(this.getItemText != null);
//                Contract.Ensures(this.bindingFactory != null);
//                Contract.Ensures(this.groupName != null);

//                this.dataContext = dataContext;
//                this.getSelectedItemProperty = getSelectedItemProperty;
//                this.getItemText = getItemText;
//                this.bindingFactory = bindingFactory;

//                this.groupName = Guid.NewGuid().ToString("N");
//            }

//            /// <summary>
//            /// The add.
//            /// </summary>
//            /// <param name="radioButton">
//            /// The radio button.
//            /// </param>
//            /// <param name="getItem">
//            /// The get item.
//            /// </param>
//            /// <returns>
//            /// The <see cref="GroupHelper"/>.
//            /// </returns>
//            public GroupHelper<T, TItem> Add(RadioButton radioButton, Func<T, IObservable<TItem>> getItem)
//            {
//                radioButton.GroupName = this.groupName;

//                bool isUpdatingControl=false;
//                IDisposable updateControlSubscription=this.dataContext.BeginChain()
//                    .Add(getItem)
//                    .Complete()
//                    .CombineLatest(
//                        this.dataContext.BeginChain().Add(this.getSelectedItemProperty).Complete(), Tuple.Create)
//                    .Subscribe(
//                        t =>
//                            {
//                                isUpdatingControl=true;
//                                try
//                                {
//                                    t.Item1.Switch(
//                                        i =>
//                                        t.Item2.Switch(
//                                            s => radioButton.IsChecked = Equals(s, i),
//                                            _ => radioButton.IsChecked = false),
//                                        _ => radioButton.IsChecked = false);
//                                }
//                                finally
//                                {isUpdatingControl=false;
//                                }
//                            });

//                IDisposable updateDataContextSubscription=this.dataContext.BeginChain()
//                    .Add(getItem)
//                    .Complete()
//                    .CombineLatest(
//                        Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
//                        h => (sender, args) =>
//                            {
//                                if (!isUpdatingControl)
//                                {
//                                    h(sender, args);
//                                }
//                            }, 
//                        h => radioButton.Checked += h, 
//                        h => radioButton.Checked -= h), (i,e)=>i)
//                    .Subscribe(
//                        i =>
//                        t.Item1.Switch(
//                            i =>
//                            t.Item2.Switch(
//                                s => radioButton.IsChecked = Equals(s, i), _ => radioButton.IsChecked = false), 
//                            _ => radioButton.IsChecked = false));

//                this.bindingFactory.CreateTwoWayBinding(
//                    this.dataContext, 
//                    this.getSelectedItemProperty, 
//                    b => Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
//                        h => (sender, args) =>
//                            {
//                                if (!b.IsUpdatingControl)
//                                {
//                                    h(sender, args);
//                                }
//                            }, 
//                        h => radioButton.Checked += h, 
//                        h => radioButton.Checked -= h), 
//                    v => 
//            }

//            /// <summary>
//            /// The complete.
//            /// </summary>
//            /// <returns>
//            /// The <see cref="IDisposable"/>.
//            /// </returns>
//            public IDisposable Complete()
//            {
//            }

//            [ContractInvariantMethod]
//            private void CodeContractsInvariants()
//            {
//                Contract.Invariant(this.dataContext != null);
//                Contract.Invariant(this.getSelectedItemProperty != null);
//                Contract.Invariant(this.bindingFactory != null);
//                Contract.Invariant(this.groupName != null);
//            }
//        }
//    }
//}