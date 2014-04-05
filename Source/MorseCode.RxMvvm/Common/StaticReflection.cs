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
    /// Provides methods for static reflection of in-scope symbols.  This allows for retrieving reflection information from symbols without the need for magic strings.
    /// </summary>
    public static class StaticReflection
    {
        /// <summary>
        /// Gets the <see cref="MemberInfo"/> for a local variable, parameter, field, or property which is in scope.
        /// </summary>
        /// <param name="inScopeMemberExpression">
        /// The expression for the in-scope member, which should be of the format <c>() => [inScopeMember]</c> or <c>() => this.[fieldOrProperty]</c>.  <c>[inScopeMember]</c> may be a local variable, parameter, field, or property.
        /// </param>
        /// <typeparam name="TMember">
        /// The type of the in-scope member.
        /// </typeparam>
        /// <returns>
        /// The <see cref="MemberInfo"/> for the in-scope member.
        /// </returns>
        public static MemberInfo GetInScopeMemberInfo<TMember>(Expression<Func<TMember>> inScopeMemberExpression)
        {
            Contract.Requires<ArgumentNullException>(inScopeMemberExpression != null, "inScopeMemberExpression");
            Contract.Ensures(Contract.Result<MemberInfo>() != null);

            return GetMemberInfoFromMemberAccess(inScopeMemberExpression);
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> for a method which is in scope.
        /// </summary>
        /// <param name="inScopeMethodCallExpression">
        /// The expression for the in-scope method call, which should be of the format <c>() => [method]()</c> or <c>() => this.[method]()</c>.  If <c>[method]</c> has parameters, the default value for each may be passed as they are only used to determine which overload to choose.
        /// </param>
        /// <returns>
        /// The <see cref="MethodInfo"/> for the in-scope method.
        /// </returns>
        public static MethodInfo GetInScopeMethodInfo(Expression<Action> inScopeMethodCallExpression)
        {
            Contract.Requires<ArgumentNullException>(inScopeMethodCallExpression != null, "inScopeMethodCallExpression");
            Contract.Ensures(Contract.Result<MemberInfo>() != null);

            return GetMethodInfoFromMethodCall(inScopeMethodCallExpression);
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> for a method which is in scope.
        /// </summary>
        /// <param name="inScopeMethodCallExpression">
        /// The expression for the in-scope method call, which should be of the format <c>() => [method]()</c> or <c>() => this.[method]()</c>.  If <c>[method]</c> has parameters, the default value for each may be passed as they are only used to determine which overload to choose.
        /// </param>
        /// <typeparam name="TReturn">
        /// The type of the return value of the function.
        /// </typeparam>
        /// <returns>
        /// The <see cref="MethodInfo"/> for the in-scope method.
        /// </returns>
        public static MethodInfo GetInScopeMethodInfo<TReturn>(Expression<Func<TReturn>> inScopeMethodCallExpression)
        {
            Contract.Requires<ArgumentNullException>(inScopeMethodCallExpression != null, "inScopeMethodCallExpression");
            Contract.Ensures(Contract.Result<MemberInfo>() != null);

            return GetMethodInfoFromMethodCall(inScopeMethodCallExpression);
        }

        internal static MemberInfo GetMemberInfoFromMemberAccess(LambdaExpression expression)
        {
            Contract.Requires<ArgumentNullException>(expression != null, "expression");
            Contract.Ensures(Contract.Result<MemberInfo>() != null);

            Expression currentExpression = expression.Body;

            if (currentExpression.NodeType == ExpressionType.Convert)
            {
                UnaryExpression convertExpression = (UnaryExpression)currentExpression;
                currentExpression = convertExpression.Operand;
            }

            if (currentExpression.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)currentExpression;
                if (memberExpression.Expression is MemberExpression)
                {
                    throw new ArgumentException(
                        "LambdaExpression must be a single member access.", "expression");
                }

                return memberExpression.Member;
            }

            throw new ArgumentException(
                "LambdaExpression must be a member access.", "expression");
        }

        internal static MethodInfo GetMethodInfoFromMethodCall(LambdaExpression expression)
        {
            Contract.Requires<ArgumentNullException>(expression != null, "expression");
            Contract.Ensures(Contract.Result<MethodInfo>() != null);

            Expression currentExpression = expression.Body;

            if (currentExpression.NodeType == ExpressionType.Convert)
            {
                UnaryExpression convertExpression = (UnaryExpression)currentExpression;
                currentExpression = convertExpression.Operand;
            }

            if (currentExpression.NodeType == ExpressionType.Call)
            {
                MethodCallExpression methodCallExpression = (MethodCallExpression)currentExpression;
                return methodCallExpression.Method;
            }

            throw new ArgumentException(
                "LambdaExpression must be a method call.", "expression");
        }
    }
}