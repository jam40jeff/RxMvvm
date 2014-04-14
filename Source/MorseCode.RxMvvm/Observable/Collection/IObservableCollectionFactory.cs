namespace MorseCode.RxMvvm.Observable.Collection
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// An interface representing a factory for creating observable collections.
    /// </summary>
    [ContractClass(typeof(ObservableCollectionFactoryContract))]
    public interface IObservableCollectionFactory
    {
        /// <summary>
        /// Creates an observable collection.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the items in the collection.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IObservableCollection{T}"/>.
        /// </returns>
        IObservableCollection<T> CreateObservableCollection<T>();

        /// <summary>
        /// Creates an observable collection from an initial list of items.
        /// </summary>
        /// <param name="list">
        /// The initial list of items.
        /// </param>
        /// <typeparam name="T">
        /// The type of the items in the collection.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IObservableCollection{T}"/>.
        /// </returns>
        IObservableCollection<T> CreateObservableCollection<T>(IList<T> list);

        /// <summary>
        /// Creates a read-only observable collection from a list of items.
        /// </summary>
        /// <param name="list">
        /// The list of items.
        /// </param>
        /// <typeparam name="T">
        /// The type of the items in the collection.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IReadableObservableCollection{T}"/>.
        /// </returns>
        IReadableObservableCollection<T> CreateReadOnlyObservableCollection<T>(IList<T> list);
    }
}