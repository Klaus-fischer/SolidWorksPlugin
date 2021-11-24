namespace SIM.SolidWorksPlugin.Tests.Helper
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.sldworks;

    [TestClass]
    public class SolidworksAddinMemberInstanceFactory_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var factory = new SolidworksAddinMemberInstanceFactory();
            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void Create_Test()
        {
            var swApplicationMock = new Mock<SldWorks>();
            var swCommandManagerMock = new Mock<CommandManager>();

            swApplicationMock.Setup(o => o.GetCommandManager(It.IsAny<int>()))
                .Returns(swCommandManagerMock.Object);

            var factory = new SolidworksAddinMemberInstanceFactory();

            var instances = factory.CreateInstances(swApplicationMock.Object, new Cookie(42));

            Assert.IsInstanceOfType(instances.DocumentManager, typeof(DocumentManager));
            Assert.IsInstanceOfType(instances.CommandManager, typeof(CommandHandler));
            Assert.IsInstanceOfType(instances.EventHandler, typeof(EventHandlerManager));
            Assert.IsInstanceOfType(instances.TabManager, typeof(TabCommandManager));
        }
    }
}
