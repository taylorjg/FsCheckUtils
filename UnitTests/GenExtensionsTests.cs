using System;
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

        // Add more tests re:
        // PickGenerators
        // SomeOfValues
        // SomeOfGenerators

        [Test]
        public void NumChar()
        {
            Spec
                .For(GenExtensions.NumChar(), c => Char.IsDigit(c))
                .Check(Configuration);
        }

        [Test]
        public void AlphaUpperChar()
        {
            Spec
                .For(GenExtensions.AlphaUpperChar(), c => Char.IsLetter(c) && Char.IsUpper(c))
                .Check(Configuration);
        }

        [Test]
        public void AlphaLowerChar()
        {
            Spec
                .For(GenExtensions.AlphaLowerChar(), c => Char.IsLetter(c) && Char.IsLower(c))
                .Check(Configuration);
        }

        [Test]
        public void AlphaChar()
        {
            Spec
                .For(GenExtensions.AlphaChar(), c => Char.IsLetter(c))
                .Check(Configuration);
        }

        [Test]
        public void AlphaNumChar()
        {
            Spec
                .For(GenExtensions.AlphaNumChar(), c => Char.IsLetterOrDigit(c))
                .Check(Configuration);
        }

        [Test]
        public void NumStr()
        {
            Spec
                .For(GenExtensions.NumStr(), s => s.All(Char.IsDigit))
                .Check(Configuration);
        }

        [Test]
        public void AlphaStr()
        {
            Spec
                .For(GenExtensions.AlphaStr(), s => s.All(Char.IsLetterOrDigit))
                .Check(Configuration);
        }

        [Test]
        public void Guid()
        {
            Spec
                .For(GenExtensions.Guid(), g =>
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
