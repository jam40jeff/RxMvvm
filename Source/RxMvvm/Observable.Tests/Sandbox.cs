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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MorseCode.RxMvvm.Common;

    [TestClass]
    [Ignore]
    public class Sandbox
    {
        private readonly string testField = null;

        [TestMethod]
        public void ObservableThreads()
        {
            Console.WriteLine("Starting Thread " + Thread.CurrentThread.ManagedThreadId);
            BehaviorSubject<int> s1 = new BehaviorSubject<int>(2);
            BehaviorSubject<int> s2 = new BehaviorSubject<int>(3);
            BehaviorSubject<int> sum = new BehaviorSubject<int>(5);
            List<int> computeThreads = new List<int>();
            List<int> receiveThreads = new List<int>();
            IScheduler computeScheduler = new EventLoopScheduler();
            IObservable<int> sumObservable = s1.ObserveOn(computeScheduler)
                                               .CombineLatest(
                                                   s2.ObserveOn(computeScheduler),
                                                   (first, second) =>
                                                       {
                                                           Console.WriteLine(
                                                               "Computing value " + first + " + " + second + " = "
                                                               + (first + second) + " on Thread "
                                                               + Thread.CurrentThread.ManagedThreadId + ".");
                                                           computeThreads.Add(Thread.CurrentThread.ManagedThreadId);
                                                           return first + second;
                                                       });
            sumObservable.Subscribe(sum.OnNext);
            sum.ObserveOn(new EventLoopScheduler()).Subscribe(
                v =>
                    {
                        Console.WriteLine(
                            "Received value " + v + " on Thread " + Thread.CurrentThread.ManagedThreadId + ".");
                        receiveThreads.Add(Thread.CurrentThread.ManagedThreadId);
                    });
            Thread.Sleep(100);
            s2.OnNext(1);
            s1.OnNext(4);
            s2.OnNext(4);
            s1.OnNext(1);
            Thread.Sleep(100);
            foreach (KeyValuePair<int, int> p in
                computeThreads.GroupBy(v => v).Select(g => new KeyValuePair<int, int>(g.Key, g.Count())))
            {
                Console.WriteLine(p.Value + " computes on Thread " + p.Key);
            }
            foreach (KeyValuePair<int, int> p in
                receiveThreads.GroupBy(v => v).Select(g => new KeyValuePair<int, int>(g.Key, g.Count())))
            {
                Console.WriteLine(p.Value + " receives on Thread " + p.Key);
            }
        }

        [TestMethod]
        public void ObservableThreadsWithThrottle()
        {
            Console.WriteLine("Starting Thread " + Thread.CurrentThread.ManagedThreadId);
            BehaviorSubject<int> s1 = new BehaviorSubject<int>(2);
            BehaviorSubject<int> s2 = new BehaviorSubject<int>(3);
            BehaviorSubject<int> sum = new BehaviorSubject<int>(5);
            List<int> computeThreads = new List<int>();
            List<int> receiveThreads = new List<int>();
            IScheduler computeScheduler = new EventLoopScheduler();
            IObservable<int> sumObservable = s1.ObserveOn(computeScheduler)
                                               .CombineLatest(
                                                   s2.ObserveOn(computeScheduler),
                                                   (first, second) =>
                                                       {
                                                           Console.WriteLine(
                                                               "Computing value " + first + " + " + second + " = "
                                                               + (first + second) + " on Thread "
                                                               + Thread.CurrentThread.ManagedThreadId + ".");
                                                           computeThreads.Add(Thread.CurrentThread.ManagedThreadId);
                                                           return first + second;
                                                       });
            sumObservable.Throttle(TimeSpan.FromMilliseconds(100), new EventLoopScheduler()).Subscribe(sum.OnNext);
            sum.ObserveOn(new EventLoopScheduler()).Subscribe(
                v =>
                    {
                        Console.WriteLine(
                            "Received value " + v + " on Thread " + Thread.CurrentThread.ManagedThreadId + ".");
                        receiveThreads.Add(Thread.CurrentThread.ManagedThreadId);
                    });
            Thread.Sleep(150);
            s2.OnNext(1);
            Thread.Sleep(50);
            s1.OnNext(4);
            Thread.Sleep(150);
            s2.OnNext(4);
            Thread.Sleep(250);
            s1.OnNext(1);
            Thread.Sleep(150);
            foreach (KeyValuePair<int, int> p in
                computeThreads.GroupBy(v => v).Select(g => new KeyValuePair<int, int>(g.Key, g.Count())))
            {
                Console.WriteLine(p.Value + " computes on Thread " + p.Key);
            }
            foreach (KeyValuePair<int, int> p in
                receiveThreads.GroupBy(v => v).Select(g => new KeyValuePair<int, int>(g.Key, g.Count())))
            {
                Console.WriteLine(p.Value + " receives on Thread " + p.Key);
            }
        }

        [TestMethod]
        public void ObservableThreadsWithThrottleOnCompute()
        {
            Console.WriteLine("Starting Thread " + Thread.CurrentThread.ManagedThreadId);
            BehaviorSubject<int> s1 = new BehaviorSubject<int>(2);
            BehaviorSubject<int> s2 = new BehaviorSubject<int>(3);
            BehaviorSubject<int> sum = new BehaviorSubject<int>(5);
            List<int> computeThreads = new List<int>();
            List<int> receiveThreads = new List<int>();
            IScheduler computeScheduler = new EventLoopScheduler();
            IObservable<int> sumObservable =
                s1.Throttle(TimeSpan.FromMilliseconds(100), computeScheduler)
                  .ObserveOn(computeScheduler)
                  .CombineLatest(
                      s2.Throttle(TimeSpan.FromMilliseconds(100), computeScheduler).ObserveOn(computeScheduler),
                      (first, second) =>
                          {
                              Console.WriteLine(
                                  "Computing value " + first + " + " + second + " = " + (first + second) + " on Thread "
                                  + Thread.CurrentThread.ManagedThreadId + ".");
                              computeThreads.Add(Thread.CurrentThread.ManagedThreadId);
                              return first + second;
                          });
            sumObservable.Subscribe(sum.OnNext);
            sum.ObserveOn(new EventLoopScheduler()).Subscribe(
                v =>
                    {
                        Console.WriteLine(
                            "Received value " + v + " on Thread " + Thread.CurrentThread.ManagedThreadId + ".");
                        receiveThreads.Add(Thread.CurrentThread.ManagedThreadId);
                    });
            Thread.Sleep(150);
            s2.OnNext(1);
            Thread.Sleep(50);
            s1.OnNext(4);
            Thread.Sleep(150);
            s2.OnNext(4);
            Thread.Sleep(250);
            s1.OnNext(1);
            Thread.Sleep(150);
            foreach (KeyValuePair<int, int> p in
                computeThreads.GroupBy(v => v).Select(g => new KeyValuePair<int, int>(g.Key, g.Count())))
            {
                Console.WriteLine(p.Value + " computes on Thread " + p.Key);
            }
            foreach (KeyValuePair<int, int> p in
                receiveThreads.GroupBy(v => v).Select(g => new KeyValuePair<int, int>(g.Key, g.Count())))
            {
                Console.WriteLine(p.Value + " receives on Thread " + p.Key);
            }
        }

        [TestMethod]
        public void ObservableThreadsWithBetterThrottleOnCompute()
        {
            Console.WriteLine("Starting Thread " + Thread.CurrentThread.ManagedThreadId);
            BehaviorSubject<int> s1 = new BehaviorSubject<int>(2);
            BehaviorSubject<int> s2 = new BehaviorSubject<int>(3);
            BehaviorSubject<int> sum = new BehaviorSubject<int>(5);
            List<int> computeThreads = new List<int>();
            List<int> receiveThreads = new List<int>();
            IScheduler computeScheduler = new EventLoopScheduler();

            IObservable<int> sumObservable =
                s1.Select(v => new Tuple<int, int>(v, s2.Value))
                  .Merge(s2.Select(v => new Tuple<int, int>(s1.Value, v)))
                  .Throttle(TimeSpan.FromMilliseconds(100), computeScheduler)
                  .Select(
                      v =>
                          {
                              Console.WriteLine(
                                  "Computing value " + v.Item1 + " + " + v.Item2 + " = " + (v.Item1 + v.Item2)
                                  + " on Thread " + Thread.CurrentThread.ManagedThreadId + ".");
                              computeThreads.Add(Thread.CurrentThread.ManagedThreadId);
                              return v.Item1 + v.Item2;
                          });

            sumObservable.Subscribe(sum.OnNext);
            sum.ObserveOn(new EventLoopScheduler()).Subscribe(
                v =>
                    {
                        Console.WriteLine(
                            "Received value " + v + " on Thread " + Thread.CurrentThread.ManagedThreadId + ".");
                        receiveThreads.Add(Thread.CurrentThread.ManagedThreadId);
                    });
            Thread.Sleep(150);
            s2.OnNext(1);
            Thread.Sleep(50);
            s1.OnNext(4);
            Thread.Sleep(150);
            s2.OnNext(4);
            Thread.Sleep(250);
            s1.OnNext(1);
            Thread.Sleep(150);
            foreach (KeyValuePair<int, int> p in
                computeThreads.GroupBy(v => v).Select(g => new KeyValuePair<int, int>(g.Key, g.Count())))
            {
                Console.WriteLine(p.Value + " computes on Thread " + p.Key);
            }
            foreach (KeyValuePair<int, int> p in
                receiveThreads.GroupBy(v => v).Select(g => new KeyValuePair<int, int>(g.Key, g.Count())))
            {
                Console.WriteLine(p.Value + " receives on Thread " + p.Key);
            }
        }

        [TestMethod]
        public void ThreadSwitching()
        {
            Console.WriteLine("Starting Thread " + Thread.CurrentThread.ManagedThreadId);
            BehaviorSubject<int> s1 = new BehaviorSubject<int>(2);
            BehaviorSubject<int> s2 = new BehaviorSubject<int>(3);
            BehaviorSubject<int> sum = new BehaviorSubject<int>(5);
            List<int> computeThreads = new List<int>();
            List<int> receiveThreads = new List<int>();
            IScheduler switchScheduler = new EventLoopScheduler();
            IScheduler computeScheduler = new EventLoopScheduler();

            IObservable<int> sumObservable = s1.Select(v => new Tuple<int, int>(v, s2.Value))
                                               .Merge(s2.Select(v => new Tuple<int, int>(s1.Value, v)))
                                               .Throttle(TimeSpan.FromMilliseconds(100), switchScheduler)
                                               .Select(
                                                   v =>
                                                       {
                                                           Console.WriteLine(
                                                               "Not yet switching computing value " + v.Item1 + " + "
                                                               + v.Item2 + " = " + (v.Item1 + v.Item2) + " from Thread "
                                                               + Thread.CurrentThread.ManagedThreadId + ".");
                                                           return v;
                                                       }).Select(
                                                           v =>
                                                               {
                                                                   Console.WriteLine(
                                                                       "Switching computing value " + v.Item1 + " + "
                                                                       + v.Item2 + " = " + (v.Item1 + v.Item2)
                                                                       + " from Thread "
                                                                       + Thread.CurrentThread.ManagedThreadId + ".");
                                                                   return v;
                                                               }).ObserveOn(computeScheduler).Select(
                                                                   v =>
                                                                       {
                                                                           Console.WriteLine(
                                                                               "Already switched computing value "
                                                                               + v.Item1 + " + " + v.Item2 + " = "
                                                                               + (v.Item1 + v.Item2) + " to Thread "
                                                                               + Thread.CurrentThread.ManagedThreadId
                                                                               + ".");
                                                                           return v;
                                                                       }).Select(
                                                                           v =>
                                                                               {
                                                                                   Console.WriteLine(
                                                                                       "Computing value " + v.Item1
                                                                                       + " + " + v.Item2 + " = "
                                                                                       + (v.Item1 + v.Item2)
                                                                                       + " on Thread "
                                                                                       + Thread.CurrentThread
                                                                                               .ManagedThreadId + ".");
                                                                                   computeThreads.Add(
                                                                                       Thread.CurrentThread
                                                                                             .ManagedThreadId);
                                                                                   return v.Item1 + v.Item2;
                                                                               });

            sumObservable.Subscribe(sum.OnNext);
            sum.ObserveOn(new EventLoopScheduler()).Subscribe(
                v =>
                    {
                        Console.WriteLine(
                            "Received value " + v + " on Thread " + Thread.CurrentThread.ManagedThreadId + ".");
                        receiveThreads.Add(Thread.CurrentThread.ManagedThreadId);
                    });
            Thread.Sleep(150);
            s2.OnNext(1);
            Thread.Sleep(50);
            s1.OnNext(4);
            Thread.Sleep(150);
            s2.OnNext(4);
            Thread.Sleep(250);
            s1.OnNext(1);
            Thread.Sleep(150);
            foreach (KeyValuePair<int, int> p in
                computeThreads.GroupBy(v => v).Select(g => new KeyValuePair<int, int>(g.Key, g.Count())))
            {
                Console.WriteLine(p.Value + " computes on Thread " + p.Key);
            }
            foreach (KeyValuePair<int, int> p in
                receiveThreads.GroupBy(v => v).Select(g => new KeyValuePair<int, int>(g.Key, g.Count())))
            {
                Console.WriteLine(p.Value + " receives on Thread " + p.Key);
            }
        }

        [TestMethod]
        public void Closures()
        {
            string testLocalVariable = null;

            string testFieldName = StaticReflection.GetInScopeMemberInfo(() => this.testField).Name;
            string testLocalVariableName = StaticReflection.GetInScopeMemberInfo(() => testLocalVariable).Name;

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