namespace MorseCode.RxMvvm.Observable.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(IObservableCollectionChanged<>))]
    internal class ObservableCollectionChangedContract<T> : IObservableCollectionChanged<T>
    {
        IReadOnlyList<T> IObservableCollectionChanged<T>.OldItems
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyList<T>>() != null);

                return null;
            }
        }

        IReadOnlyList<T> IObservableCollectionChanged<T>.NewItems
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyList<T>>() != null);

                return null;
            }
        }

        void IObservableCollectionChanged<T>.Process(Action<IReadOnlyList<T>> processOldItems, Action<IReadOnlyList<T>> processNewItems)
        {
            Contract.Requires(processOldItems != null);
            Contract.Requires(processNewItems != null);
        }
    }
}