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

namespace MorseCode.RxMvvm.Common
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A class containing global configuration.
    /// </summary>
    public static class RxMvvm
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use serialization.
        /// </summary>
        public static bool UseSerialization { get; set; }

        /// <summary>
        /// Ensures the delegate is safe to be serialized if using serialization.
        /// </summary>
        /// <param name="d">
        /// The delegate.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Throw if the delegate is not safe to be serialized.
        /// </exception>
        public static void EnsureSerializableDelegateIfUsingSerialization(Delegate d)
        {
            Contract.Requires<ArgumentNullException>(d != null, "d");

            if (UseSerialization)
            {
                if (!d.Method.IsStatic)
                {
                    throw new InvalidOperationException(
                        "Delegates must be static in order for them to be guaranteed to be safe to be serialized.");
                }
            }
        }
    }
}