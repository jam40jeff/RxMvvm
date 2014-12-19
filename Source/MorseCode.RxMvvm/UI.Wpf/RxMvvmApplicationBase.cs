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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using System.Windows;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Common.StaticReflection;
    using MorseCode.RxMvvm.UI.Wpf.Controls;
    using MorseCode.RxMvvm.ViewModel;

    /// <summary>
    /// A base class for a WPF application.
    /// </summary>
    [ContractClass(typeof(RxMvvmApplicationContract))]
    public abstract class RxMvvmApplicationBase : Application, IDisposable
    {
        #region Fields

        private readonly Dictionary<Type, ApplicationView> applicationViews = new Dictionary<Type, ApplicationView>();

        private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        private readonly ViewRegistrationHelper viewRegistrationHelper;

        private IView currentView;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RxMvvmApplicationBase"/> class.
        /// </summary>
        protected RxMvvmApplicationBase()
        {
            this.viewRegistrationHelper = new ViewRegistrationHelper(this);
        }

        #endregion

        #region Properties

        private Dictionary<Type, ApplicationView> ApplicationViews
        {
            get
            {
                Contract.Ensures(Contract.Result<Dictionary<Type, ApplicationView>>() != null);

                return this.applicationViews;
            }
        }

        #endregion

        #region Explicit Interface Methods

        void IDisposable.Dispose()
        {
            this.compositeDisposable.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the application view model.
        /// </summary>
        /// <returns>
        /// The application view model.
        /// </returns>
        protected abstract IApplicationViewModel CreateApplicationViewModel();

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnStartup(StartupEventArgs e)
        {
            RxMvvmApplicationUtility.SetupRxMvvmConfiguration(this);

            base.OnStartup(e);

            this.RegisterViews(this.viewRegistrationHelper);

            IApplicationViewModel viewModel = this.CreateApplicationViewModel();
            this.compositeDisposable.Add(viewModel.CurrentViewModel.Subscribe(this.CurrentViewModelChanged));
            viewModel.Initialize();
        }

        /// <summary>
        /// Registers the views.
        /// </summary>
        /// <param name="viewRegistrationHelper">
        /// The view registration helper.
        /// </param>
        protected abstract void RegisterViews(ViewRegistrationHelper viewRegistrationHelper);

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.applicationViews != null);
            Contract.Invariant(this.viewRegistrationHelper != null);
            Contract.Invariant(this.compositeDisposable != null);
        }

        private void CurrentViewModelChanged(object currentViewModel)
        {
            IView oldView = this.currentView;

            this.currentView = null;

            if (currentViewModel != null)
            {
                Type currentViewModelType = currentViewModel.GetType();
                if (!this.applicationViews.ContainsKey(currentViewModelType))
                {
                    throw new Exception(
                        "Could not find a view with view model type " + currentViewModelType.FullName + ".");
                }

                ApplicationView applicationView = this.applicationViews[currentViewModelType];

                if (applicationView == null)
                {
                    throw new InvalidOperationException(
                        StaticReflection.GetInScopeMemberInfo(() => applicationView).Name
                        + " may not contain null values.");
                }

                IView newView = applicationView.CreateView();

                if (newView == null)
                {
                    throw new InvalidOperationException(
                        StaticReflection<ApplicationView>.GetMethodInfo(o => o.CreateView).Name
                        + " may not return null.");
                }

                this.currentView = newView;

                // Title = _currentPage.Title;
                applicationView.Bind(newView, currentViewModel);

                newView.ShowReplacing(oldView);
            }

            if (oldView != null)
            {
                oldView.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// The view registration helper.
        /// </summary>
        public class ViewRegistrationHelper
        {
            #region Fields

            private readonly RxMvvmApplicationBase application;

            #endregion

            #region Constructors and Destructors

            internal ViewRegistrationHelper(RxMvvmApplicationBase application)
            {
                Contract.Requires<ArgumentNullException>(application != null);
                Contract.Ensures(this.application != null);

                this.application = application;
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// Registers a view with the application.
            /// </summary>
            /// <param name="createView">
            /// The create view.
            /// </param>
            /// <typeparam name="TView">
            /// The type of the view.
            /// </typeparam>
            /// <returns>
            /// The view registration helper providing second step methods.
            /// </returns>
            public ViewRegistrationHelperStep2<TView> RegisterView<TView>(Func<TView> createView)
                where TView : class, IView
            {
                Contract.Requires<ArgumentNullException>(createView != null);

                return new ViewRegistrationHelperStep2<TView>(this.application, createView);
            }

            #endregion

            #region Methods

            [ContractInvariantMethod]
            private void CodeContractsInvariants()
            {
                Contract.Invariant(this.application != null);
            }

            #endregion
        }

        /// <summary>
        /// A class providing view registration helper methods for the second step.
        /// </summary>
        /// <typeparam name="TView">
        /// The type of the view.
        /// </typeparam>
        public class ViewRegistrationHelperStep2<TView>
            where TView : class, IView
        {
            #region Fields

            private readonly RxMvvmApplicationBase application;

            private readonly Func<TView> createView;

            #endregion

            #region Constructors and Destructors

            internal ViewRegistrationHelperStep2(RxMvvmApplicationBase application, Func<TView> createView)
            {
                Contract.Requires<ArgumentNullException>(application != null);
                Contract.Requires<ArgumentNullException>(createView != null);
                Contract.Ensures(this.application != null);
                Contract.Ensures(this.createView != null);

                this.application = application;
                this.createView = createView;
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// Ties a binding to the view being added.
            /// </summary>
            /// <param name="bind">
            /// A delegate which binds the view to a data context.
            /// </param>
            /// <typeparam name="TDataContext">
            /// The type of the data context.
            /// </typeparam>
            public void WithBinding<TDataContext>(Action<TView, TDataContext> bind)
            {
                this.application.ApplicationViews.Add(
                    typeof(TDataContext),
                    new ApplicationView(this.createView, (p, d) => bind((TView)p, (TDataContext)d)));
            }

            #endregion

            #region Methods

            [ContractInvariantMethod]
            private void CodeContractsInvariants()
            {
                Contract.Invariant(this.application != null);
                Contract.Invariant(this.createView != null);
            }

            #endregion
        }

        private class ApplicationView
        {
            #region Fields

            private readonly Action<IView, object> bind;

            private readonly Func<IView> createView;

            #endregion

            #region Constructors and Destructors

            internal ApplicationView(Func<IView> createView, Action<IView, object> bind)
            {
                Contract.Requires<ArgumentNullException>(createView != null);
                Contract.Requires<ArgumentNullException>(bind != null);
                Contract.Ensures(this.createView != null);
                Contract.Ensures(this.bind != null);

                this.createView = createView;
                this.bind = bind;
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets the bind.
            /// </summary>
            public Action<IView, object> Bind
            {
                get
                {
                    Contract.Ensures(Contract.Result<Action<IView, object>>() != null);

                    return this.bind;
                }
            }

            /// <summary>
            /// Gets the create view.
            /// </summary>
            public Func<IView> CreateView
            {
                get
                {
                    Contract.Ensures(Contract.Result<Func<IView>>() != null);

                    return this.createView;
                }
            }

            #endregion

            #region Methods

            [ContractInvariantMethod]
            private void CodeContractsInvariants()
            {
                Contract.Invariant(this.createView != null);
                Contract.Invariant(this.bind != null);
            }

            #endregion
        }
    }
}