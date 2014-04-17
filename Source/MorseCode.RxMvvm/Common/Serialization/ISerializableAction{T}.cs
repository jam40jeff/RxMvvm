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

namespace MorseCode.RxMvvm.Common.Serialization
{
    /// <summary>
    /// An interface representing an action which will run initially and when deserialized.
    /// </summary>
    public interface ISerializableAction
    {
    }

    /// <summary>
    /// An interface representing an action returning a value which will run initially and when deserialized.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value.
    /// </typeparam>
    public interface ISerializableAction<out T>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        T Value { get; }
    }
}