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
    /// Provides methods for static reflection.  This allows for retrieving reflection information from symbols without the need for magic strings.
    /// </summary>
    public static class StaticReflection
    {
        /// <summary>
        /// Gets the <see cref="MemberInfo"/> for a symbol which is in scope.
        /// </summary>
        /// <param name="inScopeSymbolExpression">
        /// The expression for the in-scope symbol, which should be of the format <c>() => [inScopeSymbol]</c>.
        /// </param>
        /// <typeparam name="TSymbol">
        /// The type of the in-scope symbol.
        /// </typeparam>
        /// <returns>
        /// The <see cref="MemberInfo"/> for the in-scope symbol.
        /// </returns>
        public static MemberInfo GetInScopeSymbolInfo<TSymbol>(Expression<Func<TSymbol>> inScopeSymbolExpression)
        {
            Contract.Requires<ArgumentNullException>(inScopeSymbolExpression != null);
            Contract.Ensures(Contract.Result<MemberInfo>() != null);

            return GetMemberInfoFromMemberAccess(inScopeSymbolExpression);
        }

        internal static MemberInfo GetMemberInfoFromMemberAccess(LambdaExpression expression)
        {
            Contract.Requires<ArgumentNullException>(expression != null);
            Contract.Ensures(Contract.Result<MemberInfo>() != null);

            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                UnaryExpression body = (UnaryExpression)expression.Body;
                MemberExpression memberExpression = (MemberExpression)body.Operand;
                return memberExpression.Member;
            }

            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)expression.Body;
                if (memberExpression.Expression is MemberExpression)
                {
                    throw new ArgumentException(
                        "LambdaExpression must be a single member access.", GetInScopeSymbolInfo(() => expression).Name);
                }

                return memberExpression.Member;
            }

            throw new ArgumentException(
                "LambdaExpression must be a member access.", GetInScopeSymbolInfo(() => expression).Name);
        }
    }
}