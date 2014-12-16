namespace MorseCode.RxMvvm.Observable.Property.Internal
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClassFor(typeof(ReadableObservablePropertyBase<>))]
    internal abstract class ReadableObservablePropertyBaseContract<T> : ReadableObservablePropertyBase<T>
    {
        protected override IObservable<T> OnChanged
        {
            get
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return null;
            }
        }

        protected override IObservable<T> OnSet
        {
            get
            {
                Contract.Ensures(Contract.Result<IObservable<T>>() != null);

                return null;
            }
        }

        protected override T GetValue()
        {
            return default(T);
        }
    }
}