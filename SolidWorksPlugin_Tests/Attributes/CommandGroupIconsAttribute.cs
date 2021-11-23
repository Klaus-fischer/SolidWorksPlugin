namespace SIM.SolidWorksPlugin.Tests.Attributes
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;

    [TestClass]
    public class CommandGroupIcons_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var cgi = new CommandGroupIconsAttribute();
            Assert.IsNotNull(cgi);
            Assert.IsNotNull(cgi.IconsPath);
            Assert.IsNotNull(cgi.MainIconPath);
        }

        [TestMethod]
        public void ConstructorAndParamterAssignment()
        {
            var cgi = new CommandGroupIconsAttribute()
            {
                IconsPath = @".\Path\To\Icons{0}.png",
                MainIconPath = @".\Path\To\MainIcon{0}.png",
            };

            Assert.IsNotNull(cgi);
            Assert.AreEqual(@".\Path\To\Icons{0}.png", cgi.IconsPath);
            Assert.AreEqual(@".\Path\To\MainIcon{0}.png", cgi.MainIconPath);
        }
    }
}
