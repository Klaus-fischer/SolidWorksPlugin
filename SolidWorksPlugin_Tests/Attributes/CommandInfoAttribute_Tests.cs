namespace SIM.SolidWorksPlugin.Tests.Attributes
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class CommandInfo_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var ci = new CommandInfoAttribute("name");
            Assert.IsNotNull(ci);
            Assert.AreEqual("name", ci.Name);
            Assert.IsNull(ci.Hint);
            Assert.IsNull(ci.Tooltip);
            Assert.IsTrue(ci.HasMenu);
            Assert.IsTrue(ci.HasToolbar);
            Assert.AreEqual(-1, ci.Position);
            Assert.AreEqual(-1, ci.ImageIndex);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Fail()
        {
            new CommandInfoAttribute(null);
        }

        [TestMethod]
        public void ConstructorAndParamterAssignment()
        {
            var ci = new CommandInfoAttribute("name2")
            {
                HasMenu = false,
                HasToolbar = false,
                Hint = "hint",
                ImageIndex = 15,
                Position = 2,
                Tooltip = "tooltip",
            };

            Assert.IsNotNull(ci);
            Assert.AreEqual("name2", ci.Name);
            Assert.AreEqual("hint", ci.Hint);
            Assert.AreEqual("tooltip", ci.Tooltip);
            Assert.IsFalse(ci.HasMenu);
            Assert.IsFalse(ci.HasToolbar);
            Assert.AreEqual(2, ci.Position);
            Assert.AreEqual(15, ci.ImageIndex);
        }
    }
}
