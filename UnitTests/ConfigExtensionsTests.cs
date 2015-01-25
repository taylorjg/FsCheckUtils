using FsCheck;
using FsCheckUtils;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ConfigExtensionsTests
    {
        [Test]
        public void Test1()
        {
            var config = Config.Default
                               .WithMaxTest(1000)
                               .WithName("My Config");
        }
    }
}
