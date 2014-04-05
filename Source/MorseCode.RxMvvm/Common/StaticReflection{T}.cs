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
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Provides methods for static reflection of symbols from a specified type.  This allows for retrieving reflection information from symbols without the need for magic strings.
    /// </summary>
    /// <typeparam name="T">
    /// The type on which to perform static reflection.
    /// </typeparam>
    public static class StaticReflection<T>
    {
        /// <summary>
        /// Gets the <see cref="MemberInfo"/> for a field or property.
        /// </summary>
        /// <param name="memberExpression">
        /// The expression for the member, which should be of the format <c>o =&gt; o.[member]</c>.  <c>[member]</c> may be a field or property.
        /// </param>
        /// <typeparam name="TMember">
        /// The type of the member.
        /// </typeparam>
        /// <returns>
        /// The <see cref="MemberInfo"/> for the member.
        /// </returns>
        public static MemberInfo GetMemberInfo<TMember>(Expression<Func<T, TMember>> memberExpression)
        {
            Contract.Requires<ArgumentNullException>(memberExpression != null, "memberExpression");
            Contract.Ensures(Contract.Result<MemberInfo>() != null);

            return StaticReflection.GetMemberInfoFromMemberAccess(memberExpression);
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> for a method.
        /// </summary>
        /// <param name="methodCallExpression">
        /// The expression for method call, which should be of the format <c>o =&gt; o.[method]()</c>.  If <c>[method]</c> has parameters, the default value for each may be passed as they are only used to determine which overload to choose.
        /// </param>
        /// <returns>
        /// The <see cref="MethodInfo"/> for the method.
        /// </returns>
        public static MethodInfo GetMethodInfo(Expression<Action<T>> methodCallExpression)
        {
            Contract.Requires<ArgumentNullException>(methodCallExpression != null, "methodCallExpression");
            Contract.Ensures(Contract.Result<MemberInfo>() != null);

            return StaticReflection.GetMethodInfoFromMethodCall(methodCallExpression);
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> for a method.
        /// </summary>
        /// <param name="methodCallExpression">
        /// The expression for method call, which should be of the format <c>o =&gt; o.[method]()</c>.  If <c>[method]</c> has parameters, the default value for each may be passed as they are only used to determine which overload to choose.
        /// </param>
        /// <typeparam name="TReturn">
        /// The type of the return value of the function.
        /// </typeparam>
        /// <returns>
        /// The <see cref="MethodInfo"/> for the method.
        /// </returns>
        public static MethodInfo GetMethodInfo<TReturn>(Expression<Func<T, TReturn>> methodCallExpression)
        {
            Contract.Requires<ArgumentNullException>(methodCallExpression != null, "methodCallExpression");
            Contract.Ensures(Contract.Result<MemberInfo>() != null);

            return StaticReflection.GetMethodInfoFromMethodCall(methodCallExpression);
        }
    }
}