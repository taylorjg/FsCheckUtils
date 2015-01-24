using System;
using FsCheck;
using FsCheck.Fluent;

namespace FsCheckUtils
{
    public static class ConfigurationExtensions
    {
        public static Configuration WithMaxTest(this Configuration configuration, int maxTest)
        {
            configuration.MaxNbOfTest = maxTest;
            return configuration;
        }

        public static Configuration WithMaxFail(this Configuration configuration, int maxFail)
        {
            configuration.MaxNbOfFailedTests = maxFail;
            return configuration;
        }

        public static Configuration WithName(this Configuration configuration, string name)
        {
            configuration.Name = name;
            return configuration;
        }

        public static Configuration WithStartSize(this Configuration configuration, int startSize)
        {
            configuration.StartSize = startSize;
            return configuration;
        }

        public static Configuration WithEndSize(this Configuration configuration, int endSize)
        {
            configuration.EndSize = endSize;
            return configuration;
        }

        public static Configuration WithEvery(this Configuration configuration, Func<int, object[], string> every)
        {
            configuration.Every = every;
            return configuration;
        }

        public static Configuration WithEveryShrink(this Configuration configuration, Func<object[], string> everyShrink)
        {
            configuration.EveryShrink = everyShrink;
            return configuration;
        }

        public static Configuration WithRunner(this Configuration configuration, IRunner runner)
        {
            configuration.Runner = runner;
            return configuration;
        }
    }
}
