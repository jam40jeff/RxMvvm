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

namespace MorseCode.RxMvvm.Observable.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MorseCode.RxMvvm.Common;

    [TestClass]
    [Ignore]
    public class Sandbox
    {
        private readonly string testField = null;

        [TestMethod]
        public void Closures()
        {
            string testLocalVariable = null;

            string testFieldName = StaticReflection.GetInScopeSymbolInfo(() => this.testField).Name;
            string testLocalVariableName = StaticReflection.GetInScopeSymbolInfo(() => testLocalVariable).Name;

            Assert.AreEqual("testField", testFieldName);
            Assert.AreEqual("testLocalVariable", testLocalVariableName);
        }

        [TestMethod]
        public void StaticReflectionPerformance()
        {
            //Stopwatch watch = new Stopwatch();
            //const int Iterations = 100000;
            //watch.Start();

            //for (int i = 0; i < Iterations; i++)
            //{
            //    PropertyInfo propertyOfName = Reflect<Employee>.GetProperty(c => c.Name);
            //}

            //watch.Stop();
            //Console.WriteLine("[Reflector]: " + watch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));
            //watch.Reset();
            //watch.Start();

            //for (int i = 0; i < Iterations; i++)
            //{
            //    PropertyInfo propertyName = typeof(Employee).GetProperty("Name");
            //}

            //watch.Stop();
            //Console.WriteLine(
            //    "[Regular Reflection]: " + watch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));
        }

        private class Employee
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }
    }
}