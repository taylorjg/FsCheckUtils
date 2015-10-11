﻿using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.Fluent;

namespace FsCheckUtils
{
    /// <summary>
    /// Additional combinators inspired by ScalaCheck.
    /// </summary>
    public static class GenExtensions
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T">TODO</typeparam>
        /// <param name="source">TODO</param>
        /// <returns>TODO</returns>
        public static Gen<List<T>> Shuffle<T>(params T[] source)
        {
            var n = source.Length;

            if (n < 2) return Gen.constant(source.ToList());

            Action<List<T>, Tuple<int, int>> doExchange = (list, epr) =>
            {
                var tmp = list[epr.Item2];
                list[epr.Item2] = list[epr.Item1];
                list[epr.Item1] = tmp;
            };

            Func<IEnumerable<Tuple<int, int>>, List<T>> doExchanges = eprs =>
            {
                var copyOfOriginalList = new List<T>(source);
                foreach (var epr in eprs) doExchange(copyOfOriginalList, epr);
                return copyOfOriginalList;
            };

            // Fischer-Yates/Knuth shuffle
            // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle

            var indices = Enumerable.Range(1, n - 1).Reverse();
            var exchangePairGens = indices.Select(i => (Any.IntBetween(0, i).Select(j => Tuple.Create(i, j))));
            var genExchangePairs = Any.SequenceOf(exchangePairGens);
            return genExchangePairs.Select(doExchanges);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T">TODO</typeparam>
        /// <param name="source">TODO</param>
        /// <returns>TODO</returns>
        public static Gen<List<T>> Shuffle<T>(IEnumerable<T> source)
        {
            return Shuffle(source.ToArray());
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T">TODO</typeparam>
        /// <param name="gs">TODO</param>
        /// <returns>TODO</returns>
        public static Gen<List<T>> ShuffleGenerators<T>(params Gen<T>[] gs)
        {
            return Shuffle(gs).SelectMany(Any.SequenceOf);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T">TODO</typeparam>
        /// <param name="gs">TODO</param>
        /// <returns>TODO</returns>
        public static Gen<List<T>> ShuffleGenerators<T>(IEnumerable<Gen<T>> gs)
        {
            return ShuffleGenerators(gs.ToArray());
        }

        /// <summary>
        /// A generator that picks a given number of elements from a list, randomly.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <param name="n">The number of items to pick from the list.</param>
        /// <param name="l">The list of elements to pick from.</param>
        /// <returns>A generator that generates a given number of elements from a list, randomly.</returns>
        public static Gen<List<T>> PickValues<T>(int n, params T[] l)
        {
            if (n < 0 || n > l.Length) throw new ArgumentOutOfRangeException("n");

            Func<List<T>, List<int>, List<T>> removeItems = (b, idxs) =>
            {
                foreach (var idx in idxs) b.RemoveAt(idx % b.Count);
                return b;
            };

            var numItemsToRemove = l.Length - n;

            return
                from idxs in Any.IntBetween(0, l.Length * 10).MakeListOfLength(numItemsToRemove)
                select removeItems(new List<T>(l), idxs);
        }

        /// <summary>
        /// A generator that picks a given number of elements from a list, randomly.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <param name="n">The number of items to pick from the list.</param>
        /// <param name="l">The list of elements to pick from.</param>
        /// <returns>A generator that generates a given number of elements from a list, randomly.</returns>
        public static Gen<List<T>> PickValues<T>(int n, IEnumerable<T> l)
        {
            return PickValues(n, l.ToArray());
        }

        /// <summary>
        /// A generator that picks a given number of elements from a list of generators, randomly.
        /// </summary>
        /// <typeparam name="T">The type generated by the generators in the list of generators.</typeparam>
        /// <param name="n">The number of items to pick from the list.</param>
        /// <param name="gs">The list of generators to pick from.</param>
        /// <returns>A generator that generates a given number of elements from a list of generators, randomly.</returns>
        public static Gen<List<T>> PickGenerators<T>(int n, params Gen<T>[] gs)
        {
            var genIdxs = PickValues(n, Enumerable.Range(0, gs.Length));
            return genIdxs.SelectMany(idxs => Any.SequenceOf(idxs.Select(x => gs[x])));
        }

        /// <summary>
        /// A generator that picks a given number of elements from a list of generators, randomly.
        /// </summary>
        /// <typeparam name="T">The type generated by the generators in the list of generators.</typeparam>
        /// <param name="n">The number of items to pick from the list.</param>
        /// <param name="gs">The list of generators to pick from.</param>
        /// <returns>A generator that generates a given number of elements from a list of generators, randomly.</returns>
        public static Gen<List<T>> PickGenerators<T>(int n, IEnumerable<Gen<T>> gs)
        {
            return PickGenerators(n, gs.ToArray());
        }

        /// <summary>
        /// A generator that picks a random number of elements from a list.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <param name="l">The list of elements to pick from.</param>
        /// <returns>A generator that generates a random number of elements from a list.</returns>
        public static Gen<List<T>> SomeOfValues<T>(params T[] l)
        {
            return Gen.choose(0, l.Length).SelectMany(n => PickValues(n, l));
        }

        /// <summary>
        /// A generator that picks a random number of elements from a list.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <param name="l">The list of elements to pick from.</param>
        /// <returns>A generator that generates a random number of elements from a list.</returns>
        public static Gen<List<T>> SomeOfValues<T>(IEnumerable<T> l)
        {
            return SomeOfValues(l.ToArray());
        }

        /// <summary>
        /// A generator that picks a random number of elements from a list of generators.
        /// </summary>
        /// <typeparam name="T">The type generated by the generators in the list of generators.</typeparam>
        /// <param name="gs">The list of generators to pick from.</param>
        /// <returns>A generator that generates a random number of elements from a list of generators.</returns>
        public static Gen<List<T>> SomeOfGenerators<T>(params Gen<T>[] gs)
        {
            return Gen.choose(0, gs.Length).SelectMany(n => PickGenerators(n, gs));
        }

        /// <summary>
        /// A generator that picks a random number of elements from a list of generators.
        /// </summary>
        /// <typeparam name="T">The type generated by the generators in the list of generators.</typeparam>
        /// <param name="gs">The list of generators to pick from.</param>
        /// <returns>A generator that generates a random number of elements from a list of generators.</returns>
        public static Gen<List<T>> SomeOfGenerators<T>(IEnumerable<Gen<T>> gs)
        {
            return SomeOfGenerators(gs.ToArray());
        }

        /// <summary>
        /// Create a generator that calls <paramref name="gen" /> repeatedly until
        /// the given condition is fulfilled. The generated value is then
        /// returned. Use this combinator with care, since it may result
        /// in infinite loops.
        /// </summary>
        /// <typeparam name="T">The type of elements generated by <paramref name="gen" />.</typeparam>
        /// <param name="gen">The generator.</param>
        /// <param name="p">The condition to be fulfilled.</param>
        /// <returns>A generator that calls <paramref name="gen" /> repeatedly until
        /// the given condition is fulfilled.</returns>
        public static Gen<T> RetryUntil<T>(this Gen<T> gen, Func<T, bool> p)
        {
            return gen.Where(p);
        }

        /// <summary>
        /// Combines the given generators into one generator that produces a
        /// tuple of their generated values.
        /// </summary>
        /// <typeparam name="T1">The type of elements generated by <paramref name="g1" />.</typeparam>
        /// <typeparam name="T2">The type of elements generated by <paramref name="g2" />.</typeparam>
        /// <param name="g1">The first generator.</param>
        /// <param name="g2">The second generator.</param>
        /// <returns>A generator that Combines the given generators into one generator that produces a
        /// tuple of their generated values.</returns>
        public static Gen<Tuple<T1, T2>> Zip<T1, T2>(Gen<T1> g1, Gen<T2> g2)
        {
            return from t1 in g1
                   from t2 in g2
                   select Tuple.Create(t1, t2);
        }

        /// <summary>
        /// Combines the given generators into one generator that produces a
        /// tuple of their generated values.
        /// </summary>
        /// <typeparam name="T1">The type of elements generated by <paramref name="g1" />.</typeparam>
        /// <typeparam name="T2">The type of elements generated by <paramref name="g2" />.</typeparam>
        /// <typeparam name="T3">The type of elements generated by <paramref name="g3" />.</typeparam>
        /// <param name="g1">The first generator.</param>
        /// <param name="g2">The second generator.</param>
        /// <param name="g3">The third generator.</param>
        /// <returns>A generator that Combines the given generators into one generator that produces a
        /// tuple of their generated values.</returns>
        public static Gen<Tuple<T1, T2, T3>> Zip<T1, T2, T3>(Gen<T1> g1, Gen<T2> g2, Gen<T3> g3)
        {
            return from t1 in g1
                   from t2 in g2
                   from t3 in g3
                   select Tuple.Create(t1, t2, t3);
        }

        /// <summary>
        /// Combines the given generators into one generator that produces a
        /// tuple of their generated values.
        /// </summary>
        /// <typeparam name="T1">The type of elements generated by <paramref name="g1" />.</typeparam>
        /// <typeparam name="T2">The type of elements generated by <paramref name="g2" />.</typeparam>
        /// <typeparam name="T3">The type of elements generated by <paramref name="g3" />.</typeparam>
        /// <typeparam name="T4">The type of elements generated by <paramref name="g4" />.</typeparam>
        /// <param name="g1">The first generator.</param>
        /// <param name="g2">The second generator.</param>
        /// <param name="g3">The third generator.</param>
        /// <param name="g4">The fourth generator.</param>
        /// <returns>A generator that Combines the given generators into one generator that produces a
        /// tuple of their generated values.</returns>
        public static Gen<Tuple<T1, T2, T3, T4>> Zip<T1, T2, T3, T4>(Gen<T1> g1, Gen<T2> g2, Gen<T3> g3, Gen<T4> g4)
        {
            return from t1 in g1
                   from t2 in g2
                   from t3 in g3
                   from t4 in g4
                   select Tuple.Create(t1, t2, t3, t4);
        }

        /// <summary>
        /// Combines the given generators into one generator that produces a
        /// tuple of their generated values.
        /// </summary>
        /// <typeparam name="T1">The type of elements generated by <paramref name="g1" />.</typeparam>
        /// <typeparam name="T2">The type of elements generated by <paramref name="g2" />.</typeparam>
        /// <typeparam name="T3">The type of elements generated by <paramref name="g3" />.</typeparam>
        /// <typeparam name="T4">The type of elements generated by <paramref name="g4" />.</typeparam>
        /// <typeparam name="T5">The type of elements generated by <paramref name="g5" />.</typeparam>
        /// <param name="g1">The first generator.</param>
        /// <param name="g2">The second generator.</param>
        /// <param name="g3">The third generator.</param>
        /// <param name="g4">The fourth generator.</param>
        /// <param name="g5">The fifth generator.</param>
        /// <returns>A generator that Combines the given generators into one generator that produces a
        /// tuple of their generated values.</returns>
        public static Gen<Tuple<T1, T2, T3, T4, T5>> Zip<T1, T2, T3, T4, T5>(Gen<T1> g1, Gen<T2> g2, Gen<T3> g3, Gen<T4> g4, Gen<T5> g5)
        {
            return from t1 in g1
                   from t2 in g2
                   from t3 in g3
                   from t4 in g4
                   from t5 in g5
                   select Tuple.Create(t1, t2, t3, t4, t5);
        }

        /// <summary>
        /// Combines the given generators into one generator that produces a
        /// tuple of their generated values.
        /// </summary>
        /// <typeparam name="T1">The type of elements generated by <paramref name="g1" />.</typeparam>
        /// <typeparam name="T2">The type of elements generated by <paramref name="g2" />.</typeparam>
        /// <typeparam name="T3">The type of elements generated by <paramref name="g3" />.</typeparam>
        /// <typeparam name="T4">The type of elements generated by <paramref name="g4" />.</typeparam>
        /// <typeparam name="T5">The type of elements generated by <paramref name="g5" />.</typeparam>
        /// <typeparam name="T6">The type of elements generated by <paramref name="g6" />.</typeparam>
        /// <param name="g1">The first generator.</param>
        /// <param name="g2">The second generator.</param>
        /// <param name="g3">The third generator.</param>
        /// <param name="g4">The fourth generator.</param>
        /// <param name="g5">The fifth generator.</param>
        /// <param name="g6">The sixth generator.</param>
        /// <returns>A generator that Combines the given generators into one generator that produces a
        /// tuple of their generated values.</returns>
        public static Gen<Tuple<T1, T2, T3, T4, T5, T6>> Zip<T1, T2, T3, T4, T5, T6>(Gen<T1> g1, Gen<T2> g2, Gen<T3> g3, Gen<T4> g4, Gen<T5> g5, Gen<T6> g6)
        {
            return from t1 in g1
                   from t2 in g2
                   from t3 in g3
                   from t4 in g4
                   from t5 in g5
                   from t6 in g6
                   select Tuple.Create(t1, t2, t3, t4, t5, t6);
        }

        /// <summary>
        /// Combines the given generators into one generator that produces a
        /// tuple of their generated values.
        /// </summary>
        /// <typeparam name="T1">The type of elements generated by <paramref name="g1" />.</typeparam>
        /// <typeparam name="T2">The type of elements generated by <paramref name="g2" />.</typeparam>
        /// <typeparam name="T3">The type of elements generated by <paramref name="g3" />.</typeparam>
        /// <typeparam name="T4">The type of elements generated by <paramref name="g4" />.</typeparam>
        /// <typeparam name="T5">The type of elements generated by <paramref name="g5" />.</typeparam>
        /// <typeparam name="T6">The type of elements generated by <paramref name="g6" />.</typeparam>
        /// <typeparam name="T7">The type of elements generated by <paramref name="g7" />.</typeparam>
        /// <param name="g1">The first generator.</param>
        /// <param name="g2">The second generator.</param>
        /// <param name="g3">The third generator.</param>
        /// <param name="g4">The fourth generator.</param>
        /// <param name="g5">The fifth generator.</param>
        /// <param name="g6">The sixth generator.</param>
        /// <param name="g7">The seventh generator.</param>
        /// <returns>A generator that Combines the given generators into one generator that produces a
        /// tuple of their generated values.</returns>
        public static Gen<Tuple<T1, T2, T3, T4, T5, T6, T7>> Zip<T1, T2, T3, T4, T5, T6, T7>(Gen<T1> g1, Gen<T2> g2, Gen<T3> g3, Gen<T4> g4, Gen<T5> g5, Gen<T6> g6, Gen<T7> g7)
        {
            return from t1 in g1
                   from t2 in g2
                   from t3 in g3
                   from t4 in g4
                   from t5 in g5
                   from t6 in g6
                   from t7 in g7
                   select Tuple.Create(t1, t2, t3, t4, t5, t6, t7);
        }

        /// <summary>
        /// Generates a numerical character.
        /// </summary>
        /// <returns>A generator that generates a numerical character.</returns>
        public static Gen<char> NumChar
        {
            get
            {
                return from n in Gen.choose('0', '9')
                       select Convert.ToChar(n);
            }
        }

        /// <summary>
        /// Generates an upper-case alpha character.
        /// </summary>
        /// <returns>A generator that generates an upper-case alpha character.</returns>
        public static Gen<char> AlphaUpperChar
        {
            get
            {
                return from n in Gen.choose('A', 'Z')
                       select Convert.ToChar(n);
            }
        }

        /// <summary>
        /// Generates a lower-case alpha character.
        /// </summary>
        /// <returns>A generator that generates a lower-case alpha character.</returns>
        public static Gen<char> AlphaLowerChar
        {
            get
            {
                return from n in Gen.choose('a', 'z')
                       select Convert.ToChar(n);
            }
        }

        /// <summary>
        /// Generates an alpha character.
        /// </summary>
        /// <returns>A generator that generates an alpha character.</returns>
        public static Gen<char> AlphaChar
        {
            get
            {
                return Any.WeighedGeneratorIn(
                    new WeightAndValue<Gen<char>>(1, AlphaUpperChar),
                    new WeightAndValue<Gen<char>>(9, AlphaLowerChar));
            }
        }

        /// <summary>
        /// Generates an alphanumerical character.
        /// </summary>
        /// <returns>A generator that generates an alphanumerical character.</returns>
        public static Gen<char> AlphaNumChar
        {
            get
            {
                return Any.WeighedGeneratorIn(
                    new WeightAndValue<Gen<char>>(1, NumChar),
                    new WeightAndValue<Gen<char>>(9, AlphaChar));
            }
        }

        /// <summary>
        /// Generates a string of alpha characters.
        /// </summary>
        /// <returns>A generator that generates a string of alpha characters.</returns>
        public static Gen<string> AlphaStr
        {
            get
            {
                return from cs in AlphaChar.MakeList()
                       let s = new string(cs.ToArray())
                       where s.All(Char.IsLetter)
                       select s;
            }
        }

        /// <summary>
        /// Generates a string of digits.
        /// </summary>
        /// <returns>A generator that generates a string of digits.</returns>
        public static Gen<string> NumStr
        {
            get
            {
                return from cs in NumChar.MakeList()
                       let s = new string(cs.ToArray())
                       where s.All(Char.IsDigit)
                       select s;
            }
        }

        /// <summary>
        /// Generates a string that starts with a lower-case alpha character,
        /// and only contains alphanumerical characters.
        /// </summary>
        /// <returns>A generator that generates an identifier.</returns>
        public static Gen<string> Identifier
        {
            get
            {
                return from c in AlphaLowerChar
                       from cs in AlphaNumChar.MakeList()
                       let s = new string(new[] { c }.Concat(cs).ToArray())
                       where s.All(Char.IsLetterOrDigit)
                       select s;
            }
        }

        /// <summary>
        /// Generates a version 4 (random) UUID.
        /// See <a href="http://en.wikipedia.org/wiki/Universally_unique_identifier#Version_4_.28random.29">this link</a> for more information.
        /// </summary>
        /// <returns>A generator that generates a version 4 (random) UUID.</returns>
        public static Gen<Guid> Guid
        {
            get
            {
                return from l1Upper in Gen.choose(0, int.MaxValue)
                    from l1Lower in Gen.choose(0, int.MaxValue)
                    from l2Upper in Gen.choose(0, int.MaxValue)
                    from l2Lower in Gen.choose(0, int.MaxValue)
                    let l1 = ((long) l1Upper << 32) + l1Lower
                    let l2 = ((long) l2Upper << 32) + l2Lower
                    from y in Gen.elements(new[] {'8', '9', 'a', 'b'})
                    select MakeGuidFromBits(l1, l2, y);
            }
        }

        private static Guid MakeGuidFromBits(long l1, long l2, char y)
        {
            var s1 = l1.ToString("X16") + l2.ToString("X16");
            var chars = s1.ToCharArray();
            chars[12] = '4';
            chars[16] = y;
            var s2 = new string(chars);
            return System.Guid.Parse(s2);
        }
    }
}
