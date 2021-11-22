namespace SIM.SolidWorksPlugin.Tests.Commands
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SIM.SolidWorksPlugin.Tests.Documents.DocumentsTypes;
    using SolidWorks.Interop.sldworks;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class CommandHandler_Tests
    {

        [TestMethod]
        public void Constructor()
        {
            var swApplicationMock = new Mock<ISldWorks>();

            var cmd = new CommandHandler(swApplicationMock.Object, null, new Cookie(42));

            swApplicationMock.Setup(o => o.GetCommandManager(42)).Returns<ICommandManager>(null);
            swApplicationMock.Setup(o => o.SetAddinCallbackInfo2(0, cmd, 42));

            Assert.IsNotNull(cmd);

            swApplicationMock.Verify(o => o.GetCommandManager(42), Times.AtLeastOnce);
            swApplicationMock.Verify(o => o.SetAddinCallbackInfo2(0, cmd, 42), Times.AtLeastOnce);
        }

        [TestMethod]
        public void AddCommandGroup_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swCommandManagerMock = new Mock<CommandManager>();
            var swCommandGroupMock = new Mock<CommandGroup>();
            var documentManagerMock = new Mock<IDocumentManager>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            swCommandManagerMock
                .Setup(o => o.CreateCommandGroup2(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), true, ref It.Ref<int>.IsAny))
                .Returns(swCommandGroupMock.Object)
                .Callback(new CreateCommandGroup2Callback((int userId, string title, string tooltip, string hint, int position, bool ignore, ref int errors) =>
                {
                    Assert.AreEqual(1, userId);
                    Assert.AreEqual(nameof(CommandEnum), title);
                    Assert.AreEqual("group hint", hint);
                    Assert.AreEqual("group tooltip", tooltip);
                    Assert.AreEqual(15, position);
                }));

            swCommandGroupMock.Setup(o => o.Activate()).Returns(true);
            swCommandGroupMock.SetupSet(o => o.IconList = It.IsAny<string[]>());
            swCommandGroupMock.SetupSet(o => o.MainIconList = It.IsAny<string[]>());

            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            bool callbackInvoked = false;
            cmd.AddCommandGroup<CommandEnum>(o => callbackInvoked = true);

            Assert.IsTrue(callbackInvoked);
            swApplicationMock.Verify(o => o.GetCommandManager(It.IsAny<int>()), Times.AtLeastOnce);
            swCommandGroupMock.Verify(o => o.Activate(), Times.AtLeastOnce());

            swCommandGroupMock.VerifySet(o => o.IconList = It.IsAny<string[]>(), Times.Never);
            swCommandGroupMock.VerifySet(o => o.MainIconList = It.IsAny<string[]>(), Times.Never);
        }

        [TestMethod]
        public void AddCommandGroupWithIcons_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swCommandManagerMock = new Mock<CommandManager>();
            var swCommandGroupMock = new Mock<CommandGroup>();
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
            cmd.AddCommandGroup<CommandEnumWithIcons>(o => callbackInvoked = true);

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
            var swApplicationMock = new Mock<ISldWorks>();
            var swCommandManagerMock = new Mock<CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);

            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            var dictionary = (Dictionary<string, ICommandHandler>)cmd.GetPrivateObject("commandHandlers");
            dictionary.Add(typeof(CommandEnum).Name, null);

            cmd.AddCommandGroup<CommandEnum>(o => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCommandGroup_Fail2()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swCommandManagerMock = new Mock<CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);

            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            cmd.AddCommandGroup<CommandEnumWithoutAttributes>(o => { });
        }

        [TestMethod]
        public void CanExecute_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swCommandManagerMock = new Mock<CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();
            var commandHandlerMock = new Mock<ICommandHandler>();
            var commandMock = new Mock<ISwCommand>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            commandHandlerMock.Setup(o => o.GetCommand("myCommand")).Returns(commandMock.Object);
            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(false);

            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // get command handler dictionary
            var dictionary = (Dictionary<string, ICommandHandler>)cmd.GetPrivateObject("commandHandlers");

            // add mock entry to handler collection.
            dictionary.Add("MyHandler", commandHandlerMock.Object);

            // execute empty string, should return disabled.
            var result = cmd.CanExecute("");
            Assert.AreEqual((int)CommandCanExecuteState.Disabled, result);
            commandHandlerMock.Verify(o => o.GetCommand("myCommand"), Times.Never);

            // invoke can execute with unknown command should return disabled.
            result = cmd.CanExecute("MyHandler:someCommand");
            Assert.AreEqual((int)CommandCanExecuteState.Disabled, result);

            // verify invocation
            commandHandlerMock.Verify(o => o.GetCommand("myCommand"), Times.Never);
            commandMock.Verify(o => o.CanExecute(It.IsAny<SwDocument?>()), Times.Never);

            // invoke can execute with unknown command should return disabled.
            result = cmd.CanExecute("MyHandler:myCommand");
            Assert.AreEqual((int)CommandCanExecuteState.Disabled, result);

            // verify invocation
            commandHandlerMock.Verify(o => o.GetCommand("myCommand"), Times.Once);
            commandMock.Verify(o => o.CanExecute(It.IsAny<SwDocument?>()), Times.Once);

            // change setup of mock
            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(true);

            result = cmd.CanExecute("MyHandler:myCommand");
            Assert.AreEqual((int)CommandCanExecuteState.Enabled, result);
        }

        [TestMethod]
        public void CanExecuteToggleCommand_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swCommandManagerMock = new Mock<CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();
            var commandHandlerMock = new Mock<ICommandHandler>();
            var commandMock = new Mock<IToggleCommand>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            commandHandlerMock.Setup(o => o.GetCommand("myCommand")).Returns(commandMock.Object);

            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // get command handler dictionary
            var dictionary = (Dictionary<string, ICommandHandler>)cmd.GetPrivateObject("commandHandlers");

            // add mock entry to handler collection.
            dictionary.Add("MyHandler", commandHandlerMock.Object);

            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(false);
            commandMock.SetupGet(o => o.IsActive).Returns(false);

            // invoke can execute with unknown command should return disabled.
            var result = cmd.CanExecute("MyHandler:myCommand");
            Assert.AreEqual((int)CommandCanExecuteState.Disabled, result);

            // reconfigure
            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(true);

            // invoke can execute with unknown command should return disabled.
            result = cmd.CanExecute("MyHandler:myCommand");
            Assert.AreEqual((int)CommandCanExecuteState.Enabled, result);

            // reconfigure
            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(false);
            commandMock.SetupGet(o => o.IsActive).Returns(true);

            // invoke can execute with unknown command should return disabled.
            result = cmd.CanExecute("MyHandler:myCommand");
            Assert.AreEqual((int)CommandCanExecuteState.Selected, result);

            // reconfigure
            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(true);

            // invoke can execute with unknown command should return disabled.
            result = cmd.CanExecute("MyHandler:myCommand");
            Assert.AreEqual((int)CommandCanExecuteState.SelectedAndEnabled, result);
        }

        [TestMethod]
        public void CanExecuteActiveDoc_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swCommandManagerMock = new Mock<CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();
            var commandHandlerMock = new Mock<ICommandHandler>();
            var commandMock = new Mock<ISwCommand>();
            var activeDoc = new SwDocument_Tests.SwMockDocument(null);

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            commandHandlerMock.Setup(o => o.GetCommand("myCommand")).Returns(commandMock.Object);

            // any document returns false
            commandMock.Setup(o => o.CanExecute(It.IsAny<SwDocument?>())).Returns(false);

            // active document returns true
            commandMock.Setup(o => o.CanExecute(activeDoc)).Returns(true);

            // mock active doc
            documentManagerMock.SetupGet(o => o.ActiveDocument).Returns(activeDoc);

            // create instance
            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // get command handler dictionary
            var dictionary = (Dictionary<string, ICommandHandler>)cmd.GetPrivateObject("commandHandlers");

            // add mock entry to handler collection.
            dictionary.Add("MyHandler", commandHandlerMock.Object);

            // invoke can execute with unknown command should return disabled.
            var result = cmd.CanExecute("MyHandler:myCommand");
            Assert.AreEqual((int)CommandCanExecuteState.Enabled, result);

            commandMock.Verify(o => o.CanExecute(activeDoc), Times.AtLeastOnce);
        }

        [TestMethod]
        public void OnExecuteActiveDoc_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swCommandManagerMock = new Mock<CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();
            var commandHandlerMock = new Mock<ICommandHandler>();
            var commandMock = new Mock<ISwCommand>();
            var activeDoc = new SwDocument_Tests.SwMockDocument(null);

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            commandHandlerMock.Setup(o => o.GetCommand("myCommand")).Returns(commandMock.Object);

            // any document returns false
            commandMock.Setup(o => o.CanExecute(null)).Returns(false);

            // active document returns true
            commandMock.Setup(o => o.CanExecute(activeDoc)).Returns(true);

            commandMock.Setup(o => o.Execute(null));
            commandMock.Setup(o => o.Execute(activeDoc));

            // create instance
            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // get command handler dictionary
            var dictionary = (Dictionary<string, ICommandHandler>)cmd.GetPrivateObject("commandHandlers");

            // add mock entry to handler collection.
            dictionary.Add("MyHandler", commandHandlerMock.Object);

            // invoke can execute with unknown command should return disabled.
            cmd.OnExecute("MyHandler:myCommand");

            commandMock.Verify(o => o.CanExecute(null), Times.AtLeastOnce);
            commandMock.Verify(o => o.Execute(null), Times.Never);

            commandMock.Verify(o => o.CanExecute(activeDoc), Times.Never);
            commandMock.Verify(o => o.Execute(activeDoc), Times.Never);

            // reconfigure document manager to mock active doc
            documentManagerMock.SetupGet(o => o.ActiveDocument).Returns(activeDoc);

            // invoke can execute with unknown command should return disabled.
            cmd.OnExecute("MyHandler:myCommand");

            commandMock.Verify(o => o.Execute(null), Times.Never);

            commandMock.Verify(o => o.CanExecute(activeDoc), Times.AtLeastOnce);
            commandMock.Verify(o => o.Execute(activeDoc), Times.AtLeastOnce);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OnExecuteActiveDoc_Fail()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swCommandManagerMock = new Mock<CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);

            // create instance
            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // invoke an undefined command should throw an exception
            cmd.OnExecute("MyHandler:myCommand");
        }

        [TestMethod]
        public void Dispose_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swCommandManagerMock = new Mock<CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();
            var commandHandlerMock = new Mock<ICommandHandler>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);
            commandHandlerMock.Setup(o => o.Dispose());

            // create instance
            var cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            // get command handler dictionary
            var dictionary = (Dictionary<string, ICommandHandler>)cmd.GetPrivateObject("commandHandlers");

            // should invoke nothing.
            cmd.Dispose();

            commandHandlerMock.Verify(o => o.Dispose(), Times.Never);

            // add mock entry to handler collection.
            dictionary.Add("MyHandler", commandHandlerMock.Object);

            // should invoke nothing.
            cmd.Dispose();

            commandHandlerMock.Verify(o => o.Dispose(), Times.Once);
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void GetCallbackNames_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();

            ICommandHandlerInternals cmd = new CommandHandler(swApplicationMock.Object, null, new Cookie(42));

            var names = cmd.GetCallbackNames(CommandEnum.FirstCommand);

            Assert.AreEqual($"OnExecute({nameof(CommandEnum)}:{nameof(CommandEnum.FirstCommand)})", names.OnExecute);
            Assert.AreEqual($"CanExecute({nameof(CommandEnum)}:{nameof(CommandEnum.FirstCommand)})", names.CanExecute);
        }

        [TestMethod]
        public void GetICommandManager()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swCommandManagerMock = new Mock<CommandManager>();
            var documentManagerMock = new Mock<IDocumentManager>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>())).Returns(swCommandManagerMock.Object);

            ICommandHandlerInternals cmd = new CommandHandler(swApplicationMock.Object, documentManagerMock.Object, new Cookie(42));

            Assert.AreSame(swCommandManagerMock.Object, cmd.SwCommandManager);
        }

        [CommandGroupInfo(commandGroupId: 1, title: nameof(CommandEnum), Hint = "group hint", Position = 15, ToolTip = "group tooltip")]
        public enum CommandEnum
        {
            [CommandInfo(nameof(FirstCommand), HasMenu = true, HasToolbar = true)]
            FirstCommand,
            SecondCommand,
        }

        [CommandGroupInfo(2, nameof(CommandEnumWithIcons), Hint = "hint2", Position = 13, ToolTip = "tooltip2")]
        [CommandGroupIcons(IconsPath = "./icons{0}.png", MainIconPath = "./mainicon{0}.png")]
        public enum CommandEnumWithIcons
        {
            FirstCommand,
            SecondCommand,
        }

        public enum CommandEnumWithoutAttributes
        {
            FirstCommand,
            SecondCommand,
        }

        private delegate void CreateCommandGroup2Callback(int userId, string title, string tooltip, string hint, int position, bool ignore, ref int errors);

    }
}
