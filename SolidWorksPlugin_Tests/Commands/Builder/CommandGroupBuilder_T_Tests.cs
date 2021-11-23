namespace SIM.SolidWorksPlugin.Tests.Commands
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SIM.SolidWorksPlugin.Commands;
    using SIM.SolidWorksPlugin.Tests.Documents.DocumentsTypes;
    using SolidWorks.Interop.sldworks;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class CommandGroupBuilder_T_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var commandGroupBuilderMock = new Mock<ICommandGroupBuilder>();

            var cmd = new CommandGroupBuilder<CommandEnum>(commandGroupBuilderMock.Object, 42);

            Assert.IsNotNull(cmd);
        }

        [TestMethod]
        public void AddCommand_Test()
        {
            var commandGroupBuilderMock = new Mock<ICommandGroupBuilder>();
            var command = new RelaySwCommand(d => { });

            commandGroupBuilderMock.Setup(o => o.AddCommand(It.IsAny<CommandInfo>(), It.IsAny<ISwCommand>()))
                .Returns(new CommandInfo(1, "test", 42))
                .Callback((CommandInfo cmdInfo, ISwCommand cmd) =>
                {
                    Assert.AreEqual(command, cmd);
                    Assert.AreEqual(55, cmdInfo.UserId);
                    Assert.AreEqual(42, cmdInfo.CommandGroupId);
                    Assert.AreEqual("CmdTitle", cmdInfo.Name);
                    Assert.AreEqual(15, cmdInfo.ImageIndex);
                    Assert.AreEqual(5, cmdInfo.Position);
                    Assert.AreEqual("Hint", cmdInfo.Hint);
                    Assert.AreEqual("Tooltip", cmdInfo.Tooltip);
                });

            var cmdHwnd = new CommandGroupBuilder<CommandEnum>(commandGroupBuilderMock.Object, 42);

            cmdHwnd.AddCommand(CommandEnum.FirstCommand, command);

            commandGroupBuilderMock.Verify(o => o.AddCommand(It.IsAny<CommandInfo>(), It.IsAny<ISwCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddCommandWithoutAttribute_Test()
        {
            var commandGroupBuilderMock = new Mock<ICommandGroupBuilder>();
            var command = new RelaySwCommand(d => { });

            commandGroupBuilderMock.Setup(o => o.AddCommand(It.IsAny<CommandInfo>(), It.IsAny<ISwCommand>()))
                .Returns(new CommandInfo(1, "test", 42))
                .Callback((CommandInfo cmdInfo, ISwCommand cmd) =>
                {
                    Assert.AreEqual(command, cmd);
                    Assert.AreEqual(56, cmdInfo.UserId);
                    Assert.AreEqual(42, cmdInfo.CommandGroupId);
                    Assert.AreEqual("SecondCommand", cmdInfo.Name);
                    Assert.AreEqual(-1, cmdInfo.ImageIndex);
                    Assert.AreEqual(-1, cmdInfo.Position);
                    Assert.AreEqual("SecondCommand", cmdInfo.Hint);
                    Assert.AreEqual("SecondCommand", cmdInfo.Tooltip);
                });

            var cmdHwnd = new CommandGroupBuilder<CommandEnum>(commandGroupBuilderMock.Object, 42);

            cmdHwnd.AddCommand(CommandEnum.SecondCommand, command);

            commandGroupBuilderMock.Verify(o => o.AddCommand(It.IsAny<CommandInfo>(), It.IsAny<ISwCommand>()), Times.Once);
        }

        public enum CommandEnum
        {
            [CommandInfo("CmdTitle", ImageIndex = 15, Position = 5, Hint = "Hint", Tooltip = "Tooltip")]
            FirstCommand = 55,
            SecondCommand = 56,
        }
    }
}
