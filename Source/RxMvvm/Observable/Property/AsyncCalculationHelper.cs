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

namespace MorseCode.RxMvvm.Observable.Property
{
    using System.Reactive.Concurrency;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class containing an asynchronous calculation helper.
    /// </summary>
    public class AsyncCalculationHelper
    {
        private readonly IScheduler scheduler;

        private readonly CancellationToken token;

        internal AsyncCalculationHelper(IScheduler scheduler, CancellationToken token)
        {
            this.scheduler = scheduler;
            this.token = token;
        }

        /// <summary>
        /// Gets the scheduler.
        /// </summary>
        public IScheduler Scheduler
        {
            get
            {
                return this.scheduler;
            }
        }

        /// <summary>
        /// Gets the cancellation token.
        /// </summary>
        public CancellationToken Token
        {
            get
            {
                return this.token;
            }
        }

        /// <summary>
        /// Checks to see if a cancellation has been requested.  If it has, the method will exit with a cancellation exception.
        /// </summary>
        public void CheckCancellationToken()
        {
            if (this.token.IsCancellationRequested)
            {
                this.token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Checks to see if a cancellation has been requested by yielding back to the scheduler.  If it has, the method will not continue.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the rest of the method to run after checking for cancellation.
        /// </returns>
        public async Task CheckCancellationTokenAndYield()
        {
            await this.scheduler.Yield();
        }
    }
}