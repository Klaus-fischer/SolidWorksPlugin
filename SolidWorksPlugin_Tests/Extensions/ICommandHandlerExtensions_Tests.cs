namespace SIM.SolidWorksPlugin.Tests.Extensions
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ICommandHandlerExtensions_Tests
    {
        [TestMethod]
        public void GetCommand_Test()
        {
            var commandHandlerMock = new Mock<ICommandHandler>();

            commandHandlerMock.Setup(o => o.GetCommand(2, 4));

            ICommandHandlerExtensions.GetCommand(commandHandlerMock.Object, Commands.Firstcommand);

            commandHandlerMock.Verify(o => o.GetCommand(2, 4), Times.Once);
        }

        [TestMethod]
        public void GetCommandGroup_Test()
        {
            var commandHandlerMock = new Mock<ICommandHandler>();

            commandHandlerMock.Setup(o => o.GetCommandGroup(2));

            ICommandHandlerExtensions.GetCommandGroup<Commands>(commandHandlerMock.Object);

            commandHandlerMock.Verify(o => o.GetCommandGroup(2), Times.Once);
        }

        [CommandGroupSpec(2, "MyCommands")]
        enum Commands
        {
            Firstcommand = 4,
        }
    }
}
