using System;
using System.Collections.Generic;
using FsCheck;
using Microsoft.FSharp.Core;

namespace FsCheckUtils
{
    /// <summary>
    /// TODO
    /// </summary>
    public static class RegisterExtensions
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T">TODO</typeparam>
        /// <param name="arbitrary">TODO</param>
        public static void Register<T>(this Arbitrary<T> arbitrary)
        {
            ArbitraryWrapper<T>.ArbitraryInstance = arbitrary;
            Arb.register<ArbitraryWrapper<T>>();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T">TODO</typeparam>
        /// <param name="gen">TODO</param>
        public static void Register<T>(this Gen<T> gen)
        {
            var arbitrary = Arb.fromGen(gen);
            ArbitraryWrapper<T>.ArbitraryInstance = arbitrary;
            Arb.register<ArbitraryWrapper<T>>();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T">TODO</typeparam>
        /// <param name="gen">TODO</param>
        /// <param name="shrinker">TODO</param>
        public static void Register<T>(this Gen<T> gen, Func<T, IEnumerable<T>> shrinker)
        {
            var shrinkerFSharpFunc = FSharpFunc<T, IEnumerable<T>>.FromConverter(t => shrinker(t));
            var arbitrary = Arb.fromGenShrink(gen, shrinkerFSharpFunc);
            ArbitraryWrapper<T>.ArbitraryInstance = arbitrary;
            Arb.register<ArbitraryWrapper<T>>();
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class ArbitraryWrapper<T>
        {
            internal static Arbitrary<T> ArbitraryInstance;

            // ReSharper disable once UnusedMember.Local
            public static Arbitrary<T> Arbitrary => ArbitraryInstance;
        }
    }
}
