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

    using MorseCode.RxMvvm.Common.StaticReflection;
    using MorseCode.RxMvvm.Observable.Property;

    /// <summary>
    /// A factory for creating WPF bindings.
    /// </summary>
    /// <typeparam name="T">
    /// The type on which to create bindings.
    /// </typeparam>
    public class BindingFactory<T> : IBindingFactory<T>
    {
        private static readonly Lazy<BindingFactory<T>> InstanceLazy =
            new Lazy<BindingFactory<T>>(() => new BindingFactory<T>());

        private BindingFactory()
        {
        }

        /// <summary>
        /// Gets the singleton instance of a <see cref="BindingFactory{T}"/>.
        /// </summary>
        public static IBindingFactory<T> Instance
        {
            get
            {
                return InstanceLazy.Value;
            }
        }

        Binding IBindingFactory<T>.CreateOneWayBinding<TProperty>(
            Expression<Func<T, IReadableObservableProperty<TProperty>>> getPropertyName)
        {
            PropertyInfo pathParameters =
                typeof(IReadableObservableProperty<TProperty>).GetProperty(BindingFactoryUtility.ValuePropertyName);
            return new Binding
                       {
                           Path =
                               new PropertyPath(
                               StaticReflection<T>.GetMemberInfo(getPropertyName).Name + ".(0)", pathParameters),
                           Mode = BindingMode.OneWay
                       };
        }

        Binding IBindingFactory<T>.CreateOneWayToSourceBinding<TProperty>(
            Expression<Func<T, IWritableObservableProperty<TProperty>>> getPropertyName)
        {
            PropertyInfo pathParameters =
                typeof(IWritableObservableProperty<TProperty>).GetProperty(BindingFactoryUtility.ValuePropertyName);
            return new Binding
                       {
                           Path =
                               new PropertyPath(
                               StaticReflection<T>.GetMemberInfo(getPropertyName).Name + ".(0)", pathParameters),
                           Mode = BindingMode.OneWayToSource
                       };
        }

        Binding IBindingFactory<T>.CreateTwoWayBinding<TProperty>(
            Expression<Func<T, IObservableProperty<TProperty>>> getPropertyName)
        {
            PropertyInfo pathParameters =
                typeof(IObservableProperty<TProperty>).GetProperty(BindingFactoryUtility.ValuePropertyName);
            return new Binding
                       {
                           Path =
                               new PropertyPath(
                               StaticReflection<T>.GetMemberInfo(getPropertyName).Name + ".(0)", pathParameters),
                           Mode = BindingMode.TwoWay
                       };
        }

        Binding IBindingFactory<T>.CreateCalculatedBinding<TProperty>(
            Expression<Func<T, ICalculatedProperty<TProperty>>> getPropertyName)
        {
            PropertyInfo pathParameters =
                typeof(ICalculatedProperty<TProperty>).GetProperty(BindingFactoryUtility.LatestSuccessfulValuePropertyName);
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