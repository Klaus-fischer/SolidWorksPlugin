namespace SIM.SolidWorksPlugin.Tests.Attributes
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class CommandGroupInfoAttributes_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var cgi = new CommandGroupInfoAttribute(42, "MyCommandGroup");
            Assert.IsNotNull(cgi);
            Assert.AreEqual(42, cgi.CommandGroupId);
            Assert.AreEqual("MyCommandGroup", cgi.Title);
            Assert.IsNotNull(cgi.Hint);
            Assert.IsNotNull(cgi.ToolTip);
            Assert.AreEqual(-1, cgi.Position);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Fail()
        {
            new CommandGroupInfoAttribute(42, null);
        }

        [TestMethod]
        public void ConstructorAndParamterAssignment()
        {
            var cgi = new CommandGroupInfoAttribute(41, "MyCommandGrou1p")
            {
                Hint = "hint",
                Position = 25,
                ToolTip = "tooltip",
            };

            Assert.IsNotNull(cgi);
            Assert.AreEqual(41, cgi.CommandGroupId);
            Assert.AreEqual("MyCommandGrou1p", cgi.Title);
            Assert.AreEqual("hint", cgi.Hint);
            Assert.AreEqual("tooltip", cgi.ToolTip);
            Assert.AreEqual(25, cgi.Position);
        }
    }
}
