using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.Fluent;
using FsCheckUtils;
using NUnit.Framework;

namespace UnitTests
{
    using PickTuple = Tuple<int, List<int>>;

    [TestFixture]
    public class GenExtensionsTests
    {
        private static readonly Configuration Configuration = Config.VerboseThrowOnFailure.ToConfiguration();
        private const int SizeOfSampleValues = 20;
        private const int NumberOfSampleValues = 100;

        [Test]
        public void PickGeneratesListsOfCorrectLength()
        {
            Func<int, List<int>, bool> property = (n, l) =>
                {
                    var genPick = GenExtensions.Pick(n, l);
                    var sample = Gen.sample(SizeOfSampleValues, NumberOfSampleValues, genPick);
                    return sample.All(xs => xs.Count == n);
                };

            Spec
                .For(GenTuplesForPickTests, Uncurry(property))
                .Check(Configuration);
        }

        [Test]
        public void PickGeneratesListsContainingOnlyElementsFromTheInputList()
        {
            Func<int, List<int>, bool> property = (n, l) =>
                {
                    var genPick = GenExtensions.Pick(n, l);
                    var sample = Gen.sample(SizeOfSampleValues, NumberOfSampleValues, genPick);
                    return sample.All(xs => xs.All(l.Contains));
                };

            Spec
                .For(GenTuplesForPickTests, Uncurry(property))
                .Check(Configuration);
        }

        [Test]
        public void PickDoesNotKeepGeneratingTheSameList()
        {
            Func<int, List<int>, bool> property = (n, l) =>
            {
                var genPick = GenExtensions.Pick(n, l);
                var sample = Gen.sample(SizeOfSampleValues, NumberOfSampleValues, genPick);
                return sample.Distinct().Count() > (sample.Count() * 0.75);
            };

            Func<int, List<int>, bool> condition = (n, l) =>
                l.Count >= 4 && n >= l.Count / 2;

            Func<int, List<int>, IEnumerable<PickTuple>> shrinker = (_, __) =>
                Enumerable.Empty<PickTuple>();

            Spec
                .For(GenTuplesForPickTests, Uncurry(property))
                .When(Uncurry(condition))
                .Shrink(Uncurry(shrinker))
                .Check(Configuration);
        }

        // Add properties re:
        // Pick of Gens
        // SomeOf
        // SomeOf of Gens

        private static Func<Tuple<T1, T2>, TResult> Uncurry<T1, T2, TResult>(Func<T1, T2, TResult> f)
        {
            return tuple => f(tuple.Item1, tuple.Item2);
        }

        private static readonly Gen<PickTuple> GenTuplesForPickTests =
            from l in Any.OfType<int>().MakeList()
            from n in Any.IntBetween(0, l.Count)
            select Tuple.Create(n, l);
    }
}
