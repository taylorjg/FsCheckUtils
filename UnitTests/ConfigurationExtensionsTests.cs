using FsCheck;
using FsCheckUtils;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ConfigurationExtensionsTests
    {
        [Test]
        public void Test1()
        {
            var configuration = Config.Default
                                      .ToConfiguration()
                                      .WithMaxTest(1000)
                                      .WithName("My Configuration");
        }
    }
}
