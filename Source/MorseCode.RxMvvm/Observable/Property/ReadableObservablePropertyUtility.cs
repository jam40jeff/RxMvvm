namespace MorseCode.RxMvvm.Observable.Property
{
    using MorseCode.RxMvvm.Common;

    internal static class ReadableObservablePropertyUtility
    {
        internal static readonly string ValuePropertyName;

        static ReadableObservablePropertyUtility()
        {
            ValuePropertyName = StaticReflection<IReadableObservableProperty<object>>.GetMemberInfo(o => o.Value).Name;
        }
    }
}