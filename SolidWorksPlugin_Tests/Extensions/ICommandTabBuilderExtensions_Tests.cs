namespace SIM.SolidWorksPlugin.Tests.Extensions
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SIM.SolidWorksPlugin.Commands;
    using SolidWorks.Interop.swconst;
    using System;

    [TestClass]
    public class ICommandTabBuilderExtensions_Tests
    {
        [TestMethod]
        public void AddCommand_Test()
        {
            var commandInfo = new CommandInfo("name", new RelaySwCommand(d => { }));
            var commandHandlerMock = new Mock<ICommandHandler>();
            var commandTabBuilderMock = new Mock<ICommandTabBuilder>();

            commandHandlerMock.Setup(o => o.GetCommand(2, 4)).Returns(commandInfo);
            commandTabBuilderMock.SetupGet(o => o.CommandHandler).Returns(commandHandlerMock.Object);
            commandTabBuilderMock.Setup(o => o.AddCommand(commandInfo, swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow));

            ICommandTabBuilderExtensions.AddCommand(commandTabBuilderMock.Object, Commands.Firstcommand, swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow);

            commandTabBuilderMock.Verify(
                o => o.AddCommand(commandInfo, swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow),
                Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddCommand_Fail()
        {
            var commandInfo = new CommandInfo("name", new RelaySwCommand(d => { }));
            var commandHandlerMock = new Mock<ICommandHandler>();
            var commandTabBuilderMock = new Mock<ICommandTabBuilder>();

            commandHandlerMock.Setup(o => o.GetCommand(2, 4));
            commandTabBuilderMock.SetupGet(o => o.CommandHandler).Returns(commandHandlerMock.Object);
            commandTabBuilderMock.Setup(o => o.AddCommand(commandInfo, swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow));

            ICommandTabBuilderExtensions.AddCommand(commandTabBuilderMock.Object, Commands.Firstcommand, swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow);
        }

        [TestMethod]
        public void AddFlyout_Test()
        {
            var commandGroupInfo = new CommandGroupInfo();
            var commandHandlerMock = new Mock<ICommandHandler>();
            var commandTabBuilderMock = new Mock<ICommandTabBuilder>();

            commandHandlerMock.Setup(o => o.GetCommandGroup(2)).Returns(commandGroupInfo);
            commandTabBuilderMock.SetupGet(o => o.CommandHandler).Returns(commandHandlerMock.Object);

            commandTabBuilderMock.Setup(o => o.AddFlyout(
                commandGroupInfo, 
                swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow, 
                swCommandTabButtonFlyoutStyle_e.swCommandTabButton_SimpleFlyout));

            ICommandTabBuilderExtensions.AddFlyout<Commands>(
                commandTabBuilderMock.Object, 
                swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow, 
                swCommandTabButtonFlyoutStyle_e.swCommandTabButton_SimpleFlyout);

            commandTabBuilderMock.Verify(
                o => o.AddFlyout(
                    commandGroupInfo,
                    swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow,
                    swCommandTabButtonFlyoutStyle_e.swCommandTabButton_SimpleFlyout),
                Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddFlyout_Fail()
        {
            var commandGroupInfo = new CommandGroupInfo();
            var commandHandlerMock = new Mock<ICommandHandler>();
            var commandTabBuilderMock = new Mock<ICommandTabBuilder>();

            commandHandlerMock.Setup(o => o.GetCommandGroup(2));
            commandTabBuilderMock.SetupGet(o => o.CommandHandler).Returns(commandHandlerMock.Object);
     
            ICommandTabBuilderExtensions.AddFlyout<Commands>(commandTabBuilderMock.Object, swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow, swCommandTabButtonFlyoutStyle_e.swCommandTabButton_NoFlyout);
        }

        [CommandGroupSpec(2, "MyCommands")]
        enum Commands
        {
            Firstcommand = 4,
        }
    }
}
