using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.Fluent;

namespace FsCheckUtils
{
    public static class GenExtensions
    {
        public static Gen<List<T>> Pick<T>(int n, IReadOnlyCollection<T> l)
        {
            if (n < 0 || n > l.Count) throw new ArgumentOutOfRangeException("n");

            Func<List<T>, List<int>, List<T>> removeItems = (b, idxs) =>
            {
                foreach (var idx in idxs) b.RemoveAt(idx % b.Count);
                return b;
            };

            var numItemsToRemove = l.Count - n;

            return
                from idxs in Any.IntBetween(0, l.Count * 10).MakeListOfLength(numItemsToRemove)
                select removeItems(new List<T>(l), idxs);
        }

        public static Gen<List<T>> Pick<T>(int n, params Gen<T>[] gs)
        {
            var genIdxs = Pick(n, Enumerable.Range(0, gs.Length).ToArray());
            return genIdxs.SelectMany(idxs => Any.SequenceOf(idxs.Select(x => gs[x])));
        }

        public static Gen<List<T>> SomeOf<T>(IReadOnlyCollection<T> l)
        {
            return Gen.choose(0, l.Count).SelectMany(n => Pick(n, l));
        }

        public static Gen<List<T>> SomeOf<T>(params Gen<T>[] gs)
        {
            return Gen.choose(0, gs.Length).SelectMany(n => Pick(n, gs));
        }
    }
}
