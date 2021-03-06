﻿#region License

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

namespace MorseCode.RxMvvm.UI.Wpf
{
    using MorseCode.RxMvvm.Common.StaticReflection;
    using MorseCode.RxMvvm.Observable.Property;

    internal static class BindingFactoryUtility
    {
        internal static readonly string ValuePropertyName =
            StaticReflection<IReadableObservableProperty<object>>.GetMemberInfo(o => o.Value).Name;

        internal static readonly string LatestSuccessfulValuePropertyName =
            StaticReflection<ICalculatedProperty<object>>.GetMemberInfo(o => o.LatestSuccessfulValue).Name;
    }
}