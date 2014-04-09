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
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Observable.Property;

    /// <summary>
    /// A factory for creating WPF bindings.
    /// </summary>
    /// <typeparam name="T">
    /// The type on which to create bindings.
    /// </typeparam>
    public static class BindingFactory<T>
    {
        /// <summary>
        /// Create a one-way binding.
        /// </summary>
        /// <param name="getPropertyName">
        /// An expression to get the property to bind.
        /// </param>
        /// <typeparam name="TProperty">
        /// The type of the property to bind.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Binding"/>.
        /// </returns>
        public static Binding CreateOneWayBinding<TProperty>(
            Expression<Func<T, IReadableObservableProperty<TProperty>>> getPropertyName)
        {
            PropertyInfo pathParameters =
                typeof(IReadableObservableProperty<TProperty>).GetProperty(
                    StaticReflection<IReadableObservableProperty<TProperty>>.GetMemberInfo(o => o.Value).Name);
            return new Binding
                       {
                           Path =
                               new PropertyPath(
                               StaticReflection<T>.GetMemberInfo(getPropertyName).Name + ".(0)", pathParameters), 
                           Mode = BindingMode.OneWay
                       };
        }

        /// <summary>
        /// Create a one-way-to-source binding.
        /// </summary>
        /// <param name="getPropertyName">
        /// An expression to get the property to bind.
        /// </param>
        /// <typeparam name="TProperty">
        /// The type of the property to bind.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Binding"/>.
        /// </returns>
        public static Binding CreateOneWayToSourceBinding<TProperty>(
            Expression<Func<T, IWritableObservableProperty<TProperty>>> getPropertyName)
        {
            PropertyInfo pathParameters =
                typeof(IWritableObservableProperty<TProperty>).GetProperty(
                    StaticReflection<IReadableObservableProperty<TProperty>>.GetMemberInfo(o => o.Value).Name);
            return new Binding
                       {
                           Path =
                               new PropertyPath(
                               StaticReflection<T>.GetMemberInfo(getPropertyName).Name + ".(0)", pathParameters), 
                           Mode = BindingMode.OneWayToSource
                       };
        }

        /// <summary>
        /// Create a two-way binding.
        /// </summary>
        /// <param name="getPropertyName">
        /// An expression to get the property to bind.
        /// </param>
        /// <typeparam name="TProperty">
        /// The type of the property to bind.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Binding"/>.
        /// </returns>
        public static Binding CreateTwoWayBinding<TProperty>(
            Expression<Func<T, IObservableProperty<TProperty>>> getPropertyName)
        {
            PropertyInfo pathParameters =
                typeof(IObservableProperty<TProperty>).GetProperty(
                    StaticReflection<IObservableProperty<TProperty>>.GetMemberInfo(o => o.Value).Name);
            return new Binding
                       {
                           Path =
                               new PropertyPath(
                               StaticReflection<T>.GetMemberInfo(getPropertyName).Name + ".(0)", pathParameters), 
                           Mode = BindingMode.TwoWay
                       };
        }

        /// <summary>
        /// Create a one-way binding from an <see cref="ICalculatedProperty{TProperty}"/> by accessing the <see cref="ICalculatedProperty{TProperty}.LatestSuccessfulValue"/> property.
        /// </summary>
        /// <param name="getPropertyName">
        /// An expression to get the property to bind.
        /// </param>
        /// <typeparam name="TProperty">
        /// The type of the property to bind.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Binding"/>.
        /// </returns>
        public static Binding CreateCalculatedBinding<TProperty>(
            Expression<Func<T, ICalculatedProperty<TProperty>>> getPropertyName)
        {
            PropertyInfo pathParameters =
                typeof(ICalculatedProperty<TProperty>).GetProperty(
                    StaticReflection<ICalculatedProperty<TProperty>>.GetMemberInfo(o => o.LatestSuccessfulValue).Name);
            return new Binding
                       {
                           Path =
                               new PropertyPath(
                               StaticReflection<T>.GetMemberInfo(getPropertyName).Name + ".(0)", pathParameters), 
                           Mode = BindingMode.OneWay
                       };
        }
    }
}