using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.NUnit;
using FsCheck.Fluent;
using FsCheckUtils;

namespace UnitTests
{
    using Property = Gen<Rose<Result>>;

    [NUnit.Framework.TestFixture]
    internal class GenExtensionsTests
    {
        private const int SizeOfSampleValues = 20;
        private const int NumberOfSampleValues = 100;

        [Property]
        public Property PickReturnsListOfCorrectLengthProperty()
        {
            return Spec
                .For(GenPickTestTuple, tuple =>
                {
                    var n = tuple.Item1;
                    var l = tuple.Item2.ToArray();
                    var genPick = GenExtensions.Pick(n, l);
                    var sample = Gen.sample(SizeOfSampleValues, NumberOfSampleValues, genPick);
                    return sample.All(xs => xs.Count == n);
                })
                .Build();
        }

        // PickReturnsListsContainingElementsFromTheInputListProperty

        private static readonly Gen<Tuple<int, List<int>>> GenPickTestTuple =
            from r in Any.OfType<int>()
            from l in Any.OfType<int>().MakeList()
            let n = Math.Abs(r) % (l.Count + 1)
            select Tuple.Create(n, l);
    }
}
