using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.Fluent;
using FsCheckUtils;
using Microsoft.FSharp.Core;
using NUnit.Framework;

namespace UnitTests
{
    using Property = Gen<Rose<Result>>;

    [TestFixture]
    public class PropExtensionsTests
    {
        [Test]
        public void AndSucceedsWhenBothPropertiesSucceed()
        {
            Func<int, bool> leftPropertyFunc = _ => true;
            Func<int, bool> rightPropertyFunc = _ => true;
            var andPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(
                n => AndPropertiesWithLabels(n, leftPropertyFunc, rightPropertyFunc));
            Check.QuickThrowOnFailure(andPropertiesFsFunc);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void AndFailsWhenEitherPropertyFails(bool leftResult, bool rightResult)
        {
            Func<int, bool> leftPropertyFunc = _ => leftResult;
            Func<int, bool> rightPropertyFunc = _ => rightResult;
            var andPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(
                n => AndPropertiesWithLabels(n, leftPropertyFunc, rightPropertyFunc));
            var ex = Assert.Throws<Exception>(() => Check.QuickThrowOnFailure(andPropertiesFsFunc));
            Assert.That(ex.Message, Is.StringStarting("Falsifiable"));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void OrSucceedsWhenEitherPropertiesSucceeds(bool leftResult, bool rightResult)
        {
            Func<int, bool> leftPropertyFunc = _ => leftResult;
            Func<int, bool> rightPropertyFunc = _ => rightResult;
            var orPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(
                n => OrPropertiesWithLabels(n, leftPropertyFunc, rightPropertyFunc));
            Check.QuickThrowOnFailure(orPropertiesFsFunc);
        }

        [Test]
        public void OrFailsWhenBothPropertiesFail()
        {
            Func<int, bool> leftPropertyFunc = _ => false;
            Func<int, bool> rightPropertyFunc = _ => false;
            var orPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(
                n => OrPropertiesWithLabels(n, leftPropertyFunc, rightPropertyFunc));
            var ex = Assert.Throws<Exception>(() => Check.QuickThrowOnFailure(orPropertiesFsFunc));
            Assert.That(ex.Message, Is.StringStarting("Falsifiable"));
        }

        [TestCase(1, new bool[]{}, TestName = "Params array empty")]
        [TestCase(2, new []{true}, TestName = "Params array with single true element")]
        [TestCase(3, true, true)]
        [TestCase(4, true, true, true)]
        public void AndAllSucceedsWhenAllPropertiesSucceed(int dummyUnique, params bool[] results)
        {
            var propertyFuncs = results.Select(result => new Func<int, bool>(_ => result));
            var andAllPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(
                n => AndAllPropertiesWithLabels(n, propertyFuncs));
            Check.QuickThrowOnFailure(andAllPropertiesFsFunc);
        }

        [TestCase(1, new[]{false}, TestName = "Params array with single false element")]
        [TestCase(2, false, false)]
        [TestCase(3, false, false, false)]
        [TestCase(4, true, false)]
        [TestCase(5, false, true)]
        [TestCase(6, false, false, false, false, false, true)]
        public void AndAllFailsWhenAnyPropertyFails(int dummyUnique, params bool[] results)
        {
            var propertyFuncs = results.Select(result => new Func<int, bool>(_ => result));
            var andAllPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(
                n => AndAllPropertiesWithLabels(n, propertyFuncs));
            var ex = Assert.Throws<Exception>(() => Check.QuickThrowOnFailure(andAllPropertiesFsFunc));
            Assert.That(ex.Message, Is.StringStarting("Falsifiable"));
        }

        [TestCase(1, new[]{true}, TestName = "Params array with single true element")]
        [TestCase(2, false, true)]
        [TestCase(3, true, false)]
        [TestCase(4, false, false, false, false, true)]
        public void OrAllSucceedsWhenAnyPropertySucceeds(int dummyUnique, params bool[] results)
        {
            var propertyFuncs = results.Select(result => new Func<int, bool>(_ => result));
            var orAllPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(
                n => OrAllPropertiesWithLabels(n, propertyFuncs));
            Check.QuickThrowOnFailure(orAllPropertiesFsFunc);
        }

        [TestCase(1, new[] { false }, TestName = "Params array with single false element")]
        [TestCase(2, false, false)]
        [TestCase(3, false, false, false)]
        [TestCase(4, false, false, false, false)]
        public void OrAllFailsWhenAllPropertiesFail(int dummyUnique, params bool[] results)
        {
            var propertyFuncs = results.Select(result => new Func<int, bool>(_ => result));
            var orAllPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(
                n => OrAllPropertiesWithLabels(n, propertyFuncs));
            var ex = Assert.Throws<Exception>(() => Check.QuickThrowOnFailure(orAllPropertiesFsFunc));
            Assert.That(ex.Message, Is.StringStarting("Falsifiable"));
        }

        [Test]
        public void LabelOncePassingTest()
        {
            var baseConfig = Config.Default;
            var spyingRunner = new SpyingRunner(baseConfig.Runner);
            var config = baseConfig.WithRunner(spyingRunner);

            Converter<int, Property> body = _ => PropExtensions.Label(Prop.ofTestable(true), "Label1");
            var bodyFSharpFunc = FSharpFunc<int, Property>.FromConverter(body);
            var property = Prop.forAll(Arb.from<int>(), bodyFSharpFunc);
            Check.One(config, property);

            Assert.That(spyingRunner.TestResult.IsTrue, Is.True);
            var testResultTrue = (TestResult.True)spyingRunner.TestResult;
            var labels = testResultTrue.Item.Labels.ToList();
            Assert.That(labels.Count, Is.EqualTo(0));
        }

        [Test]
        public void LabelOnceFailingTest()
        {
            var baseConfig = Config.Default;
            var spyingRunner = new SpyingRunner(baseConfig.Runner);
            var config = baseConfig.WithRunner(spyingRunner);

            Converter<int, Property> body = _ => PropExtensions.Label(Prop.ofTestable(false), "Label1");
            var bodyFSharpFunc = FSharpFunc<int, Property>.FromConverter(body);
            var property = Prop.forAll(Arb.from<int>(), bodyFSharpFunc);
            Check.One(config, property);

            Assert.That(spyingRunner.TestResult.IsFalse, Is.True);
            var testResultFalse = (TestResult.False)spyingRunner.TestResult;
            var labels = testResultFalse.Item1.Labels.ToList();
            Assert.That(labels.Count, Is.EqualTo(1));
            Assert.That(labels, Is.EqualTo(new[] {"Label1"}));
        }

        [Test]
        public void LabelTwiceFailingTest()
        {
            var baseConfig = Config.Default;
            var spyingRunner = new SpyingRunner(baseConfig.Runner);
            var config = baseConfig.WithRunner(spyingRunner);

            Converter<int, Property> body = _ => PropExtensions.Label(PropExtensions.Label(Prop.ofTestable(false), "Label1"), "Label2");
            var bodyFSharpFunc = FSharpFunc<int, Property>.FromConverter(body);
            var property = Prop.forAll(Arb.from<int>(), bodyFSharpFunc);
            Check.One(config, property);

            Assert.That(spyingRunner.TestResult.IsFalse, Is.True);
            var testResultFalse = (TestResult.False)spyingRunner.TestResult;
            var labels = testResultFalse.Item1.Labels.ToList();
            Assert.That(labels.Count, Is.EqualTo(2));
            Assert.That(labels, Is.EqualTo(new[] { "Label1", "Label2" }));
        }

        [Test]
        public void Implies()
        {
            var baseConfig = Config.Default;
            var spyingRunner = new SpyingRunner(baseConfig.Runner);
            var config = baseConfig.WithRunner(spyingRunner).WithMaxTest(1);

            var numAttempts = 0;
            Converter<int, Property> body = _ => PropExtensions.Implies(++numAttempts > 3, Prop.ofTestable(true));
            var bodyFSharpFunc = FSharpFunc<int, Property>.FromConverter(body);
            var property = Prop.forAll(Arb.from<int>(), bodyFSharpFunc);
            Check.One(config, property);

            Assert.That(spyingRunner.TestResult.IsTrue, Is.True);
            Assert.That(spyingRunner.NumCallsToOnArguments, Is.EqualTo(4));
        }

        [Test]
        public void ClassifyNonFluent()
        {
            var baseConfig = Config.Default;
            var spyingRunner = new SpyingRunner(baseConfig.Runner);
            var config = baseConfig.WithRunner(spyingRunner);

            Converter<int, Property> body = n =>
                PropExtensions.Classify<Property>(IsEven(n), "Even", "Odd")
                    .Invoke(Prop.ofTestable(true));
            var bodyFSharpFunc = FSharpFunc<int, Property>.FromConverter(body);
            var property = Prop.forAll(Arb.from<int>(), bodyFSharpFunc);
            Check.One(config, property);

            Assert.That(spyingRunner.TestResult.IsTrue, Is.True);
            var testResultTrue = (TestResult.True) spyingRunner.TestResult;
            var stamps = testResultTrue.Item.Stamps.ToList();
            Assert.That(stamps.Count, Is.EqualTo(2));
            var actualStampStrings = stamps.Select(stamp => stamp.Item2.First());
            var expectedStampStrings = new[] {"Even", "Odd"};
            Assert.That(actualStampStrings, Is.EqualTo(expectedStampStrings).Or.EqualTo(expectedStampStrings.Reverse()));
        }

        [Test]
        public void ClassifyFluentWithOneTypeParameter()
        {
            ClassifyFluentCommon(configuration =>
            {
                Func<int, bool> assertion = n => true;
                Spec
                    .ForAny(assertion)
                    .Classify(IsEven, "Even", "Odd")
                    .Check(configuration);
            });
        }

        [Test]
        public void ClassifyFluentWithTwoTypeParameters()
        {
            ClassifyFluentCommon(configuration =>
            {
                Func<int, int, bool> assertion = (n1, n2) => true;
                Func<int, int, bool> areEven = (n1, n2) => IsEven(n1) && IsEven(n2);
                Spec
                    .ForAny(assertion)
                    .Classify(areEven, "Even", "Odd")
                    .Check(configuration);
            });
        }

        [Test]
        public void ClassifyFluentWithThreeTypeParameters()
        {
            ClassifyFluentCommon(configuration =>
            {
                Func<int, int, int, bool> assertion = (n1, n2, n3) => true;
                Func<int, int, int, bool> areEven = (n1, n2, n3) => IsEven(n1) && IsEven(n2) && IsEven(n3);
                Spec
                    .ForAny(assertion)
                    .Classify(areEven, "Even", "Odd")
                    .Check(configuration);
            });
        }

        [Test]
        public void ChainParamsArrayOnly()
        {
            Converter<int, Property> body = n =>
            {
                var p1 = PropExtensions.Classify<Property>(IsEven(n), "Even", "Odd");
                var p2 = PropExtensions.Classify<Property>(IsLarge(n), "Large", "Small");
                var p3 = Prop.label<Property>("MyLabel");
                var p4 = Prop.trivial<Property>(n == 0);
                return PropExtensions.Chain(p1, p2, p3, p4);
            };
            var bodyFSharpFunc = FSharpFunc<int, Property>.FromConverter(body);
            var property = Prop.forAll(Arb.from<int>(), bodyFSharpFunc);
            Check.One(Config.QuickThrowOnFailure, property);
        }

        [Test]
        public void ChainParamsArrayAndInitialBool()
        {
            Converter<int, Property> body = n =>
            {
                var p1 = PropExtensions.Classify<Property>(IsEven(n), "Even", "Odd");
                var p2 = PropExtensions.Classify<Property>(IsLarge(n), "Large", "Small");
                var p3 = Prop.label<Property>("MyLabel");
                var p4 = Prop.trivial<Property>(n == 0);
                return PropExtensions.Chain(true, p1, p2, p3, p4);
            };
            var bodyFSharpFunc = FSharpFunc<int, Property>.FromConverter(body);
            var property = Prop.forAll(Arb.from<int>(), bodyFSharpFunc);
            Check.One(Config.QuickThrowOnFailure, property);
        }

        [Test]
        public void ChainParamsArrayAndInitialProperty()
        {
            Converter<int, Property> body = n =>
            {
                var p0 = Prop.ofTestable(true);
                var p1 = PropExtensions.Classify<Property>(IsEven(n), "Even", "Odd");
                var p2 = PropExtensions.Classify<Property>(IsLarge(n), "Large", "Small");
                var p3 = Prop.label<Property>("MyLabel");
                var p4 = Prop.trivial<Property>(n == 0);
                return PropExtensions.Chain(p0, p1, p2, p3, p4);
            };
            var bodyFSharpFunc = FSharpFunc<int, Property>.FromConverter(body);
            var property = Prop.forAll(Arb.from<int>(), bodyFSharpFunc);
            Check.One(Config.QuickThrowOnFailure, property);
        }

        [Test]
        public void ChainComplex()
        {
            Converter<int, Property> body = n =>
                PropExtensions.Chain(
                    PropExtensions.OrAll(
                        PropExtensions.Label(PropExtensions.And(IsEven(n), IsLarge(n)), "Even and large"),
                        PropExtensions.Label(PropExtensions.And(IsEven(n), !IsLarge(n)), "Even and small"),
                        PropExtensions.Label(PropExtensions.And(!IsEven(n), IsLarge(n)), "Odd and large"),
                        PropExtensions.Label(PropExtensions.And(!IsEven(n), !IsLarge(n)), "Odd and small")),
                    Prop.label<Property>("MyLabel"),
                    Prop.trivial<Property>(n == 0));
            var bodyFSharpFunc = FSharpFunc<int, Property>.FromConverter(body);
            var property = Prop.forAll(Arb.from<int>(), bodyFSharpFunc);
            Check.One(Config.QuickThrowOnFailure, property);
        }

        private static bool IsEven(int n)
        {
            return n % 2 == 0;
        }

        private static bool IsLarge(int n)
        {
            return n > 10;
        }

        private static void ClassifyFluentCommon(Action<Configuration> action)
        {
            var baseConfiguration = Config.Default.ToConfiguration();
            var spyingRunner = new SpyingRunner(baseConfiguration.Runner);
            var configuration = baseConfiguration.WithRunner(spyingRunner);

            action(configuration);

            Assert.That(spyingRunner.TestResult.IsTrue, Is.True);
            var testResultTrue = (TestResult.True)spyingRunner.TestResult;
            var stamps = testResultTrue.Item.Stamps.ToList();
            Assert.That(stamps.Count, Is.EqualTo(2));
            var actualStampStrings = stamps.Select(stamp => stamp.Item2.First());
            var expectedStampStrings = new[] { "Even", "Odd" };
            Assert.That(actualStampStrings, Is.EqualTo(expectedStampStrings).Or.EqualTo(expectedStampStrings.Reverse()));
        }

        private static Property AndPropertiesWithLabels(int n, Func<int, bool> leftPropertyFunc, Func<int, bool> rightPropertyFunc)
        {
            var p1 = PropExtensions.Label(leftPropertyFunc(n), "Left property");
            var p2 = PropExtensions.Label(rightPropertyFunc(n), "Right property");
            return PropExtensions.And(p1, p2);
        }

        private static Property OrPropertiesWithLabels(int n, Func<int, bool> leftPropertyFunc, Func<int, bool> rightPropertyFunc)
        {
            var p1 = PropExtensions.Label(leftPropertyFunc(n), "Left property");
            var p2 = PropExtensions.Label(rightPropertyFunc(n), "Right property");
            return PropExtensions.Or(p1, p2);
        }

        private static Property AndAllPropertiesWithLabels(int n, IEnumerable<Func<int, bool>> propertyFuncs)
        {
            var properties = propertyFuncs.Select((pf, index) => PropExtensions.Label(pf(n), string.Format("Property[{0}]", index)));
            return PropExtensions.AndAll(properties.ToArray());
        }

        private static Property OrAllPropertiesWithLabels(int n, IEnumerable<Func<int, bool>> propertyFuncs)
        {
            var properties = propertyFuncs.Select((pf, index) => PropExtensions.Label(pf(n), string.Format("Property[{0}]", index)));
            return PropExtensions.OrAll(properties.ToArray());
        }
    }
}
