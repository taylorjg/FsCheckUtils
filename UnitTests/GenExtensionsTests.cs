using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.Fluent;
using FsCheckUtils;
using NUnit.Framework;

namespace UnitTests
{
    using PickTuple = Tuple<int, int[]>;

    [TestFixture]
    public class GenExtensionsTests
    {
        private static readonly Configuration Configuration = Config.VerboseThrowOnFailure.ToConfiguration();
        private const int SizeOfSampleValues = 20;
        private const int NumberOfSampleValues = 100;

        [Test]
        public void PickValuesGeneratesListsOfCorrectLength()
        {
            Func<int, int[], bool> property = (n, l) =>
                {
                    var genPick = GenExtensions.PickValues(n, l);
                    var sample = Gen.sample(SizeOfSampleValues, NumberOfSampleValues, genPick);
                    return sample.All(xs => xs.Count == n);
                };

            Spec
                .For(GenTuplesForPickValuesTests, Uncurry(property))
                .Check(Configuration);
        }

        [Test]
        public void PickValuesGeneratesListsContainingOnlyElementsFromTheInputList()
        {
            Func<int, int[], bool> property = (n, l) =>
                {
                    var genPick = GenExtensions.PickValues(n, l);
                    var sample = Gen.sample(SizeOfSampleValues, NumberOfSampleValues, genPick);
                    return sample.All(xs => xs.All(l.Contains));
                };

            Spec
                .For(GenTuplesForPickValuesTests, Uncurry(property))
                .Check(Configuration);
        }

        [Test]
        public void PickValuesDoesNotKeepGeneratingTheSameList()
        {
            Func<int, int[], bool> property = (n, l) =>
            {
                var genPick = GenExtensions.PickValues(n, l);
                var sample = Gen.sample(SizeOfSampleValues, NumberOfSampleValues, genPick);
                return sample.Distinct().Count() > (sample.Count() * 0.75);
            };

            Func<int, int[], bool> condition = (n, l) => l.Length >= 4 && n >= l.Length / 2;

            Func<int, int[], IEnumerable<PickTuple>> shrinker = (_, __) =>
                Enumerable.Empty<PickTuple>();

            Spec
                .For(GenTuplesForPickValuesTests, Uncurry(property))
                .When(Uncurry(condition))
                .Shrink(Uncurry(shrinker))
                .Check(Configuration);
        }

        [Test]
        public void Shuffle()
        {
            var numbers = Enumerable.Range(1, 10).ToList();
            var genShuffledNumbers = GenExtensions.Shuffle(numbers.AsEnumerable());
            var shuffledNumberSamples = Gen.sample(1, 100, genShuffledNumbers).ToList();
            var comparer = new ShuffledNumbersSampleComparer();
            Assert.That(shuffledNumberSamples.Distinct(comparer).Count(), Is.EqualTo(shuffledNumberSamples.Count));
        }

        private class ShuffledNumbersSampleComparer : IEqualityComparer<IEnumerable<int>>
        {
            public bool Equals(IEnumerable<int> sample1, IEnumerable<int> sample2)
            {
                return sample1.SequenceEqual(sample2);
            }

            public int GetHashCode(IEnumerable<int> sample)
            {
                return 0;
            }
        }

        // Add more tests re:
        // PickGenerators
        // SomeOfValues
        // SomeOfGenerators

        [Test]
        public void Zip2()
        {
            var g1 = Gen.constant(1);
            var g2 = Gen.constant(2);
            var gen = GenExtensions.Zip(g1, g2);

            Spec
                .For(gen, tuple =>
                    tuple.Item1 == 1 &&
                    tuple.Item2 == 2)
                .Check(Configuration);
        }

        [Test]
        public void Zip3()
        {
            var g1 = Gen.constant(1);
            var g2 = Gen.constant(2);
            var g3 = Gen.constant(3);
            var gen = GenExtensions.Zip(g1, g2, g3);

            Spec
                .For(gen, tuple =>
                    tuple.Item1 == 1 &&
                    tuple.Item2 == 2 &&
                    tuple.Item3 == 3)
                .Check(Configuration);
        }

        [Test]
        public void Zip4()
        {
            var g1 = Gen.constant(1);
            var g2 = Gen.constant(2);
            var g3 = Gen.constant(3);
            var g4 = Gen.constant(4);
            var gen = GenExtensions.Zip(g1, g2, g3, g4);

            Spec
                .For(gen, tuple =>
                    tuple.Item1 == 1 &&
                    tuple.Item2 == 2 &&
                    tuple.Item3 == 3 &&
                    tuple.Item4 == 4)
                .Check(Configuration);
        }

