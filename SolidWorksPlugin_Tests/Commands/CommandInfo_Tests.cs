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
            var command = new Mock<ISwCommand>();
            var commandInfo = new CommandInfo(command.Object, 1, 2, "MyCommand", 15, 20);

            Assert.IsNotNull(commandInfo);

            Assert.AreSame(command.Object, commandInfo.Command);
            Assert.AreEqual(1, commandInfo.Id);
            Assert.AreEqual(2, commandInfo.CommandGroupId);
            Assert.AreEqual("MyCommand", commandInfo.Name);
            Assert.AreEqual(15, commandInfo.UserId);
            Assert.AreEqual(20, commandInfo.ImageIndex);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Fail()
        {
            new CommandInfo(null, 1, 2, "MyCommand", 15, 20);
        }
    }
}
