namespace MorseCode.RxMvvm.Observable.Property.Internal
{
    using MorseCode.RxMvvm.Common.StaticReflection;

    internal static class LazyReadOnlyPropertyUtility
    {
        internal static readonly string ValueOrExceptionPropertyName;

        internal static readonly string CalculationExceptionPropertyName;

        internal static readonly string IsCalculatingPropertyName;

        internal static readonly string IsCalculatedPropertyName;

        /// <summary>
        /// Initializes static members of the <see cref="LazyReadOnlyPropertyUtility"/> class.
        /// </summary>
        static LazyReadOnlyPropertyUtility()
        {
            ValueOrExceptionPropertyName =
                StaticReflection<ILazyReadOnlyProperty<object>>.GetMemberInfo(o => o.ValueOrException).Name;
            CalculationExceptionPropertyName =
                StaticReflection<ILazyReadOnlyProperty<object>>.GetMemberInfo(o => o.CalculationException).Name;
            IsCalculatingPropertyName =
                StaticReflection<ILazyReadOnlyProperty<object>>.GetMemberInfo(o => o.IsCalculating).Name;
            IsCalculatedPropertyName =
                StaticReflection<ILazyReadOnlyProperty<object>>.GetMemberInfo(o => o.IsCalculated).Name;
        }
    }
}