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
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    internal class SerializableActionWithContext<TContext> : ISerializableAction, ISerializable
    {
        private readonly TContext context;

        private readonly Action<TContext> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableActionWithContext{TContext}"/> class. 
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="action">
        /// The value factory.
        /// </param>
        public SerializableActionWithContext(TContext context, Action<TContext> action)
        {
            Contract.Requires<ArgumentNullException>(action != null, "valueFactory");
            Contract.Ensures(this.action != null);

            RxMvvm.EnsureSerializableDelegateIfUsingSerialization(action);

            this.context = context;
            this.action = action;

            action(context);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableActionWithContext{TContext}"/> class. 
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The serialization context.
        /// </param>
        [ContractVerification(false)]
        // ReSharper disable UnusedParameter.Local
        protected SerializableActionWithContext(SerializationInfo info, StreamingContext context)
            // ReSharper restore UnusedParameter.Local
            : this(
                (TContext)(info.GetValue("c", typeof(TContext)) ?? default(TContext)),
                (Action<TContext>)info.GetValue("a", typeof(Action<TContext>)))
        {
        }

        /// <summary>
        /// Gets the object data to serialize.
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="streamingContext">
        /// The serialization context.
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext streamingContext)
        {
            info.AddValue("c", this.context);
            info.AddValue("a", this.action);
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.action != null);
        }
    }
}