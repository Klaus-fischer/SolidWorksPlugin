namespace SIM.SolidWorksPlugin.Tests.Commands
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.swconst;
    using System;

    [TestClass]
    public class CommandSpec_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var commandInfo = new CommandSpec(1, "MyCommand", 2);

            Assert.IsNotNull(commandInfo);

            Assert.AreEqual(1, commandInfo.UserId);
            Assert.AreEqual(2, commandInfo.CommandGroupId);
            Assert.AreEqual("MyCommand", commandInfo.Name);
            Assert.AreEqual(-1, commandInfo.ImageIndex);
            Assert.AreEqual(-1, commandInfo.Position);
        }

        [TestMethod]
        public void ToolTipAndHint_Test()
        {
            var commandInfo = new CommandSpec(1, "MyCommand", 2);

            Assert.AreEqual("MyCommand", commandInfo.Tooltip);
            Assert.AreEqual("MyCommand", commandInfo.Hint);

            commandInfo.Tooltip = "ToolTip";

            Assert.AreEqual("ToolTip", commandInfo.Tooltip);
            Assert.AreEqual("ToolTip", commandInfo.Hint);

            commandInfo.Hint = "Hint";

            Assert.AreEqual("ToolTip", commandInfo.Tooltip);
            Assert.AreEqual("Hint", commandInfo.Hint);
        }

        [TestMethod]
        public void GetSwCommandItemType_e_Test()
        {
            var ci = new CommandSpec(1, "name2", 2) { HasMenu = false, HasToolbar = false, };

            Assert.AreEqual(0, ci.GetSwCommandItemType_e());

            ci = new CommandSpec(1, "name2", 2) { HasMenu = true, HasToolbar = false, };

            Assert.AreEqual((int)swCommandItemType_e.swMenuItem, ci.GetSwCommandItemType_e());

            ci = new CommandSpec(1, "name2", 2) { HasMenu = false, HasToolbar = true, };

            Assert.AreEqual((int)swCommandItemType_e.swToolbarItem, ci.GetSwCommandItemType_e());

            ci = new CommandSpec(1, "name2", 2) { HasMenu = true, HasToolbar = true, };

            Assert.AreEqual((int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem), ci.GetSwCommandItemType_e());
        }
    }
}