        [Test]
        public void Zip5()
        {
            var g1 = Gen.constant(1);
            var g2 = Gen.constant(2);
            var g3 = Gen.constant(3);
            var g4 = Gen.constant(4);
            var g5 = Gen.constant(5);
            var gen = GenExtensions.Zip(g1, g2, g3, g4, g5);

            Spec
                .For(gen, tuple =>
                    tuple.Item1 == 1 &&
                    tuple.Item2 == 2 &&
                    tuple.Item3 == 3 &&
                    tuple.Item4 == 4 &&
                    tuple.Item5 == 5)
                .Check(Configuration);
        }

        [Test]
        public void Zip6()
        {
            var g1 = Gen.constant(1);
            var g2 = Gen.constant(2);
            var g3 = Gen.constant(3);
            var g4 = Gen.constant(4);
            var g5 = Gen.constant(5);
            var g6 = Gen.constant(6);
            var gen = GenExtensions.Zip(g1, g2, g3, g4, g5, g6);

            Spec
                .For(gen, tuple =>
                    tuple.Item1 == 1 &&
                    tuple.Item2 == 2 &&
                    tuple.Item3 == 3 &&
                    tuple.Item4 == 4 &&
                    tuple.Item5 == 5 &&
                    tuple.Item6 == 6)
                .Check(Configuration);
        }

        [Test]
        public void Zip7()
        {
            var g1 = Gen.constant(1);
            var g2 = Gen.constant(2);
            var g3 = Gen.constant(3);
            var g4 = Gen.constant(4);
            var g5 = Gen.constant(5);
            var g6 = Gen.constant(6);
            var g7 = Gen.constant(7);
            var gen = GenExtensions.Zip(g1, g2, g3, g4, g5, g6, g7);

            Spec
                .For(gen, tuple =>
                    tuple.Item1 == 1 &&
                    tuple.Item2 == 2 &&
                    tuple.Item3 == 3 &&
                    tuple.Item4 == 4 &&
                    tuple.Item5 == 5 &&
                    tuple.Item6 == 6 &&
                    tuple.Item7 == 7)
                .Check(Configuration);
        }

        [Test]
        public void NumChar()
        {
            Spec
                .For(GenExtensions.NumChar, c => Char.IsDigit(c))
                .Check(Configuration);
        }

        [Test]
        public void AlphaUpperChar()
        {
            Spec
                .For(GenExtensions.AlphaUpperChar, c => Char.IsLetter(c) && Char.IsUpper(c))
                .Check(Configuration);
        }

        [Test]
        public void AlphaLowerChar()
        {
            Spec
                .For(GenExtensions.AlphaLowerChar, c => Char.IsLetter(c) && Char.IsLower(c))
                .Check(Configuration);
        }

        [Test]
        public void AlphaChar()
        {
            Spec
                .For(GenExtensions.AlphaChar, c => Char.IsLetter(c))
                .Check(Configuration);
        }

        [Test]
        public void AlphaNumChar()
        {
            Spec
                .For(GenExtensions.AlphaNumChar, c => Char.IsLetterOrDigit(c))
                .Check(Configuration);
        }

        [Test]
        public void NumStr()
        {
            Spec
                .For(GenExtensions.NumStr, s => s.All(Char.IsDigit))
                .Check(Configuration);
        }

        [Test]
        public void AlphaStr()
        {
            Spec
                .For(GenExtensions.AlphaStr, s => s.All(Char.IsLetterOrDigit))
                .Check(Configuration);
        }

        [Test]
        public void Guid()
        {
            Spec
                .For(GenExtensions.Guid, g =>
                    {
                        var s = g.ToString("N");
                        return s[12] == '4' && s[16] >= '8' && s[16] <= 'b';
                    })
                .Check(Configuration);
        }

        private static Func<Tuple<T1, T2>, TResult> Uncurry<T1, T2, TResult>(Func<T1, T2, TResult> f)
        {
            return tuple => f(tuple.Item1, tuple.Item2);
        }

        private static readonly Gen<PickTuple> GenTuplesForPickValuesTests =
            from l in Any.OfType<int>().MakeList()
            from n in Any.IntBetween(0, l.Count)
            select Tuple.Create(n, l.ToArray());
    }
}
