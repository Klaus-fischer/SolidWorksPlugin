namespace SIM.SolidWorksPlugin.Tests.Commands
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;
    using System;
    using System.Collections.ObjectModel;

    [TestClass]
    public class TabCommandManager_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var commandManagerMock = new Mock<IInternalCommandHandler>();

            var tcm = new TabCommandManager(commandManagerMock.Object);

            Assert.IsNotNull(tcm);

            Assert.AreSame(commandManagerMock.Object, tcm.GetPrivateObject("commandHandler"));
        }

        [TestMethod]
        public void BuildCommandTab_Test()
        {
            var factoryMethodInvoked = false;

            var factoryMethod = new CommandTabBuilderDelegate(builder =>
            {
                factoryMethodInvoked = true;
                Assert.IsInstanceOfType(builder, typeof(CommandTabBuilder));
            });

            var commandManagerMock = new Mock<ICommandManager>();
            var commandTabMock = new Mock<CommandTab>();

            var commandHandler = new Mock<IInternalCommandHandler>();
            commandHandler.SetupGet(o => o.SwCommandManager).Returns(commandManagerMock.Object);

            commandManagerMock.Setup(o => o.GetCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"));
            commandManagerMock.Setup(o => o.AddCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"))
                .Returns(commandTabMock.Object);

            commandTabMock.Setup(o => o.AddCommandTabBox());

            var tcm = new TabCommandManager(commandHandler.Object);

            tcm.BuildCommandTab("MainTitle", factoryMethod, swDocumentTypes_e.swDocASSEMBLY);

            commandManagerMock.Verify(o => o.GetCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"), Times.AtLeastOnce);
            commandManagerMock.Verify(o => o.AddCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "MainTitle"), Times.AtLeastOnce);
            commandTabMock.Verify(o => o.AddCommandTabBox(), Times.AtLeastOnce);

            Assert.IsTrue(factoryMethodInvoked);
        }

        [TestMethod]
        public void Dispose_Test()
        {
            var commandManagerMock = new Mock<ICommandManager>();
            var disposableMock = new Mock<IDisposable>();

            var commandHandler = new Mock<IInternalCommandHandler>();
            commandHandler.SetupGet(o => o.SwCommandManager).Returns(commandManagerMock.Object);
            disposableMock.Setup(o => o.Dispose());

            var tcm = new TabCommandManager(commandHandler.Object);

            var collection = (Collection<IDisposable>)tcm.GetPrivateObject("disposables")!;
            collection.Add(disposableMock.Object);

            Assert.AreEqual(1, collection.Count);

            tcm.Dispose();

            Assert.AreEqual(0, collection.Count);
            disposableMock.Verify(o => o.Dispose(), Times.Once());

            // fake adding items after dispose.
            collection.Add(disposableMock.Object);
            Assert.AreEqual(1, collection.Count);

            // double dispose should be revoked.
            tcm.Dispose();
            Assert.AreEqual(1, collection.Count);
            disposableMock.Verify(o => o.Dispose(), Times.Once());
        }
    }
}
