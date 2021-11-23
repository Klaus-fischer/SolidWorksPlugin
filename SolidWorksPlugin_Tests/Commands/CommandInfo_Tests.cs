namespace SIM.SolidWorksPlugin.Tests.Commands
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;

    [TestClass]
    public class CommandInfo_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var commandInfo = new CommandInfo(1, "MyCommand", 2);

            Assert.IsNotNull(commandInfo);

            Assert.AreEqual(0, commandInfo.Id);
            Assert.AreEqual(1, commandInfo.UserId);
            Assert.AreEqual(2, commandInfo.CommandGroupId);
            Assert.AreEqual("MyCommand", commandInfo.Name);
            Assert.AreEqual(-1, commandInfo.ImageIndex);
            Assert.AreEqual(-1, commandInfo.Position);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToolTipAndHint_Test()
        {
            var commandInfo = new CommandInfo(1, "MyCommand", 2);

            Assert.AreEqual("MyCommand", commandInfo.Tooltip);
            Assert.AreEqual("MyCommand", commandInfo.Hint);

            commandInfo.Tooltip = "ToolTip";

            Assert.AreEqual("ToolTip", commandInfo.Tooltip);
            Assert.AreEqual("ToolTip", commandInfo.Hint);

            commandInfo.Hint = "Hint";

            Assert.AreEqual("ToolTip", commandInfo.Tooltip);
            Assert.AreEqual("Hint", commandInfo.Hint);
        }
    }
}
