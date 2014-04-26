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
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    using MorseCode.RxMvvm.Common.StaticReflection;
    using MorseCode.RxMvvm.Observable;
    using MorseCode.RxMvvm.Observable.Collection;
    using MorseCode.RxMvvm.Observable.Property;

    /// <summary>
    /// Provides combo box extension methods for binding.
    /// </summary>
    public static class ComboBoxExtensionMethods
    {
        /// <summary>
        /// Binds the <see cref="ItemsControl.ItemsSource"/> and <see cref="Selector.SelectedItem"/> properties of a <see cref="ComboBox"/>.
        /// </summary>
        /// <param name="comboBox">
        /// The combo box.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getItems">
        /// A delegate to get the items.
        /// </param>
        /// <param name="getItemText">
        /// A delegate to get the text to display for an item.
        /// </param>
        /// <param name="getSelectedItemProperty">
        /// A delegate to get the selected item property.
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
        /// An <see cref="IComboBoxItemsBindings"/> which will clean up the bindings when disposed.
        /// </returns>
        public static IComboBoxItemsBindings BindItems<T, TItem>(
            this ComboBox comboBox,
            IObservable<T> dataContext,
            Func<T, IObservable<IReadableObservableCollection<TItem>>> getItems,
            Func<TItem, string> getItemText,
            Func<T, IObservableProperty<TItem>> getSelectedItemProperty,
            IBindingFactory<T> bindingFactory)
            where T : class
            where TItem : class
        {
            Contract.Requires<ArgumentNullException>(comboBox != null, "comboBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getItems != null, "getItems");
            Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IComboBoxItemsBindings>() != null);

            return BindItemsInternal(
                comboBox,
                dataContext,
                getItems,
                getItemText,
                false,
                null,
                getSelectedItemProperty,
                v => v,
                v => v,
                v => v,
                null,
                v => v == null,
                (x, y) => x == y,
                bindingFactory);
        }

        /// <summary>
        /// Binds the <see cref="ItemsControl.ItemsSource"/> and <see cref="Selector.SelectedItem"/> properties of a <see cref="ComboBox"/>.
        /// </summary>
        /// <param name="comboBox">
        /// The combo box.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getItems">
        /// A delegate to get the items.
        /// </param>
        /// <param name="getItemText">
        /// A delegate to get the text to display for an item.
        /// </param>
        /// <param name="getSelectedItemProperty">
        /// A delegate to get the selected item property.
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
        /// An <see cref="IComboBoxItemsBindings"/> which will clean up the bindings when disposed.
        /// </returns>
        public static IComboBoxItemsBindings BindItemsForStruct<T, TItem>(
            this ComboBox comboBox,
            IObservable<T> dataContext,
            Func<T, IReadableObservableProperty<IReadableObservableCollection<TItem>>> getItems,
            Func<TItem, string> getItemText,
            Func<T, IObservableProperty<TItem>> getSelectedItemProperty,
            IBindingFactory<T> bindingFactory)
            where T : class
            where TItem : struct
        {
            Contract.Requires<ArgumentNullException>(comboBox != null, "comboBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getItems != null, "getItems");
            Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IComboBoxItemsBindings>() != null);

            return BindItemsInternal(
                comboBox,
                dataContext,
                getItems,
                getItemText,
                false,
                null,
                getSelectedItemProperty,
                v => v,
                // ReSharper disable PossibleInvalidOperationException
                v => v.Value,
                // ReSharper restore PossibleInvalidOperationException
                v => v,
                (TItem?)null,
                v => v == null,
                (x, y) => x.Equals(y),
                bindingFactory);
        }

        /// <summary>
        /// Binds the <see cref="ItemsControl.ItemsSource"/> and <see cref="Selector.SelectedItem"/> properties of a <see cref="ComboBox"/>.
        /// </summary>
        /// <param name="comboBox">
        /// The combo box.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getItems">
        /// A delegate to get the items.
        /// </param>
        /// <param name="getItemText">
        /// A delegate to get the text to display for an item.
        /// </param>
        /// <param name="noSelectionText">
        /// The text to show when nothing is selected.
        /// </param>
        /// <param name="getSelectedItemProperty">
        /// A delegate to get the selected item property.
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
        /// An <see cref="IComboBoxItemsBindings"/> which will clean up the bindings when disposed.
        /// </returns>
        public static IComboBoxItemsBindings BindItemsWithNoSelection<T, TItem>(
            this ComboBox comboBox,
            IReadableObservableProperty<T> dataContext,
            Func<T, IReadableObservableProperty<IReadableObservableCollection<TItem>>> getItems,
            Func<TItem, string> getItemText,
            string noSelectionText,
            Func<T, IObservableProperty<TItem>> getSelectedItemProperty,
            IBindingFactory<T> bindingFactory)
            where T : class
            where TItem : class
        {
            Contract.Requires<ArgumentNullException>(comboBox != null, "comboBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getItems != null, "getItems");
            Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IComboBoxItemsBindings>() != null);

            return BindItemsInternal(
                comboBox,
                dataContext,
                getItems,
                getItemText,
                true,
                noSelectionText,
                getSelectedItemProperty,
                v => v,
                v => v,
                v => v,
                null,
                v => v == null,
                (x, y) => x == y,
                bindingFactory);
        }

        /// <summary>
        /// Binds the <see cref="ItemsControl.ItemsSource"/> and <see cref="Selector.SelectedItem"/> properties of a <see cref="ComboBox"/>.
        /// </summary>
        /// <param name="comboBox">
        /// The combo box.
        /// </param>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getItems">
        /// A delegate to get the items.
        /// </param>
        /// <param name="getItemText">
        /// A delegate to get the text to display for an item.
        /// </param>
        /// <param name="noSelectionText">
        /// The text to show when nothing is selected.
        /// </param>
        /// <param name="getSelectedItemProperty">
        /// A delegate to get the selected item property.
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
        /// An <see cref="IComboBoxItemsBindings"/> which will clean up the bindings when disposed.
        /// </returns>
        public static IComboBoxItemsBindings BindItemsForStructWithNoSelection<T, TItem>(
            this ComboBox comboBox,
            IReadableObservableProperty<T> dataContext,
            Func<T, IReadableObservableProperty<IReadableObservableCollection<TItem>>> getItems,
            Func<TItem, string> getItemText,
            string noSelectionText,
            Func<T, IObservableProperty<TItem?>> getSelectedItemProperty,
            IBindingFactory<T> bindingFactory)
            where T : class
            where TItem : struct
        {
            Contract.Requires<ArgumentNullException>(comboBox != null, "comboBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getItems != null, "getItems");
            Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IComboBoxItemsBindings>() != null);

            return BindItemsInternal(
                comboBox,
                dataContext,
                getItems,
                getItemText,
                true,
                noSelectionText,
                getSelectedItemProperty,
                v => v,
                v => v,
                v => v,
                // ReSharper disable RedundantCast - The type parameters are not correctly inferred when this cast is omitted.
                (TItem?)null,
                // ReSharper restore RedundantCast
                v => v == null,
                (x, y) => x.Equals(y),
                bindingFactory);
        }

        private static IComboBoxItemsBindings BindItemsInternal<T, TItem, TItemNullable, TSelectedItem>(
            this ComboBox comboBox,
            IObservable<T> dataContext,
            Func<T, IObservable<IReadableObservableCollection<TItem>>> getItems,
            Func<TItem, string> getItemText,
            bool addNoSelection,
            string noSelectionText,
            Func<T, IObservableProperty<TSelectedItem>> getSelectedItemProperty,
            Func<TSelectedItem, TItemNullable> convertFromSelectedItem,
            Func<TItemNullable, TSelectedItem> convertToSelectedItem,
            Func<TItem, TItemNullable> convertToNullable,
            TItemNullable nullValue,
            Func<TItemNullable, bool> isNull,
            Func<TItemNullable, TItemNullable, bool> areEqual,
            IBindingFactory<T> bindingFactory) where T : class
        {
            Contract.Requires<ArgumentNullException>(comboBox != null, "comboBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getItems != null, "getItems");
            Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");
            Contract.Ensures(Contract.Result<IComboBoxItemsBindings>() != null);

            CompositeDisposable compositeDisposable = new CompositeDisposable();
            List<ComboBoxItem<TItemNullable>> latestItems = null;
            Func<IEnumerable<TItem>, List<ComboBoxItem<TItemNullable>>> getComboBoxItems = c =>
                {
                    List<ComboBoxItem<TItemNullable>> items = new List<ComboBoxItem<TItemNullable>>();

                    if (addNoSelection)
                    {
                        items.Add(new ComboBoxItem<TItemNullable>(nullValue, noSelectionText));
                    }

                    if (c != null)
                    {
                        items.AddRange(
                            c.Select(i => new ComboBoxItem<TItemNullable>(convertToNullable(i), getItemText(i))));
                    }

                    latestItems = items;

                    return items;
                };

            bool settingSelectedItem = false;
            bool itemsChanging = false;
            IObservable<TSelectedItem> selectedItemObservable =
                dataContext.BeginChain().Add(getSelectedItemProperty).CompleteWithDefaultIfNotComputable();
            IObservable<IReadableObservableCollection<TItem>> itemsObservable =
                dataContext.BeginChain().Add(getItems).CompleteWithDefaultIfNotComputable();
            compositeDisposable.Add(
                selectedItemObservable.Join(
                    itemsObservable.MergeCollectionPropertyWithChanges(itemsObservable),
                    s => selectedItemObservable.Skip(1),
                    i => Observable.Empty<Unit>(),
                    Tuple.Create).ObserveOnDispatcher().Subscribe(
                        v =>
                        {
                            itemsChanging = true;
                            try
                            {
                                comboBox.ItemsSource = getComboBoxItems(v.Item2.Collection);
                            }
                            finally
                            {
                                itemsChanging = false;
                            }

                            TItemNullable nullableValue = convertFromSelectedItem(v.Item1);

                            if (!addNoSelection && isNull(nullableValue))
                            {
                                throw new InvalidOperationException("Selected item may not be null.");
                            }

                            settingSelectedItem = true;
                            try
                            {
                                comboBox.SelectedItem =
                                    latestItems.FirstOrDefault(i => areEqual(i.Value, nullableValue));
                            }
                            finally
                            {
                                settingSelectedItem = false;
                            }
                        }));
            IBinding selectedItemBinding = bindingFactory.CreateTwoWayBinding(
                    dataContext,
                    getSelectedItemProperty,
                    b =>
                    Observable.FromEventPattern<SelectionChangedEventHandler, SelectionChangedEventArgs>(
                        h => (sender, args) =>
                            {
                                if (!b.IsUpdatingControl && !settingSelectedItem && !itemsChanging)
                                {
                                    h(sender, args);
                                }
                            },
                        h => comboBox.SelectionChanged += h,
                        h => comboBox.SelectionChanged -= h),
                    v =>
                    {
                        if (latestItems == null)
                        {
                            return;
                        }

                        TItemNullable nullableValue = convertFromSelectedItem(v);

                        if (!addNoSelection && isNull(nullableValue))
                        {
                            throw new InvalidOperationException("Selected item may not be null.");
                        }

                        comboBox.SelectedItem = latestItems.FirstOrDefault(i => areEqual(i.Value, nullableValue));
                    },
                    () =>
                    {
                        ComboBoxItem<TItemNullable> selectedItem =
                            comboBox.SelectedItem as ComboBoxItem<TItemNullable>;
                        if (!addNoSelection && (selectedItem == null || isNull(selectedItem.Value)))
                        {
                            throw new InvalidOperationException(
                                StaticReflection<ComboBox>.GetMemberInfo(o => o.SelectedItem).Name
                                + " may not be null.");
                        }

                        return convertToSelectedItem(selectedItem == null ? nullValue : selectedItem.Value);
                    });
            compositeDisposable.Add(selectedItemBinding);
            return new ComboBoxItemsBindings(compositeDisposable, () => selectedItemBinding.IsUpdatingControl || settingSelectedItem, () => itemsChanging);
        }

        private class ComboBoxItem<T>
        {
            private readonly T value;

            private readonly string text;

            /// <summary>
            /// Initializes a new instance of the <see cref="ComboBoxItem{T}"/> class.
            /// </summary>
            /// <param name="value">
            /// The value.
            /// </param>
            /// <param name="text">
            /// The text.
            /// </param>
            public ComboBoxItem(T value, string text)
            {
                this.value = value;
                this.text = text;
            }

            /// <summary>
            /// Gets the value.
            /// </summary>
            public T Value
            {
                get
                {
                    return this.value;
                }
            }

            /// <summary>
            /// The to string.
            /// </summary>
            /// <returns>
            /// The <see cref="string"/>.
            /// </returns>
            public override string ToString()
            {
                return this.text ?? string.Empty;
            }
        }
    }
}