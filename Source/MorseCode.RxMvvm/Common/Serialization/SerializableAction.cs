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
    internal class SerializableAction : ISerializableAction, ISerializable
    {
        private readonly Action action;

        internal SerializableAction(Action action)
        {
            Contract.Requires<ArgumentNullException>(action != null, "action");
            Contract.Ensures(this.action != null);

            RxMvvm.EnsureSerializableDelegateIfUsingSerialization(action);

            this.action = action;

            action();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableAction"/> class. 
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The serialization context.
        /// </param>
        [ContractVerification(false)]
        // ReSharper disable UnusedParameter.Local
        protected SerializableAction(SerializationInfo info, StreamingContext context)
            // ReSharper restore UnusedParameter.Local
            : this((Action)info.GetValue("a", typeof(Action)))
        {
        }

        /// <summary>
        /// Gets the object data to serialize.
        /// </summary>
        /// <param name="info">
        /// The serialization info.
        /// </param>
        /// <param name="context">
        /// The serialization context.
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("a", this.action);
        }

        [ContractInvariantMethod]
        private void CodeContractsInvariants()
        {
            Contract.Invariant(this.action != null);
        }
    }
}