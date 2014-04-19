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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    using MorseCode.RxMvvm.Common;
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
        /// <param name="getItemsProperty">
        /// The items property.
        /// </param>
        /// <param name="getItemText">
        /// A delegate to get the text to display for an item.
        /// </param>
        /// <param name="getSelectedItemProperty">
        /// The selected item property.
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
        public static void BindItems<T, TItem>(
            this ComboBox comboBox,
            IReadableObservableProperty<T> dataContext,
            Expression<Func<T, IReadableObservableProperty<IReadableObservableCollection<TItem>>>> getItemsProperty,
            Func<TItem, string> getItemText,
            Expression<Func<T, IObservableProperty<TItem>>> getSelectedItemProperty,
            IBindingFactory<T> bindingFactory) where TItem : class
        {
            Contract.Requires<ArgumentNullException>(comboBox != null, "comboBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getItemsProperty != null, "getItemsProperty");
            Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");

            Binding itemsBinding = bindingFactory.CreateOneWayBinding(getItemsProperty);
            itemsBinding.Converter = new ItemsConverterForClass<TItem>(getItemText);
            comboBox.SetBinding(ItemsControl.ItemsSourceProperty, itemsBinding);

            Binding selectedItemBinding = bindingFactory.CreateTwoWayBinding(getSelectedItemProperty);
            selectedItemBinding.Converter = new SelectedItemConverterForClass<TItem>(comboBox);
            comboBox.SetBinding(Selector.SelectedItemProperty, selectedItemBinding);
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
        /// <param name="getItemsProperty">
        /// The items property.
        /// </param>
        /// <param name="getItemText">
        /// A delegate to get the text to display for an item.
        /// </param>
        /// <param name="getSelectedItemProperty">
        /// The selected item property.
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
        public static void BindItemsForStruct<T, TItem>(
            this ComboBox comboBox,
            IReadableObservableProperty<T> dataContext,
            Expression<Func<T, IReadableObservableProperty<IReadableObservableCollection<TItem>>>> getItemsProperty,
            Func<TItem, string> getItemText,
            Expression<Func<T, IObservableProperty<TItem>>> getSelectedItemProperty,
            IBindingFactory<T> bindingFactory) where TItem : struct
        {
            Contract.Requires<ArgumentNullException>(comboBox != null, "comboBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getItemsProperty != null, "getItemsProperty");
            Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");

            Binding itemsBinding = bindingFactory.CreateOneWayBinding(getItemsProperty);
            itemsBinding.Converter = new ItemsConverterForStruct<TItem>(getItemText);
            comboBox.SetBinding(ItemsControl.ItemsSourceProperty, itemsBinding);

            Binding selectedItemBinding = bindingFactory.CreateTwoWayBinding(getSelectedItemProperty);
            selectedItemBinding.Converter = new SelectedItemConverterForStruct<TItem>(comboBox);
            comboBox.SetBinding(Selector.SelectedItemProperty, selectedItemBinding);
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
        /// <param name="getItemsProperty">
        /// The items property.
        /// </param>
        /// <param name="getItemText">
        /// A delegate to get the text to display for an item.
        /// </param>
        /// <param name="noSelectionText">
        /// The text to show when nothing is selected.
        /// </param>
        /// <param name="getSelectedItemProperty">
        /// The selected item property.
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
        public static void BindItemsWithNoSelection<T, TItem>(
            this ComboBox comboBox,
            IReadableObservableProperty<T> dataContext,
            Expression<Func<T, IReadableObservableProperty<IReadableObservableCollection<TItem>>>> getItemsProperty,
            Func<TItem, string> getItemText,
            string noSelectionText,
            Expression<Func<T, IObservableProperty<TItem>>> getSelectedItemProperty,
            IBindingFactory<T> bindingFactory) where TItem : class
        {
            Contract.Requires<ArgumentNullException>(comboBox != null, "comboBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getItemsProperty != null, "getItemsProperty");
            Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");

            Binding itemsBinding = bindingFactory.CreateOneWayBinding(getItemsProperty);
            itemsBinding.Converter = new AddNoSelectionItemConverterForClass<TItem>(getItemText, noSelectionText);
            comboBox.SetBinding(ItemsControl.ItemsSourceProperty, itemsBinding);

            Binding selectedItemBinding = bindingFactory.CreateTwoWayBinding(getSelectedItemProperty);
            selectedItemBinding.Converter = new SelectedItemConverterForClass<TItem>(comboBox);
            comboBox.SetBinding(Selector.SelectedItemProperty, selectedItemBinding);
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
        /// <param name="getItemsProperty">
        /// The items property.
        /// </param>
        /// <param name="getItemText">
        /// A delegate to get the text to display for an item.
        /// </param>
        /// <param name="noSelectionText">
        /// The text to show when nothing is selected.
        /// </param>
        /// <param name="getSelectedItemProperty">
        /// The selected item property.
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
        public static void BindItemsForStructWithNoSelection<T, TItem>(
            this ComboBox comboBox,
            IReadableObservableProperty<T> dataContext,
            Expression<Func<T, IReadableObservableProperty<IReadableObservableCollection<TItem>>>> getItemsProperty,
            Func<TItem, string> getItemText,
            string noSelectionText,
            Expression<Func<T, IObservableProperty<TItem?>>> getSelectedItemProperty,
            IBindingFactory<T> bindingFactory) where TItem : struct
        {
            Contract.Requires<ArgumentNullException>(comboBox != null, "comboBox");
            Contract.Requires<ArgumentNullException>(dataContext != null, "dataContext");
            Contract.Requires<ArgumentNullException>(getItemsProperty != null, "getItemsProperty");
            Contract.Requires<ArgumentNullException>(getSelectedItemProperty != null, "getSelectedItemProperty");
            Contract.Requires<ArgumentNullException>(bindingFactory != null, "bindingFactory");

            Binding itemsBinding = bindingFactory.CreateOneWayBinding(getItemsProperty);
            itemsBinding.Converter = new AddNoSelectionItemConverterForStruct<TItem>(getItemText, noSelectionText);
            comboBox.SetBinding(ItemsControl.ItemsSourceProperty, itemsBinding);

            Binding selectedItemBinding = bindingFactory.CreateTwoWayBinding(getSelectedItemProperty);
            selectedItemBinding.Converter = new SelectedItemConverterForStruct<TItem>(comboBox);
            comboBox.SetBinding(Selector.SelectedItemProperty, selectedItemBinding);
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

        private class ItemsConverterForClass<T> : AddNoSelectionItemConverter<T, T>
            where T : class
        {
            internal ItemsConverterForClass(Func<T, string> getItemText)
                : base(getItemText, false, null, null)
            {
            }
        }

        private class ItemsConverterForStruct<T> : AddNoSelectionItemConverter<T, T?>
            where T : struct
        {
            internal ItemsConverterForStruct(Func<T, string> getItemText)
                : base(getItemText, false, null, null)
            {
            }
        }

        private class AddNoSelectionItemConverterForClass<T> : AddNoSelectionItemConverter<T, T>
            where T : class
        {
            internal AddNoSelectionItemConverterForClass(Func<T, string> getItemText, string noSelectionText)
                : base(getItemText, true, noSelectionText, null)
            {
            }
        }

        private class AddNoSelectionItemConverterForStruct<T> : AddNoSelectionItemConverter<T, T?>
            where T : struct
        {
            internal AddNoSelectionItemConverterForStruct(Func<T, string> getItemText, string noSelectionText)
                : base(getItemText, true, noSelectionText, null)
            {
            }
        }

        private abstract class AddNoSelectionItemConverter<T, TNullable> : IValueConverter
        {
            private readonly Func<T, string> getItemText;

            private readonly bool addNoSelection;

            private readonly string noSelectionText;

            private readonly TNullable nullValue;

            /// <summary>
            /// Initializes a new instance of the <see cref="AddNoSelectionItemConverter{T,TNullable}"/> class.
            /// </summary>
            /// <param name="getItemText">
            /// The get item text.
            /// </param>
            /// <param name="addNoSelection">
            /// Whether to add the no selection item.
            /// </param>
            /// <param name="noSelectionText">
            /// The text to show when nothing is selected.
            /// </param>
            /// <param name="nullValue">
            /// The null value.
            /// </param>
            protected AddNoSelectionItemConverter(Func<T, string> getItemText, bool addNoSelection, string noSelectionText, TNullable nullValue)
            {
                this.getItemText = getItemText ?? (o => o.SafeToString());
                this.addNoSelection = addNoSelection;
                this.noSelectionText = noSelectionText;
                this.nullValue = nullValue;
            }

            object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                List<ComboBoxItem<TNullable>> items = new List<ComboBoxItem<TNullable>>();

                if (this.addNoSelection)
                {
                    items.Add(new ComboBoxItem<TNullable>(this.nullValue, this.noSelectionText));
                }

                if (value != null)
                {
                    items.AddRange(
                        ((IEnumerable)value).Cast<TNullable>()
                                            .Select(i => new ComboBoxItem<TNullable>(i, this.getItemText((T)(object)i))));
                }

                return items;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new InvalidOperationException();
            }
        }

        private class SelectedItemConverterForClass<T> : SelectedItemConverter<T>
            where T : class
        {
            internal SelectedItemConverterForClass(ComboBox comboBox)
                : base(comboBox)
            {
            }
        }

        private class SelectedItemConverterForStruct<T> : SelectedItemConverter<T?>
            where T : struct
        {
            internal SelectedItemConverterForStruct(ComboBox comboBox)
                : base(comboBox)
            {
            }
        }

        private abstract class SelectedItemConverter<T> : IValueConverter
        {
            private readonly ComboBox comboBox;

            /// <summary>
            /// Initializes a new instance of the <see cref="SelectedItemConverter{T}"/> class.
            /// </summary>
            /// <param name="comboBox">
            /// The combo box.
            /// </param>
            protected SelectedItemConverter(ComboBox comboBox)
            {
                this.comboBox = comboBox;
            }

            object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                ItemCollection items = this.comboBox.Items;

                if (items != null)
                {
                    return
                        items.Cast<ComboBoxItem<T>>()
                             .FirstOrDefault(i => ReferenceEquals(null, i.Value) ? value == null : i.Value.Equals(value));
                }

                return null;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return ((ComboBoxItem<T>)value).Value;
            }
        }
    }
}