using System;
using FsCheck;
using FsCheck.Fluent;
using Microsoft.FSharp.Core;
using Microsoft.FSharp.Collections;

namespace FsCheckUtils
{
    /// <summary>
    /// Extension methods for <see cref="FsCheck.Config" />.
    /// </summary>
    public static class ConfigExtensions
    {
        /// <summary>
        /// Clones a <see cref="FsCheck.Config" /> object but overrides the <see cref="FsCheck.Config.MaxTest" /> property.
        /// </summary>
        /// <param name="config">The <see cref="FsCheck.Config" /> object to clone.</param>
        /// <param name="maxTest">The value to user to override <see cref="FsCheck.Config.MaxTest" /></param>
        /// <returns>A <see cref="FsCheck.Config" /> object.</returns>
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

        /// <summary>
        /// Clones a <see cref="FsCheck.Config" /> object but overrides the <see cref="FsCheck.Config.MaxFail" /> property.
        /// </summary>
        /// <param name="config">The <see cref="FsCheck.Config" /> object to clone.</param>
        /// <param name="maxFail">The value to user to override <see cref="FsCheck.Config.MaxFail" /></param>
        /// <returns>A <see cref="FsCheck.Config" /> object.</returns>
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

        /// <summary>
        /// Clones a <see cref="FsCheck.Config" /> object but overrides the <see cref="FsCheck.Config.Replay" /> property
        /// with a value of Some(<paramref name="replay"/>).
        /// </summary>
        /// <param name="config">The <see cref="FsCheck.Config" /> object to clone.</param>
        /// <param name="replay">The value to user to override <see cref="FsCheck.Config.Replay" /></param>
        /// <returns>A <see cref="FsCheck.Config" /> object.</returns>
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

        /// <summary>
        /// Clones a <see cref="FsCheck.Config" /> object but overrides the <see cref="FsCheck.Config.Replay" /> property
        /// with a value of None.
        /// </summary>
        /// <param name="config">The <see cref="FsCheck.Config" /> object to clone.</param>
        /// <returns>A <see cref="FsCheck.Config" /> object.</returns>
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

        /// <summary>
        /// Clones a <see cref="FsCheck.Config" /> object but overrides the <see cref="FsCheck.Config.Name" /> property.
        /// </summary>
        /// <param name="config">The <see cref="FsCheck.Config" /> object to clone.</param>
        /// <param name="name">The value to user to override <see cref="FsCheck.Config.Name" /></param>
        /// <returns>A <see cref="FsCheck.Config" /> object.</returns>
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

        /// <summary>
        /// Clones a <see cref="FsCheck.Config" /> object but overrides the <see cref="FsCheck.Config.StartSize" /> property.
        /// </summary>
        /// <param name="config">The <see cref="FsCheck.Config" /> object to clone.</param>
        /// <param name="startSize">The value to user to override <see cref="FsCheck.Config.StartSize" /></param>
        /// <returns>A <see cref="FsCheck.Config" /> object.</returns>
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

        /// <summary>
        /// Clones a <see cref="FsCheck.Config" /> object but overrides the <see cref="FsCheck.Config.EndSize" /> property.
        /// </summary>
        /// <param name="config">The <see cref="FsCheck.Config" /> object to clone.</param>
        /// <param name="endSize">The value to user to override <see cref="FsCheck.Config.EndSize" /></param>
        /// <returns>A <see cref="FsCheck.Config" /> object.</returns>
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

        /// <summary>
        /// Clones a <see cref="FsCheck.Config" /> object but overrides the <see cref="FsCheck.Config.Every" /> property.
        /// </summary>
        /// <param name="config">The <see cref="FsCheck.Config" /> object to clone.</param>
        /// <param name="every">The value to user to override <see cref="FsCheck.Config.Every" /></param>
        /// <returns>A <see cref="FsCheck.Config" /> object.</returns>
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

        /// <summary>
        /// Clones a <see cref="FsCheck.Config" /> object but overrides the <see cref="FsCheck.Config.EveryShrink" /> property.
        /// </summary>
        /// <param name="config">The <see cref="FsCheck.Config" /> object to clone.</param>
        /// <param name="everyShrink">The value to user to override <see cref="FsCheck.Config.EveryShrink" /></param>
        /// <returns>A <see cref="FsCheck.Config" /> object.</returns>
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

        /// <summary>
        /// Clones a <see cref="FsCheck.Config" /> object but overrides the <see cref="FsCheck.Config.Arbitrary" /> property.
        /// </summary>
        /// <param name="config">The <see cref="FsCheck.Config" /> object to clone.</param>
        /// <param name="arbitrary">The value to user to override <see cref="FsCheck.Config.Arbitrary" /></param>
        /// <returns>A <see cref="FsCheck.Config" /> object.</returns>
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

        /// <summary>
        /// Clones a <see cref="FsCheck.Config" /> object but overrides the <see cref="FsCheck.Config.Runner" /> property.
        /// </summary>
        /// <param name="config">The <see cref="FsCheck.Config" /> object to clone.</param>
        /// <param name="runner">The value to user to override <see cref="FsCheck.Config.Runner" /></param>
        /// <returns>A <see cref="FsCheck.Config" /> object.</returns>
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

        /// <summary>
        /// Creates a <see cref="FsCheck.Fluent.Configuration" /> object from a <see cref="FsCheck.Config" /> object.
        /// </summary>
        /// <param name="config">The <see cref="FsCheck.Config" /> object to be used to populate the
        /// properties of the new <see cref="FsCheck.Fluent.Configuration" /> object.</param>
        /// <returns>A <see cref="FsCheck.Fluent.Configuration" /> object.</returns>
        public static Configuration ToConfiguration(this Config config)
        {
            return new Configuration
                {
                    MaxNbOfTest = config.MaxTest,
                    MaxNbOfFailedTests = config.MaxFail,
                    Name = config.Name,
                    StartSize = config.StartSize,
                    EndSize = config.EndSize,
                    Every = ConvertEveryToFunc(config.Every),
                    EveryShrink = ConvertEveryShrinkToFunc(config.EveryShrink),
                    Runner = config.Runner
                };
        }

        private static Func<int, object[], string> ConvertEveryToFunc(FSharpFunc<int, FSharpFunc<FSharpList<object>, string>> every)
        {
            return (n, args) => every.Invoke(n).Invoke(ListModule.OfSeq(args));
        }

        private static Func<object[], string> ConvertEveryShrinkToFunc(FSharpFunc<FSharpList<object>, string> everyShrink)
        {
            return args => everyShrink.Invoke(ListModule.OfSeq(args));
        }
    }
}
