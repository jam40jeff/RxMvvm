#region License

// Copyright 2014 MorseCode Software
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

namespace MorseCode.RxMvvm.Common.Tests
{
    using System;
    using System.Reflection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StaticReflectionTests
    {
        private readonly string testInScopeField = null;

        public DateTime TestInScopeProperty { get; set; }

        #region GetInScopeMemberInfo

        [TestMethod]
        public void GetInScopeMemberInfoNeedsNonNullInScopeMemberExpression()
        {
            ArgumentNullException expectedException = null;

            try
            {
                StaticReflection.GetInScopeMemberInfo<string>(null);
            }
            catch (ArgumentNullException e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.AreEqual("inScopeMemberExpression", expectedException.ParamName);
        }

        [TestMethod]
        public void GetInScopeMemberInfoWorksWithField()
        {
            MemberInfo info = StaticReflection.GetInScopeMemberInfo(() => testInScopeField);

            Assert.IsInstanceOfType(info, typeof(FieldInfo));
            Assert.AreEqual("testInScopeField", info.Name);
        }

        [TestMethod]
        public void GetInScopeMemberInfoWorksWithFieldWithThis()
        {
            MemberInfo info = StaticReflection.GetInScopeMemberInfo(() => this.testInScopeField);

            Assert.IsInstanceOfType(info, typeof(FieldInfo));
            Assert.AreEqual("testInScopeField", info.Name);
        }

        [TestMethod]
        public void GetInScopeMemberInfoWorksWithProperty()
        {
            MemberInfo info = StaticReflection.GetInScopeMemberInfo(() => TestInScopeProperty);

            Assert.IsInstanceOfType(info, typeof(PropertyInfo));
            Assert.AreEqual("TestInScopeProperty", info.Name);
        }

        [TestMethod]
        public void GetInScopeMemberInfoWorksWithPropertyWithThis()
        {
            MemberInfo info = StaticReflection.GetInScopeMemberInfo(() => this.TestInScopeProperty);

            Assert.IsInstanceOfType(info, typeof(PropertyInfo));
            Assert.AreEqual("TestInScopeProperty", info.Name);
        }

        [TestMethod]
        public void GetInScopeMemberInfoWorksWithLocalVariable()
        {
            // ReSharper disable ConvertToConstant.Local
            int testLocalVariable = 5;
            // ReSharper restore ConvertToConstant.Local

            MemberInfo info = StaticReflection.GetInScopeMemberInfo(() => testLocalVariable);

            Assert.IsInstanceOfType(info, typeof(FieldInfo));
            Assert.AreEqual("testLocalVariable", info.Name);
        }

        [TestMethod]
        public void GetInScopeMemberInfoWorksWithParameter()
        {
            MemberInfo info = GetInScopeMemberInfoWorksWithParameterHelper(2.5);

            Assert.IsInstanceOfType(info, typeof(FieldInfo));
            Assert.AreEqual("testParameter", info.Name);
        }

        private MemberInfo GetInScopeMemberInfoWorksWithParameterHelper(double testParameter)
        {
            return StaticReflection.GetInScopeMemberInfo(() => testParameter);
        }

        [TestMethod]
        public void GetInScopeMemberInfoWorksWithFieldWithThisWithConvert()
        {
            MemberInfo info = StaticReflection.GetInScopeMemberInfo(() => (object)this.testInScopeField);

            Assert.IsInstanceOfType(info, typeof(FieldInfo));
            Assert.AreEqual("testInScopeField", info.Name);
        }

        [TestMethod]
        public void GetInScopeMemberInfoWorksWithPropertyWithThisWithConvert()
        {
            MemberInfo info = StaticReflection.GetInScopeMemberInfo(() => (object)this.TestInScopeProperty);

            Assert.IsInstanceOfType(info, typeof(PropertyInfo));
            Assert.AreEqual("TestInScopeProperty", info.Name);
        }

        [TestMethod]
        public void GetInScopeMemberInfoThrowsExceptionForMultipleMemberAccesses()
        {
            ArgumentException expectedException = null;

            try
            {
                StaticReflection.GetInScopeMemberInfo(() => this.TestInScopeProperty.Date);
            }
            catch (ArgumentException e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.IsTrue(expectedException.Message.StartsWith("LambdaExpression must be a single member access."));
            Assert.AreEqual("expression", expectedException.ParamName);
        }

        [TestMethod]
        public void GetInScopeMemberInfoThrowsExceptionForNonMemberAccess()
        {
            ArgumentException expectedException = null;

            try
            {
                StaticReflection.GetInScopeMemberInfo(() => 2L);
            }
            catch (ArgumentException e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.IsTrue(expectedException.Message.StartsWith("LambdaExpression must be a member access."));
            Assert.AreEqual("expression", expectedException.ParamName);
        }

        #endregion GetInScopeMemberInfo

        #region GetInScopeMethodInfo

        [TestMethod]
        public void GetInScopeMethodInfoForActionNeedsNonNullInScopeMethodCallExpression()
        {
            ArgumentNullException expectedException = null;

            try
            {
                StaticReflection.GetInScopeMethodInfo(null);
            }
            catch (ArgumentNullException e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.AreEqual("inScopeMethodCallExpression", expectedException.ParamName);
        }

        [TestMethod]
        public void GetInScopeMethodInfoForFuncNeedsNonNullInScopeMethodCallExpression()
        {
            ArgumentNullException expectedException = null;

            try
            {
                StaticReflection.GetInScopeMethodInfo<string>(null);
            }
            catch (ArgumentNullException e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.AreEqual("inScopeMethodCallExpression", expectedException.ParamName);
        }

        [TestMethod]
        public void GetInScopeMethodInfoForActionWorks()
        {
            MethodInfo info = StaticReflection.GetInScopeMethodInfo(() => GetInScopeMethodInfoHelperAction());

            Assert.AreEqual("GetInScopeMethodInfoHelperAction", info.Name);
            Assert.AreEqual(0, info.GetParameters().Length);
        }

        [TestMethod]
        public void GetInScopeMethodInfoForActionWorksWithThis()
        {
            MethodInfo info = StaticReflection.GetInScopeMethodInfo(() => this.GetInScopeMethodInfoHelperAction());

            Assert.AreEqual("GetInScopeMethodInfoHelperAction", info.Name);
            Assert.AreEqual(0, info.GetParameters().Length);
        }

        [TestMethod]
        public void GetInScopeMethodInfoForFuncWorks()
        {
            MethodInfo info = StaticReflection.GetInScopeMethodInfo(() => GetInScopeMethodInfoHelperFunc());

            Assert.AreEqual("GetInScopeMethodInfoHelperFunc", info.Name);
            Assert.AreEqual(0, info.GetParameters().Length);
        }

        [TestMethod]
        public void GetInScopeMethodInfoForFuncWorksWithThis()
        {
            MethodInfo info = StaticReflection.GetInScopeMethodInfo(() => this.GetInScopeMethodInfoHelperFunc());

            Assert.AreEqual("GetInScopeMethodInfoHelperFunc", info.Name);
            Assert.AreEqual(0, info.GetParameters().Length);
        }

        [TestMethod]
        public void GetInScopeMethodInfoForFuncWorksWithThisWithConvert()
        {
            MethodInfo info = StaticReflection.GetInScopeMethodInfo(() => (object)this.GetInScopeMethodInfoHelperFunc());

            Assert.AreEqual("GetInScopeMethodInfoHelperFunc", info.Name);
            Assert.AreEqual(0, info.GetParameters().Length);
        }

        [TestMethod]
        public void GetInScopeMethodInfoForActionWorksChoosingOverloadWithGetInScopeMethodInfoHelperClass1()
        {
            MethodInfo info = StaticReflection.GetInScopeMethodInfo(() => GetInScopeMethodInfoHelperAction(default(GetInScopeMethodInfoHelperClass1)));

            Assert.AreEqual("GetInScopeMethodInfoHelperAction", info.Name);
            ParameterInfo[] parameters = info.GetParameters();
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(typeof(GetInScopeMethodInfoHelperClass1), parameters[0].ParameterType);
        }

        [TestMethod]
        public void GetInScopeMethodInfoForActionWorksChoosingOverloadWithGetInScopeMethodInfoHelperClass2()
        {
            MethodInfo info = StaticReflection.GetInScopeMethodInfo(() => GetInScopeMethodInfoHelperAction(default(GetInScopeMethodInfoHelperClass2)));

            Assert.AreEqual("GetInScopeMethodInfoHelperAction", info.Name);
            ParameterInfo[] parameters = info.GetParameters();
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(typeof(GetInScopeMethodInfoHelperClass2), parameters[0].ParameterType);
        }

        [TestMethod]
        public void GetInScopeMethodInfoForFuncWorksChoosingOverloadWithGetInScopeMethodInfoHelperClass1()
        {
            MethodInfo info = StaticReflection.GetInScopeMethodInfo(() => GetInScopeMethodInfoHelperFunc(default(GetInScopeMethodInfoHelperClass1)));

            Assert.AreEqual("GetInScopeMethodInfoHelperFunc", info.Name);
            ParameterInfo[] parameters = info.GetParameters();
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(typeof(GetInScopeMethodInfoHelperClass1), parameters[0].ParameterType);
        }

        [TestMethod]
        public void GetInScopeMethodInfoForFuncWorksChoosingOverloadWithGetInScopeMethodInfoHelperClass2()
        {
            MethodInfo info = StaticReflection.GetInScopeMethodInfo(() => GetInScopeMethodInfoHelperFunc(default(GetInScopeMethodInfoHelperClass2)));

            Assert.AreEqual("GetInScopeMethodInfoHelperFunc", info.Name);
            ParameterInfo[] parameters = info.GetParameters();
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(typeof(GetInScopeMethodInfoHelperClass2), parameters[0].ParameterType);
        }

        [TestMethod]
        public void GetInScopeMethodInfoThrowsExceptionForNonMethodCall()
        {
            ArgumentException expectedException = null;

            try
            {
                StaticReflection.GetInScopeMethodInfo(() => 2L);
            }
            catch (ArgumentException e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.IsTrue(expectedException.Message.StartsWith("LambdaExpression must be a method call."));
            Assert.AreEqual("expression", expectedException.ParamName);
        }

        private void GetInScopeMethodInfoHelperAction()
        {
        }

        // ReSharper disable UnusedParameter.Local
        private void GetInScopeMethodInfoHelperAction(GetInScopeMethodInfoHelperClass1 p1)
        // ReSharper restore UnusedParameter.Local
        {
        }

        // ReSharper disable UnusedParameter.Local
        private void GetInScopeMethodInfoHelperAction(GetInScopeMethodInfoHelperClass2 p1)
        // ReSharper restore UnusedParameter.Local
        {
        }

        private string GetInScopeMethodInfoHelperFunc()
        {
            return null;
        }

        // ReSharper disable UnusedParameter.Local
        private string GetInScopeMethodInfoHelperFunc(GetInScopeMethodInfoHelperClass1 p1)
        // ReSharper restore UnusedParameter.Local
        {
            return null;
        }

        // ReSharper disable UnusedParameter.Local
        private string GetInScopeMethodInfoHelperFunc(GetInScopeMethodInfoHelperClass2 p1)
        // ReSharper restore UnusedParameter.Local
        {
            return null;
        }

        private class GetInScopeMethodInfoHelperClass1
        {
        }

        private class GetInScopeMethodInfoHelperClass2
        {
        }

        #endregion GetInScopeMethodInfo

        #region GetMemberInfo

        [TestMethod]
        public void GetMemberInfoNeedsNonNullMemberExpression()
        {
            ArgumentNullException expectedException = null;

            try
            {
                StaticReflection<GetMethodInfoHelperClass1>.GetMemberInfo<string>(null);
            }
            catch (ArgumentNullException e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.AreEqual("memberExpression", expectedException.ParamName);
        }

        [TestMethod]
        public void GetMemberInfoWorksWithField()
        {
            MemberInfo info = StaticReflection<GetMethodInfoHelperClass1>.GetMemberInfo(o => o.testField);

            Assert.IsInstanceOfType(info, typeof(FieldInfo));
            Assert.AreEqual("testField", info.Name);
        }

        [TestMethod]
        public void GetMemberInfoWorksWithProperty()
        {
            MemberInfo info = StaticReflection<GetMethodInfoHelperClass1>.GetMemberInfo(o => o.TestProperty);

            Assert.IsInstanceOfType(info, typeof(PropertyInfo));
            Assert.AreEqual("TestProperty", info.Name);
        }

        [TestMethod]
        public void GetMemberInfoWorksWithFieldWithConvert()
        {
            MemberInfo info = StaticReflection<GetMethodInfoHelperClass1>.GetMemberInfo(o => (object)o.testField);

            Assert.IsInstanceOfType(info, typeof(FieldInfo));
            Assert.AreEqual("testField", info.Name);
        }

        [TestMethod]
        public void GetMemberInfoWorksWithPropertyWithConvert()
        {
            MemberInfo info = StaticReflection<GetMethodInfoHelperClass1>.GetMemberInfo(o => (object)o.TestProperty);

            Assert.IsInstanceOfType(info, typeof(PropertyInfo));
            Assert.AreEqual("TestProperty", info.Name);
        }

        [TestMethod]
        public void GetMemberInfoThrowsExceptionForMultipleMemberAccesses()
        {
            ArgumentException expectedException = null;

            try
            {
                StaticReflection<GetMethodInfoHelperClass1>.GetMemberInfo(o => o.TestProperty.Date);
            }
            catch (ArgumentException e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.IsTrue(expectedException.Message.StartsWith("LambdaExpression must be a single member access."));
            Assert.AreEqual("expression", expectedException.ParamName);
        }

        [TestMethod]
        public void GetMemberInfoThrowsExceptionForNonMemberAccess()
        {
            ArgumentException expectedException = null;

            try
            {
                StaticReflection<GetMethodInfoHelperClass1>.GetMemberInfo(o => 2L);
            }
            catch (ArgumentException e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.IsTrue(expectedException.Message.StartsWith("LambdaExpression must be a member access."));
            Assert.AreEqual("expression", expectedException.ParamName);
        }

        #endregion GetMemberInfo

        #region GetMethodInfo

        [TestMethod]
        public void GetMethodInfoForActionNeedsNonNullMethodCallExpression()
        {
            ArgumentNullException expectedException = null;

            try
            {
                StaticReflection<GetMethodInfoHelperClass1>.GetMethodInfo(null);
            }
            catch (ArgumentNullException e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.AreEqual("methodCallExpression", expectedException.ParamName);
        }

        [TestMethod]
        public void GetMethodInfoForFuncNeedsNonNullMethodCallExpression()
        {
            ArgumentNullException expectedException = null;

            try
            {
                StaticReflection<GetMethodInfoHelperClass1>.GetMethodInfo<string>(null);
            }
            catch (ArgumentNullException e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.AreEqual("methodCallExpression", expectedException.ParamName);
        }

        [TestMethod]
        public void GetMethodInfoForActionWorks()
        {
            MethodInfo info = StaticReflection<GetMethodInfoHelperClass1>.GetMethodInfo(o => o.GetMethodInfoHelperAction());

            Assert.AreEqual("GetMethodInfoHelperAction", info.Name);
            Assert.AreEqual(0, info.GetParameters().Length);
        }

        [TestMethod]
        public void GetMethodInfoForFuncWorks()
        {
            MethodInfo info = StaticReflection<GetMethodInfoHelperClass1>.GetMethodInfo(o => o.GetMethodInfoHelperFunc());

            Assert.AreEqual("GetMethodInfoHelperFunc", info.Name);
            Assert.AreEqual(0, info.GetParameters().Length);
        }

        [TestMethod]
        public void GetMethodInfoForFuncWorksWithConvert()
        {
            MethodInfo info = StaticReflection<GetMethodInfoHelperClass1>.GetMethodInfo(o => (object)o.GetMethodInfoHelperFunc());

            Assert.AreEqual("GetMethodInfoHelperFunc", info.Name);
            Assert.AreEqual(0, info.GetParameters().Length);
        }

        [TestMethod]
        public void GetMethodInfoForActionWorksChoosingOverloadWithGetMethodInfoHelperClass1()
        {
            MethodInfo info = StaticReflection<GetMethodInfoHelperClass1>.GetMethodInfo(o => o.GetMethodInfoHelperAction(default(GetMethodInfoHelperClass1)));

            Assert.AreEqual("GetMethodInfoHelperAction", info.Name);
            ParameterInfo[] parameters = info.GetParameters();
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(typeof(GetMethodInfoHelperClass1), parameters[0].ParameterType);
        }

        [TestMethod]
        public void GetMethodInfoForActionWorksChoosingOverloadWithGetMethodInfoHelperClass2()
        {
            MethodInfo info = StaticReflection<GetMethodInfoHelperClass1>.GetMethodInfo(o => o.GetMethodInfoHelperAction(default(GetMethodInfoHelperClass2)));

            Assert.AreEqual("GetMethodInfoHelperAction", info.Name);
            ParameterInfo[] parameters = info.GetParameters();
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(typeof(GetMethodInfoHelperClass2), parameters[0].ParameterType);
        }

        [TestMethod]
        public void GetMethodInfoForFuncWorksChoosingOverloadWithGetMethodInfoHelperClass1()
        {
            MethodInfo info = StaticReflection<GetMethodInfoHelperClass1>.GetMethodInfo(o => o.GetMethodInfoHelperFunc(default(GetMethodInfoHelperClass1)));

            Assert.AreEqual("GetMethodInfoHelperFunc", info.Name);
            ParameterInfo[] parameters = info.GetParameters();
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(typeof(GetMethodInfoHelperClass1), parameters[0].ParameterType);
        }

        [TestMethod]
        public void GetMethodInfoForFuncWorksChoosingOverloadWithGetMethodInfoHelperClass2()
        {
            MethodInfo info = StaticReflection<GetMethodInfoHelperClass1>.GetMethodInfo(o => o.GetMethodInfoHelperFunc(default(GetMethodInfoHelperClass2)));

            Assert.AreEqual("GetMethodInfoHelperFunc", info.Name);
            ParameterInfo[] parameters = info.GetParameters();
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual(typeof(GetMethodInfoHelperClass2), parameters[0].ParameterType);
        }

        [TestMethod]
        public void GetMethodInfoThrowsExceptionForNonMethodCall()
        {
            ArgumentException expectedException = null;

            try
            {
                StaticReflection<GetMethodInfoHelperClass1>.GetMethodInfo(o => 2L);
            }
            catch (ArgumentException e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.IsTrue(expectedException.Message.StartsWith("LambdaExpression must be a method call."));
            Assert.AreEqual("expression", expectedException.ParamName);
        }

        private class GetMethodInfoHelperClass1
        {
// ReSharper disable InconsistentNaming
#pragma warning disable 169
            public readonly string testField = null;
#pragma warning restore 169
// ReSharper restore InconsistentNaming

// ReSharper disable UnusedAutoPropertyAccessor.Local
            public DateTime TestProperty { get; set; }
// ReSharper restore UnusedAutoPropertyAccessor.Local

            public void GetMethodInfoHelperAction()
            {
            }

            // ReSharper disable UnusedParameter.Local
            public void GetMethodInfoHelperAction(GetMethodInfoHelperClass1 p1)
            // ReSharper restore UnusedParameter.Local
            {
            }

            // ReSharper disable UnusedParameter.Local
            public void GetMethodInfoHelperAction(GetMethodInfoHelperClass2 p1)
            // ReSharper restore UnusedParameter.Local
            {
            }

            public string GetMethodInfoHelperFunc()
            {
                return null;
            }

            // ReSharper disable UnusedParameter.Local
            public string GetMethodInfoHelperFunc(GetMethodInfoHelperClass1 p1)
            // ReSharper restore UnusedParameter.Local
            {
                return null;
            }

            // ReSharper disable UnusedParameter.Local
            public string GetMethodInfoHelperFunc(GetMethodInfoHelperClass2 p1)
            // ReSharper restore UnusedParameter.Local
            {
                return null;
            }
        }

        private class GetMethodInfoHelperClass2
        {
        }

        #endregion GetInScopeMethodInfo
    }
}