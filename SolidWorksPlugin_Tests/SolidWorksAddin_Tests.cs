namespace SIM.SolidWorksPlugin.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.sldworks;
    using System;

    [TestClass]
    public class SolidWorksAddin_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var addin = new TestAddIn();
            Assert.IsNotNull(addin);

            var factory = addin.GetPrivateObject<SolidWorksAddin>(SolidWorksAddin.NameOfMemberInstanceFactory);
            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void Constructor_Factory()
        {
            var factoryMock = new Mock<ISolidworksAddinMemberInstanceFactory>();

            var addin = new TestAddIn(factoryMock.Object);
            Assert.IsNotNull(addin);

            var factory = addin.GetPrivateObject<SolidWorksAddin>(SolidWorksAddin.NameOfMemberInstanceFactory);
            Assert.IsNotNull(factory);
            Assert.AreSame(factoryMock.Object, factory);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AccessToEmptySwApplication()
        {
            var addin = new TestAddIn();
            var swApp = addin.SwApplication;
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AccessToEmptyDocumentManager()
        {
            var addin = new TestAddIn();
            var docMan = addin.DocumentManager;
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AccessToEmptyCommandHandler()
        {
            var addin = new TestAddIn();
            var cmdHwnd = addin.CommandHandler;
        }

        [TestMethod]
        public void Connect_Test()
        {
            var cmdInvoked = false;
            var eventInvoked = false;

            var factoryMock = new Mock<ISolidworksAddinMemberInstanceFactory>();
            var documentManagerMock = new Mock<IDocumentManagerInternals>();
            var commandManagerMock = new Mock<IInternalCommandHandler>();
            var eventManagerMock = new Mock<IEventHandlerManagerInternals>();
            var swApplicationMock = new Mock<SldWorks>();
            var myCookie = 42;

            factoryMock.Setup(o => o.CreateInstances(It.IsAny<SldWorks>(), It.IsAny<Cookie>()))
                .Returns((documentManagerMock.Object, commandManagerMock.Object, eventManagerMock.Object))
                .Callback<SldWorks, Cookie>((swApp, cookie) =>
                {
                    Assert.AreSame(swApplicationMock.Object, swApp);
                    Assert.AreEqual(myCookie, cookie.Value);
                });

            var addin = new TestAddIn(factoryMock.Object);

            addin.OnRegisterCommands = cmd =>
            {
                cmdInvoked = true;
                Assert.AreSame(commandManagerMock.Object, cmd);
            };

            addin.OnRegisterEventHandler = evt =>
            {
                eventInvoked = true;
                Assert.AreSame(eventManagerMock.Object, evt);
            };

            var result = addin.ConnectToSW(swApplicationMock.Object, myCookie);

            Assert.IsTrue(result);

            Assert.AreSame(swApplicationMock.Object, addin.SwApplication);
            Assert.AreSame(documentManagerMock.Object, addin.DocumentManager);

            Assert.IsTrue(cmdInvoked);
            Assert.IsTrue(eventInvoked);
            factoryMock.Verify(o => o.CreateInstances(It.IsAny<SldWorks>(), It.IsAny<Cookie>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void ConnectFail_Test()
        {
            var factoryMock = new Mock<ISolidworksAddinMemberInstanceFactory>();
            var documentManagerMock = new Mock<IDocumentManagerInternals>();
            var commandManagerMock = new Mock<IInternalCommandHandler>();
            var eventManagerMock = new Mock<IEventHandlerManagerInternals>();
            var swApplicationMock = new Mock<SldWorks>();
            var myCookie = 42;

            factoryMock.Setup(o => o.CreateInstances(It.IsAny<SldWorks>(), It.IsAny<Cookie>()))
                .Returns((documentManagerMock.Object, commandManagerMock.Object, eventManagerMock.Object))
                .Callback<SldWorks, Cookie>((swApp, cookie) =>
                {
                    Assert.AreSame(swApplicationMock.Object, swApp);
                    Assert.AreEqual(myCookie, cookie.Value);
                });

            documentManagerMock.Setup(o => o.Dispose());
            commandManagerMock.Setup(o => o.Dispose());
            eventManagerMock.Setup(o => o.Dispose());

            var addin = new TestAddIn(factoryMock.Object);

            addin.OnRegisterCommands = cmd =>
            {
                throw new InvalidOperationException();
            };

            var result = addin.ConnectToSW(swApplicationMock.Object, myCookie);

            Assert.IsFalse(result);

            documentManagerMock.Verify(o => o.Dispose(), Times.AtLeastOnce());
            commandManagerMock.Verify(o => o.Dispose(), Times.AtLeastOnce());
            eventManagerMock.Verify(o => o.Dispose(), Times.AtLeastOnce());
            factoryMock.Verify(o => o.CreateInstances(It.IsAny<SldWorks>(), It.IsAny<Cookie>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void DisconnectEmpty_Test()
        {
            var factoryMock = new Mock<ISolidworksAddinMemberInstanceFactory>();
            var addin = new TestAddIn(factoryMock.Object);
            Assert.IsTrue(addin.DisconnectFromSW());
        }

        private class TestAddIn : SolidWorksAddin
        {
            public TestAddIn()
                : base()
            {
            }

            public TestAddIn(ISolidworksAddinMemberInstanceFactory factory)
                : base(factory)
            {
            }

            public Action<ICommandGroupHandler> OnRegisterCommands;
            protected override void RegisterCommands(ICommandGroupHandler commandGroupHandler)
            {
                this.OnRegisterCommands?.Invoke(commandGroupHandler);
            }

            public Action<IEventHandlerManager> OnRegisterEventHandler;
            protected override void RegisterEventHandler(IEventHandlerManager eventHandlerManager)
            {
                this.OnRegisterEventHandler(eventHandlerManager);
            }
        }
    }
}
