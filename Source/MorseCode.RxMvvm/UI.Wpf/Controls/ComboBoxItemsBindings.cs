namespace MorseCode.RxMvvm.UI.Wpf.Controls
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reactive.Disposables;

    internal class ComboBoxItemsBindings : IComboBoxItemsBindings
    {
        private readonly CompositeDisposable compositeDisposable;

        private readonly Func<bool> isUpdatingControlItems;

        private readonly Func<bool> isUpdatingControlSelectedItem;

        public ComboBoxItemsBindings(CompositeDisposable compositeDisposable, Func<bool> isUpdatingControlItems, Func<bool> isUpdatingControlSelectedItem)
        {
            Contract.Requires<ArgumentNullException>(compositeDisposable != null, "compositeDisposable");
            Contract.Requires<ArgumentNullException>(isUpdatingControlItems != null, " isUpdatingControlItems");
            Contract.Requires<ArgumentNullException>(isUpdatingControlSelectedItem != null, "isUpdatingControlSelectedItem");
            Contract.Ensures(this.compositeDisposable != null);
            Contract.Ensures(this.isUpdatingControlItems != null);
            Contract.Ensures(this.isUpdatingControlSelectedItem != null);

            this.compositeDisposable = compositeDisposable;
            this.isUpdatingControlItems = isUpdatingControlItems;
            this.isUpdatingControlSelectedItem = isUpdatingControlSelectedItem;
        }

        bool IComboBoxItemsBindings.IsUpdatingControlItems
        {
            get
            {
                return this.isUpdatingControlItems();
            }
        }

        bool IComboBoxItemsBindings.IsUpdatingControlSelectedItem
        {
            get
            {
                return this.isUpdatingControlSelectedItem();
            }
        }

        void IDisposable.Dispose()
        {
            this.compositeDisposable.Dispose();
        }

        internal void Add(IDisposable item)
        {
            this.compositeDisposable.Add(item);
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.compositeDisposable != null);
            Contract.Invariant(this.isUpdatingControlItems != null);
            Contract.Invariant(this.isUpdatingControlSelectedItem != null);
        }
    }
}