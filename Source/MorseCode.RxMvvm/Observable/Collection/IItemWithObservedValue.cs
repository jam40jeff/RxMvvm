namespace MorseCode.RxMvvm.Observable.Collection
{
    /// <summary>
    /// Represents an item with an observed value.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the item.
    /// </typeparam>
    /// <typeparam name="TObserved">
    /// The type of the observed value.
    /// </typeparam>
    public interface IItemWithObservedValue<out T, out TObserved>
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        T Item { get; }

        /// <summary>
        /// Gets the observed value.
        /// </summary>
        TObserved ObservedValue { get; }
    }
}