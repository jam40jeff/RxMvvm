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
    using System.Reactive;
    using System.Reactive.Linq;

    using MorseCode.RxMvvm.Common.DiscriminatedUnion;
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
        [ContractVerification(false)]
        public static IBindingFactory<T> Instance
        {
            get
            {
                Contract.Ensures(Contract.Result<IBindingFactory<T>>() != null);

                return InstanceLazy.Value;
            }
        }

        IBinding IBindingFactory<T>.CreateOneWayBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IObservable<TProperty>> getDataContextValue, 
            Action<TProperty> setControlValue)
        {
            return
                CreateOneWayBinding(
                    dataContext.BeginChain().Add(getDataContextValue).CompleteWithDefaultIfNotComputable(), 
                    setControlValue);
        }

        IBinding IBindingFactory<T>.CreateChainedOneWayBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IObservable<IDiscriminatedUnion<object, TProperty, NonComputable>>> getDataContextValue, 
            Action<TProperty> setControlValue)
        {
            return
                CreateOneWayBinding(
                    dataContext.BeginChain()
                               .Add(getDataContextValue)
                               .Complete()
                               .Select(u => u.Flatten().Switch(v => v, _ => default(TProperty))), 
                    setControlValue);
        }

        IBinding IBindingFactory<T>.CreateOneWayToSourceBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IWritableObservableProperty<TProperty>> getDataContextProperty, 
            Func<IBinding, IObservable<object>> createUiObservable, 
            Func<TProperty> getControlValue)
        {
            return CreateOneWayToSourceBinding(
                dataContext, 
                d =>
                Observable.Return(
                    DiscriminatedUnion.First<object, IWritableObservableProperty<TProperty>, NonComputable>(
                        getDataContextProperty(d))), 
                createUiObservable, 
                getControlValue);
        }

        IBinding IBindingFactory<T>.CreateChainedOneWayToSourceBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IObservable<IDiscriminatedUnion<object, IWritableObservableProperty<TProperty>, NonComputable>>> getDataContextProperty, 
            Func<IBinding, IObservable<object>> createUiObservable, 
            Func<TProperty> getControlValue)
        {
            return CreateOneWayToSourceBinding(dataContext, getDataContextProperty, createUiObservable, getControlValue);
        }

        IBinding IBindingFactory<T>.CreateTwoWayBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IObservableProperty<TProperty>> getDataContextProperty, 
            Func<IBinding, IObservable<object>> createUiObservable, 
            Action<TProperty> setControlValue, 
            Func<TProperty> getControlValue)
        {
            return CreateTwoWayBinding(
                dataContext, 
                d =>
                Observable.Return(
                    DiscriminatedUnion.First<object, IObservableProperty<TProperty>, NonComputable>(
                        getDataContextProperty(d))), 
                createUiObservable, 
                setControlValue, 
                getControlValue);
        }

        IBinding IBindingFactory<T>.CreateChainedTwoWayBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IObservable<IDiscriminatedUnion<object, IObservableProperty<TProperty>, NonComputable>>> getDataContextProperty, 
            Func<IBinding, IObservable<object>> createUiObservable, 
            Action<TProperty> setControlValue, 
            Func<TProperty> getControlValue)
        {
            return CreateTwoWayBinding(
                dataContext, getDataContextProperty, createUiObservable, setControlValue, getControlValue);
        }

        IBinding IBindingFactory<T>.CreateActionBinding(
            IObservable<T> dataContext, 
            Func<T, Action> getDataContextAction, 
            Func<IBinding, IObservable<object>> createUiObservable)
        {
            return this.CreateActionBinding(
                dataContext, 
                d => Observable.Return(DiscriminatedUnion.First<object, Action, NonComputable>(getDataContextAction(d))), 
                createUiObservable);
        }

        IBinding IBindingFactory<T>.CreateActionBinding(
            IObservable<T> dataContext, 
            Func<T, IObservable<IDiscriminatedUnion<object, Action, NonComputable>>> getDataContextAction, 
            Func<IBinding, IObservable<object>> createUiObservable)
        {
            return this.CreateActionBinding(dataContext, getDataContextAction, createUiObservable);
        }

        private IBinding CreateOneWayBinding<TProperty>(
            IObservable<TProperty> dataContextValueObservable, Action<TProperty> setControlValue)
        {
            Binding binding = new Binding();
            IDisposable subscriptionDisposable =
                dataContextValueObservable.ObserveOnDispatcher().Subscribe(setControlValue);
            if (subscriptionDisposable == null)
            {
                throw new InvalidOperationException(
                    "Result of " + StaticReflection<IObservable<TProperty>>.GetMethodInfo(o => o.Subscribe()).Name
                    + " cannot be null.");
            }

            binding.Add(subscriptionDisposable);
            return binding;
        }

        private IBinding CreateOneWayToSourceBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IObservable<IDiscriminatedUnion<object, IWritableObservableProperty<TProperty>, NonComputable>>> getDataContextProperty, 
            Func<IBinding, IObservable<object>> createUiObservable, 
            Func<TProperty> getControlValue)
        {
            Binding binding = new Binding();
            IObservable<object> uiObservable = createUiObservable(binding);
            IObservable<IDiscriminatedUnion<object, IWritableObservableProperty<TProperty>, NonComputable>> dataContextPropertyObservableForJoin =
                    dataContext.BeginChain()
                               .Add(getDataContextProperty)
                               .Complete()
                               .Select(u => u.Flatten())
                               .Publish()
                               .RefCount();
            IDisposable subscriptionDisposable =
                dataContextPropertyObservableForJoin.Join(
                    uiObservable, u => dataContextPropertyObservableForJoin, e => Observable.Empty<Unit>(), (u, e) => u)
                                                    .ObserveOnDispatcher()
                                                    .Subscribe(
                                                        u => u.Switch(p => p.Value = getControlValue(), _ => { }));
            if (subscriptionDisposable == null)
            {
                throw new InvalidOperationException(
                    "Result of " + StaticReflection<IObservable<TProperty>>.GetMethodInfo(o => o.Subscribe()).Name
                    + " cannot be null.");
            }

            binding.Add(subscriptionDisposable);
            return binding;
        }

        private IBinding CreateTwoWayBinding<TProperty>(
            IObservable<T> dataContext, 
            Func<T, IObservable<IDiscriminatedUnion<object, IObservableProperty<TProperty>, NonComputable>>> getDataContextProperty, 
            Func<IBinding, IObservable<object>> createUiObservable, 
            Action<TProperty> setControlValue, 
            Func<TProperty> getControlValue)
        {
            Binding binding = new Binding();
            IObservable<object> uiObservable = createUiObservable(binding);
            IObservable<IDiscriminatedUnion<object, IObservableProperty<TProperty>, NonComputable>> dataContextPropertyObservable =
                    dataContext.BeginChain().Add(getDataContextProperty).Complete().Select(u => u.Flatten());

            IDisposable subscriptionDisposable1 =
                dataContextPropertyObservable.ObserveOnDispatcher()
                                             .Subscribe(
                                                 u => setControlValue(u.Switch(p => p.Value, _ => default(TProperty))));
            if (subscriptionDisposable1 == null)
            {
                throw new InvalidOperationException(
                    "Result of " + StaticReflection<IObservable<TProperty>>.GetMethodInfo(o => o.Subscribe()).Name
                    + " cannot be null.");
            }

            IObservable<IDiscriminatedUnion<object, IObservableProperty<TProperty>, NonComputable>> dataContextPropertyObservableForJoin = dataContextPropertyObservable.Publish().RefCount();
            IDisposable subscriptionDisposable2 =
                dataContextPropertyObservableForJoin.Join(
                    uiObservable, u => dataContextPropertyObservableForJoin, e => Observable.Empty<Unit>(), (u, e) => u)
                                                    .ObserveOnDispatcher()
                                                    .Subscribe(
                                                        u => u.Switch(p => p.Value = getControlValue(), _ => { }));
            if (subscriptionDisposable2 == null)
            {
                throw new InvalidOperationException(
                    "Result of " + StaticReflection<IObservable<TProperty>>.GetMethodInfo(o => o.Subscribe()).Name
                    + " cannot be null.");
            }

            binding.Add(subscriptionDisposable2);
            return binding;
        }

        private IBinding CreateActionBinding(
            IObservable<T> dataContext, 
            Func<T, IObservable<IDiscriminatedUnion<object, Action, NonComputable>>> getDataContextProperty, 
            Func<IBinding, IObservable<object>> createUiObservable)
        {
            Binding binding = new Binding();
            IObservable<object> uiObservable = createUiObservable(binding);
            IObservable<IDiscriminatedUnion<object, Action, NonComputable>> dataContextActionObservableForJoin =
                dataContext.BeginChain()
                           .Add(getDataContextProperty)
                           .Complete()
                           .Select(u => u.Flatten())
                           .Publish()
                           .RefCount();
            IDisposable subscriptionDisposable =
                dataContextActionObservableForJoin.Join(
                    uiObservable, u => dataContextActionObservableForJoin, e => Observable.Empty<Unit>(), (u, e) => u)
                                                  .ObserveOnDispatcher()
                                                  .Subscribe(u => u.Switch(a => a(), _ => { }));
            if (subscriptionDisposable == null)
            {
                throw new InvalidOperationException(
                    "Result of "
                    + StaticReflection<IObservable<IDiscriminatedUnion<object, Action, NonComputable>>>.GetMethodInfo(
                        o => o.Subscribe()).Name + " cannot be null.");
            }

            binding.Add(subscriptionDisposable);
            return binding;
        }
    }
}