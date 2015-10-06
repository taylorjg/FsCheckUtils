using FsCheck;

namespace FsCheckUtils
{
    /// <summary>
    /// TODO
    /// </summary>
    public static class ArbitraryExtensions
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T">TODO</typeparam>
        /// <param name="arbitrary">TODO</param>
        public static void Register<T>(Arbitrary<T> arbitrary)
        {
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
