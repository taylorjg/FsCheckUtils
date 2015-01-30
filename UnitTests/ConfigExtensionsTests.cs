using System;
using FsCheck;
using FsCheckUtils;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using NUnit.Framework;
using Random = FsCheck.Random;

namespace UnitTests
{
    [TestFixture]
    public class ConfigExtensionsTests
    {
        [Test]
        public void WithMaxTest()
        {
            var config = Config.Default.WithMaxTest(123);

            Assert.That(config.MaxTest, Is.EqualTo(123));
            Assert.That(config.MaxFail, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(config.Replay, Is.EqualTo(Config.Default.Replay));
            Assert.That(config.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(config.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(config.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(config.Arbitrary, Is.EqualTo(Config.Default.Arbitrary));
            Assert.That(config.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithMaxFail()
        {
            var config = Config.Default.WithMaxFail(123);

            Assert.That(config.MaxTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(config.MaxFail, Is.EqualTo(123));
            Assert.That(config.Replay, Is.EqualTo(Config.Default.Replay));
            Assert.That(config.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(config.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(config.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(config.Arbitrary, Is.EqualTo(Config.Default.Arbitrary));
            Assert.That(config.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithReplay()
        {
            var replay = Random.newSeed();
            var config = Config.Default.WithReplay(replay);

            Assert.That(config.MaxTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(config.MaxFail, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(config.Replay.Value, Is.EqualTo(replay));
            Assert.That(config.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(config.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(config.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(config.Arbitrary, Is.EqualTo(Config.Default.Arbitrary));
            Assert.That(config.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithNoReplay()
        {
            var replay = Random.newSeed();
            var configWithReplay = Config.Default.WithReplay(replay);
            Assert.That(configWithReplay.Replay.Value, Is.EqualTo(replay));

            var config = configWithReplay.WithNoReplay();

            Assert.That(config.MaxTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(config.MaxFail, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(config.Replay, Is.EqualTo(FSharpOption<Random.StdGen>.None));
            Assert.That(config.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(config.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(config.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(config.Arbitrary, Is.EqualTo(Config.Default.Arbitrary));
            Assert.That(config.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithName()
        {
            var config = Config.Default.WithName("My Config");

            Assert.That(config.MaxTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(config.MaxFail, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(config.Replay, Is.EqualTo(Config.Default.Replay));
            Assert.That(config.Name, Is.EqualTo("My Config"));
            Assert.That(config.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(config.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(config.Arbitrary, Is.EqualTo(Config.Default.Arbitrary));
            Assert.That(config.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithStartSize()
        {
            var config = Config.Default.WithStartSize(123);

            Assert.That(config.MaxTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(config.MaxFail, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(config.Replay, Is.EqualTo(Config.Default.Replay));
            Assert.That(config.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(config.StartSize, Is.EqualTo(123));
            Assert.That(config.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(config.Arbitrary, Is.EqualTo(Config.Default.Arbitrary));
            Assert.That(config.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithEndSize()
        {
            var config = Config.Default.WithEndSize(123);

            Assert.That(config.MaxTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(config.MaxFail, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(config.Replay, Is.EqualTo(Config.Default.Replay));
            Assert.That(config.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(config.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(config.EndSize, Is.EqualTo(123));
            Assert.That(config.Arbitrary, Is.EqualTo(Config.Default.Arbitrary));
            Assert.That(config.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithArbitrary()
        {
            var arbitrary = SeqModule.ToList(new[] {typeof (int)});
            var config = Config.Default.WithArbitrary(arbitrary);

            Assert.That(config.MaxTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(config.MaxFail, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(config.Replay, Is.EqualTo(Config.Default.Replay));
            Assert.That(config.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(config.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(config.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(config.Arbitrary, Is.EqualTo(arbitrary));
            Assert.That(config.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithRunner()
        {
            var runner = new MyRunner();
            var config = Config.Default.WithRunner(runner);

            Assert.That(config.MaxTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(config.MaxFail, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(config.Replay, Is.EqualTo(Config.Default.Replay));
            Assert.That(config.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(config.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(config.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(config.Arbitrary, Is.EqualTo(Config.Default.Arbitrary));
            Assert.That(config.Runner, Is.EqualTo(runner));
        }

        private class MyRunner : IRunner
        {
            public void OnStartFixture(Type obj0)
            {
            }

            public void OnArguments(int obj0, FSharpList<object> obj1, FSharpFunc<int, FSharpFunc<FSharpList<object>, string>> obj2)
            {
            }

            public void OnShrink(FSharpList<object> obj0, FSharpFunc<FSharpList<object>, string> obj1)
            {
            }

            public void OnFinished(string obj0, TestResult obj1)
            {
            }
        }
    }
}
