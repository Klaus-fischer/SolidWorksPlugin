namespace SIM.SolidWorksPlugin.Tests.EventHandler
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static SIM.SolidWorksPlugin.Tests.Documents.DocumentsTypes.SwDocument_Tests;

    [TestClass]
    public class EventHandlerManager_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var swApplicationMock = new Mock<SldWorks>();
            var documentManagerMock = new Mock<IDocumentManagerInternals>();

            swApplicationMock.SetupAdd(o => o.FileNewNotify2 += It.IsAny<DSldWorksEvents_FileNewNotify2EventHandler>());
            swApplicationMock.SetupAdd(o => o.FileOpenPostNotify += It.IsAny<DSldWorksEvents_FileOpenPostNotifyEventHandler>());
            documentManagerMock.SetupAdd(o => o.OnDocumentAdded += It.IsAny<EventHandler<ISwDocument>>());

            var eman = new EventHandlerManager(swApplicationMock.Object, documentManagerMock.Object);

            Assert.IsNotNull(eman);

            swApplicationMock.VerifyAdd(o => o.FileNewNotify2 += It.IsAny<DSldWorksEvents_FileNewNotify2EventHandler>(), Times.Once);
            swApplicationMock.VerifyAdd(o => o.FileOpenPostNotify += It.IsAny<DSldWorksEvents_FileOpenPostNotifyEventHandler>(), Times.Once);
            documentManagerMock.VerifyAdd(o => o.OnDocumentAdded += It.IsAny<EventHandler<ISwDocument>>(), Times.Once);
        }

        [TestMethod]
        public void RegisterSolidWorksEventHandler_Test()
        {
            var swApplicationMock = new Mock<SldWorks>();
            var documentManagerMock = new Mock<IDocumentManagerInternals>();
            var swEventHandlerMock = new Mock<ISolidWorksEventHandler>();
            var documentMock = new Mock<ISwDocument>();

            swEventHandlerMock.Setup(o => o.AttachSwEvents(swApplicationMock.Object));
            documentManagerMock.Setup(o => o.GetOpenDocuments()).Returns(new ISwDocument[] { documentMock.Object });
            documentMock.SetupAdd(o => o.OnDestroy += It.IsAny<DocumentEventHandler<swDestroyNotifyType_e>>());

            var eman = new EventHandlerManager(swApplicationMock.Object, documentManagerMock.Object);

            var collection = (ICollection<ISolidWorksEventHandler>)eman.GetPrivateObject("solidWorksEventHandlers");

            Assert.AreEqual(0, collection.Count);

            eman.RegisterSolidWorksEventHandler(swEventHandlerMock.Object);

            swEventHandlerMock.Verify(o => o.AttachSwEvents(swApplicationMock.Object), Times.Once);
            documentMock.VerifyAdd(o => o.OnDestroy += It.IsAny<DocumentEventHandler<swDestroyNotifyType_e>>(), Times.Once);

            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void RegisterDocumentEventHandler_Test()
        {
            var swApplicationMock = new Mock<SldWorks>();
            var documentManagerMock = new Mock<IDocumentManagerInternals>();
            var swEventHandlerMock = new Mock<IDocumentEventHandler>();
            var documentMock = new Mock<ISwDocument>();

            swEventHandlerMock.Setup(o => o.AttachDocumentEvents(documentMock.Object));
            documentManagerMock.Setup(o => o.GetOpenDocuments()).Returns(new ISwDocument[] { documentMock.Object });
            documentMock.SetupAdd(o => o.OnDestroy += It.IsAny<DocumentEventHandler<swDestroyNotifyType_e>>());

            var eman = new EventHandlerManager(swApplicationMock.Object, documentManagerMock.Object);

            var collection = (ICollection<IDocumentEventHandler>)eman.GetPrivateObject("documentEventHandlers");

            Assert.AreEqual(0, collection.Count);

            eman.RegisterDocumentEventHandler(swEventHandlerMock.Object);

            swEventHandlerMock.Verify(o => o.AttachDocumentEvents(documentMock.Object), Times.Once);
            documentManagerMock.Verify(o => o.GetOpenDocuments(), Times.AtLeastOnce);
            documentMock.VerifyAdd(o => o.OnDestroy += It.IsAny<DocumentEventHandler<swDestroyNotifyType_e>>(), Times.Once);

            Assert.AreEqual(1, collection.Count);
        }

        [TestMethod]
        public void AttachEventsToAllOpenedDocumentByNew_Test()
        {
            var swApplicationMock = new Mock<SldWorks>();
            var documentManagerMock = new Mock<IDocumentManagerInternals>();
            var swEventHandlerMock = new Mock<IDocumentEventHandler>();
            var documentMock = new Mock<ISwDocument>();

            swEventHandlerMock.Setup(o => o.AttachDocumentEvents(documentMock.Object));
            documentManagerMock.Setup(o => o.GetAllUnknownDocuments()).Returns(new ISwDocument[] { documentMock.Object });

            var eman = new EventHandlerManager(swApplicationMock.Object, documentManagerMock.Object);
            eman.RegisterDocumentEventHandler(swEventHandlerMock.Object);

            swApplicationMock.Raise(o => o.FileNewNotify2 += null, null, 0, string.Empty);

            swEventHandlerMock.Verify(o => o.AttachDocumentEvents(documentMock.Object), Times.Once);
            documentManagerMock.Verify(o => o.GetAllUnknownDocuments(), Times.Once);
        }

        [TestMethod]
        public void AttachEventsToAllOpenedDocumentByOpen_Test()
        {
            var swApplicationMock = new Mock<SldWorks>();
            var documentManagerMock = new Mock<IDocumentManagerInternals>();
            var swEventHandlerMock = new Mock<IDocumentEventHandler>();
            var documentMock = new Mock<ISwDocument>();

            swEventHandlerMock.Setup(o => o.AttachDocumentEvents(documentMock.Object));
            documentManagerMock.Setup(o => o.GetAllUnknownDocuments()).Returns(new ISwDocument[] { documentMock.Object });

            var eman = new EventHandlerManager(swApplicationMock.Object, documentManagerMock.Object);
            eman.RegisterDocumentEventHandler(swEventHandlerMock.Object);

            swApplicationMock.Raise(o => o.FileOpenPostNotify += null, string.Empty);

            swEventHandlerMock.Verify(o => o.AttachDocumentEvents(documentMock.Object), Times.Once);
            documentManagerMock.Verify(o => o.GetAllUnknownDocuments(), Times.Once);
        }

        [TestMethod]
        public void OnOpenDocumentAdded_Test()
        {
            var swApplicationMock = new Mock<SldWorks>();
            var documentManagerMock = new Mock<IDocumentManagerInternals>();
            var swEventHandlerMock = new Mock<IDocumentEventHandler>();
            var documentMock = new Mock<ISwDocument>();

            swEventHandlerMock.Setup(o => o.AttachDocumentEvents(documentMock.Object));
            documentManagerMock.Setup(o => o.GetOpenDocuments()).Returns(new ISwDocument[0]);
            documentMock.SetupAdd(o => o.OnDestroy += It.IsAny<DocumentEventHandler<swDestroyNotifyType_e>>());

            var eman = new EventHandlerManager(swApplicationMock.Object, documentManagerMock.Object);

            var collection = (ICollection<IDocumentEventHandler>)eman.GetPrivateObject("documentEventHandlers");

            eman.RegisterDocumentEventHandler(swEventHandlerMock.Object);

            swEventHandlerMock.Verify(o => o.AttachDocumentEvents(documentMock.Object), Times.Never);

            documentManagerMock.Raise(o => o.OnDocumentAdded += null, documentManagerMock.Object, documentMock.Object);

            swEventHandlerMock.Verify(o => o.AttachDocumentEvents(documentMock.Object), Times.Once);
            documentMock.VerifyAdd(o => o.OnDestroy += It.IsAny<DocumentEventHandler<swDestroyNotifyType_e>>(), Times.Once);

        }

        [TestMethod]
        public void DetachEventsFromDocument_Test()
        {
            var swApplicationMock = new Mock<SldWorks>();
            var documentManagerMock = new Mock<IDocumentManagerInternals>();
            var swEventHandlerMock = new Mock<IDocumentEventHandler>();
            var documentMock = new Mock<ISwDocument>();

            documentManagerMock.Setup(o => o.GetOpenDocuments()).Returns(new ISwDocument[] { documentMock.Object });
            documentManagerMock.Setup(o => o.DisposeDocument(documentMock.Object));
            swEventHandlerMock.Setup(o => o.DetachDocumentEvents(documentMock.Object));

            var eman = new EventHandlerManager(swApplicationMock.Object, documentManagerMock.Object);

            eman.RegisterDocumentEventHandler(swEventHandlerMock.Object);

            documentMock.Raise(o => o.OnDestroy += null, documentMock.Object, swDestroyNotifyType_e.swDestroyNotifyHidden);
            swEventHandlerMock.Verify(o => o.DetachDocumentEvents(documentMock.Object), Times.Never);
            documentManagerMock.Verify(o => o.DisposeDocument(documentMock.Object), Times.Never);

            documentMock.Raise(o => o.OnDestroy += null, documentMock.Object, swDestroyNotifyType_e.swDestroyNotifyDestroy);
            swEventHandlerMock.Verify(o => o.DetachDocumentEvents(documentMock.Object), Times.Once);
            documentManagerMock.Verify(o => o.DisposeDocument(documentMock.Object), Times.Once);
        }

        [TestMethod]
        public void Dispose_Test()
        {
            var swApplicationMock = new Mock<SldWorks>();
            var documentManagerMock = new Mock<IDocumentManagerInternals>();
            var swEventHandlerMock = new Mock<IDocumentEventHandler>();
            var swSwEventHandlerMock = new Mock<ISolidWorksEventHandler>();
            var documentMock = new Mock<ISwDocument>();

            swApplicationMock.SetupRemove(o => o.FileNewNotify2 -= It.IsAny<DSldWorksEvents_FileNewNotify2EventHandler>());
            swApplicationMock.SetupRemove(o => o.FileOpenPostNotify -= It.IsAny<DSldWorksEvents_FileOpenPostNotifyEventHandler>());
            documentManagerMock.SetupRemove(o => o.OnDocumentAdded -= It.IsAny<EventHandler<ISwDocument>>());

            documentManagerMock.Setup(o => o.GetOpenDocuments()).Returns(new ISwDocument[] { documentMock.Object });

            swEventHandlerMock.Setup(o => o.DetachDocumentEvents(documentMock.Object));
            swSwEventHandlerMock.Setup(o => o.DetachSwEvents(swApplicationMock.Object));

            var eman = new EventHandlerManager(swApplicationMock.Object, documentManagerMock.Object);

            eman.RegisterDocumentEventHandler(swEventHandlerMock.Object);
            eman.RegisterSolidWorksEventHandler(swSwEventHandlerMock.Object);

            eman.Dispose();

            swApplicationMock.VerifyRemove(o => o.FileNewNotify2 -= It.IsAny<DSldWorksEvents_FileNewNotify2EventHandler>(), Times.Once);
            swApplicationMock.VerifyRemove(o => o.FileOpenPostNotify -= It.IsAny<DSldWorksEvents_FileOpenPostNotifyEventHandler>(), Times.Once);
            documentManagerMock.VerifyRemove(o => o.OnDocumentAdded -= It.IsAny<EventHandler<ISwDocument>>(), Times.Once);

            documentManagerMock.Verify(o => o.GetOpenDocuments(), Times.AtLeastOnce);

            swEventHandlerMock.Verify(o => o.DetachDocumentEvents(documentMock.Object), Times.AtLeastOnce);
            swSwEventHandlerMock.Verify(o => o.DetachSwEvents(swApplicationMock.Object), Times.AtLeastOnce);
        }
    }
}
