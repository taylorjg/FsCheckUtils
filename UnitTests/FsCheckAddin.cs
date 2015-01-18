using FsCheck.NUnit.Addin;
using NUnit.Core.Extensibility;

namespace UnitTests
{
    [NUnitAddin(Description = "FsCheck addin")]
    public class FsCheckAddin : IAddin
    {
        public bool Install(IExtensionHost host)
        {
            var tcBuilder = new FsCheckTestCaseBuider();
            host.GetExtensionPoint("TestCaseBuilders").Install(tcBuilder);
            return true;
        }
    }
}
