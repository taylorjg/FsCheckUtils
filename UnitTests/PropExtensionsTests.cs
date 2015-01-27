using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;
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
                n => AndProperties(n, leftPropertyFunc, rightPropertyFunc));
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
                n => AndProperties(n, leftPropertyFunc, rightPropertyFunc));
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
                n => OrProperties(n, leftPropertyFunc, rightPropertyFunc));
            Check.QuickThrowOnFailure(orPropertiesFsFunc);
        }

        [Test]
        public void OrFailsWhenBothPropertiesFail()
        {
            Func<int, bool> leftPropertyFunc = _ => false;
            Func<int, bool> rightPropertyFunc = _ => false;
            var orPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(
                n => OrProperties(n, leftPropertyFunc, rightPropertyFunc));
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
                n => AndAllProperties(n, propertyFuncs));
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
                n => AndAllProperties(n, propertyFuncs));
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
                n => OrAllProperties(n, propertyFuncs));
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
                n => OrAllProperties(n, propertyFuncs));
            var ex = Assert.Throws<Exception>(() => Check.QuickThrowOnFailure(orAllPropertiesFsFunc));
            Assert.That(ex.Message, Is.StringStarting("Falsifiable"));
        }

        private static Property AndProperties(int n, Func<int, bool> leftPropertyFunc, Func<int, bool> rightPropertyFunc)
        {
            var p1 = PropExtensions.Label(leftPropertyFunc(n), "Left property");
            var p2 = PropExtensions.Label(rightPropertyFunc(n), "Right property");
            return PropExtensions.And(p1, p2);
        }

        private static Property OrProperties(int n, Func<int, bool> leftPropertyFunc, Func<int, bool> rightPropertyFunc)
        {
            var p1 = PropExtensions.Label(leftPropertyFunc(n), "Left property");
            var p2 = PropExtensions.Label(rightPropertyFunc(n), "Right property");
            return PropExtensions.Or(p1, p2);
        }

        private static Property AndAllProperties(int n, IEnumerable<Func<int, bool>> propertyFuncs)
        {
            var properties = propertyFuncs.Select((pf, index) => PropExtensions.Label(pf(n), string.Format("Property[{0}]", index)));
            return PropExtensions.AndAll(properties.ToArray());
        }

        private static Property OrAllProperties(int n, IEnumerable<Func<int, bool>> propertyFuncs)
        {
            var properties = propertyFuncs.Select((pf, index) => PropExtensions.Label(pf(n), string.Format("Property[{0}]", index)));
            return PropExtensions.OrAll(properties.ToArray());
        }
    }
}
