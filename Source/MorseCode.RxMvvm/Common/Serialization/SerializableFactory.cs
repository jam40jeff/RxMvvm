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
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A factory for creating actions which may be serialized.
    /// </summary>
    public class SerializableActionFactory : ISerializableActionFactory
    {
        private static readonly Lazy<SerializableActionFactory> InstanceLazy =
            new Lazy<SerializableActionFactory>(() => new SerializableActionFactory());

        private SerializableActionFactory()
        {
        }

        /// <summary>
        /// Gets the singleton instance of an <see cref="SerializableActionFactory"/>.
        /// </summary>
        [ContractVerification(false)]
        public static ISerializableActionFactory Instance
        {
            get
            {
                Contract.Ensures(Contract.Result<ISerializableActionFactory>() != null);

                return InstanceLazy.Value;
            }
        }

        ISerializableAction ISerializableActionFactory.CreateSerializableAction(Action action)
        {
            return new SerializableAction(action);
        }

        ISerializableAction<T> ISerializableActionFactory.CreateSerializable<T>(Func<T> action)
        {
            return new SerializableAction<T>(action);
        }

        ISerializableAction ISerializableActionFactory.CreateSerializableActionWithContext<TContext>(TContext context, Action<TContext> action)
        {
            return new SerializableActionWithContext<TContext>(context, action);
        }

        ISerializableAction<T> ISerializableActionFactory.CreateSerializableActionWithContext<TContext, T>(TContext context, Func<TContext, T> action)
        {
            return new SerializableActionWithContext<TContext, T>(context, action);
        }
    }
}