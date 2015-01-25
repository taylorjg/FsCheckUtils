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
            var genIdxs = Pick(n, (IReadOnlyCollection<int>) Enumerable.Range(0, gs.Length).ToArray());
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

        /// <summary>
        /// Generates a numerical character
        /// </summary>
        /// <returns>A generator that generates a numerical character</returns>
        public static Gen<char> NumChar()
        {
            return from n in Gen.choose('0', '9')
                   select Convert.ToChar(n);
        }

        /// <summary>
        /// Generates an upper-case alpha character
        /// </summary>
        /// <returns>A generator that generates an upper-case alpha character</returns>
        public static Gen<char> AlphaUpperChar()
        {
            return from n in Gen.choose('A', 'Z')
                   select Convert.ToChar(n);
        }

        /// <summary>
        /// Generates a lower-case alpha character
        /// </summary>
        /// <returns>A generator that generates a lower-case alpha character</returns>
        public static Gen<char> AlphaLowerChar()
        {
            return from n in Gen.choose('a', 'z')
                   select Convert.ToChar(n);
        }

        /// <summary>
        /// Generates an alpha character
        /// </summary>
        /// <returns>A generator that generates an alpha character</returns>
        public static Gen<char> AlphaChar()
        {
            return Any.WeighedGeneratorIn(
                new WeightAndValue<Gen<char>>(1, AlphaUpperChar()),
                new WeightAndValue<Gen<char>>(9, AlphaLowerChar()));
        }

        /// <summary>
        /// Generates an alphanumerical character
        /// </summary>
        /// <returns>A generator that generates an alphanumerical character</returns>
        public static Gen<char> AlphaNumChar()
        {
            return Any.WeighedGeneratorIn(
                new WeightAndValue<Gen<char>>(1, NumChar()),
                new WeightAndValue<Gen<char>>(9, AlphaChar()));
        }

        /// <summary>
        /// Generates a string of alpha characters
        /// </summary>
        /// <returns>A generator that generates a string of alpha characters</returns>
        public static Gen<string> AlphaStr()
        {
            return from cs in AlphaChar().MakeList()
                   let s = new string(cs.ToArray())
                   where s.All(Char.IsLetter)
                   select s;
        }

        /// <summary>
        /// Generates a string of digits
        /// </summary>
        /// <returns>A generator that generates a string of digits</returns>
        public static Gen<string> NumStr()
        {
            return from cs in NumChar().MakeList()
                   let s = new string(cs.ToArray())
                   where s.All(Char.IsDigit)
                   select s;
        }
    }
}
