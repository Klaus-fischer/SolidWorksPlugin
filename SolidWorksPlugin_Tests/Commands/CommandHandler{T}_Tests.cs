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
    public class CommandHandler_T_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var commandHandlerMock = new Mock<ICommandHandlerInternals>();
            var swCommandGroupMock = new Mock<CommandGroup>();

            var cmd = new CommandHandler<CommandEnum>(commandHandlerMock.Object, swCommandGroupMock.Object, "Test", 42);

            Assert.IsNotNull(cmd);
        }

        [TestMethod]
        public void RegisterCommand_Test()
        {
            var commandHandlerMock = new Mock<ICommandHandlerInternals>();
            var swCommandGroupMock = new Mock<CommandGroup>();
            var commandMock = new Mock<ISwCommand>();

            swCommandGroupMock
                .Setup(o => o.AddCommandItem2("My Command", 12, "Hint", "ToolTip", 5, It.IsAny<string>(), It.IsAny<string>(), 15, 3))
                .Returns(42);

            swCommandGroupMock.SetupSet(o => o.HasMenu = true);
            swCommandGroupMock.SetupSet(o => o.HasToolbar = true);

            commandHandlerMock.Setup(o => o.GetCallbackNames(CommandEnum.CommandWithoutAttribute)).Returns(("", ""));

            var cmd = new CommandHandler<CommandEnum>(commandHandlerMock.Object, swCommandGroupMock.Object, "Test", 155);

            var cmdInfo = cmd.RegisterCommand(CommandEnum.FirstCommand, commandMock.Object);

            swCommandGroupMock.Verify(o => o.AddCommandItem2("My Command", 12, "Hint", "ToolTip", 5, It.IsAny<string>(), It.IsAny<string>(), 15, 3), Times.Once);

            swCommandGroupMock.VerifySet(o => o.HasMenu = true, Times.Once);
            swCommandGroupMock.VerifySet(o => o.HasToolbar = true, Times.Once);
            Assert.AreEqual(commandMock.Object, cmdInfo.Command);
            Assert.AreEqual(155, cmdInfo.CommandGroupId);
            Assert.AreEqual(42, cmdInfo.Id);
            Assert.AreEqual(5, cmdInfo.ImageIndex);
            Assert.AreEqual("My Command", cmdInfo.Name);
            Assert.AreEqual(15, cmdInfo.UserId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterCommandTwice_Fail()
        {
            var commandHandlerMock = new Mock<ICommandHandlerInternals>();
            var swCommandGroupMock = new Mock<CommandGroup>();
            var commandMock = new Mock<ISwCommand>();

            var cmd = new CommandHandler<CommandEnum>(commandHandlerMock.Object, swCommandGroupMock.Object, "Test", 155);

            cmd.RegisterCommand(CommandEnum.FirstCommand, commandMock.Object);

            cmd.RegisterCommand(CommandEnum.FirstCommand, commandMock.Object);
        }

        [TestMethod]
        public void RegisterCommandWithoutAttributes_Test()
        {
            var commandHandlerMock = new Mock<ICommandHandlerInternals>();
            var swCommandGroupMock = new Mock<CommandGroup>();
            var commandMock = new Mock<ISwCommand>();

            swCommandGroupMock
                .Setup(o => o.AddCommandItem2(It.IsAny<string>(), -1, "", "", -1, It.IsAny<string>(), It.IsAny<string>(), 18, 3))
                .Returns(42);

            swCommandGroupMock.SetupSet(o => o.HasMenu = true);
            swCommandGroupMock.SetupSet(o => o.HasToolbar = true);

            commandHandlerMock.Setup(o => o.GetCallbackNames(It.IsAny<CommandEnum>())).Returns(("", ""));

            var cmd = new CommandHandler<CommandEnum>(commandHandlerMock.Object, swCommandGroupMock.Object, "Test", 155);

            var cmdInfo = cmd.RegisterCommand(CommandEnum.CommandWithoutAttribute, new RelaySwCommand(d => { }));

            swCommandGroupMock.Verify(o => o.AddCommandItem2(It.IsAny<string>(), -1, "", "", -1, It.IsAny<string>(), It.IsAny<string>(), 18, 3), Times.Once);

            swCommandGroupMock.VerifySet(o => o.HasMenu = true, Times.Once);
            swCommandGroupMock.VerifySet(o => o.HasToolbar = true, Times.Once);

            Assert.IsInstanceOfType(cmdInfo.Command, typeof(RelaySwCommand));
            Assert.AreEqual(155, cmdInfo.CommandGroupId);
            Assert.AreEqual(42, cmdInfo.Id);
            Assert.AreEqual(-1, cmdInfo.ImageIndex);
            Assert.AreEqual(nameof(CommandEnum.CommandWithoutAttribute), cmdInfo.Name);
            Assert.AreEqual(18, cmdInfo.UserId);
        }

        [TestMethod]
        public void RegisterCommandWithoutMenu_Test()
        {
            var commandHandlerMock = new Mock<ICommandHandlerInternals>();
            var swCommandGroupMock = new Mock<CommandGroup>();
            var commandMock = new Mock<ISwCommand>();

            swCommandGroupMock
                .Setup(o => o.AddCommandItem2(It.IsAny<string>(), -1, "", "", -1, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(42);

            swCommandGroupMock.SetupSet(o => o.HasMenu = true);
            swCommandGroupMock.SetupSet(o => o.HasToolbar = true);

            commandHandlerMock.Setup(o => o.GetCallbackNames(It.IsAny<CommandEnum>())).Returns(("", ""));

            var cmd = new CommandHandler<CommandEnum>(commandHandlerMock.Object, swCommandGroupMock.Object, "Test", 155);

            var cmdInfo = cmd.RegisterCommand(CommandEnum.CommandWithoutMenu, new RelaySwCommand(d => { }));

            swCommandGroupMock.Verify(o => o.AddCommandItem2(It.IsAny<string>(), -1, "", "", -1, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            swCommandGroupMock.VerifySet(o => o.HasMenu = true, Times.Never);
            swCommandGroupMock.VerifySet(o => o.HasToolbar = true, Times.Once);
        }

        [TestMethod]
        public void RegisterCommandWithoutToolbar_Test()
        {
            var commandHandlerMock = new Mock<ICommandHandlerInternals>();
            var swCommandGroupMock = new Mock<CommandGroup>();
            var commandMock = new Mock<ISwCommand>();

            swCommandGroupMock
                .Setup(o => o.AddCommandItem2(It.IsAny<string>(), -1, "", "", -1, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(42);

            swCommandGroupMock.SetupSet(o => o.HasMenu = true);
            swCommandGroupMock.SetupSet(o => o.HasToolbar = true);

            commandHandlerMock.Setup(o => o.GetCallbackNames(It.IsAny<CommandEnum>())).Returns(("", ""));

            var cmd = new CommandHandler<CommandEnum>(commandHandlerMock.Object, swCommandGroupMock.Object, "Test", 155);

            var cmdInfo = cmd.RegisterCommand(CommandEnum.CommandWithoutToolbar, new RelaySwCommand(d => { }));

            swCommandGroupMock.Verify(o => o.AddCommandItem2(It.IsAny<string>(), -1, "", "", -1, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            swCommandGroupMock.VerifySet(o => o.HasMenu = true, Times.Once);
            swCommandGroupMock.VerifySet(o => o.HasToolbar = true, Times.Never);
        }

        [TestMethod]
        public void AddCommandGroup_Test()
        {
            var commandHandlerMock = new Mock<ICommandHandlerInternals>();
            var swCommandGroupMock = new Mock<CommandGroup>();

            Action<ICommandHandler<CommandEnum>> factoryMethod = d => { };

            var cmd = new CommandHandler<CommandEnum>(commandHandlerMock.Object, swCommandGroupMock.Object, "Test", 42);
            commandHandlerMock.Setup(o => o.AddCommandGroup(factoryMethod, @"Test\"));

            cmd.AddCommandGroup<CommandEnum>(factoryMethod);

            commandHandlerMock.Verify(o => o.AddCommandGroup(factoryMethod, @"Test\"), Times.Once);
        }

        [TestMethod]
        public void GetCommand_Test()
        {
            var commandHandlerMock = new Mock<ICommandHandlerInternals>();
            var swCommandGroupMock = new Mock<CommandGroup>();
            var cmd = new RelaySwCommand(d => { });

            Action<ICommandHandler<CommandEnum>> factoryMethod = d => { };

            var cmdHwnd = new CommandHandler<CommandEnum>(commandHandlerMock.Object, swCommandGroupMock.Object, "Test", 42);

            // get command dictionary
            var dictionary = (Dictionary<CommandEnum, ISwCommand>)cmdHwnd.GetPrivateObject("registeredCommands");
            dictionary.Add(CommandEnum.FirstCommand, cmd);

            Assert.AreEqual(cmd, cmdHwnd.GetCommand(nameof(CommandEnum.FirstCommand)));
            Assert.IsNull(cmdHwnd.GetCommand(nameof(CommandEnum.CommandWithoutToolbar)));
            Assert.IsNull(cmdHwnd.GetCommand("Something"));
        }

        [TestMethod]
        public void Dispose_Test()
        {
            var commandHandlerMock = new Mock<ICommandHandlerInternals>();
            var swCommandManager = new Mock<ICommandManager>();
            var swCommandGroupMock = new Mock<CommandGroup>();

            commandHandlerMock.SetupGet(o => o.SwCommandManager).Returns(swCommandManager.Object);
            swCommandManager.Setup(o => o.RemoveCommandGroup2(42, It.IsAny<bool>()));

            var cmd = new CommandHandler<CommandEnum>(commandHandlerMock.Object, swCommandGroupMock.Object, "Test", 42);

            var dict = (Dictionary<CommandEnum, ISwCommand>)cmd.GetPrivateObject("registeredCommands");
            dict.Add(CommandEnum.FirstCommand, new RelaySwCommand(d => { }));

            cmd.Dispose();

            swCommandManager.Verify(o => o.RemoveCommandGroup2(42, It.IsAny<bool>()), Times.Once);
            Assert.AreEqual(0, dict.Count);

            // calling dispose twice should have no effect.
            cmd.Dispose();
            swCommandManager.Verify(o => o.RemoveCommandGroup2(42, It.IsAny<bool>()), Times.Once);
        }


        public enum CommandEnum
        {
            [CommandInfo("My Command", Hint = "Hint", ImageIndex = 5, Position = 12, Tooltip = "ToolTip")]
            FirstCommand = 15,
            [CommandInfo(nameof(FirstCommand), HasMenu = false)]
            CommandWithoutMenu = 16,
            [CommandInfo(nameof(FirstCommand), HasToolbar = false)]
            CommandWithoutToolbar = 17,
            CommandWithoutAttribute = 18,
        }
    }
}
