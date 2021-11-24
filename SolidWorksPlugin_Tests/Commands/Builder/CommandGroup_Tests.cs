namespace SIM.SolidWorksPlugin.Tests.Commands
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SIM.SolidWorksPlugin.Commands;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SW = SolidWorks.Interop.sldworks;

    [TestClass]
    public class CommandGroup_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var commandGroupInfo = new CommandGroupSpec(42, "MyCommandGroup");
            var swCommandManagerMock = new Mock<SW.ICommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();

            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object);

            var cmd = new CommandGroup(swCommandManagerMock.Object, commandGroupInfo);

            Assert.IsNotNull(cmd);
        }

        [TestMethod]
        public void RegisterCommand_Test()
        {
            var commandGroupInfo = new CommandGroupSpec(155, "MyCommandGroup");
            var commandInfo = new CommandSpec(15, "My Command", 1) { Hint = "Hint", Tooltip = "ToolTip", Position = 12, ImageIndex = 5 };
            var swCommandManagerMock = new Mock<SW.ICommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();
            var command = new RelaySwCommand(d => { });

            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object);

            swCommandGroupMock
                .Setup(o => o.AddCommandItem2("My Command", 12, "Hint", "ToolTip", 5, It.IsAny<string>(), It.IsAny<string>(), 15, 3))
                .Returns(42);

            swCommandGroupMock.SetupSet(o => o.HasMenu = true);
            swCommandGroupMock.SetupSet(o => o.HasToolbar = true);

            var cmd = new CommandGroup(swCommandManagerMock.Object, commandGroupInfo);

            var cmdInfo = cmd.AddCommand(commandInfo, command);

            swCommandGroupMock.Verify(o => o.AddCommandItem2("My Command", 12, "Hint", "ToolTip", 5, It.IsAny<string>(), It.IsAny<string>(), 15, 3), Times.Once);

            swCommandGroupMock.VerifySet(o => o.HasMenu = true, Times.Once);
            swCommandGroupMock.VerifySet(o => o.HasToolbar = true, Times.Once);

            Assert.AreEqual(command, cmdInfo.Command);
            Assert.AreEqual(42, cmdInfo.Id);
            Assert.AreEqual(15, cmdInfo.UserId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterCommandInfoNull_Fail()
        {
            var swCommandManagerMock = new Mock<SW.ICommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();

            var commandGroupInfo = new CommandGroupSpec(155, "MyCommandGroup");
            var commandInfo = new CommandSpec(15, "My Command", 1) { Hint = "Hint", Tooltip = "ToolTip", Position = 12, ImageIndex = 5 };
            var command = new RelaySwCommand(d => { });

            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object);

            var cmd = new CommandGroup(swCommandManagerMock.Object, commandGroupInfo);

            var cmdInfo = cmd.AddCommand(null, command);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterCommandCommandNull_Fail()
        {
            var swCommandManagerMock = new Mock<SW.ICommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();

            var commandGroupInfo = new CommandGroupSpec(155, "MyCommandGroup");
            var commandInfo = new CommandSpec(15, "My Command", 1) { Hint = "Hint", Tooltip = "ToolTip", Position = 12, ImageIndex = 5 };
            var command = new RelaySwCommand(d => { });

            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object);

            var cmd = new CommandGroup(swCommandManagerMock.Object, commandGroupInfo);

            var cmdInfo = cmd.AddCommand(commandInfo, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterCommandTwice_Fail()
        {
            var swCommandManagerMock = new Mock<SW.ICommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();

            var commandGroupInfo = new CommandGroupSpec(155, "MyCommandGroup");
            var commandInfo = new CommandSpec(15, "My Command", 1) { Hint = "Hint", Tooltip = "ToolTip", Position = 12, ImageIndex = 5 };
            var command = new RelaySwCommand(d => { });

            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object);

            var cmd = new CommandGroup(swCommandManagerMock.Object, commandGroupInfo);

            var cmdInfo = cmd.AddCommand(commandInfo, command);

            cmd.AddCommand(commandInfo, command);

            cmd.AddCommand(commandInfo, command);
        }

        [TestMethod]
        public void RegisterCommandWithoutMenu_Test()
        {
            var swCommandManagerMock = new Mock<SW.ICommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();

            var commandGroupInfo = new CommandGroupSpec(155, "MyCommandGroup");
            var commandInfo = new CommandSpec(15, "My Command", 1) { Hint = "Hint", Tooltip = "ToolTip", Position = 12, ImageIndex = 5, HasMenu = false };
            var command = new RelaySwCommand(d => { });

            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object);

            swCommandGroupMock
                .Setup(o => o.AddCommandItem2("My Command", 12, "Hint", "ToolTip", 5, It.IsAny<string>(), It.IsAny<string>(), 15, It.IsAny<int>()))
                .Returns(42);

            swCommandGroupMock.SetupSet(o => o.HasMenu = true);
            swCommandGroupMock.SetupSet(o => o.HasToolbar = true);


            var cmd = new CommandGroup(swCommandManagerMock.Object, commandGroupInfo);

            var cmdInfo = cmd.AddCommand(commandInfo, command);

            swCommandGroupMock.VerifySet(o => o.HasMenu = true, Times.Never);
            swCommandGroupMock.VerifySet(o => o.HasToolbar = true, Times.Once);
        }

        [TestMethod]
        public void RegisterCommandWithoutToolbar_Test()
        {
            var swCommandManagerMock = new Mock<SW.ICommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();

            var commandGroupInfo = new CommandGroupSpec(155, "MyCommandGroup");
            var commandInfo = new CommandSpec(15, "My Command", 1) { Hint = "Hint", Tooltip = "ToolTip", Position = 12, ImageIndex = 5, HasToolbar = false };
            var command = new RelaySwCommand(d => { });

            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object);

            swCommandGroupMock
                .Setup(o => o.AddCommandItem2("My Command", 12, "Hint", "ToolTip", 5, It.IsAny<string>(), It.IsAny<string>(), 15, It.IsAny<int>()))
                .Returns(42);

            swCommandGroupMock.SetupSet(o => o.HasMenu = true);
            swCommandGroupMock.SetupSet(o => o.HasToolbar = true);

            var cmd = new CommandGroup(swCommandManagerMock.Object, commandGroupInfo);

            var cmdInfo = cmd.AddCommand(commandInfo, command);

            swCommandGroupMock.VerifySet(o => o.HasMenu = true, Times.Once);
            swCommandGroupMock.VerifySet(o => o.HasToolbar = true, Times.Never);
        }

        [TestMethod]
        public void GetCommand_Test()
        {
            var swCommandManagerMock = new Mock<SW.ICommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();

            var commandGroupInfo = new CommandGroupSpec(155, "MyCommandGroup"); 
            var command = new RelaySwCommand(d => { });
            var commandInfo = new CommandInfo("My Command", command) ;
           
            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object);

            var cmd = new CommandGroup(swCommandManagerMock.Object, commandGroupInfo);

            // get command dictionary
            var dictionary = (Dictionary<int, ICommandInfo>)cmd.GetPrivateObject("registeredCommands");
            dictionary.Add(0, commandInfo);

            Assert.AreEqual(commandInfo, cmd.GetCommand(0));
            Assert.IsNull(cmd.GetCommand(1));
        }

        [TestMethod]
        public void Dispose_Test()
        {
            var swCommandManagerMock = new Mock<SW.ICommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();

            var commandGroupInfo = new CommandGroupSpec(42, "MyCommandGroup");
            var command = new RelaySwCommand(d => { });
            var commandInfo = new CommandInfo("My Command", command);

            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object);

            swCommandManagerMock.Setup(o => o.RemoveCommandGroup2(42, true));

            var cmd = new CommandGroup(swCommandManagerMock.Object, commandGroupInfo);

            var dict = (Dictionary<int, ICommandInfo>)cmd.GetPrivateObject("registeredCommands");
            dict.Add(0, commandInfo);

            cmd.Dispose();

            swCommandManagerMock.Verify(o => o.RemoveCommandGroup2(42, true), Times.Once);
            Assert.AreEqual(0, dict.Count);

            // calling dispose twice should have no effect.
            cmd.Dispose();
            swCommandManagerMock.Verify(o => o.RemoveCommandGroup2(42, true), Times.Once);
        }

        [TestMethod]
        public void GetCallbackNames_Test()
        {
            var swCommandManagerMock = new Mock<SW.ICommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();

            var commandGroupInfo = new CommandGroupSpec(42, "MyCommandGroup");
            var commandInfo = new CommandSpec(15, "My Command", 42) { Hint = "Hint", Tooltip = "ToolTip", Position = 12, ImageIndex = 5, HasToolbar = false };

            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object);

            var cmd = new CommandGroup(swCommandManagerMock.Object, commandGroupInfo);

            var names = ((string OnExecute, string CanExecute))cmd.InvokePrivateMethod("GetCallbackNames", commandInfo);

            Assert.AreEqual($"OnExecute(42:15)", names.OnExecute);
            Assert.AreEqual($"CanExecute(42:15)", names.CanExecute);
        }

        [TestMethod]
        public void Activate_Test()
        {
            var swCommandManagerMock = new Mock<SW.ICommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();

            var commandGroupInfo = new CommandGroupSpec(42, "MyCommandGroup");
            var commandInfo = new CommandSpec(15, "My Command", 1) { Hint = "Hint", Tooltip = "ToolTip", Position = 12, ImageIndex = 5, HasToolbar = false };

            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object);

            swCommandGroupMock.Setup(o => o.Activate());

            var cmd = new CommandGroup(swCommandManagerMock.Object, commandGroupInfo);

            swCommandGroupMock.Verify(o => o.Activate(), Times.Never);

            cmd.Activate();

            swCommandGroupMock.Verify(o => o.Activate(), Times.Once);
        }
    }
}
