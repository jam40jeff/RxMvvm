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

namespace MorseCode.RxMvvm.Observable.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MorseCode.RxMvvm.Common;
    using MorseCode.RxMvvm.Common.Serialization;
    using MorseCode.RxMvvm.Common.StaticReflection;
    using MorseCode.RxMvvm.Observable.Collection;
    using MorseCode.RxMvvm.Observable.Property;

    [TestClass]
    //[Ignore]
    public class Sandbox
    {
        #region Fields

        private readonly string testField = null;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void AsyncObservableThreadsWithBetterThrottleOnComputeAndIsCalculating()
        {
            Console.WriteLine("Starting Thread " + Thread.CurrentThread.ManagedThreadId);
            BehaviorSubject<int> s1 = new BehaviorSubject<int>(2);
            BehaviorSubject<int> s2 = new BehaviorSubject<int>(3);
            BehaviorSubject<int> sum = new BehaviorSubject<int>(5);
            List<int> computeThreads = new List<int>();
            List<int> receiveThreads = new List<int>();
            IScheduler throttleScheduler = new EventLoopScheduler();
            Func<IScheduler> getComputeScheduler = () => new EventLoopScheduler();
            IScheduler receiveScheduler = new EventLoopScheduler();

            IObservable<Tuple<int, int>> sumObservable =
                s1.CombineLatest(s2, Tuple.Create).Throttle(TimeSpan.FromMilliseconds(100), throttleScheduler);

            IDisposable sumObservableSubscription = null;
            using (sumObservable.Subscribe(
                v =>
                {
                    if (sumObservableSubscription != null)
                    {
                        Console.WriteLine("Canceling previous.");
                        sumObservableSubscription.Dispose();
                    }
                    sumObservableSubscription = Observable.Create<int>(
                        (o, token) => Task.Factory.StartNew(
                            () =>
                            {
                                Thread.Sleep(200);
                                if (!token.IsCancellationRequested)
                                {
                                    Console.WriteLine(
                                        "Computing value " + v.Item1 + " + " + v.Item2 + " = "
                                        + (v.Item1 + v.Item2) + " on Thread "
                                        + Thread.CurrentThread.ManagedThreadId + ".");
                                    computeThreads.Add(Thread.CurrentThread.ManagedThreadId);
                                    o.OnNext(v.Item1 + v.Item2);
                                }
                                o.OnCompleted();
                                return Disposable.Empty;
                            })).ObserveOn(receiveScheduler).Subscribe(
                                    v2 =>
                                    {
                                        Console.WriteLine(
                                            "Received value " + v2 + " on Thread "
                                            + Thread.CurrentThread.ManagedThreadId + ".");
                                        receiveThreads.Add(Thread.CurrentThread.ManagedThreadId);
                                    });
                }))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Thread.Sleep(150);
                s2.OnNext(1);
                Thread.Sleep(50);
                s1.OnNext(4);
                Thread.Sleep(250);
                s2.OnNext(4);
                Thread.Sleep(150);
                s1.OnNext(1);
                Thread.Sleep(350);

                stopwatch.Stop();
                Console.WriteLine("Total Time: " + stopwatch.ElapsedMilliseconds + " ms");

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
        }

        [TestMethod]
        public void Buffer()
        {
            Employee employee1 = new Employee();
            employee1.Id.Value = "1";
            employee1.FirstName.Value = "First";
            employee1.LastName.Value = "Employee";
            Employee employee2 = new Employee();
            employee2.Id.Value = "2";
            employee2.FirstName.Value = "Second";
            employee2.LastName.Value = "Employee";
            Employee employee3 = new Employee();
            employee3.Id.Value = "3";
            employee3.FirstName.Value = "Third";
            employee3.LastName.Value = "Employee";
            Employee employee4 = new Employee();
            employee4.Id.Value = "4";
            employee4.FirstName.Value = "Fourth";
            employee4.LastName.Value = "Employee";

            IObservableCollection<Employee> employees =
                ObservableCollectionFactory.Instance.CreateObservableCollection(
                    new List<Employee> { employee1, employee2 });

            IObservableCollection<Employee> employees2 =
                ObservableCollectionFactory.Instance.CreateObservableCollection(
                    new List<Employee> { employee3, employee4 });

            IObservableProperty<IObservableCollection<Employee>> employeesProperty =
                ObservablePropertyFactory.Instance.CreateProperty(employees);

            employeesProperty.StartWith(new IObservableCollection<Employee>[] { null }).Buffer(2, 1).Subscribe(
                v =>
                {
                    string previousFirstEmployeeId = v[0] == null ? null : v[0][0].Id.Value;
                    string firstEmployeeId = v[1] == null ? null : v[1][0].Id.Value;
                    Console.WriteLine("Previous: " + previousFirstEmployeeId + ", Current: " + firstEmployeeId);
                });

            employeesProperty.Value = employees2;
            employeesProperty.Value = null;
            employeesProperty.Value = employees;
        }

        [TestMethod]
        public void CalculatedProperty()
        {
            Employee employee = new Employee();
            employee.FullName.Subscribe(Console.WriteLine);
            employee.FirstName.Value = "John";
            employee.LastName.Value = "Smith";
            Thread.Sleep(100);
            employee.FirstName.Value = "Fred";
            employee.LastName.Value = "Davis";
            Thread.Sleep(100);
        }

        [TestMethod]
        public void ChainedCollectionObservable()
        {
            Employee employee1 = new Employee();
            employee1.Id.Value = "1";
            employee1.FirstName.Value = "John";
            employee1.LastName.Value = "Smith";
            Employee employee2 = new Employee();
            employee2.Id.Value = "2";
            employee2.FirstName.Value = "Fred";
            employee2.LastName.Value = "Davis";
            Company company1 = new Company();
            company1.Name.Value = "First Company";
            Company company2 = new Company();
            company2.Name.Value = "Second Company";
            Company company3 = new Company();
            company3.Name.Value = "Third Company";
            Company company4 = new Company();
            company4.Name.Value = "Fourth Company";
            employee1.Company.Value = company1;
            employee2.Company.Value = company3;

            IObservableCollection<Employee> employees =
                ObservableCollectionFactory.Instance.CreateObservableCollection(new List<Employee> { employee1 });

            employees.SubscribeItems(
                e =>
                e.Company.BeginChain()
                 .Add(c => c.Name)
                 .CompleteWithDefaultIfNotComputable()
                 .Subscribe(v => Console.WriteLine("Employee " + e.Id.Value + ": " + v)));

            company1.Name.Value = "First Company Revised";
            employee1.Company.Value = company2;
            company2.Name.Value = "Second Company Revised";
            employees.Add(employee2);
            company3.Name.Value = "Third Company Revised";
            company4.Name.Value = "Fourth Company Revised";
            employee2.Company.Value = company4;
            company2.Name.Value = "Second Company Revised Again";
            employees.Remove(employee1);
            company2.Name.Value = "Second Company Revised A Third Time";
            company4.Name.Value = "Fourth Company Revised Again";
        }

        [TestMethod]
        public void ChainedProperty()
        {
            WorkTask workTask = new WorkTask();
            Employee employee1 = new Employee();
            employee1.FirstName.Value = "John";
            employee1.LastName.Value = "Smith";
            Employee employee2 = new Employee();
            employee2.FirstName.Value = "Fred";
            employee2.LastName.Value = "Davis";
            workTask.Employee.Value = employee1;

            workTask.Employee.Select(
                e => e == null ? Observable.Return<string>(null) : e.FullName.OnSuccessfulValueChanged)
                    .Switch()
                    .DistinctUntilChanged()
                    .Subscribe(v => Console.WriteLine("All: " + v));
            workTask.Employee.Select(
                e => (e == null ? Observable.Return<string>(null) : e.FullName.OnSuccessfulValueChanged).Skip(1))
                    .Switch()
                    .DistinctUntilChanged()
                    .Subscribe(v => Console.WriteLine("Leaf: " + v));

            employee1.LastName.Value = "Franklin";
            workTask.Employee.Value = null;
            workTask.Employee.Value = employee1;
            employee1.FirstName.Value = "Fred";
            employee1.LastName.Value = "Davis";
            workTask.Employee.Value = employee2;
            employee2.FirstName.Value = "Greg";
        }

        [TestMethod]
        public void ChainedPropertyUsingFactory()
        {
            WorkTask workTask = new WorkTask();
            Employee employee1 = new Employee();
            employee1.FirstName.Value = "John";
            employee1.LastName.Value = "Smith";
            Employee employee2 = new Employee();
            employee2.FirstName.Value = "Fred";
            employee2.LastName.Value = "Davis";
            workTask.Employee.Value = employee1;

            workTask.Employee.BeginChain()
                    .Add(e => e.FullName.OnSuccessfulValueChanged)
                    .CompleteWithDefaultIfNotComputable()
                    .Subscribe(v => Console.WriteLine("All: " + v));

            workTask.Employee.BeginChain()
                    .Add(e => e.FullName.OnSuccessfulValueChanged)
                    .CompleteWithDefaultIfNotComputable(true)
                    .Subscribe(v => Console.WriteLine("Leaf: " + v));

            employee1.LastName.Value = "Franklin";
            workTask.Employee.Value = null;
            workTask.Employee.Value = employee1;
            employee1.FirstName.Value = "Fred";
            employee1.LastName.Value = "Davis";
            workTask.Employee.Value = employee2;
            employee2.FirstName.Value = "Greg";
        }

        [TestMethod]
        public void ChainedPropertyWithThrottle()
        {
            WorkTask workTask = new WorkTask();
            Employee employee1 = new Employee();
            employee1.FirstName.Value = "John";
            employee1.LastName.Value = "Smith";
            Thread.Sleep(100);
            Employee employee2 = new Employee();
            employee2.FirstName.Value = "Fred";
            employee2.LastName.Value = "Davis";
            workTask.Employee.Value = employee1;

            workTask.Employee.Select(
                e => e == null ? Observable.Return<string>(null) : e.FullName.OnSuccessfulValueChanged)
                    .Switch()
                    .DistinctUntilChanged()
                    .Throttle(TimeSpan.FromMilliseconds(50))
                    .Subscribe(v => Console.WriteLine("All: " + v));
            workTask.Employee.Select(
                e => (e == null ? Observable.Return<string>(null) : e.FullName.OnSuccessfulValueChanged).Skip(1))
                    .Switch()
                    .DistinctUntilChanged()
                    .Throttle(TimeSpan.FromMilliseconds(50))
                    .Subscribe(v => Console.WriteLine("Leaf: " + v));

            employee1.LastName.Value = "Franklin";
            workTask.Employee.Value = null;
            workTask.Employee.Value = employee1;
            employee1.FirstName.Value = "Fred";
            employee1.LastName.Value = "Davis";
            workTask.Employee.Value = employee2;
            Thread.Sleep(100);
            employee2.FirstName.Value = "Greg";
            Thread.Sleep(100);
        }

        [TestMethod]
        public void ChainedPropertyWithThrottleUsingFactory()
        {
            WorkTask workTask = new WorkTask();
            Employee employee1 = new Employee();
            employee1.FirstName.Value = "John";
            employee1.LastName.Value = "Smith";
            Thread.Sleep(100);
            Employee employee2 = new Employee();
            employee2.FirstName.Value = "Fred";
            employee2.LastName.Value = "Davis";
            workTask.Employee.Value = employee1;

            workTask.Employee.BeginChain()
                    .Add(e => e.FullName.OnSuccessfulValueChanged)
                    .CompleteWithDefaultIfNotComputable()
                    .Throttle(TimeSpan.FromMilliseconds(50))
                    .Subscribe(v => Console.WriteLine("All: " + v));

            workTask.Employee.BeginChain()
                    .Add(e => e.FullName.OnSuccessfulValueChanged)
                    .CompleteWithDefaultIfNotComputable(true)
                    .Throttle(TimeSpan.FromMilliseconds(50))
                    .Subscribe(v => Console.WriteLine("Leaf: " + v));

            employee1.LastName.Value = "Franklin";
            workTask.Employee.Value = null;
            workTask.Employee.Value = employee1;
            employee1.FirstName.Value = "Fred";
            employee1.LastName.Value = "Davis";
            workTask.Employee.Value = employee2;
            Thread.Sleep(100);
            employee2.FirstName.Value = "Greg";
            Thread.Sleep(100);
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
        public void CollectionObservable()
        {
            Employee employee1 = new Employee();
            employee1.Id.Value = "1";
            employee1.FirstName.Value = "First";
            employee1.LastName.Value = "Employee";
            Employee employee2 = new Employee();
            employee2.Id.Value = "2";
            employee2.FirstName.Value = "Second";
            employee2.LastName.Value = "Employee";
            Employee employee3 = new Employee();
            employee3.Id.Value = "3";
            employee3.FirstName.Value = "Third";
            employee3.LastName.Value = "Employee";
            Employee employee4 = new Employee();
            employee4.Id.Value = "4";
            employee4.FirstName.Value = "Fourth";
            employee4.LastName.Value = "Employee";

            IObservableCollection<Employee> employees =
                ObservableCollectionFactory.Instance.CreateObservableCollection(
                    new List<Employee> { employee1, employee2 });

            employees.SubscribeItems(
                e =>
                e.FullName.OnSuccessfulValueChanged.Subscribe(
                    v => Console.WriteLine("Employee " + e.Id.Value + ": " + v)));

            employee1.FirstName.Value = "1st";
            employee2.FirstName.Value = "2nd";
            employee3.FirstName.Value = "3rdd";
            employee3.FirstName.Value = "Third";
            employees.Add(employee3);
            employee3.FirstName.Value = "3rd";
            employees.Remove(employee2);
            employee2.FirstName.Value = "2ndd";
            employee1.FirstName.Value = "1stt";
            employee4.FirstName.Value = "4th";
            employees.Add(employee4);
        }

        [TestMethod]
        public void Join()
        {
            Employee employee = new Employee();

            employee.FirstName.Value = "John";
            employee.LastName.Value = "Davis";

            IReadOnlyProperty<Employee> employeeProperty =
                ObservablePropertyFactory.Instance.CreateReadOnlyProperty(employee);

            IObservable<string> firstNameObservable =
                employeeProperty.BeginChain().Add(e => e.FirstName).CompleteWithDefaultIfNotComputable();
            firstNameObservable.Subscribe(Console.WriteLine);
            IObservable<string> lastNameObservable =
                employeeProperty.BeginChain().Add(e => e.LastName).CompleteWithDefaultIfNotComputable();
            lastNameObservable.Subscribe(Console.WriteLine);
            firstNameObservable.Join(
                lastNameObservable, s => firstNameObservable.Skip(1), i => Observable.Empty<Unit>(), Tuple.Create)
                               .Subscribe(Console.WriteLine);

            employee.FirstName.Value = "Fred";
            employee.LastName.Value = "Smith";
            employee.FirstName.Value = "John";
            employee.LastName.Value = "Davis";
            employee.FirstName.Value = "Fred";
            employee.LastName.Value = "Smith";
        }

        [TestMethod]
        public void LazyReadOnlyPropertyValue()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () => true);

            ConsoleWriteLineWithStopwatch(sw, "Value: " + lazyReadOnlyProperty.Value);

            Thread.Sleep(1000);

            ConsoleWriteLineWithStopwatch(sw, "Value: " + lazyReadOnlyProperty.Value);
        }

        [TestMethod]
        public void LazyReadOnlyPropertyValueException()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () =>
            { throw new InvalidOperationException("Expected exception."); });

            ConsoleWriteLineWithStopwatch(sw, "Value: " + lazyReadOnlyProperty.Value);

            Thread.Sleep(1000);

            ConsoleWriteLineWithStopwatch(sw, "Value: " + lazyReadOnlyProperty.Value);
        }

        [TestMethod]
        public void LazyReadOnlyPropertyOnCalculationException()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () =>
                { throw new InvalidOperationException("Expected exception."); });

            lazyReadOnlyProperty.OnCalculationException.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "CalculationException: " + v));

            Thread.Sleep(1000);
        }

        [TestMethod]
        public void LazyReadOnlyPropertyOnCalculationExceptionNoException()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () => true);

            lazyReadOnlyProperty.OnCalculationException.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "CalculationException: " + v));

            Thread.Sleep(1000);
        }

        [TestMethod]
        public void LazyReadOnlyPropertyOnChanged()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () => true);

            lazyReadOnlyProperty.OnChanged.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "Change: " + v));

            Thread.Sleep(1000);
        }

        [TestMethod]
        public void LazyReadOnlyPropertyOnChangedException()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () =>
            { throw new InvalidOperationException("Expected exception."); });

            lazyReadOnlyProperty.OnChanged.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "Change: " + v));

            Thread.Sleep(1000);
        }

        [TestMethod]
        public void LazyReadOnlyPropertyOnSet()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () => true);

            lazyReadOnlyProperty.OnSet.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "Set: " + v));

            Thread.Sleep(1000);
        }

        [TestMethod]
        public void LazyReadOnlyPropertyOnSetException()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () =>
            { throw new InvalidOperationException("Expected exception."); });

            lazyReadOnlyProperty.OnSet.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "Set: " + v));

            Thread.Sleep(1000);
        }

        [TestMethod]
        public void LazyReadOnlyPropertyOnValueOrDefaultChanged()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () => true);

            lazyReadOnlyProperty.OnValueOrDefaultChanged.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "ValueOrDefaultChange: " + v));

            Thread.Sleep(1000);
        }

        [TestMethod]
        public void LazyReadOnlyPropertyOnValueOrDefaultChangedException()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () =>
            { throw new InvalidOperationException("Expected exception."); });

            lazyReadOnlyProperty.OnValueOrDefaultChanged.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "ValueOrDefaultChange: " + v));

            Thread.Sleep(1000);
        }

        [TestMethod]
        public void LazyReadOnlyPropertyOnValueOrDefaultSet()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () => true);

            lazyReadOnlyProperty.OnValueOrDefaultSet.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "ValueOrDefaultSet: " + v));

            Thread.Sleep(1000);
        }

        [TestMethod]
        public void LazyReadOnlyPropertyOnValueOrDefaultSetException()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () =>
            { throw new InvalidOperationException("Expected exception."); });

            lazyReadOnlyProperty.OnValueOrDefaultSet.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "ValueOrDefaultSet: " + v));

            Thread.Sleep(1000);
        }

        [TestMethod]
        public void LazyReadOnlyPropertyOnValueOrDefaultChangedMultiple()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty = SetupLazyReadOnlyProperty(sw, () => true);

            lazyReadOnlyProperty.OnValueOrDefaultChanged.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "ValueOrDefaultChange: " + v));

            Thread.Sleep(250);

            lazyReadOnlyProperty.OnValueOrDefaultChanged.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "ValueOrDefaultChange2: " + v));

            Thread.Sleep(500);

            lazyReadOnlyProperty.OnValueOrDefaultChanged.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "ValueOrDefaultChange3: " + v));

            Thread.Sleep(250);
        }

        private ILazyReadOnlyProperty<bool> SetupLazyReadOnlyProperty(Stopwatch sw, Func<bool> getValue)
        {
            RxMvvmConfiguration.SetNotifyPropertyChangedSchedulerFactory(() => NewThreadScheduler.Default);

            ConsoleWriteLineWithStopwatch(sw, "Creating property.");

            ILazyReadOnlyProperty<bool> lazyReadOnlyProperty =
                ObservablePropertyFactory.Instance.CreateLazyReadOnlyProperty(
                    async () =>
                    {
                        Console.WriteLine("Calculating value.");
                        await Task.Delay(500).ConfigureAwait(false);
                        try
                        {
                            bool value = getValue();
                            ConsoleWriteLineWithStopwatch(sw, "Producing value " + value + ".");
                            return value;
                        }
                        catch (Exception)
                        {
                            ConsoleWriteLineWithStopwatch(sw, "Producing exception" + ".");
                            throw;
                        }
                        finally
                        {
                            Console.WriteLine("Calculated value.");
                        }
                    });

            ConsoleWriteLineWithStopwatch(sw, "Created property.");

            lazyReadOnlyProperty.OnIsCalculatedChanged.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "IsCalculated: " + v));
            lazyReadOnlyProperty.OnIsCalculatingChanged.ObserveOn(NewThreadScheduler.Default).Subscribe(v => ConsoleWriteLineWithStopwatch(sw, "IsCalculating: " + v));
            lazyReadOnlyProperty.PropertyChanged +=
                (sender, args) => ConsoleWriteLineWithStopwatch(sw, "PropertyChanged: " + args.PropertyName + ", Value: " + lazyReadOnlyProperty.GetType().GetProperty(args.PropertyName).GetValue(lazyReadOnlyProperty));

            Thread.Sleep(100);

            return lazyReadOnlyProperty;
        }

        private void ConsoleWriteLineWithStopwatch(Stopwatch sw, string s)
        {
            Console.WriteLine("Elapsed: " + sw.ElapsedMilliseconds + ", " + s);
        }

        [TestMethod]
        public void MergeCollectionPropertyWithChanges()
        {
            Employee employee1 = new Employee();
            employee1.Id.Value = "1";
            employee1.FirstName.Value = "First";
            employee1.LastName.Value = "Employee";
            Employee employee2 = new Employee();
            employee2.Id.Value = "2";
            employee2.FirstName.Value = "Second";
            employee2.LastName.Value = "Employee";
            Employee employee3 = new Employee();
            employee3.Id.Value = "3";
            employee3.FirstName.Value = "Third";
            employee3.LastName.Value = "Employee";
            Employee employee4 = new Employee();
            employee4.Id.Value = "4";
            employee4.FirstName.Value = "Fourth";
            employee4.LastName.Value = "Employee";

            IObservableCollection<Employee> employees =
                ObservableCollectionFactory.Instance.CreateObservableCollection(
                    new List<Employee> { employee1, employee2 });

            IObservableCollection<Employee> employees2 =
                ObservableCollectionFactory.Instance.CreateObservableCollection(
                    new List<Employee> { employee3, employee4 });

            IObservableProperty<IObservableCollection<Employee>> employeesProperty =
                ObservablePropertyFactory.Instance.CreateProperty(employees);

            employeesProperty.MergeCollectionPropertyWithChanges(employeesProperty)
                             .Subscribe(
                                 v =>
                                 Console.WriteLine(
                                     "New IDs: "
                                     + string.Join(",", v.CollectionChanged.NewItems.Select(e => e.Id.Value))
                                     + ", Old IDs: "
                                     + string.Join(",", v.CollectionChanged.OldItems.Select(e => e.Id.Value))
                                     + ", Collection: "
                                     + string.Join(
                                         ",",
                                         ((IEnumerable<Employee>)v.Collection ?? new Employee[0]).Select(
                                             e => e.Id.Value))));

            employeesProperty.Value = employees2;
            employees2.Add(employee2);
            employeesProperty.Value = null;
            employees.Add(employee3);
            employeesProperty.Value = employees;
            employees.Remove(employee3);
        }

        [TestMethod]
        public void MergeCollectionPropertyWithChanges2()
        {
            Employee employee1 = new Employee();
            employee1.Id.Value = "1";
            employee1.FirstName.Value = "First";
            employee1.LastName.Value = "Employee";
            Employee employee2 = new Employee();
            employee2.Id.Value = "2";
            employee2.FirstName.Value = "Second";
            employee2.LastName.Value = "Employee";
            Employee employee3 = new Employee();
            employee3.Id.Value = "3";
            employee3.FirstName.Value = "Third";
            employee3.LastName.Value = "Employee";
            Employee employee4 = new Employee();
            employee4.Id.Value = "4";
            employee4.FirstName.Value = "Fourth";
            employee4.LastName.Value = "Employee";

            IObservableCollection<Employee> employees =
                ObservableCollectionFactory.Instance.CreateObservableCollection(
                    new List<Employee> { employee1, employee2 });

            IObservableCollection<Employee> employees2 =
                ObservableCollectionFactory.Instance.CreateObservableCollection(
                    new List<Employee> { employee3, employee4 });

            IObservableProperty<IObservableCollection<Employee>> employeesProperty =
                ObservablePropertyFactory.Instance.CreateProperty(employees);

            employeesProperty.MergeCollectionPropertyWithChanges(employeesProperty)
                             .Subscribe(
                                 v =>
                                 Console.WriteLine(
                                     "New IDs: "
                                     + string.Join(",", v.CollectionChanged.NewItems.Select(e => e.Id.Value))
                                     + ", Old IDs: "
                                     + string.Join(",", v.CollectionChanged.OldItems.Select(e => e.Id.Value))
                                     + ", Collection: "
                                     + string.Join(
                                         ",",
                                         ((IEnumerable<Employee>)v.Collection ?? new Employee[0]).Select(
                                             e => e.Id.Value))));

            employees.Add(employee3);
            employees.Remove(employee3);
        }

        [TestMethod]
        public void MergeCollectionPropertyWithChangesWithReadOnlyProperty()
        {
            Employee employee1 = new Employee();
            employee1.Id.Value = "1";
            employee1.FirstName.Value = "First";
            employee1.LastName.Value = "Employee";
            Employee employee2 = new Employee();
            employee2.Id.Value = "2";
            employee2.FirstName.Value = "Second";
            employee2.LastName.Value = "Employee";
            Employee employee3 = new Employee();
            employee3.Id.Value = "3";
            employee3.FirstName.Value = "Third";
            employee3.LastName.Value = "Employee";
            Employee employee4 = new Employee();
            employee4.Id.Value = "4";
            employee4.FirstName.Value = "Fourth";
            employee4.LastName.Value = "Employee";

            IObservableCollection<Employee> employees =
                ObservableCollectionFactory.Instance.CreateObservableCollection(
                    new List<Employee> { employee1, employee2 });

            IObservableCollection<Employee> employees2 =
                ObservableCollectionFactory.Instance.CreateObservableCollection(
                    new List<Employee> { employee3, employee4 });

            IReadOnlyProperty<IObservableCollection<Employee>> employeesProperty =
                ObservablePropertyFactory.Instance.CreateReadOnlyProperty(employees);

            employeesProperty.MergeCollectionPropertyWithChanges(employeesProperty)
                             .Subscribe(
                                 v =>
                                 Console.WriteLine(
                                     "New IDs: "
                                     + string.Join(",", v.CollectionChanged.NewItems.Select(e => e.Id.Value))
                                     + ", Old IDs: "
                                     + string.Join(",", v.CollectionChanged.OldItems.Select(e => e.Id.Value))
                                     + ", Collection: "
                                     + string.Join(
                                         ",",
                                         ((IEnumerable<Employee>)v.Collection ?? new Employee[0]).Select(
                                             e => e.Id.Value))));

            employees.Add(employee3);
            employees.Remove(employee3);
        }

        [TestMethod]
        public void MultiChainedProperty()
        {
            WorkTask workTask = new WorkTask();
            Employee employee1 = new Employee();
            employee1.FirstName.Value = "John";
            employee1.LastName.Value = "Smith";
            Employee employee2 = new Employee();
            employee2.FirstName.Value = "Fred";
            employee2.LastName.Value = "Davis";
            Company company1 = new Company();
            company1.Name.Value = "First Company";
            Company company2 = new Company();
            company2.Name.Value = "Second Company";
            Company company3 = new Company();
            company3.Name.Value = "Third Company";
            Company company4 = new Company();
            company4.Name.Value = "Fourth Company";
            workTask.Employee.Value = employee1;
            employee1.Company.Value = company1;
            employee2.Company.Value = company3;

            workTask.Employee.Select(e => e == null ? Observable.Return<Company>(null) : e.Company)
                    .Switch()
                    .Select(c => c == null ? Observable.Return<string>(null) : c.Name)
                    .Switch()
                    .DistinctUntilChanged()
                    .Subscribe(v => Console.WriteLine("All: " + v));
            workTask.Employee.Select(e => e == null ? Observable.Return<Company>(null) : e.Company)
                    .Switch()
                    .Select(c => (c == null ? Observable.Return<string>(null) : c.Name).Skip(1))
                    .Switch()
                    .DistinctUntilChanged()
                    .Subscribe(v => Console.WriteLine("Leaf: " + v));

            company1.Name.Value = "First Company Revised";
            workTask.Employee.Value = null;
            workTask.Employee.Value = employee1;
            employee1.Company.Value = company2;
            company2.Name.Value = "Second Company Revised";
            workTask.Employee.Value = employee2;
            company3.Name.Value = "Third Company Revised";
            company4.Name.Value = "Fourth Company Revised";
            employee2.Company.Value = company4;
        }

        [TestMethod]
        public void MultiChainedPropertyUsingFactory()
        {
            WorkTask workTask = new WorkTask();
            Employee employee1 = new Employee();
            employee1.FirstName.Value = "John";
            employee1.LastName.Value = "Smith";
            Employee employee2 = new Employee();
            employee2.FirstName.Value = "Fred";
            employee2.LastName.Value = "Davis";
            Company company1 = new Company();
            company1.Name.Value = "First Company";
            Company company2 = new Company();
            company2.Name.Value = "Second Company";
            Company company3 = new Company();
            company3.Name.Value = "Third Company";
            Company company4 = new Company();
            company4.Name.Value = "Fourth Company";
            workTask.Employee.Value = employee1;
            employee1.Company.Value = company1;
            employee2.Company.Value = company3;

            workTask.Employee.BeginChain()
                    .Add(e => e.Company)
                    .Add(c => c.Name)
                    .CompleteWithDefaultIfNotComputable()
                    .Subscribe(v => Console.WriteLine("All: " + v));

            workTask.Employee.BeginChain()
                    .Add(e => e.Company)
                    .Add(c => c.Name)
                    .CompleteWithDefaultIfNotComputable(true)
                    .Subscribe(v => Console.WriteLine("Leaf: " + v));

            company1.Name.Value = "First Company Revised";
            workTask.Employee.Value = null;
            workTask.Employee.Value = employee1;
            employee1.Company.Value = company2;
            company2.Name.Value = "Second Company Revised";
            workTask.Employee.Value = employee2;
            company3.Name.Value = "Third Company Revised";
            company4.Name.Value = "Fourth Company Revised";
            employee2.Company.Value = company4;
        }

        [TestMethod]
        public void NotifyPropertyChangedForCollections()
        {
            RxMvvmConfiguration.SetNotifyPropertyChangedSchedulerFactory(() => Scheduler.Immediate);

            Employee employee1 = new Employee();
            Employee employee2 = new Employee();
            Employee employee3 = new Employee();
            Employee employee4 = new Employee();
            IObservableCollection<Employee> employees =
                ObservableCollectionFactory.Instance.CreateObservableCollection(
                    new List<Employee> { employee1, employee2 });
            employees.PropertyChanged +=
                (sender, args) => Console.WriteLine(args.PropertyName + " changed, count now " + employees.Count + ".");
            employees.CollectionChanged +=
                (sender, args) => Console.WriteLine("Collection changed: " + args.Action + ".");
            employees.Add(employee3);
            employees[1] = employee4;
            employees.Remove(employee1);
            employees.Remove(employee2);
        }

        [TestMethod]
        public void NotifyPropertyChangedForProperties()
        {
            RxMvvmConfiguration.SetNotifyPropertyChangedSchedulerFactory(() => Scheduler.Immediate);

            Employee employee = new Employee();
            employee.FullName.PropertyChanged +=
                (sender, args) =>
                Console.WriteLine(args.PropertyName + " changed, now " + employee.FullName.LatestSuccessfulValue + ".");
            employee.FirstName.Value = "John";
            employee.LastName.Value = "Smith";
            employee.LastName.Value = "Smith";
            employee.LastName.Value = "Davis";
        }

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
        public void ObservableThreadsWithBetterThrottleOnCompute()
        {
            Console.WriteLine("Starting Thread " + Thread.CurrentThread.ManagedThreadId);
            BehaviorSubject<int> s1 = new BehaviorSubject<int>(2);
            BehaviorSubject<int> s2 = new BehaviorSubject<int>(3);
            BehaviorSubject<int> sum = new BehaviorSubject<int>(5);
            List<int> computeThreads = new List<int>();
            List<int> receiveThreads = new List<int>();

            IObservable<int> sumObservable =
                s1.Select(v => new Tuple<int, int>(v, s2.Value))
                  .Merge(s2.Select(v => new Tuple<int, int>(s1.Value, v)))
                  .Throttle(TimeSpan.FromMilliseconds(100), new NewThreadScheduler())
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
        public void ObservableThreadsWithBetterThrottleOnComputeAndIsCalculating()
        {
            Console.WriteLine("Starting Thread " + Thread.CurrentThread.ManagedThreadId);
            BehaviorSubject<int> s1 = new BehaviorSubject<int>(2);
            BehaviorSubject<int> s2 = new BehaviorSubject<int>(3);
            BehaviorSubject<int> sum = new BehaviorSubject<int>(5);
            List<int> computeThreads = new List<int>();
            List<int> receiveThreads = new List<int>();
            IScheduler throttleScheduler = new EventLoopScheduler();
            Func<IScheduler> getComputeScheduler = () => new EventLoopScheduler();
            IScheduler receiveScheduler = new EventLoopScheduler();

            IObservable<Tuple<int, int>> sumObservable =
                s1.CombineLatest(s2, Tuple.Create).Throttle(TimeSpan.FromMilliseconds(100), throttleScheduler);

            IDisposable sumObservableSubscription = null;
            using (sumObservable.Subscribe(
                v =>
                {
                    if (sumObservableSubscription != null)
                    {
                        Console.WriteLine("Canceling previous.");
                        sumObservableSubscription.Dispose();
                    }
                    sumObservableSubscription = Observable.Create<int>(
                        o =>
                        {
                            Thread.Sleep(200);
                            Console.WriteLine(
                                "Computing value " + v.Item1 + " + " + v.Item2 + " = " + (v.Item1 + v.Item2)
                                + " on Thread " + Thread.CurrentThread.ManagedThreadId + ".");
                            computeThreads.Add(Thread.CurrentThread.ManagedThreadId);
                            o.OnNext(v.Item1 + v.Item2);
                            o.OnCompleted();
                            return Disposable.Empty;
                        }).SubscribeOn(getComputeScheduler()).ObserveOn(receiveScheduler).Subscribe(
                                v2 =>
                                {
                                    Console.WriteLine(
                                        "Received value " + v2 + " on Thread "
                                        + Thread.CurrentThread.ManagedThreadId + ".");
                                    receiveThreads.Add(Thread.CurrentThread.ManagedThreadId);
                                });
                }))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Thread.Sleep(150);
                s2.OnNext(1);
                Thread.Sleep(50);
                s1.OnNext(4);
                Thread.Sleep(250);
                s2.OnNext(4);
                Thread.Sleep(150);
                s1.OnNext(1);
                Thread.Sleep(350);

                stopwatch.Stop();
                Console.WriteLine("Total Time: " + stopwatch.ElapsedMilliseconds + " ms");

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
        public void ReadOnlyProperty()
        {
            IReadOnlyProperty<string> a = ObservablePropertyFactory.Instance.CreateReadOnlyProperty("a");
            IReadOnlyProperty<string> b = ObservablePropertyFactory.Instance.CreateReadOnlyProperty("b");
            IReadOnlyProperty<string> c = ObservablePropertyFactory.Instance.CreateReadOnlyProperty("c");
            IReadOnlyProperty<string> d = ObservablePropertyFactory.Instance.CreateReadOnlyProperty("d");

            a.Subscribe(Console.WriteLine);
            b.Subscribe(Console.WriteLine);
            c.Subscribe(Console.WriteLine);
            d.Subscribe(Console.WriteLine);
            d.Subscribe(Console.WriteLine);
            d.Subscribe(Console.WriteLine);
            d.Subscribe(Console.WriteLine);
        }

        [TestMethod]
        public void RxOnlyAsyncObservableThreadsWithBetterThrottleOnComputeAndIsCalculating()
        {
            Console.WriteLine("Starting Thread " + Thread.CurrentThread.ManagedThreadId);
            BehaviorSubject<int> s1 = new BehaviorSubject<int>(2);
            BehaviorSubject<int> s2 = new BehaviorSubject<int>(3);
            List<int> computeThreads = new List<int>();
            List<int> receiveThreads = new List<int>();
            IScheduler throttleScheduler = new EventLoopScheduler();
            IScheduler computeScheduler = NewThreadScheduler.Default;
            IScheduler receiveScheduler = new EventLoopScheduler();

            IObservable<Tuple<int, int>> sumObservable =
                s1.CombineLatest(s2, Tuple.Create).Throttle(TimeSpan.FromMilliseconds(100), throttleScheduler);

            Func<CalculatedPropertyHelper, int, int, Task<int>> calculate = async (helper, v1, v2) =>
                {
                    Thread.Sleep(200);
                    helper.CheckCancellationToken();
                    Console.WriteLine(
                        "Computing value " + v1 + " + " + v2 + " = " + (v1 + v2) + " on Thread "
                        + Thread.CurrentThread.ManagedThreadId + ".");
                    computeThreads.Add(Thread.CurrentThread.ManagedThreadId);
                    return await Task.FromResult(v1 + v2);
                };

            BehaviorSubject<int> sum = new BehaviorSubject<int>(0);
            IDisposable scheduledTask = computeScheduler.ScheduleAsync(
                async (scheduler, token) =>
                {
                    await scheduler.Yield();
                    sum.OnNext(await calculate(new CalculatedPropertyHelper(scheduler, token), s1.Value, s2.Value));
                });
            using (sumObservable.Subscribe(
                v =>
                {
                    if (scheduledTask != null)
                    {
                        Console.WriteLine("Canceling previous.");
                        scheduledTask.Dispose();
                    }
                    scheduledTask = computeScheduler.ScheduleAsync(
                        async (scheduler, token) =>
                        {
                            await scheduler.Yield();
                            sum.OnNext(
                                await
                                calculate(new CalculatedPropertyHelper(scheduler, token), v.Item1, v.Item2));
                        });
                }))
            {
                using (sum.ObserveOn(receiveScheduler).Subscribe(
                    v2 =>
                    {
                        Console.WriteLine(
                            "Received value " + v2 + " on Thread " + Thread.CurrentThread.ManagedThreadId + ".");
                        receiveThreads.Add(Thread.CurrentThread.ManagedThreadId);
                    }))
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    Thread.Sleep(150);
                    s2.OnNext(1);
                    Thread.Sleep(50);
                    s1.OnNext(4);
                    Thread.Sleep(250);
                    s2.OnNext(4);
                    Thread.Sleep(150);
                    s1.OnNext(1);
                    Thread.Sleep(350);

                    stopwatch.Stop();
                    Console.WriteLine("Total Time: " + stopwatch.ElapsedMilliseconds + " ms");

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
            }
        }

        [TestMethod]
        public void ScheduleAsync()
        {
            this.RunScheduleAsync(
                () =>
                {
                    Console.WriteLine("Originally running on thread: " + Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(500);
                    Console.WriteLine("Running on thread: " + Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(500);
                    Console.WriteLine("Still running on thread: " + Thread.CurrentThread.ManagedThreadId);
                    return 5;
                });
        }

        [TestMethod]
        public void ScheduleAsyncCancellable()
        {
            this.RunScheduleAsync(
                async checkCancellationToken =>
                {
                    Console.WriteLine("Originally running on thread: " + Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(500);
                    await checkCancellationToken();
                    Console.WriteLine("Running on thread: " + Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(500);
                    await checkCancellationToken();
                    Console.WriteLine("Still running on thread: " + Thread.CurrentThread.ManagedThreadId);
                    return 5;
                });
        }

        [TestMethod]
        public void Serialization()
        {
            RxMvvmConfiguration.UseSerialization = true;

            Employee employee = new Employee();
            employee.FirstName.Value = "John";
            employee.LastName.Value = "Smith";

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream s = new MemoryStream();
            formatter.Serialize(s, employee);

            s.Seek(0, SeekOrigin.Begin);

            Employee e = (Employee)formatter.Deserialize(s);

            e.FullName.OnSuccessfulValueChanged.Subscribe(Console.WriteLine);

            e.LastName.Value = "Davis";
        }

        [TestMethod]
        public void Serialization2()
        {
            RxMvvmConfiguration.UseSerialization = true;

            EmployeeWithFullNameConsoleWrite employee = new EmployeeWithFullNameConsoleWrite();
            employee.FirstName.Value = "John";
            employee.LastName.Value = "Smith";

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream s = new MemoryStream();
            formatter.Serialize(s, employee);

            s.Seek(0, SeekOrigin.Begin);

            EmployeeWithFullNameConsoleWrite e = (EmployeeWithFullNameConsoleWrite)formatter.Deserialize(s);

            e.LastName.Value = "Davis";
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

        [TestMethod]
        public void Switch()
        {
            IConnectableObservable<string> o1 =
                Observable.Interval(TimeSpan.FromSeconds(1)).Select(v => "a" + v).Publish();
            IConnectableObservable<string> o2 =
                Observable.Interval(TimeSpan.FromSeconds(1)).Select(v => "b" + v).Publish();
            IConnectableObservable<string> o3 =
                Observable.Interval(TimeSpan.FromSeconds(1)).Select(v => "c" + v).Publish();
            IConnectableObservable<string> o4 =
                Observable.Interval(TimeSpan.FromSeconds(1)).Select(v => "d" + v).Publish();

            IObservable<long> o = Observable.Interval(TimeSpan.FromSeconds(1.7500012312));

            o1.Connect();
            o2.Connect();
            o3.Connect();
            o4.Connect();

            IObservable<string> observable = o.Select(
                v =>
                {
                    switch (v % 4)
                    {
                        case 0:
                            return o1;
                        case 1:
                            return o2;
                        case 2:
                            return o3;
                        case 3:
                            return o4;
                        default:
                            throw new InvalidOperationException();
                    }
                }).Switch();

            observable.Subscribe(Console.WriteLine);

            Thread.Sleep(12000);
        }

        [TestMethod]
        public void SwitchWithCompletedInnerSequences()
        {
            IObservable<int?> innerObservable1 = Observable.Return((int?)1);
            IObservable<int?> innerObservable2 = Observable.Return((int?)2);
            IObservable<int?> innerObservable3 = Observable.Return((int?)3);
            IObservable<int?> innerObservable4 = Observable.Return((int?)4);
            IObservable<int?> innerObservable5 = Observable.Return((int?)5);

            IObservable<IObservable<int?>> outerObservable1 = Observable.Create<IObservable<int?>>(
                o =>
                {
                    o.OnNext(innerObservable1);
                    o.OnNext(innerObservable2);
                    o.OnNext(null);
                    o.OnNext(innerObservable3);
                    o.OnCompleted();
                    return Disposable.Empty;
                });

            IObservable<IObservable<int?>> outerObservable2 = Observable.Create<IObservable<int?>>(
                o =>
                {
                    o.OnNext(innerObservable4);
                    o.OnNext(null);
                    o.OnNext(innerObservable5);
                    o.OnCompleted();
                    return Disposable.Empty;
                });

            IObservableProperty<IObservable<IObservable<int?>>> outerObservableProperty =
                ObservablePropertyFactory.Instance.CreateProperty(outerObservable1);

            outerObservableProperty.Select(o => o ?? Observable.Return<IObservable<int?>>(null))
                                   .Switch()
                                   .Select(o => o ?? Observable.Return<int?>(null))
                                   .Switch()
                                   .Subscribe(v => Console.WriteLine(v == null ? "(NULL)" : v.ToString()));

            outerObservableProperty.Value = null;
            outerObservableProperty.Value = outerObservable2;
        }

        [TestMethod]
        public void TaskTest()
        {
            IObservable<int> observable = Observable.Create<int>(
                o =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Sending synchronous value.");
                    o.OnNext(3);
                    o.OnCompleted();
                    return Disposable.Empty;
                });
            IObservable<int> observableAsync = Observable.Create<int>(
                (o, token) => Task.Factory.StartNew(
                    () =>
                    {
                        Thread.Sleep(1000);
                        if (!token.IsCancellationRequested)
                        {
                            Console.WriteLine("Sending asynchronous value.");
                            o.OnNext(5);
                        }
                        o.OnCompleted();
                    }));

            Console.WriteLine("Observing synchronous.");
            IDisposable syncSubscription = observable.Subscribe(Console.WriteLine);
            Console.WriteLine("Observed synchronous.");
            syncSubscription.Dispose();
            Console.WriteLine("Disposed synchronous.");
            Console.WriteLine("Observing asynchronous.");
            IDisposable asyncSubscription1 = observableAsync.Subscribe(Console.WriteLine);
            Console.WriteLine("Observed asynchronous.");
            asyncSubscription1.Dispose();
            Console.WriteLine("Disposed asynchronous.");
            Thread.Sleep(1500);
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
        public void TwoScheduleAsync()
        {
            DateTime start = DateTime.Now;
            this.RunTwoScheduleAsync(
                start,
                async helper =>
                {
                    Console.WriteLine(
                        this.GetElapsedTimeString(start) + ": Originally running 1 on thread: "
                        + Thread.CurrentThread.ManagedThreadId);

                    //await Task.Delay(500, cancellationToken);
                    //await scheduler.Sleep(TimeSpan.FromSeconds(0.5), cancellationToken);
                    Thread.Sleep(500);
                    helper.CheckCancellationToken();
                    Console.WriteLine(this.GetElapsedTimeString(start) + ": Checking token.");

                    Console.WriteLine(
                        this.GetElapsedTimeString(start) + ": Running 1 on thread: "
                        + Thread.CurrentThread.ManagedThreadId);

                    //await Task.Delay(500, cancellationToken);
                    //await scheduler.Sleep(TimeSpan.FromSeconds(0.5), cancellationToken);
                    Thread.Sleep(500);
                    helper.CheckCancellationToken();
                    Console.WriteLine(this.GetElapsedTimeString(start) + ": Checking token.");

                    Console.WriteLine(
                        this.GetElapsedTimeString(start) + ": Still running 1 on thread: "
                        + Thread.CurrentThread.ManagedThreadId);
                    return await Task.FromResult(5);
                },
                () =>
                {
                    Console.WriteLine(
                        this.GetElapsedTimeString(start) + ": Originally running 2 on thread: "
                        + Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(500);
                    Console.WriteLine(
                        this.GetElapsedTimeString(start) + ": Running 2 on thread: "
                        + Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(500);
                    Console.WriteLine(
                        this.GetElapsedTimeString(start) + ": Still running 2 on thread: "
                        + Thread.CurrentThread.ManagedThreadId);
                    return 5;
                });
        }

        #endregion

        #region Methods

        private string GetElapsedTimeString(DateTime start)
        {
            return (DateTime.Now - start).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
        }

        private void RunScheduleAsync(Func<int> calculate)
        {
            this.RunScheduleAsync(async checkCancellationToken => await Task.FromResult(calculate()));
        }

        private void RunScheduleAsync(Func<Func<SchedulerOperation>, Task<int>> calculate)
        {
            Console.WriteLine("Running on thread: " + Thread.CurrentThread.ManagedThreadId);
            IObservable<int> observable = Observable.Create<int>(
                o =>
                {
                    IDisposable d = new NewThreadScheduler().ScheduleAsync(
                        async (scheduler, token) =>
                        {
                            o.OnNext(await calculate(scheduler.Yield));
                            o.OnCompleted();
                        });
                    return Disposable.Create(
                        () =>
                        {
                            d.Dispose();
                            Console.WriteLine("Disposed!");
                        });
                });
            Console.WriteLine("Subscribing.");
            CompositeDisposable s = new CompositeDisposable();
            for (int i = 0; i < 12; i++)
            {
                s.Add(observable.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed.")));
            }
            Console.WriteLine("Subscribed.");
            Thread.Sleep(700);
            s.Dispose();
            Console.WriteLine("Should have disposed.");
            Thread.Sleep(2000);
            Console.WriteLine("Done.");
        }

        private void RunTwoScheduleAsync(
            DateTime start, Func<CalculatedPropertyHelper, Task<int>> calculate1, Func<int> calculate2)
        {
            IScheduler eventLoopScheduler = new EventLoopScheduler();
            Console.WriteLine(
                this.GetElapsedTimeString(start) + ": Running on thread: " + Thread.CurrentThread.ManagedThreadId);
            IObservable<int> observable1 = Observable.Create<int>(
                o =>
                {
                    IDisposable d = eventLoopScheduler.ScheduleAsync(
                        async (scheduler, token) =>
                        {
                            await scheduler.Yield();
                            o.OnNext(await calculate1(new CalculatedPropertyHelper(scheduler, token)));
                            o.OnCompleted();
                        });
                    return Disposable.Create(
                        () =>
                        {
                            d.Dispose();
                            Console.WriteLine(this.GetElapsedTimeString(start) + ": Disposed!");
                        });
                });
            IObservable<int> observable2 = Observable.Create<int>(
                o =>
                {
                    IDisposable d = eventLoopScheduler.ScheduleAsync(
                        async (scheduler, token) =>
                        {
                            o.OnNext(await Task.FromResult(calculate2()));
                            o.OnCompleted();
                        });
                    return Disposable.Create(
                        () =>
                        {
                            d.Dispose();
                            Console.WriteLine(this.GetElapsedTimeString(start) + ": Disposed!");
                        });
                });
            Console.WriteLine(this.GetElapsedTimeString(start) + ": Subscribing.");
            CompositeDisposable s = new CompositeDisposable();
            for (int i = 0; i < 8; i++)
            {
                s.Add(
                    observable1.Subscribe(
                        Console.WriteLine, () => Console.WriteLine(this.GetElapsedTimeString(start) + ": Completed 1.")));
            }

            //for (int i = 0; i < 8; i++)
            //{
            //    s.Add(observable2.Subscribe(Console.WriteLine, () => Console.WriteLine(GetElapsedTimeString(start) + ": Completed 2.")));
            //}
            Console.WriteLine(this.GetElapsedTimeString(start) + ": Subscribed.");
            Thread.Sleep(1700);
            s.Dispose();
            Console.WriteLine(this.GetElapsedTimeString(start) + ": Should have disposed.");
            Thread.Sleep(2000);
            Console.WriteLine(this.GetElapsedTimeString(start) + ": Done.");
        }

        #endregion

        private class CalculatedPropertyHelper
        {
            #region Fields

            private readonly IScheduler scheduler;

            private readonly CancellationToken token;

            #endregion

            #region Constructors and Destructors

            public CalculatedPropertyHelper(IScheduler scheduler, CancellationToken token)
            {
                this.scheduler = scheduler;
                this.token = token;
            }

            #endregion

            #region Public Properties

            public IScheduler Scheduler
            {
                get
                {
                    return this.scheduler;
                }
            }

            public CancellationToken Token
            {
                get
                {
                    return this.token;
                }
            }

            #endregion

            #region Public Methods and Operators

            public void CheckCancellationToken()
            {
                if (this.token.IsCancellationRequested)
                {
                    this.token.ThrowIfCancellationRequested();
                }
            }

            public async Task CheckCancellationTokenAndYield()
            {
                await this.Scheduler.Yield();
            }

            #endregion
        }

        private class Company
        {
            #region Fields

            private readonly IObservableProperty<string> id =
                ObservablePropertyFactory.Instance.CreateProperty<string>(null);

            private readonly IObservableProperty<string> name =
                ObservablePropertyFactory.Instance.CreateProperty<string>(null);

            #endregion

            #region Public Properties

            public IObservableProperty<string> Id
            {
                get
                {
                    return this.id;
                }
            }

            public IObservableProperty<string> Name
            {
                get
                {
                    return this.name;
                }
            }

            #endregion
        }

        [Serializable]
        private class Employee
        {
            #region Fields

            private readonly IObservableProperty<Company> company =
                ObservablePropertyFactory.Instance.CreateProperty<Company>(null);

            private readonly IObservableProperty<string> firstName =
                ObservablePropertyFactory.Instance.CreateProperty<string>(null);

            private readonly ICalculatedProperty<string> fullName;

            private readonly IObservableProperty<string> id =
                ObservablePropertyFactory.Instance.CreateProperty<string>(null);

            private readonly IObservableProperty<string> lastName =
                ObservablePropertyFactory.Instance.CreateProperty<string>(null);

            #endregion

            #region Constructors and Destructors

            public Employee()
            {
                this.fullName = ObservablePropertyFactory.Instance.CreateCalculatedProperty(
                    this.firstName, this.lastName, (firstName, lastName) => firstName + " " + lastName);
            }

            #endregion

            #region Public Properties

            public IObservableProperty<Company> Company
            {
                get
                {
                    return this.company;
                }
            }

            public IObservableProperty<string> FirstName
            {
                get
                {
                    return this.firstName;
                }
            }

            public ICalculatedProperty<string> FullName
            {
                get
                {
                    return this.fullName;
                }
            }

            public IObservableProperty<string> Id
            {
                get
                {
                    return this.id;
                }
            }

            public IObservableProperty<string> LastName
            {
                get
                {
                    return this.lastName;
                }
            }

            #endregion
        }

        [Serializable]
        private class EmployeeWithFullNameConsoleWrite : Employee, IDisposable
        {
            #region Fields

            private readonly ISerializableAction<IDisposable> fullNameSubscription;

            #endregion

            #region Constructors and Destructors

            public EmployeeWithFullNameConsoleWrite()
            {
                this.fullNameSubscription =
                    SerializableActionFactory.Instance.CreateSerializableActionWithContext(
                        this.FullName, v => v.Subscribe(Console.WriteLine));
            }

            #endregion

            #region Public Methods and Operators

            public void Dispose()
            {
                this.fullNameSubscription.Value.Dispose();
            }

            #endregion
        }

        private class WorkTask
        {
            #region Fields

            private readonly IObservableProperty<Employee> employee =
                ObservablePropertyFactory.Instance.CreateProperty<Employee>(null);

            private readonly IObservableProperty<double> estimatedHours =
                ObservablePropertyFactory.Instance.CreateProperty(0.0);

            private readonly IObservableProperty<string> id =
                ObservablePropertyFactory.Instance.CreateProperty<string>(null);

            private readonly IObservableProperty<string> name =
                ObservablePropertyFactory.Instance.CreateProperty<string>(null);

            #endregion

            #region Public Properties

            public IObservableProperty<Employee> Employee
            {
                get
                {
                    return this.employee;
                }
            }

            public IObservableProperty<double> EstimatedHours
            {
                get
                {
                    return this.estimatedHours;
                }
            }

            public IObservableProperty<string> Id
            {
                get
                {
                    return this.id;
                }
            }

            public IObservableProperty<string> Name
            {
                get
                {
                    return this.name;
                }
            }

            #endregion
        }
    }
}