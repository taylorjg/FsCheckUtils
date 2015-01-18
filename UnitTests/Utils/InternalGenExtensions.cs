using System;
using FsCheck;

namespace UnitTests.Utils
{
    internal static class InternalGenExtensions
    {
        public static void DumpSamples<T>(this Gen<T> gen, Func<T, string> itemFormatter)
        {
            const int size = 10;
            const int n = 10;
            Console.WriteLine("Sample for generator {0} (size: {1}, n: {2})", gen, size, n);
            var sample = Gen.sample(size, n, gen);
            sample.ForEach((item, index) => Console.WriteLine("Sample[{0}]: {1}", index, itemFormatter(item)));
        }

        public static void DumpSamples<T>(this Gen<T> gen)
        {
            gen.DumpSamples(Formatters.DefaultItemFormatter<T>());
        }
    }
}
