namespace SIM.SolidWorksPlugin.Tests.Commands
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SIM.SolidWorksPlugin.Commands;

    [TestClass]
    public class CommandInfo_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var command = new RelaySwCommand(d => { });
            var commandInfo = new CommandInfo("MyCommand", command);

            Assert.IsNotNull(commandInfo);

            Assert.AreEqual(0, commandInfo.UserId);
            Assert.AreEqual(0, commandInfo.Id);
            Assert.AreEqual("MyCommand", commandInfo.Name);
            Assert.AreSame(command, commandInfo.Command);
        }


        [TestMethod]
        public void ConstructorWithParameter()
        {
            var command = new RelaySwCommand(d => { });
            var commandInfo = new CommandInfo("MyCommand", command)
            {
                Id = 200,
                UserId = 12,
            };

            Assert.IsNotNull(commandInfo);

            Assert.AreEqual(12, commandInfo.UserId);
            Assert.AreEqual(200, commandInfo.Id);
            Assert.AreEqual("MyCommand", commandInfo.Name);
            Assert.AreSame(command, commandInfo.Command);
        }
    }
}
