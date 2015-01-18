using System;
using FsCheck;
using Microsoft.FSharp.Core;
using Microsoft.FSharp.Collections;

namespace FsCheckUtils
{
    public static class ConfigExtensions
    {
        public static Config WithMaxTest(this Config config, int maxTest)
        {
            return new Config(
                maxTest,
                config.MaxFail,
                config.Replay,
                config.Name,
                config.StartSize,
                config.EndSize,
                config.Every,
                config.EveryShrink,
                config.Arbitrary,
                config.Runner);
        }

        public static Config WithMaxFail(this Config config, int maxFail)
        {
            return new Config(
                config.MaxTest,
                maxFail,
                config.Replay,
                config.Name,
                config.StartSize,
                config.EndSize,
                config.Every,
                config.EveryShrink,
                config.Arbitrary,
                config.Runner);
        }

        public static Config WithReplay(this Config config, FsCheck.Random.StdGen replay)
        {
            return new Config(
                config.MaxTest,
                config.MaxFail,
                FSharpOption<FsCheck.Random.StdGen>.Some(replay),
                config.Name,
                config.StartSize,
                config.EndSize,
                config.Every,
                config.EveryShrink,
                config.Arbitrary,
                config.Runner);
        }

        public static Config WithNoReplay(this Config config)
        {
            return new Config(
                config.MaxTest,
                config.MaxFail,
                FSharpOption<FsCheck.Random.StdGen>.None,
                config.Name,
                config.StartSize,
                config.EndSize,
                config.Every,
                config.EveryShrink,
                config.Arbitrary,
                config.Runner);
        }

        public static Config WithName(this Config config, string name)
        {
            return new Config(
                config.MaxTest,
                config.MaxFail,
                config.Replay,
                name,
                config.StartSize,
                config.EndSize,
                config.Every,
                config.EveryShrink,
                config.Arbitrary,
                config.Runner);
        }

        public static Config WithStartSize(this Config config, int startSize)
        {
            return new Config(
                config.MaxTest,
                config.MaxFail,
                config.Replay,
                config.Name,
                startSize,
                config.EndSize,
                config.Every,
                config.EveryShrink,
                config.Arbitrary,
                config.Runner);
        }

        public static Config WithEndSize(this Config config, int endSize)
        {
            return new Config(
                config.MaxTest,
                config.MaxFail,
                config.Replay,
                config.Name,
                config.StartSize,
                endSize,
                config.Every,
                config.EveryShrink,
                config.Arbitrary,
                config.Runner);
        }

        public static Config WithEvery(this Config config, FSharpFunc<int, FSharpFunc<FSharpList<object>, string>> every)
        {
            return new Config(
                config.MaxTest,
                config.MaxFail,
                config.Replay,
                config.Name,
                config.StartSize,
                config.EndSize,
                every,
                config.EveryShrink,
                config.Arbitrary,
                config.Runner);
        }

        public static Config WithEveryShrink(this Config config, FSharpFunc<FSharpList<object>, string> everyShrink)
        {
            return new Config(
                config.MaxTest,
                config.MaxFail,
                config.Replay,
                config.Name,
                config.StartSize,
                config.EndSize,
                config.Every,
                everyShrink,
                config.Arbitrary,
                config.Runner);
        }

        public static Config WithArbitrary(this Config config, FSharpList<Type> arbitrary)
        {
            return new Config(
                config.MaxTest,
                config.MaxFail,
                config.Replay,
                config.Name,
                config.StartSize,
                config.EndSize,
                config.Every,
                config.EveryShrink,
                arbitrary,
                config.Runner);
        }

        public static Config WithRunner(this Config config, IRunner runner)
        {
            return new Config(
                config.MaxTest,
                config.MaxFail,
                config.Replay,
                config.Name,
                config.StartSize,
                config.EndSize,
                config.Every,
                config.EveryShrink,
                config.Arbitrary,
                runner);
        }
    }
}
