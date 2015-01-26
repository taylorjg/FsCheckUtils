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
        private static Property AndProperties(int n, Func<int, bool> leftPropertyFunc, Func<int, bool> rightPropertyFunc)
        {
            var p1 = PropExtensions.Label(leftPropertyFunc(n), "Left property");
            var p2 = PropExtensions.Label(rightPropertyFunc(n), "Right property");
            return PropExtensions.And(p1, p2);
        }

        [Test]
        public void AndSucceedsWhenBothPropertiesSucceed()
        {
            Func<int, bool> leftPropertyFunc = _ => true;
            Func<int, bool> rightPropertyFunc = _ => true;
            var andPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(n => AndProperties(n, leftPropertyFunc, rightPropertyFunc));
            Check.QuickThrowOnFailure(andPropertiesFsFunc);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void AndFailsWhenEitherPropertyFails(bool leftResult, bool rightResult)
        {
            Func<int, bool> leftPropertyFunc = _ => leftResult;
            Func<int, bool> rightPropertyFunc = _ => rightResult;
            var andPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(n => AndProperties(n, leftPropertyFunc, rightPropertyFunc));
            var ex = Assert.Throws<Exception>(() => Check.QuickThrowOnFailure(andPropertiesFsFunc));
            Assert.That(ex.Message, Is.StringStarting("Falsifiable"));
        }

        private static Property OrProperties(int n, Func<int, bool> leftPropertyFunc, Func<int, bool> rightPropertyFunc)
        {
            var p1 = PropExtensions.Label(leftPropertyFunc(n), "Left property");
            var p2 = PropExtensions.Label(rightPropertyFunc(n), "Right property");
            return PropExtensions.Or(p1, p2);
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void OrSucceedsWhenEitherPropertiesSucceeds(bool leftResult, bool rightResult)
        {
            Func<int, bool> leftPropertyFunc = _ => leftResult;
            Func<int, bool> rightPropertyFunc = _ => rightResult;
            var orPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(n => OrProperties(n, leftPropertyFunc, rightPropertyFunc));
            Check.QuickThrowOnFailure(orPropertiesFsFunc);
        }

        [Test]
        public void OrFailsWhenBothPropertiesFail()
        {
            Func<int, bool> leftPropertyFunc = _ => false;
            Func<int, bool> rightPropertyFunc = _ => false;
            var orPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(n => OrProperties(n, leftPropertyFunc, rightPropertyFunc));
            var ex = Assert.Throws<Exception>(() => Check.QuickThrowOnFailure(orPropertiesFsFunc));
            Assert.That(ex.Message, Is.StringStarting("Falsifiable"));
        }

        private static Property AndAllProperties(int n, IEnumerable<Func<int, bool>> propertyFuncs)
        {
            var index = 0;
            var properties = propertyFuncs.Select(pf => PropExtensions.Label(pf(n), string.Format("Property[{0}]", index++)));
            return PropExtensions.AndAll(properties.ToArray());
        }

        [TestCase(new bool[]{})]
        [TestCase(new[] { true})]
        [TestCase(true, true)]
        [TestCase(true, true, true)]
        public void AndAllSucceedsWhenAllPropertiesSucceed(params bool[] results)
        {
            var propertyFuncs = results.Select(result => new Func<int, bool>(_ => result));
            var andAllPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(n => AndAllProperties(n, propertyFuncs));
            Check.QuickThrowOnFailure(andAllPropertiesFsFunc);
        }

        [TestCase(new[]{false})]
        [TestCase(false, false)]
        [TestCase(false, false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false, false, false, false, true)]
        public void AndAllFailsWhenAnyPropertyFails(params bool[] results)
        {
            var propertyFuncs = results.Select(result => new Func<int, bool>(_ => result));
            var andAllPropertiesFsFunc = FSharpFunc<int, Property>.FromConverter(n => AndAllProperties(n, propertyFuncs));
            var ex = Assert.Throws<Exception>(() => Check.QuickThrowOnFailure(andAllPropertiesFsFunc));
            Assert.That(ex.Message, Is.StringStarting("Falsifiable"));
        }
    }
}
