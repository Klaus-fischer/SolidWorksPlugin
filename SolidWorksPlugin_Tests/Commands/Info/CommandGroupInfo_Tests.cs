namespace SIM.SolidWorksPlugin.Tests.Commands
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SIM.SolidWorksPlugin.Commands;
    using SolidWorks.Interop.swconst;
    using System;

    [TestClass]
    public class CommandGroupInfo_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var commandInfo = new CommandGroupInfo();

            Assert.IsNotNull(commandInfo);

            Assert.AreEqual(0, commandInfo.UserId);
            Assert.AreEqual(0, commandInfo.ToolbarId);
            Assert.IsNotNull(commandInfo.Title);
        }


        [TestMethod]
        public void ConstructorWithParameter()
        {
            var commandInfo = new CommandGroupInfo()
            {
                Title = "Title",
                ToolbarId = 21,
                UserId = 33,
            };

            Assert.IsNotNull(commandInfo);

            Assert.AreEqual(33, commandInfo.UserId);
            Assert.AreEqual(21, commandInfo.ToolbarId);
            Assert.AreEqual("Title", commandInfo.Title);
        }
    }
}
