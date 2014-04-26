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
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Observable;
    using MorseCode.RxMvvm.Observable.Property;

    /// <summary>
    /// Provides methods for binding radio buttons.
    /// </summary>
    public static class RadioButtonBindingUtility
    {
        /// <summary>
        /// Binds the <see cref="CheckBox.IsChecked"/> property of a <see cref="CheckBox"/>.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getSelectedItemProperty">
        /// A delegate to get the selected item property.
        /// </param>
        /// <param name="getItemText">
        /// A delegate to get the text to display for an item.
        /// </param>
        /// <param name="bindingFactory">
        /// The binding factory.
        /// </param>
        /// <typeparam name="T">
        /// The type of the data context.
        /// </typeparam>
        /// <typeparam name="TItem">
        /// The type of the items.
        /// </typeparam>
        /// <returns>
        /// A <see cref="GroupHelper{T,TItem}"/> for adding radio buttons to the group.
        /// </returns>
        public static GroupHelper<T, TItem> BindGroup<T, TItem>(
            IObservable<T> dataContext, 
            Func<T, IObservableProperty<TItem>> getSelectedItemProperty, 
            Func<TItem, string> getItemText, 
            IBindingFactory<T> bindingFactory) where T : class
        {
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
            Contract.Requires<ArgumentNullException>(getItemText != null, "getItemText");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<GroupHelper<T, TItem>>() != null);

            return BindGroup(
                dataContext, 
                d =>
                Observable.Return(
                    DiscriminatedUnion.First<object, IObservableProperty<TItem>, NonComputable>(
                        getSelectedItemProperty(d))), 
                getItemText, 
                bindingFactory);
        }

        /// <summary>
        /// Binds the <see cref="CheckBox.IsChecked"/> property of a <see cref="CheckBox"/>.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getSelectedItemProperty">
        /// A delegate to get the selected item property.
        /// </param>
        /// <param name="getItemText">
        /// A delegate to get the text to display for an item.
        /// </param>
        /// <param name="bindingFactory">
        /// The binding factory.
        /// </param>
        /// <typeparam name="T">
        /// The type of the data context.
        /// </typeparam>
        /// <typeparam name="TItem">
        /// The type of the items.
        /// </typeparam>
        /// <returns>
        /// A <see cref="GroupHelper{T,TItem}"/> for adding radio buttons to the group.
        /// </returns>
        public static GroupHelper<T, TItem> BindGroup<T, TItem>(
            IObservable<T> dataContext, 
            Func<T, IObservable<IDiscriminatedUnion<object, IObservableProperty<TItem>, NonComputable>>> getSelectedItemProperty, 
            Func<TItem, string> getItemText, 
            IBindingFactory<T> bindingFactory) where T : class
        {
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
            Contract.Requires<ArgumentNullException>(getItemText != null, "getItemText");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<GroupHelper<T, TItem>>() != null);

            return new GroupHelper<T, TItem>(dataContext, getSelectedItemProperty, getItemText, bindingFactory);
        }

        /// <summary>
        /// Provides methods for binding a group of radio buttons.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the data context.
        /// </typeparam>
        /// <typeparam name="TItem">
        /// The type of the items.
        /// </typeparam>
        public class GroupHelper<T, TItem>
            where T : class
        {
            private readonly IObservable<T> dataContext;

            private readonly
                Func<T, IObservable<IDiscriminatedUnion<object, IObservableProperty<TItem>, NonComputable>>> getSelectedItemProperty;

            private readonly Func<TItem, string> getItemText;

            private readonly IBindingFactory<T> bindingFactory;

            private readonly string groupName;

            private readonly Dictionary<RadioButton, IBinding> bindingsByRadioButtonField =
                new Dictionary<RadioButton, IBinding>();

            internal GroupHelper(
                IObservable<T> dataContext, 
                Func<T, IObservable<IDiscriminatedUnion<object, IObservableProperty<TItem>, NonComputable>>> getSelectedItemProperty, 
                Func<TItem, string> getItemText, 
                IBindingFactory<T> bindingFactory)
            {
                Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
                Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
                Contract.Requires<ArgumentNullException>(getItemText != null, "getItemText");
                Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
                Contract.Ensures(this.dataContext != null);
                Contract.Ensures(this.getItemText != null);
                Contract.Ensures(this.bindingFactory != null);

                this.dataContext = dataContext;
                this.getSelectedItemProperty = getSelectedItemProperty;
                this.getItemText = getItemText;
                this.bindingFactory = bindingFactory;

                this.groupName = Guid.NewGuid().ToString("N");
            }

            /// <summary>
            /// Adds a radio button to the radio button group.
            /// </summary>
            /// <param name="radioButton">
            /// The radio button.
            /// </param>
            /// <param name="getItem">
            /// A delegate to get the item this radio button represents.
            /// </param>
            /// <returns>
            /// The radio button group helper.
            /// </returns>
            public GroupHelper<T, TItem> Add(RadioButton radioButton, Func<T, IObservable<TItem>> getItem)
            {
                Contract.Requires<ArgumentNullException>(radioButton != null, "radioButton");
                Contract.Requires<ArgumentNullException>(getItem != null, "getItem");
                Contract.Ensures(Contract.Result<GroupHelper<T, TItem>>() != null);

                return this.Add(
                    radioButton, d => getItem(d).Select(DiscriminatedUnion.First<object, TItem, NonComputable>));
            }

            /// <summary>
            /// Adds a radio button to the radio button group.
            /// </summary>
            /// <param name="radioButton">
            /// The radio button.
            /// </param>
            /// <param name="getItem">
            /// A delegate to get the item this radio button represents.
            /// </param>
            /// <returns>
            /// The radio button group helper.
            /// </returns>
            public GroupHelper<T, TItem> Add(
                RadioButton radioButton, Func<T, IObservable<IDiscriminatedUnion<object, TItem, NonComputable>>> getItem)
            {
                Contract.Requires<ArgumentNullException>(radioButton != null, "radioButton");
                Contract.Requires<ArgumentNullException>(getItem != null, "getItem");
                Contract.Ensures(Contract.Result<GroupHelper<T, TItem>>() != null);

                radioButton.GroupName = this.groupName;

                Binding binding = new Binding();

                bool isUpdatingControl = false;
                binding.Add(
                    this.dataContext.Select(
                        d =>
                        d == null
                            ? Observable.Return(
                                DiscriminatedUnion.Second<object, Tuple<TItem, TItem>, NonComputable>(
                                    NonComputable.Value))
                            : getItem(d)
                                  .CombineLatest(
                                      this.getSelectedItemProperty(d)
                                          .Select(
                                              u =>
                                              u.Switch(
                                                  o => o.Select(DiscriminatedUnion.First<object, TItem, NonComputable>), 
                                                  n =>
                                                  Observable.Return(
                                                      DiscriminatedUnion.Second<object, TItem, NonComputable>(n))))
                                          .Switch(), 
                                      (i, s) => i.CombineWith(s, Tuple.Create)))
                        .Switch()
                        .ObserveOnDispatcher()
                        .Subscribe(
                            u =>
                                {
                                    isUpdatingControl = true;
                                    try
                                    {
                                        u.Switch(
                                            t => radioButton.IsChecked = Equals(t.Item1, t.Item2), 
                                            _ => radioButton.IsChecked = false);
                                    }
                                    finally
                                    {
                                        isUpdatingControl = false;
                                    }
                                }));

                IObservable<IDiscriminatedUnion<object, Tuple<TItem, IObservableProperty<TItem>>, NonComputable>> itemAndSelectedItemPropertyObservable =
                        this.dataContext.Select(
                            d =>
                            d == null
                                ? Observable.Return(
                                    DiscriminatedUnion
                                      .Second<object, Tuple<TItem, IObservableProperty<TItem>>, NonComputable>(
                                          NonComputable.Value))
                                : getItem(d)
                                      .CombineLatest(
                                          this.getSelectedItemProperty(d), (i, s) => i.CombineWith(s, Tuple.Create)))
                            .Switch();

                binding.Add(
                    itemAndSelectedItemPropertyObservable.Join(
                        Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                            h => (sender, args) =>
                                {
                                    if (!isUpdatingControl)
                                    {
                                        h(sender, args);
                                    }
                                }, 
                            h => radioButton.Checked += h, 
                            h => radioButton.Checked -= h), 
                        u => itemAndSelectedItemPropertyObservable.Skip(1), 
                        e => Observable.Empty<Unit>(), 
                        (u, e) => u).Subscribe(
                            u => u.Switch(
                                t =>
                                    {
                                        if (radioButton.IsChecked != null && radioButton.IsChecked.Value)
                                        {
                                            t.Item2.Value = t.Item1;
                                        }
                                    }, 
                                _ => { })));

                binding.Add(
                    this.bindingFactory.CreateOneWayBinding(
                        this.dataContext, 
                        d => getItem(d).Select(u => u.Switch(v => v, _ => default(TItem))), 
                        v => radioButton.Content = this.getItemText(v)));

                this.bindingsByRadioButtonField.Add(radioButton, binding);

                return this;
            }

            /// <summary>
            /// Completes the radio button group.
            /// </summary>
            /// <returns>
            /// An <see cref="IDisposable"/> which will clean up the bindings when disposed.
            /// </returns>
            public IDisposable Complete()
            {
                Contract.Ensures(Contract.Result<IDisposable>() != null);

                CompositeDisposable compositeDisposable = new CompositeDisposable();

                foreach (IBinding binding in this.bindingsByRadioButtonField.Values)
                {
                    compositeDisposable.Add(binding);
                }

                return compositeDisposable;
            }

            /// <summary>
            /// Completes the radio button group.
            /// </summary>
            /// <param name="bindingsByRadioButton">
            /// A dictionary containing the bindings by radio button.
            /// </param>
            /// <returns>
            /// An <see cref="IDisposable"/> which will clean up the bindings when disposed.
            /// </returns>
            public IDisposable Complete(out Dictionary<RadioButton, IBinding> bindingsByRadioButton)
            {
                Contract.Ensures(Contract.Result<IDisposable>() != null);

                IDisposable disposable = this.Complete();

                bindingsByRadioButton = this.bindingsByRadioButtonField;

                return disposable;
            }

            [ContractInvariantMethod]
            private void CodeContractsInvariants()
            {
                Contract.Invariant(this.dataContext != null);
                Contract.Invariant(this.getSelectedItemProperty != null);
                Contract.Invariant(this.bindingFactory != null);
                Contract.Invariant(this.bindingsByRadioButtonField != null);
            }
        }
    }
}