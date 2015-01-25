using System;
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
        private static Property IntProperty(int n)
        {
            var p1 = PropExtensions.Label(Math.Abs(n) >= 0, "Math.Abs(n) >= 0");
            var p2 = PropExtensions.Label(n + n == n * 2, "n + n == n * 2");
            var p3 = PropExtensions.Label(n + 1 == n + 2 - 1, "n + 1 == n + 2 - 1");
            return PropExtensions.AndAll(p1, p2, p3);
        }

        [Test]
        public void Test1()
        {
            var intPropertyFsFunc = FSharpFunc<int, Property>.FromConverter(IntProperty);
            Check.QuickThrowOnFailure(intPropertyFsFunc);
        }
    }
}
