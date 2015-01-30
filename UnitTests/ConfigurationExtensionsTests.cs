using FsCheck;
using FsCheckUtils;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ConfigurationExtensionsTests
    {
        [Test]
        public void WithMaxTest()
        {
            var configuration = Config.Default.ToConfiguration().WithMaxTest(123);

            Assert.That(configuration.MaxNbOfTest, Is.EqualTo(123));
            Assert.That(configuration.MaxNbOfFailedTests, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(configuration.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(configuration.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(configuration.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(configuration.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithMaxFail()
        {
            var configuration = Config.Default.ToConfiguration().WithMaxFail(123);

            Assert.That(configuration.MaxNbOfTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(configuration.MaxNbOfFailedTests, Is.EqualTo(123));
            Assert.That(configuration.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(configuration.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(configuration.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(configuration.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithName()
        {
            var configuration = Config.Default.ToConfiguration().WithName("My Configuration");

            Assert.That(configuration.MaxNbOfTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(configuration.MaxNbOfFailedTests, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(configuration.Name, Is.EqualTo("My Configuration"));
            Assert.That(configuration.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(configuration.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(configuration.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithStartSize()
        {
            var configuration = Config.Default.ToConfiguration().WithStartSize(123);

            Assert.That(configuration.MaxNbOfTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(configuration.MaxNbOfFailedTests, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(configuration.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(configuration.StartSize, Is.EqualTo(123));
            Assert.That(configuration.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(configuration.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithEndSize()
        {
            var configuration = Config.Default.ToConfiguration().WithEndSize(123);

            Assert.That(configuration.MaxNbOfTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(configuration.MaxNbOfFailedTests, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(configuration.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(configuration.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(configuration.EndSize, Is.EqualTo(123));
            Assert.That(configuration.Runner, Is.EqualTo(Config.Default.Runner));
        }

        [Test]
        public void WithRunner()
        {
            var runner = new MyRunner();
            var configuration = Config.Default.ToConfiguration().WithRunner(runner);

            Assert.That(configuration.MaxNbOfTest, Is.EqualTo(Config.Default.MaxTest));
            Assert.That(configuration.MaxNbOfFailedTests, Is.EqualTo(Config.Default.MaxFail));
            Assert.That(configuration.Name, Is.EqualTo(Config.Default.Name));
            Assert.That(configuration.StartSize, Is.EqualTo(Config.Default.StartSize));
            Assert.That(configuration.EndSize, Is.EqualTo(Config.Default.EndSize));
            Assert.That(configuration.Runner, Is.EqualTo(runner));
        }
    }
}
