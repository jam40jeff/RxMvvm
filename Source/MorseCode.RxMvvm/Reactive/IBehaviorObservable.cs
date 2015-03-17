#region License

// Copyright 2015 MorseCode Software
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

namespace MorseCode.RxMvvm.Reactive
{
    using System;
    using System.Reactive.Subjects;

    /// <summary>
    /// Represents an observable that always produces a value on subscription and does not complete.
    /// </summary>
    /// <typeparam name="T">The type of the object that provides notification information.</typeparam>
    /// <remarks>This type is meant to represent observables on time-varied values, such as <see cref="BehaviorSubject{T}"/>.</remarks>
    public interface IBehaviorObservable<out T> : IObservable<T>
    {
    }
}