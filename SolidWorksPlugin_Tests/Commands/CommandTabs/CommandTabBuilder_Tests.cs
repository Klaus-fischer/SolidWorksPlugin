namespace SIM.SolidWorksPlugin.Tests.Commands
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SIM.SolidWorksPlugin.Commands;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class CommandTabBuilder_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var commandManagerMock = new Mock<ICommandManager>();
            var commandTabMock = new Mock<CommandTab>();

            var commandHandler = new Mock<IInternalCommandHandler>();
            commandHandler.SetupGet(o => o.SwCommandManager).Returns(commandManagerMock.Object);

            commandManagerMock.Setup(o => o.GetCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"));
            commandManagerMock.Setup(o => o.AddCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"))
                .Returns(commandTabMock.Object);

            commandTabMock.Setup(o => o.AddCommandTabBox());

            var ctb = new CommandTabBuilder(commandHandler.Object, "MainTitle", swDocumentTypes_e.swDocASSEMBLY);

            Assert.IsNotNull(ctb);

            Assert.AreSame(commandHandler.Object, ((ICommandTabBuilder)ctb).CommandHandler);

            commandManagerMock.Verify(o => o.GetCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"), Times.AtLeastOnce);
            commandManagerMock.Verify(o => o.AddCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"), Times.AtLeastOnce);
            commandTabMock.Verify(o => o.AddCommandTabBox(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void Constructor_AddExisting()
        {
            var commandManagerMock = new Mock<ICommandManager>();
            var commandTabMock = new Mock<CommandTab>();
            var commandTabBoxMock = new Mock<CommandTabBox>();

            var commandHandler = new Mock<IInternalCommandHandler>();
            commandHandler.SetupGet(o => o.SwCommandManager).Returns(commandManagerMock.Object);

            commandManagerMock.Setup(o => o.GetCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"))
                .Returns(commandTabMock.Object);

            commandManagerMock.Setup(o => o.AddCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"))
                .Returns(commandTabMock.Object);

            commandManagerMock.Setup(o => o.RemoveCommandTab(It.IsAny<CommandTab>())).Callback<CommandTab>(commandTab =>
            {
                Assert.AreSame(commandTabMock.Object, commandTab);
            });

            commandTabMock.Setup(o => o.AddCommandTabBox()).Returns(commandTabBoxMock.Object);

            var ctb = new CommandTabBuilder(commandHandler.Object, "MainTitle", swDocumentTypes_e.swDocASSEMBLY);

            Assert.IsNotNull(ctb);

            commandManagerMock.Verify(o => o.GetCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"), Times.AtLeastOnce);
            commandManagerMock.Verify(o => o.AddCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"), Times.AtLeastOnce);
            commandManagerMock.Verify(o => o.RemoveCommandTab(It.IsAny<CommandTab>()), Times.AtLeastOnce);

            commandTabMock.Verify(o => o.AddCommandTabBox(), Times.AtLeastOnce);

            Assert.AreEqual(commandTabBoxMock.Object, ctb.SwCommandTabBox);
        }


        [TestMethod]
        public void AddCommand_Test()
        {
            var commandManagerMock = new Mock<ICommandManager>();
            var commandTabMock = new Mock<CommandTab>();
            var commandTabBoxMock = new Mock<CommandTabBox>();
            var command = new RelaySwCommand(d => { });

            var commandHandler = new Mock<IInternalCommandHandler>();
            commandHandler.SetupGet(o => o.SwCommandManager).Returns(commandManagerMock.Object);

            commandManagerMock.Setup(o => o.AddCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"))
                .Returns(commandTabMock.Object);

            commandTabMock.Setup(o => o.AddCommandTabBox()).Returns(commandTabBoxMock.Object);

            var ctb = new CommandTabBuilder(commandHandler.Object, "MainTitle", swDocumentTypes_e.swDocASSEMBLY);

            commandTabBoxMock.Setup(o => o.AddCommands(It.IsAny<object>(), It.IsAny<object>()))
                .Callback<object, object>((commandIds, displayStyles) =>
                {
                    if (commandIds is int[] cmds && cmds[0] is 5)
                    {
                        if (displayStyles is int[] styles && styles[0] == 2)
                        {
                            return;
                        }
                    }

                    Assert.Fail();
                });

            ctb.AddCommand(new CommandInfo("MyCommand", command) { Id = 5 }, swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow);
        }

        [TestMethod]
        public void AddSpacer_Test()
        {
            var commandManagerMock = new Mock<ICommandManager>();
            var commandTabMock = new Mock<CommandTab>();
            var commandTabBoxMock = new Mock<CommandTabBox>();
            var command = new RelaySwCommand(d => { });

            var commandHandler = new Mock<IInternalCommandHandler>();
            commandHandler.SetupGet(o => o.SwCommandManager).Returns(commandManagerMock.Object);

            commandManagerMock.Setup(o => o.AddCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"))
                .Returns(commandTabMock.Object);

            commandTabMock.Setup(o => o.AddCommandTabBox()).Returns(commandTabBoxMock.Object);

            var ctb = new CommandTabBuilder(commandHandler.Object, "MainTitle", swDocumentTypes_e.swDocASSEMBLY);

            // 1x is default in ctor.
            commandTabMock.Verify(o => o.AddCommandTabBox(), Times.Once);

            ctb.AddSpacer();

            // add spacer will invoke 1x.
            commandTabMock.Verify(o => o.AddCommandTabBox(), Times.Exactly(2));
        }


        [TestMethod]
        public void AddFlyout_Test()
        {
            var commandManagerMock = new Mock<ICommandManager>();
            var commandTabMock = new Mock<CommandTab>();
            var commandTabBoxMock = new Mock<CommandTabBox>();
            var command = new RelaySwCommand(d => { });

            var commandHandler = new Mock<IInternalCommandHandler>();
            commandHandler.SetupGet(o => o.SwCommandManager).Returns(commandManagerMock.Object);

            commandManagerMock.Setup(o => o.AddCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"))
                .Returns(commandTabMock.Object);

            commandTabMock.Setup(o => o.AddCommandTabBox()).Returns(commandTabBoxMock.Object);

            var ctb = new CommandTabBuilder(commandHandler.Object, "MainTitle", swDocumentTypes_e.swDocASSEMBLY);

            commandTabBoxMock.Setup(o => o.AddCommands(It.IsAny<object>(), It.IsAny<object>()))
                .Callback<object, object>((commandIds, displayStyles) =>
                {
                    if (commandIds is int[] cmds && cmds[0] is 45)
                    {
                        if (displayStyles is int[] styles && styles[0] == 34)
                        {
                            return;
                        }
                    }

                    Assert.Fail();
                });

            ctb.AddFlyout(
                new CommandGroupInfo() { ToolbarId = 45 },
                swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow,
                swCommandTabButtonFlyoutStyle_e.swCommandTabButton_ActionFlyout);

            commandTabBoxMock.Verify(o => o.AddCommands(It.IsAny<object>(), It.IsAny<object>()), Times.Once);
        }

        [TestMethod]
        public void Dispose_Test()
        {
            var commandManagerMock = new Mock<ICommandManager>();
            var commandTabMock = new Mock<CommandTab>();
            var commandTabBoxMock = new Mock<CommandTabBox>();

            var commandHandler = new Mock<IInternalCommandHandler>();
            commandHandler.SetupGet(o => o.SwCommandManager).Returns(commandManagerMock.Object);

            commandManagerMock.Setup(o => o.RemoveCommandTab(commandTabMock.Object));
            commandManagerMock.Setup(o => o.GetCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"));
            commandManagerMock.Setup(o => o.AddCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"))
                .Returns(commandTabMock.Object);

            commandTabMock.Setup(o => o.AddCommandTabBox()).Returns(commandTabBoxMock.Object);
            commandTabMock.Setup(o => o.RemoveCommandTabBox(commandTabBoxMock.Object));

            var ctb = new CommandTabBuilder(commandHandler.Object, "MainTitle", swDocumentTypes_e.swDocASSEMBLY);

            // to add a new commandTabbox (the same instance in here);
            ctb.AddSpacer();

            ctb.Dispose();

            commandTabMock.Verify(o => o.RemoveCommandTabBox(commandTabBoxMock.Object), Times.Exactly(2));
            commandManagerMock.Verify(o => o.RemoveCommandTab(commandTabMock.Object), Times.Once);

            // dispose twice should have no effect.
            ctb.Dispose();

            commandManagerMock.Verify(o => o.RemoveCommandTab(commandTabMock.Object), Times.Once);
        }
    }
}
