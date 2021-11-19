namespace SIM.SolidWorksPlugin.Tests.Extensions
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SIM.SolidWorksPlugin.Extensions;
    using System;
    using System.Linq;
    using Moq;
    using SolidWorks.Interop.sldworks;

    [TestClass]
    public class IModelDoc2Extensions_Tests
    {
        [TestMethod]
        public void GetConfigurationNameStrings_Tests()
        {
            var modelDocMock = new Mock<IModelDoc2>();
            modelDocMock.Setup(o => o.GetConfigurationNames()).Returns(new object[] { "Config a", 1, "config b" });

            var configNames = IModelDoc2Extensions.GetConfigurationNameStrings(modelDocMock.Object);

            Assert.IsNotNull(configNames);
            Assert.IsTrue(configNames.SequenceEqual(new string[] { "Config a", "config b" }));
        }

        [TestMethod]
        public void GetConfigurationNameStrings_Fail()
        {
            var modelDocMock = new Mock<IModelDoc2>();
            modelDocMock.Setup(o => o.GetConfigurationNames()).Returns(null);

            var configNames = IModelDoc2Extensions.GetConfigurationNameStrings(modelDocMock.Object);

            Assert.IsNotNull(configNames);
            Assert.AreEqual(0, configNames.Length);
        }
    }
}
