namespace SIM.SolidWorksPlugin.Tests.Commands
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SIM.SolidWorksPlugin.Tests.Documents.DocumentsTypes;
    using SW = SolidWorks.Interop.sldworks;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class CommandHandler_Tests
    {

        [TestMethod]
        public void Constructor()
        {
            var swApplicationMock = new Mock<SW.ISldWorks>();

            var cmd = new CommandHandler(swApplicationMock.Object, null, new Cookie(42));

            swApplicationMock.Setup(o => o.GetCommandManager(42)).Returns<SW.ICommandManager>(null);
            swApplicationMock.Setup(o => o.SetAddinCallbackInfo2(0, cmd, 42));

            Assert.IsNotNull(cmd);

            swApplicationMock.Verify(o => o.GetCommandManager(42), Times.AtLeastOnce);
            swApplicationMock.Verify(o => o.SetAddinCallbackInfo2(0, cmd, 42), Times.AtLeastOnce);
        }

        [TestMethod]
        public void AddCommandGroup_Test()
        {
            var swApplicationMock = new Mock<SW.ISldWorks>();
            var swCommandManagerMock = new Mock<SW.CommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();
            var documentManagerMock = new Mock<IDocumentManager>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), true, ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object)
                .Callback(new CreateCommandGroup2Callback((int userId, string title, string tooltip, string hint, int position, bool ignore, ref int errors) =>
                {
                    Assert.AreEqual(1, userId);
                    Assert.AreEqual("commands", title);
                    Assert.AreEqual("group hint", hint);
                    Assert.AreEqual("group tooltip", tooltip);
                    Assert.AreEqual(15, position);
                }));

            swCommandGroupMock.Setup(o => o.Activate()).Returns(true);
            swCommandGroupMock.SetupSet(o => o.IconList = It.IsAny<string[]>());
            swCommandGroupMock.SetupSet(o => o.MainIconList = It.IsAny<string[]>());

            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            bool callbackInvoked = false;
            cmd.AddCommandGroup(new CommandGroupSpec(1, "commands")
            {
                Hint = "group hint",
                Tooltip = "group tooltip",
                Position = 15,
            }, o => callbackInvoked = true);

            Assert.IsTrue(callbackInvoked);
            swApplicationMock.Verify(o => o.GetCommandManager(It.IsAny<int>()), Times.AtLeastOnce);
            swCommandGroupMock.Verify(o => o.Activate(), Times.AtLeastOnce());

            swCommandGroupMock.VerifySet(o => o.IconList = It.IsAny<string[]>(), Times.Never);
            swCommandGroupMock.VerifySet(o => o.MainIconList = It.IsAny<string[]>(), Times.Never);
        }

        [TestMethod]
        public void AddCommandGroupWithIcons_Test()
        {
            var swApplicationMock = new Mock<SW.ISldWorks>();
            var swCommandManagerMock = new Mock<SW.CommandManager>();
            var swCommandGroupMock = new Mock<SW.CommandGroup>();
            var documentManagerMock = new Mock<IDocumentManager>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), true, ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object);

            swCommandGroupMock.Setup(o => o.Activate()).Returns(true);
            swCommandGroupMock.SetupSet(o => o.IconList = It.IsAny<string[]>());
            swCommandGroupMock.SetupSet(o => o.MainIconList = It.IsAny<string[]>());

            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            bool callbackInvoked = false;
            cmd.AddCommandGroup(
                new CommandGroupSpec(0, "CommandGroup")
                {
                    IconsPath = "./icons{0}.png",
                    MainIconPath = "./mainicon{0}.png",
                },
                o => callbackInvoked = true);

            Assert.IsTrue(callbackInvoked);
            swApplicationMock.Verify(o => o.GetCommandManager(It.IsAny<int>()), Times.AtLeastOnce);
            swCommandGroupMock.Verify(o => o.Activate(), Times.AtLeastOnce());

            swCommandGroupMock.VerifySet(o => o.IconList = It.IsAny<string[]>(), Times.AtLeastOnce);
            swCommandGroupMock.VerifySet(o => o.MainIconList = It.IsAny<string[]>(), Times.AtLeastOnce);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddCommandGroup_Fail()
        {
            var swApplicationMock = new Mock<SW.ISldWorks>();
            var swCommandManagerMock = new Mock<SW.CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);

            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            var dictionary = (Dictionary<int, ICommandGroup>)cmd.GetPrivateObject("commandHandlers")!;
            dictionary.Add(0, null);

            cmd.AddCommandGroup(new CommandGroupSpec(0, ""), o => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddCommandGroup_Fail2()
        {
            var swApplicationMock = new Mock<SW.ISldWorks>();
            var swCommandManagerMock = new Mock<SW.CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);

            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            cmd.AddCommandGroup(null, o => { });
        }

        [TestMethod]
        public void CanExecute_Test()
        {
            var swApplicationMock = new Mock<SW.ISldWorks>();
            var swCommandManagerMock = new Mock<SW.CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();
            var commandHandlerMock = new Mock<ICommandGroup>();
            var commandInfoMock = new Mock<ICommandInfo>();
            var commandMock = new Mock<ISwCommand>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);

            commandHandlerMock.Setup(o => o.GetCommand(It.IsAny<int>())).Returns<ICommandInfo>(null);
            commandHandlerMock.Setup(o => o.GetCommand(0)).Returns(commandInfoMock.Object);

            commandInfoMock.SetupGet(o => o.Command).Returns(commandMock.Object);
            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(false);

            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // get command handler dictionary
            var dictionary = (Dictionary<int, ICommandGroup>)cmd.GetPrivateObject("commandHandlers")!;

            // add mock entry to handler collection.
            dictionary.Add(0, commandHandlerMock.Object);

            // execute empty string, should return disabled.
            var result = cmd.CanExecute("NoColon");
            Assert.AreEqual((int)CommandCanExecuteState.Disabled, result);
            commandHandlerMock.Verify(o => o.GetCommand(It.IsAny<int>()), Times.Never);

            // invoke can execute with unknown command should return disabled.
            result = cmd.CanExecute("NoInteger:0");
            Assert.AreEqual((int)CommandCanExecuteState.Disabled, result);

            // invoke can execute with unknown command should return disabled.
            result = cmd.CanExecute("0:NoInteger");
            Assert.AreEqual((int)CommandCanExecuteState.Disabled, result);

            // invoke can execute with unknown command should return disabled.
            result = cmd.CanExecute("0:15");
            Assert.AreEqual((int)CommandCanExecuteState.Disabled, result);

            // verify invocation
            commandHandlerMock.Verify(o => o.GetCommand(0), Times.Never);
            commandMock.Verify(o => o.CanExecute(It.IsAny<SwDocument?>()), Times.Never);

            // invoke can execute command should return disabled by return value.
            result = cmd.CanExecute("0:0");
            Assert.AreEqual((int)CommandCanExecuteState.Disabled, result);

            // verify invocation
            commandHandlerMock.Verify(o => o.GetCommand(0), Times.Once);
            commandMock.Verify(o => o.CanExecute(It.IsAny<SwDocument?>()), Times.Once);

            // change setup of mock
            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(true);

            // invoke can execute command should return enabled by return value.
            result = cmd.CanExecute("0:0");
            Assert.AreEqual((int)CommandCanExecuteState.Enabled, result);
        }

        [TestMethod]
        public void CanExecuteToggleCommand_Test()
        {
            var swApplicationMock = new Mock<SW.ISldWorks>();
            var swCommandManagerMock = new Mock<SW.CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();
            var commandHandlerMock = new Mock<ICommandGroup>();
            var commandInfoMock = new Mock<ICommandInfo>();
            var commandMock = new Mock<IToggleCommand>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            commandHandlerMock.Setup(o => o.GetCommand(It.IsAny<int>())).Returns(commandInfoMock.Object);
            commandInfoMock.SetupGet(o => o.Command).Returns(commandMock.Object);

            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // get command handler dictionary
            var dictionary = (Dictionary<int, ICommandGroup>)cmd.GetPrivateObject("commandHandlers");

            // add mock entry to handler collection.
            dictionary.Add(0, commandHandlerMock.Object);

            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(false);
            commandMock.SetupGet(o => o.IsActive).Returns(false);

            // invoke can execute with unknown command should return disabled.
            var result = cmd.CanExecute("0:0");
            Assert.AreEqual((int)CommandCanExecuteState.Disabled, result);

            // reconfigure
            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(true);

            // invoke can execute with unknown command should return disabled.
            result = cmd.CanExecute("0:0");
            Assert.AreEqual((int)CommandCanExecuteState.Enabled, result);

            // reconfigure
            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(false);
            commandMock.SetupGet(o => o.IsActive).Returns(true);

            // invoke can execute with unknown command should return disabled.
            result = cmd.CanExecute("0:0");
            Assert.AreEqual((int)CommandCanExecuteState.Selected, result);

            // reconfigure
            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(true);

            // invoke can execute with unknown command should return disabled.
            result = cmd.CanExecute("0:0");
            Assert.AreEqual((int)CommandCanExecuteState.SelectedAndEnabled, result);
        }

        [TestMethod]
        public void CanExecuteActiveDoc_Test()
        {
            var swApplicationMock = new Mock<SW.ISldWorks>();
            var swCommandManagerMock = new Mock<SW.CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();
            var commandHandlerMock = new Mock<ICommandGroup>();
            var commandInfoMock = new Mock<ICommandInfo>();
            var commandMock = new Mock<ISwCommand>();
            var activeDoc = new SwDocument_Tests.SwMockDocument(null);

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            commandHandlerMock.Setup(o => o.GetCommand(It.IsAny<int>())).Returns(commandInfoMock.Object);
            commandInfoMock.SetupGet(o => o.Command).Returns(commandMock.Object);

            // any document returns false
            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(false);

            // active document returns true
            commandMock.Setup(o => o.CanExecute(activeDoc)).Returns(true);

            // mock active doc
            documentManagerMock.SetupGet(o => o.ActiveDocument).Returns(activeDoc);

            // create instance
            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // get command handler dictionary
            var dictionary = (Dictionary<int, ICommandGroup>)cmd.GetPrivateObject("commandHandlers")!;

            // add mock entry to handler collection.
            dictionary.Add(0, commandHandlerMock.Object);

            // invoke can execute with unknown command should return disabled.
            var result = cmd.CanExecute("0:0");
            Assert.AreEqual((int)CommandCanExecuteState.Enabled, result);

            commandMock.Verify(o => o.CanExecute(activeDoc), Times.AtLeastOnce);
        }

        [TestMethod]
        public void OnExecuteActiveDoc_Test()
        {
            var swApplicationMock = new Mock<SW.ISldWorks>();
            var swCommandManagerMock = new Mock<SW.CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();
            var commandHandlerMock = new Mock<ICommandGroup>();
            var commandInfoMock = new Mock<ICommandInfo>();
            var commandMock = new Mock<ISwCommand>();
            var activeDocMock = new Mock<ISwDocument>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            commandHandlerMock.Setup(o => o.GetCommand(It.IsAny<int>())).Returns(commandInfoMock.Object);
            commandInfoMock.SetupGet(o => o.Command).Returns(commandMock.Object);

            // any document returns false
            commandMock.Setup(o => o.CanExecute(null)).Returns(false);

            // active document returns true
            commandMock.Setup(o => o.CanExecute(activeDocMock.Object)).Returns(true);

            commandMock.Setup(o => o.Execute(null));
            commandMock.Setup(o => o.Execute(activeDocMock.Object));

            // create instance
            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // get command handler dictionary
            var dictionary = (Dictionary<int, ICommandGroup>)cmd.GetPrivateObject("commandHandlers")!;

            // add mock entry to handler collection.
            dictionary.Add(0, commandHandlerMock.Object);

            // invoke can execute with unknown command should return disabled.
            cmd.OnExecute("0:0");

            commandMock.Verify(o => o.CanExecute(null), Times.AtLeastOnce);
            commandMock.Verify(o => o.Execute(null), Times.Never);

            commandMock.Verify(o => o.CanExecute(activeDocMock.Object), Times.Never);
            commandMock.Verify(o => o.Execute(activeDocMock.Object), Times.Never);

            // reconfigure document manager to mock active doc
            documentManagerMock.SetupGet(o => o.ActiveDocument).Returns(activeDocMock.Object);

            // invoke can execute with unknown command should return disabled.
            cmd.OnExecute("0:0");

            commandMock.Verify(o => o.Execute(null), Times.Never);

            commandMock.Verify(o => o.CanExecute(activeDocMock.Object), Times.AtLeastOnce);
            commandMock.Verify(o => o.Execute(activeDocMock.Object), Times.AtLeastOnce);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OnExecuteActiveDoc_Fail()
        {
            var swApplicationMock = new Mock<SW.ISldWorks>();
            var swCommandManagerMock = new Mock<SW.CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);

            // create instance
            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // invoke an undefined command should throw an exception
            cmd.OnExecute("0:0");
        }

        [TestMethod]
        public void Dispose_Test()
        {
            var swApplicationMock = new Mock<SW.ISldWorks>();
            var swCommandManagerMock = new Mock<SW.CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();
            var commandHandlerMock = new Mock<ICommandGroup>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            commandHandlerMock.Setup(o => o.Dispose());

            // create instance
            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // get command handler dictionary
            var dictionary = (Dictionary<int, ICommandGroup>)cmd.GetPrivateObject("commandHandlers")!;

            // add mock entry to handler collection.
            dictionary.Add(0, commandHandlerMock.Object);

            // should invoke dispose.
            cmd.Dispose();

            commandHandlerMock.Verify(o => o.Dispose(), Times.Once);
            Assert.AreEqual(0, dictionary.Count);

            // should invoke nothing.
            cmd.Dispose();

            commandHandlerMock.Verify(o => o.Dispose(), Times.Once);
        }

        [TestMethod]
        public void GetCommand_Test()
        {
            var swApplicationMock = new Mock<SW.ISldWorks>();
            var swCommandManagerMock = new Mock<SW.CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();
            var commandHandlerMock = new Mock<ICommandGroup>();
            var commandInfoMock = new Mock<ICommandInfo>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            commandHandlerMock.Setup(o => o.GetCommand(0)).Returns(commandInfoMock.Object);
            commandHandlerMock.Setup(o => o.GetCommand(1)).Returns<ICommandInfo>(null);

            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // get command handler dictionary
            var dictionary = (Dictionary<int, ICommandGroup>)cmd.GetPrivateObject("commandHandlers")!;

            // add mock entry to handler collection.
            dictionary!.Add(0, commandHandlerMock.Object);

            Assert.AreEqual(commandInfoMock.Object, cmd.GetCommand(0, 0));
            Assert.IsNull(cmd.GetCommand(0, 1));
            Assert.IsNull(cmd.GetCommand(1, 0));
        }

        private delegate void CreateCommandGroup2Callback(int userId, string title, string tooltip, string hint, int position, bool ignore, ref int errors);

    }
}
