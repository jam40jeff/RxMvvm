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
    /// <summary>
    /// A class providing extension methods for the <see cref="object"/> type.
    /// </summary>
    public static class ObjectExtensionMethods
    {
        /// <summary>
        /// A safe version of the <see cref="object.ToString"/> method which will return <value>null</value> when called on a <value>null</value> instance.
        /// </summary>
        /// <param name="o">
        /// The object.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> resulting from calling <see cref="object.ToString"/> on <paramref name="o"/>.  If <paramref name="o"/> is <value>null</value>, <value>null</value> will be returned.
        /// </returns>
        public static string SafeToString(this object o)
        {
            return o == null ? null : o.ToString();
        }
    }
}