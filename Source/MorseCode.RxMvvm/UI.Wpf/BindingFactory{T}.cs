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
    using System.Reactive;
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Common.StaticReflection;
    using MorseCode.RxMvvm.Observable;
    using MorseCode.RxMvvm.Observable.Property;

    /// <summary>
    /// A factory for creating WPF bindings.
    /// </summary>
    /// <typeparam name="T">
    /// The type on which to create bindings.
    /// </typeparam>
    public class BindingFactory<T> : IBindingFactory<T>
        where T : class
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

        IBinding IBindingFactory<T>.CreateOneWayBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IObservable<TProperty>> getDataContextValue, 
            Action<TProperty> setControlValue)
        {
            Binding binding = new Binding();
            IObservable<TProperty> viewModelObservable =
                dataContext.BeginChain().Add(getDataContextValue).CompleteWithDefaultIfNotComputable();
            IDisposable subscriptionDisposable = viewModelObservable.ObserveOnDispatcher().Subscribe(setControlValue);
            if (subscriptionDisposable == null)
            {
                throw new InvalidOperationException(
                    "Result of " + StaticReflection<IObservable<TProperty>>.GetMethodInfo(o => o.Subscribe()).Name
                    + " cannot be null.");
            }

            binding.Add(subscriptionDisposable);
            return binding;
        }

        IBinding IBindingFactory<T>.CreateOneWayToSourceBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IWritableObservableProperty<TProperty>> getDataContextProperty, 
            Func<IBinding, IObservable<object>> createUiObservable, 
            Func<TProperty> getControlValue)
        {
            Binding binding = new Binding();
            IObservable<object> uiObservable = createUiObservable(binding);
            IDisposable subscriptionDisposable =
                dataContext.Select(v => v == null ? null : getDataContextProperty(v))
                           .Select(v => uiObservable.Select(e => v))
                           .Switch()
                           .ObserveOnDispatcher()
                           .Subscribe(
                               p =>
                                   {
                                       if (p != null)
                                       {
                                           p.Value = getControlValue();
                                       }
                                   });
            if (subscriptionDisposable == null)
            {
                throw new InvalidOperationException(
                    "Result of " + StaticReflection<IObservable<TProperty>>.GetMethodInfo(o => o.Subscribe()).Name
                    + " cannot be null.");
            }

            binding.Add(subscriptionDisposable);
            return binding;
        }

        IBinding IBindingFactory<T>.CreateTwoWayBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IObservableProperty<TProperty>> getDataContextProperty, 
            Func<IBinding, IObservable<object>> createUiObservable, 
            Action<TProperty> setControlValue, 
            Func<TProperty> getControlValue)
        {
            {
                Binding binding = new Binding();
                IObservable<TProperty> viewModelObservable =
                    dataContext.BeginChain().Add(getDataContextProperty).CompleteWithDefaultIfNotComputable();
                IDisposable subscriptionDisposable1 =
                    viewModelObservable.ObserveOnDispatcher().Subscribe(setControlValue);
                if (subscriptionDisposable1 == null)
                {
                    throw new InvalidOperationException(
                        "Result of " + StaticReflection<IObservable<TProperty>>.GetMethodInfo(o => o.Subscribe()).Name
                        + " cannot be null.");
                }

                binding.Add(subscriptionDisposable1);
                IObservable<object> uiObservable = createUiObservable(binding);
                IObservable<IObservableProperty<TProperty>> propertyObservable =
                    dataContext.Select(d => d == null ? null : getDataContextProperty(d));
                IDisposable subscriptionDisposable2 =
                    propertyObservable.Join(
                        uiObservable, p => propertyObservable.Skip(1), e => Observable.Empty<Unit>(), (p, e) => p)
                                      .ObserveOnDispatcher()
                                      .Subscribe(
                                          p =>
                                              {
                                                  if (p != null)
                                                  {
                                                      p.Value = getControlValue();
                                                  }
                                              });
                if (subscriptionDisposable2 == null)
                {
                    throw new InvalidOperationException(
                        "Result of "
                        + StaticReflection<IObservable<IObservableProperty<TProperty>>>.GetMethodInfo(
                            o => o.Subscribe()).Name + " cannot be null.");
                }

                binding.Add(subscriptionDisposable2);
                return binding;
            }
        }
    }
}