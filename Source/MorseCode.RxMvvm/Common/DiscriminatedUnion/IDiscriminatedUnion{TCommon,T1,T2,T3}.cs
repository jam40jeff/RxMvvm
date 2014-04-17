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

namespace MorseCode.RxMvvm.Common.DiscriminatedUnion
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Interface representing the F# discriminated union with two possible types.  A value may only be specified for one of the types at a time.
    /// </summary>
    /// <typeparam name="TCommon">
    /// The common type of all types allowed in the discriminated union.
    /// </typeparam>
    /// <typeparam name="T1">
    /// The first type of the discriminated union.
    /// </typeparam>
    /// <typeparam name="T2">
    /// The second type of the discriminated union.
    /// </typeparam>
    /// <typeparam name="T3">
    /// The third type of the discriminated union.
    /// </typeparam>
    [ContractClass(typeof(DiscriminatedUnionInterfaceContract<,,,>))]
    public interface IDiscriminatedUnion<out TCommon, out T1, out T2, out T3> : IDiscriminatedUnionSimple<T1, T2, T3>
        where T1 : TCommon
        where T2 : TCommon
        where T3 : TCommon
        where TCommon : class
    {
        /// <summary>
        /// Gets the value as <typeparamref name="TCommon" /> regardless of which of the three values are held in the discriminated union.
        /// </summary>
        TCommon Value { get; }
    }
}