namespace SIM.SolidWorksPlugin.Tests.Extensions
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SIM.SolidWorksPlugin.Commands;
    using System;

    [TestClass]
    public class ICommandGroupHandlerExtensions_Tests
    {
        [TestMethod]
        public void AddCommandGroup_Test()
        {
            var handler = new Mock<ICommandGroupHandler>();

            var factoryMethod = new CommandGroupBuilderDelegate<Commands>(builder => { });

            handler.Setup(o => o.AddCommandGroup(It.IsAny<CommandGroupInfo>(), It.IsAny<CommandGroupBuilderDelegate>()))
                .Callback((CommandGroupInfo info, CommandGroupBuilderDelegate del) =>
                {
                    Assert.IsNotNull(info);
                    Assert.IsNotNull(del);
                    Assert.AreEqual(1, info.UserId);
                    Assert.AreEqual("Group Title", info.Title);
                    Assert.AreEqual("Group Hint", info.Hint);
                    Assert.AreEqual("Group ToolTip", info.Tooltip);
                    Assert.AreEqual(15, info.Position);
                });

            ICommandGroupHandlerExtensions.AddCommandGroup(handler.Object, factoryMethod);

            handler.Verify(o => o.AddCommandGroup(It.IsAny<CommandGroupInfo>(), It.IsAny<CommandGroupBuilderDelegate>()), Times.Once);
        }

        [TestMethod]
        public void AddCommandsInvoke()
        {
            var handler = new Mock<ICommandGroupHandler>();
            var builder = new Mock<ICommandGroupBuilder>();

            var command = new RelaySwCommand(d => { });

            var factoryMethod = new CommandGroupBuilderDelegate<Commands>(builder =>
            {
                builder.AddCommand(Commands.FirstCommand, command);
            });


            handler.Setup(o => o.AddCommandGroup(It.IsAny<CommandGroupInfo>(), It.IsAny<CommandGroupBuilderDelegate>()))
                .Callback((CommandGroupInfo info, CommandGroupBuilderDelegate callback) =>
                {
                    callback.Invoke(builder.Object);
                });

            builder.Setup(o => o.AddCommand(It.IsAny<CommandInfo>(), command)).
                Callback((CommandInfo info, ISwCommand cmd) =>
                {
                    Assert.IsNotNull(info);
                    Assert.AreSame(command, cmd);
                    Assert.AreEqual("First Command Name", info.Name);
                    Assert.AreEqual("Command Hint", info.Hint);
                    Assert.AreEqual("Command ToolTip", info.Tooltip);
                    Assert.AreEqual(45, info.UserId);
                    Assert.AreEqual(15, info.Position);
                    Assert.AreEqual(5, info.ImageIndex);
                });

            ICommandGroupHandlerExtensions.AddCommandGroup(handler.Object, factoryMethod);

            handler.Verify(o => o.AddCommandGroup(It.IsAny<CommandGroupInfo>(), It.IsAny<CommandGroupBuilderDelegate>()), Times.Once);
            builder.Verify(o => o.AddCommand(It.IsAny<CommandInfo>(), command), Times.Once);
        }

        [TestMethod]
        public void AddCommandGroupWithIcons_Test()
        {
            var handler = new Mock<ICommandGroupHandler>();

            var factoryMethod = new CommandGroupBuilderDelegate<CommandsWithIcons>(builder => { });

            handler.Setup(o => o.AddCommandGroup(It.IsAny<CommandGroupInfo>(), It.IsAny<CommandGroupBuilderDelegate>()))
                .Callback((CommandGroupInfo info, CommandGroupBuilderDelegate del) =>
                {
                    Assert.IsNotNull(info);
                    Assert.IsNotNull(del);
                    Assert.AreEqual(@".\Icons\Icons{0}.png", info.IconsPath);
                    Assert.AreEqual(@".\Icons\MainIcon{0}.png", info.MainIconPath);
                });

            ICommandGroupHandlerExtensions.AddCommandGroup(handler.Object, factoryMethod);

            handler.Verify(o => o.AddCommandGroup(It.IsAny<CommandGroupInfo>(), It.IsAny<CommandGroupBuilderDelegate>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCommandGroupWithoutAttributes_Test()
        {
            var handler = new Mock<ICommandGroupHandler>();

            var factoryMethod = new CommandGroupBuilderDelegate<CommandsWithoutAttributes>(builder => { });

            ICommandGroupHandlerExtensions.AddCommandGroup(handler.Object, factoryMethod);
        }


        [CommandGroupInfo(1, "Group Title", Hint = "Group Hint", Position = 15, ToolTip = "Group ToolTip")]
        enum Commands
        {
            [CommandInfo("First Command Name", Hint = "Command Hint", Tooltip = "Command ToolTip", ImageIndex = 5, Position = 15)]
            FirstCommand = 45,
        }

        [CommandGroupInfo(1, "Group Title", Hint = "Group Hint", Position = 15, ToolTip = "Group ToolTip")]
        [CommandGroupIcons(IconsPath = @".\Icons\Icons{0}.png", MainIconPath = @".\Icons\MainIcon{0}.png")]
        enum CommandsWithIcons
        {
            FirstCommand,
        }

        enum CommandsWithoutAttributes
        {
            FirstCommand,
        }
    }
}
