using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.Fluent;
using FsCheckUtils;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class RegisterExtensionsTests
    {
        private static readonly Configuration Configuration = Config.VerboseThrowOnFailure.ToConfiguration();

        [Test]
        public void RegisterArb()
        {
            var gen = Gen.constant(Enumerable.Range(1, 5).ToList());
            var arb = Arb.fromGen(gen);
            arb.Register();

            Func<List<int>, bool> assertion = xs => xs.SequenceEqual(new[] { 1, 2, 3, 4, 5 });
            Spec.ForAny(assertion).Check(Configuration);
        }

        [Test]
        public void RegisterGen()
        {
            var gen = Gen.constant(Enumerable.Range(1, 5).Reverse().ToList());
            gen.Register();

            Func<List<int>, bool> assertion = xs => xs.SequenceEqual(new[] {5, 4, 3, 2, 1});
            Spec.ForAny(assertion).Check(Configuration);
        }

        [Test]
        public void RegisterGenLastOneWins()
        {
            var gen1 = Gen.constant(Enumerable.Range(1, 5).ToList());
            gen1.Register();

            var gen2 = Gen.constant(Enumerable.Range(1, 5).Reverse().ToList());
            gen2.Register();

            var gen3 = Gen.constant(Enumerable.Repeat(1, 5).ToList());
            gen3.Register();

            Func<List<int>, bool> assertion = xs => xs.SequenceEqual(new[] {1, 1, 1, 1, 1});
            Spec.ForAny(assertion).Check(Configuration);
        }
    }
}
