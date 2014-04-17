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
    using System;

    /// <summary>
    /// An interface representing a factory for creating actions which may be serialized.
    /// </summary>
    public interface ISerializableActionFactory
    {
        /// <summary>
        /// Creates an action which will run initially and when deserialized.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The <see cref="ISerializableAction"/>.
        /// </returns>
        ISerializableAction CreateSerializableAction(Action action);

        /// <summary>
        /// Creates an action returning a value which will run initially and when deserialized.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// The type of the value.
        /// </typeparam>
        /// <returns>
        /// The <see cref="ISerializableAction{T}"/>.
        /// </returns>
        ISerializableAction<T> CreateSerializable<T>(Func<T> action);

        /// <summary>
        /// Creates an action with context which will run initially and when deserialized.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
        /// <returns>
        /// The <see cref="ISerializableAction"/>.
        /// </returns>
        ISerializableAction CreateSerializableActionWithContext<TContext>(TContext context, Action<TContext> action);

        /// <summary>
        /// Creates an action with context returning a value which will run initially and when deserialized.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="TContext">
        /// The type of the context.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of the value.
        /// </typeparam>
        /// <returns>
        /// The <see cref="ISerializableAction{T}"/>.
        /// </returns>
        ISerializableAction<T> CreateSerializableActionWithContext<TContext, T>(TContext context, Func<TContext, T> action);
    }
}