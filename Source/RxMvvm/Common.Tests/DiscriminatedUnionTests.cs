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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DiscriminatedUnionTests
    {
        #region DiscriminatedUnionTwo

        #region Using First

        [TestMethod]
        public void DiscriminatedUnionTwoUsingFirstIsFirstWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.First<Base, Derived1, Derived2>(d1);
            Assert.IsTrue(d.IsFirst);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingFirstIsSecondWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.First<Base, Derived1, Derived2>(d1);
            Assert.IsFalse(d.IsSecond);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingFirstFirstWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.First<Base, Derived1, Derived2>(d1);
            Assert.AreSame(d1, d.First);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingFirstSecondWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.First<Base, Derived1, Derived2>(d1);
            Derived2 d2;
            try
            {
                d2 = d.Second;
            }
            catch
            {
                d2 = null;
            }
            Assert.IsNull(d2);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingFirstValueWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.First<Base, Derived1, Derived2>(d1);
            Assert.AreSame(d1, d.Value);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingFirstSwitchActionWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.First<Base, Derived1, Derived2>(d1);
            bool ranFirst = false;
            d.Switch(v => { ranFirst = true; }, v => { ranFirst = false; });
            Assert.IsTrue(ranFirst);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingFirstSwitchFuncWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.First<Base, Derived1, Derived2>(d1);
            bool ranFirst = d.Switch(v => true, v => false);
            Assert.IsTrue(ranFirst);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingFirstToString()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.First<Base, Derived1, Derived2>(d1);
            Assert.AreEqual("{First:" + d1 + "}", d.ToString());
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingFirstToStringOnNull()
        {
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.First<Base, Derived1, Derived2>(null);
            Assert.AreEqual("{First:}", d.ToString());
        }

        #endregion Using First

        #region Using Second

        [TestMethod]
        public void DiscriminatedUnionTwoUsingSecondIsFirstWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.Second<Base, Derived1, Derived2>(d2);
            Assert.IsFalse(d.IsFirst);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingSecondIsSecondWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.Second<Base, Derived1, Derived2>(d2);
            Assert.IsTrue(d.IsSecond);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingSecondFirstWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.Second<Base, Derived1, Derived2>(d2);
            Derived1 d1;
            try
            {
                d1 = d.First;
            }
            catch
            {
                d1 = null;
            }
            Assert.IsNull(d1);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingSecondSecondWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.Second<Base, Derived1, Derived2>(d2);
            Assert.AreSame(d2, d.Second);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingSecondValueWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.Second<Base, Derived1, Derived2>(d2);
            Assert.AreSame(d2, d.Value);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingSecondSwitchActionWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.Second<Base, Derived1, Derived2>(d2);
            bool ranSecond = false;
            d.Switch(v => { ranSecond = false; }, v => { ranSecond = true; });
            Assert.IsTrue(ranSecond);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingSecondSwitchFuncWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.Second<Base, Derived1, Derived2>(d2);
            bool ranSecond = d.Switch(v => false, v => true);
            Assert.IsTrue(ranSecond);
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingSecondToString()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.Second<Base, Derived1, Derived2>(d2);
            Assert.AreEqual("{Second:" + d2 + "}", d.ToString());
        }

        [TestMethod]
        public void DiscriminatedUnionTwoUsingSecondToStringOnNull()
        {
            IDiscriminatedUnion<Base, Derived1, Derived2> d = DiscriminatedUnion.Second<Base, Derived1, Derived2>(null);
            Assert.AreEqual("{Second:}", d.ToString());
        }

        #endregion Using Second

        #endregion DiscriminatedUnionTwo

        #region DiscriminatedUnionThree

        #region Using First

        [TestMethod]
        public void DiscriminatedUnionThreeUsingFirstIsFirstWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.First<Base, Derived1, Derived2, Derived3>(d1);
            Assert.IsTrue(d.IsFirst);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingFirstIsSecondWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.First<Base, Derived1, Derived2, Derived3>(d1);
            Assert.IsFalse(d.IsSecond);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingFirstIsThirdWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.First<Base, Derived1, Derived2, Derived3>(d1);
            Assert.IsFalse(d.IsThird);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingFirstFirstWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.First<Base, Derived1, Derived2, Derived3>(d1);
            Assert.AreSame(d1, d.First);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingFirstSecondWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.First<Base, Derived1, Derived2, Derived3>(d1);
            Derived2 d2;
            try
            {
                d2 = d.Second;
            }
            catch
            {
                d2 = null;
            }
            Assert.IsNull(d2);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingFirstThirdWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.First<Base, Derived1, Derived2, Derived3>(d1);
            Derived3 d3;
            try
            {
                d3 = d.Third;
            }
            catch
            {
                d3 = null;
            }
            Assert.IsNull(d3);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingFirstValueWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.First<Base, Derived1, Derived2, Derived3>(d1);
            Assert.AreSame(d1, d.Value);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingFirstSwitchActionWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.First<Base, Derived1, Derived2, Derived3>(d1);
            bool ranFirst = false;
            d.Switch(v => { ranFirst = true; }, v => { ranFirst = false; }, v => { ranFirst = false; });
            Assert.IsTrue(ranFirst);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingFirstSwitchFuncWorking()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.First<Base, Derived1, Derived2, Derived3>(d1);
            bool ranFirst = d.Switch(v => true, v => false, v => false);
            Assert.IsTrue(ranFirst);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingFirstToString()
        {
            Derived1 d1 = new Derived1();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.First<Base, Derived1, Derived2, Derived3>(d1);
            Assert.AreEqual("{First:" + d1 + "}", d.ToString());
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingFirstToStringOnNull()
        {
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.First<Base, Derived1, Derived2, Derived3>(null);
            Assert.AreEqual("{First:}", d.ToString());
        }

        #endregion Using First

        #region Using Second

        [TestMethod]
        public void DiscriminatedUnionThreeUsingSecondIsFirstWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Second<Base, Derived1, Derived2, Derived3>(d2);
            Assert.IsFalse(d.IsFirst);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingSecondIsSecondWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Second<Base, Derived1, Derived2, Derived3>(d2);
            Assert.IsTrue(d.IsSecond);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingSecondIsThirdWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Second<Base, Derived1, Derived2, Derived3>(d2);
            Assert.IsFalse(d.IsThird);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingSecondFirstWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Second<Base, Derived1, Derived2, Derived3>(d2);
            Derived1 d1;
            try
            {
                d1 = d.First;
            }
            catch
            {
                d1 = null;
            }
            Assert.IsNull(d1);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingSecondSecondWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Second<Base, Derived1, Derived2, Derived3>(d2);
            Assert.AreSame(d2, d.Second);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingSecondThirdWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Second<Base, Derived1, Derived2, Derived3>(d2);
            Derived3 d3;
            try
            {
                d3 = d.Third;
            }
            catch
            {
                d3 = null;
            }
            Assert.IsNull(d3);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingSecondValueWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Second<Base, Derived1, Derived2, Derived3>(d2);
            Assert.AreSame(d2, d.Value);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingSecondSwitchActionWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Second<Base, Derived1, Derived2, Derived3>(d2);
            bool ranSecond = false;
            d.Switch(v => { ranSecond = false; }, v => { ranSecond = true; }, v => { ranSecond = false; });
            Assert.IsTrue(ranSecond);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingSecondSwitchFuncWorking()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Second<Base, Derived1, Derived2, Derived3>(d2);
            bool ranSecond = d.Switch(v => false, v => true, v => false);
            Assert.IsTrue(ranSecond);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingSecondToString()
        {
            Derived2 d2 = new Derived2();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Second<Base, Derived1, Derived2, Derived3>(d2);
            Assert.AreEqual("{Second:" + d2 + "}", d.ToString());
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingSecondToStringOnNull()
        {
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Second<Base, Derived1, Derived2, Derived3>(null);
            Assert.AreEqual("{Second:}", d.ToString());
        }

        #endregion Using Second

        #region Using Third

        [TestMethod]
        public void DiscriminatedUnionThreeUsingThirdIsFirstWorking()
        {
            Derived3 d3 = new Derived3();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Third<Base, Derived1, Derived2, Derived3>(d3);
            Assert.IsFalse(d.IsFirst);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingThirdIsSecondWorking()
        {
            Derived3 d3 = new Derived3();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Third<Base, Derived1, Derived2, Derived3>(d3);
            Assert.IsFalse(d.IsSecond);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingThirdIsThirdWorking()
        {
            Derived3 d3 = new Derived3();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Third<Base, Derived1, Derived2, Derived3>(d3);
            Assert.IsTrue(d.IsThird);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingThirdFirstWorking()
        {
            Derived3 d3 = new Derived3();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Third<Base, Derived1, Derived2, Derived3>(d3);
            Derived1 d1;
            try
            {
                d1 = d.First;
            }
            catch
            {
                d1 = null;
            }
            Assert.IsNull(d1);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingThirdSecondWorking()
        {
            Derived3 d3 = new Derived3();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Third<Base, Derived1, Derived2, Derived3>(d3);
            Derived2 d2;
            try
            {
                d2 = d.Second;
            }
            catch
            {
                d2 = null;
            }
            Assert.IsNull(d2);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingThirdThirdWorking()
        {
            Derived3 d3 = new Derived3();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Third<Base, Derived1, Derived2, Derived3>(d3);
            Assert.AreSame(d3, d.Third);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingThirdValueWorking()
        {
            Derived3 d3 = new Derived3();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Third<Base, Derived1, Derived2, Derived3>(d3);
            Assert.AreSame(d3, d.Value);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingThirdSwitchActionWorking()
        {
            Derived3 d3 = new Derived3();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Third<Base, Derived1, Derived2, Derived3>(d3);
            bool ranThird = false;
            d.Switch(v => { ranThird = false; }, v => { ranThird = false; }, v => { ranThird = true; });
            Assert.IsTrue(ranThird);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingThirdSwitchFuncWorking()
        {
            Derived3 d3 = new Derived3();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Third<Base, Derived1, Derived2, Derived3>(d3);
            bool ranThird = d.Switch(v => false, v => false, v => true);
            Assert.IsTrue(ranThird);
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingThirdToString()
        {
            Derived3 d3 = new Derived3();
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Third<Base, Derived1, Derived2, Derived3>(d3);
            Assert.AreEqual("{Third:" + d3 + "}", d.ToString());
        }

        [TestMethod]
        public void DiscriminatedUnionThreeUsingThirdToStringOnNull()
        {
            IDiscriminatedUnion<Base, Derived1, Derived2, Derived3> d = DiscriminatedUnion.Third<Base, Derived1, Derived2, Derived3>(null);
            Assert.AreEqual("{Third:}", d.ToString());
        }

        #endregion Using Third

        #endregion DiscriminatedUnionThree

        private abstract class Base
        {
        }

        private class Derived1 : Base
        {
        }

        private class Derived2 : Base
        {
        }

        private class Derived3 : Base
        {
        }
    }
}