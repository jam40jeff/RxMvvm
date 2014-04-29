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

namespace MorseCode.RxMvvm.UI.Wpf
{
    using System;
    using System.Diagnostics.Contracts;

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
    using MorseCode.RxMvvm.Observable;
    using MorseCode.RxMvvm.Observable.Property;

    /// <summary>
    /// An interface representing a factory for creating WPF bindings.
    /// </summary>
    /// <typeparam name="T">
    /// The type on which to create bindings.
    /// </typeparam>
    [ContractClass(typeof(BindingFactoryContract<>))]
    public interface IBindingFactory<T>
        where T : class
    {
        /// <summary>
        /// Create a one-way binding.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getDataContextValue">
        /// An delegate to get the value to bind.
        /// </param>
        /// <param name="setControlValue">
        /// An delegate to update the control from the latest value.
        /// </param>
        /// <typeparam name="TProperty">
        /// The type of the property to bind.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IBinding"/> which will clean up the bindings when disposed.
        /// </returns>
        IBinding CreateOneWayBinding<TProperty>(
            IObservable<T> dataContext,
            Func<T, IObservable<TProperty>> getDataContextValue,
            Action<TProperty> setControlValue);

        /// <summary>
        /// Create a one-way binding.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getDataContextValue">
        /// An delegate to get the value to bind.
        /// </param>
        /// <param name="setControlValue">
        /// An delegate to update the control from the latest value.
        /// </param>
        /// <typeparam name="TProperty">
        /// The type of the property to bind.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IBinding"/> which will clean up the bindings when disposed.
        /// </returns>
        IBinding CreateChainedOneWayBinding<TProperty>(
            IObservable<T> dataContext,
            Func<T, IObservable<IDiscriminatedUnion<object, TProperty, NonComputable>>> getDataContextValue,
            Action<TProperty> setControlValue);

        /// <summary>
        /// Create a one-way-to-source binding.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getDataContextProperty">
        /// An delegate to get the property to bind.
        /// </param>
        /// <param name="createUiObservable">
        /// An delegate returning an observable notifying that the UI is ready to provide a new value.  The value of this observable is ignored.
        /// </param>
        /// <param name="getControlValue">
        /// An delegate returning the value of the control.
        /// </param>
        /// <typeparam name="TProperty">
        /// The type of the property to bind.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IBinding"/> which will clean up the bindings when disposed.
        /// </returns>
        IBinding CreateOneWayToSourceBinding<TProperty>(
            IObservable<T> dataContext,
            Func<T, IWritableObservableProperty<TProperty>> getDataContextProperty,
            Func<IBinding, IObservable<object>> createUiObservable,
            Func<TProperty> getControlValue);

        /// <summary>
        /// Create a one-way-to-source binding.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getDataContextProperty">
        /// An delegate to get the property to bind.
        /// </param>
        /// <param name="createUiObservable">
        /// An delegate returning an observable notifying that the UI is ready to provide a new value.  The value of this observable is ignored.
        /// </param>
        /// <param name="getControlValue">
        /// An delegate returning the value of the control.
        /// </param>
        /// <typeparam name="TProperty">
        /// The type of the property to bind.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IBinding"/> which will clean up the bindings when disposed.
        /// </returns>
        IBinding CreateChainedOneWayToSourceBinding<TProperty>(
            IObservable<T> dataContext,
            Func<T, IObservable<IDiscriminatedUnion<object, IWritableObservableProperty<TProperty>, NonComputable>>> getDataContextProperty,
            Func<IBinding, IObservable<object>> createUiObservable,
            Func<TProperty> getControlValue);

        /// <summary>
        /// Create a two-way binding.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getDataContextProperty">
        /// An delegate to get the property to bind.
        /// </param>
        /// <param name="createUiObservable">
        /// An delegate returning an observable notifying that the UI is ready to provide a new value.  The value of this observable is ignored.
        /// </param>
        /// <param name="setControlValue">
        /// An delegate to update the control from the latest value.
        /// </param>
        /// <param name="getControlValue">
        /// An delegate returning the value of the control.
        /// </param>
        /// <typeparam name="TProperty">
        /// The type of the property to bind.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IBinding"/> which will clean up the bindings when disposed.
        /// </returns>
        IBinding CreateTwoWayBinding<TProperty>(
            IObservable<T> dataContext,
            Func<T, IObservableProperty<TProperty>> getDataContextProperty,
            Func<IBinding, IObservable<object>> createUiObservable,
            Action<TProperty> setControlValue,
            Func<TProperty> getControlValue);

        /// <summary>
        /// Create a two-way binding.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getDataContextProperty">
        /// An delegate to get the property to bind.
        /// </param>
        /// <param name="createUiObservable">
        /// An delegate returning an observable notifying that the UI is ready to provide a new value.  The value of this observable is ignored.
        /// </param>
        /// <param name="setControlValue">
        /// An delegate to update the control from the latest value.
        /// </param>
        /// <param name="getControlValue">
        /// An delegate returning the value of the control.
        /// </param>
        /// <typeparam name="TProperty">
        /// The type of the property to bind.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IBinding"/> which will clean up the bindings when disposed.
        /// </returns>
        IBinding CreateChainedTwoWayBinding<TProperty>(
            IObservable<T> dataContext,
            Func<T, IObservable<IDiscriminatedUnion<object, IObservableProperty<TProperty>, NonComputable>>> getDataContextProperty,
            Func<IBinding, IObservable<object>> createUiObservable,
            Action<TProperty> setControlValue,
            Func<TProperty> getControlValue);

        /// <summary>
        /// Create an action binding.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getDataContextAction">
        /// An delegate to get the action to execute.
        /// </param>
        /// <param name="createUiObservable">
        /// An delegate returning an observable notifying that the UI wishes to execute the action.  The value of this observable is ignored.
        /// </param>
        /// <returns>
        /// An <see cref="IBinding"/> which will clean up the bindings when disposed.
        /// </returns>
        IBinding CreateActionBinding(
            IObservable<T> dataContext,
            Func<T, Action> getDataContextAction,
            Func<IBinding, IObservable<object>> createUiObservable);

        /// <summary>
        /// Create an action binding.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="getDataContextAction">
        /// An delegate to get the action to execute.
        /// </param>
        /// <param name="createUiObservable">
        /// An delegate returning an observable notifying that the UI wishes to execute the action.  The value of this observable is ignored.
        /// </param>
        /// <returns>
        /// An <see cref="IBinding"/> which will clean up the bindings when disposed.
        /// </returns>
        IBinding CreateActionBinding(
            IObservable<T> dataContext,
            Func<T, IObservable<IDiscriminatedUnion<object, Action, NonComputable>>> getDataContextAction,
            Func<IBinding, IObservable<object>> createUiObservable);
    }
}