namespace SIM.SolidWorksPlugin.Tests.Documents
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static SIM.SolidWorksPlugin.Tests.Documents.DocumentsTypes.SwDocument_Tests;

    [TestClass]
    public class DocumentManager_Tests
    {
        public interface ISwPartDoc : ModelDoc2, PartDoc { }


        [TestMethod]
        public void Constructor()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);

            swApplicationMock.Setup(o => o.GetFirstDocument()).Returns((object)swModelMock.Object);
            swModelMock.Setup(o => o.GetNext()).Returns(null);
            swModelMock.Setup(o => o.GetTitle()).Returns("Model1");
            documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);

            var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

            Assert.IsNotNull(dm);

            Assert.AreSame(swApplicationMock.Object, dm.GetPrivateObject("swApplication"));
            Assert.AreSame(documentFactoryMock.Object, dm.GetPrivateObject("documentFactory"));

            var dict = (Dictionary<IModelDoc2, ISwDocument>?)dm.GetPrivateObject("openDocuments");

            Assert.IsNotNull(dict);
            Assert.AreEqual(1, dict.Count);
            Assert.AreSame(mockDocument, dict[swModelMock.Object]);
        }

        [TestMethod]
        public void GetActiveDocument_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);

            swApplicationMock.Setup(o => o.GetFirstDocument()).Returns(swModelMock.Object);
            swApplicationMock.SetupGet(o => o.ActiveDoc).Returns(swModelMock.Object);
            swModelMock.Setup(o => o.GetNext()).Returns(null);
            swModelMock.Setup(o => o.GetTitle()).Returns("Model1");
            documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);

            var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

            Assert.AreSame(mockDocument, dm.ActiveDocument);

            swApplicationMock.VerifyGet(o => o.ActiveDoc, Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetActiveDocumentNull_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;

            swApplicationMock.SetupGet(o => o.ActiveDoc).Returns(null);
            swModelMock.Setup(o => o.GetNext()).Returns(null);
            swModelMock.Setup(o => o.GetTitle()).Returns("Model1");

            var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

            Assert.IsNull(dm.ActiveDocument);

            swApplicationMock.VerifyGet(o => o.ActiveDoc, Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetDocument_Test()
        {
            bool invoked = false;
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);

            documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);

            var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);
            dm.OnDocumentAdded += (s, a) => invoked = true;

            var doc = (SwDocument?)dm.GetDocument(mockDocument.Model);

            Assert.AreSame(mockDocument, doc);
            Assert.IsTrue(invoked);

            invoked = false;

            // get document from cache.
            doc = (SwDocument?)dm.GetDocument(mockDocument.Model);

            Assert.AreSame(mockDocument, doc);
            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void SetActiveDocument_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);
            var filePath = Path.GetTempFileName();

            try
            {
                swApplicationMock.Setup(o => o.GetFirstDocument()).Returns(swModelMock.Object);
                swApplicationMock.Setup(o => o.ActivateDoc(Path.GetFileName(filePath)));

                swModelMock.Setup(o => o.GetNext()).Returns(null);
                swModelMock.Setup(o => o.GetTitle()).Returns("Model1");
                swModelMock.Setup(o => o.GetPathName()).Returns(filePath);
                documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);

                var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

                dm.ActiveDocument = mockDocument;

                swApplicationMock.Verify(o => o.ActivateDoc(Path.GetFileName(filePath)), Times.AtLeastOnce());
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void OpenExistingDocument_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);

            swApplicationMock.Setup(o => o.GetOpenDocumentByName("someFilename")).Returns(swModelMock.Object);
            documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);

            var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

            var doc = dm.OpenDocument("someFilename", out bool wasOpen);

            Assert.IsNotNull(doc);
            Assert.AreSame(mockDocument, doc);
            Assert.IsTrue(wasOpen);

            swApplicationMock.Verify(o => o.GetOpenDocumentByName(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void OpenNewDocument_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);

            swApplicationMock.Setup(o => o.OpenDoc6("someFilename.sldprt", 1, 1, "", ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(swModelMock.Object);
            documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);

            var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

            var doc = dm.OpenDocument("someFilename.sldprt", out bool wasOpen);

            Assert.IsNotNull(doc);
            Assert.AreSame(mockDocument, doc);
            Assert.IsFalse(wasOpen);

            swApplicationMock.Verify(o => o.OpenDoc6("someFilename.sldprt", 1, 1, "", ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenNewDocument_Fail()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);

            swApplicationMock
                .Setup(o => o.OpenDoc6("someFilename.sldprt", 1, 1, "", ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny))
                .Callback(new OpenDoc6Delegate((string _, int _, int _, string _, ref int Errors, ref int _) =>
                {
                    Errors = (int)swFileLoadError_e.swApplicationBusy;
                }))
                .Returns<ModelDoc2>(null);

            documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);

            var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

            var doc = dm.OpenDocument("someFilename.sldprt", out bool wasOpen);
        }


        [TestMethod]
        public void SaveDocument_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);

            documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);
            swModelMock.Setup(o => o.Save3(It.IsAny<int>(), ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(true); ;

            var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

            dm.SaveDocument(mockDocument);

            swModelMock.Verify(o => o.Save3(It.IsAny<int>(), ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny), Times.Once);
        }

        [TestMethod]
        public void SaveDocumentAs_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var swModelExtensionsMock = new Mock<ModelDocExtension>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);
            var exportData = new object();

            documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);
            swModelMock.SetupGet(o => o.Extension).Returns(swModelExtensionsMock.Object);
            swModelExtensionsMock.Setup(o => o.SaveAs2(It.IsAny<string>(), 0, 3, exportData, null, false, ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny)).Returns(true);

            var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

            dm.SaveDocument(mockDocument, "newFilename.sldasm", true, exportData);

            swModelExtensionsMock.Verify(o => o.SaveAs2(It.IsAny<string>(), 0, 3, exportData, null, false, ref It.Ref<int>.IsAny, ref It.Ref<int>.IsAny));
        }

        [TestMethod]
        public void ReloadDocument_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);
            var exportData = new object();
            var filePath = Path.GetTempFileName();

            try
            {
                swModelMock.Setup(o => o.GetPathName()).Returns(filePath);
                documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);
                swModelMock.Setup(o => o.ReloadOrReplace(false, filePath, true));

                var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

                dm.ReloadDocument(mockDocument, false);

                swModelMock.Verify(o => o.ReloadOrReplace(false, filePath, true), Times.Once);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void CloseDocument_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);
            var exportData = new object();
            var filePath = Path.GetTempFileName();

            try
            {
                swApplicationMock.Setup(o => o.CloseDoc(filePath));
                swModelMock.Setup(o => o.GetPathName()).Returns(filePath);
                documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);

                var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

                dm.CloseDocument(mockDocument);

                swApplicationMock.Verify(o => o.CloseDoc(filePath), Times.Once);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void DisposeDocument_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);

            swApplicationMock.Setup(o => o.GetFirstDocument()).Returns((object)swModelMock.Object);
            swModelMock.Setup(o => o.GetNext()).Returns(null);
            swModelMock.Setup(o => o.GetTitle()).Returns("Model1");
            documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);

            var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

            var dict = (Dictionary<IModelDoc2, ISwDocument>?)dm.GetPrivateObject("openDocuments");
            Assert.IsNotNull(dict);
            Assert.AreEqual(1, dict.Count);

            dm.DisposeDocument(mockDocument);

            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void Dispose_Test()
        {
            var swApplicationMock = new Mock<ISldWorks>();
            var swModelMock = new Mock<ISwPartDoc>();
            var documentFactoryMock = new Mock<ISwDocumentFactory>();
            var equalityComparer = EqualityComparer<IModelDoc2>.Default;
            var mockDocument = new SwMockDocument(swModelMock.Object);

            swApplicationMock.Setup(o => o.GetFirstDocument()).Returns((object)swModelMock.Object);
            swModelMock.Setup(o => o.GetNext()).Returns(null);
            swModelMock.Setup(o => o.GetTitle()).Returns("Model1");
            documentFactoryMock.Setup(o => o.Create(It.IsAny<IModelDoc2>())).Returns(mockDocument);

            var dm = new DocumentManager(swApplicationMock.Object, documentFactoryMock.Object, equalityComparer);

            var dict = (Dictionary<IModelDoc2, ISwDocument>?)dm.GetPrivateObject("openDocuments");
            Assert.IsNotNull(dict);
            Assert.AreEqual(1, dict.Count);

            dm.Dispose();

            Assert.AreEqual(0, dict.Count);
        }


        delegate void OpenDoc6Delegate(string FileName, int Type, int Options, string Configuration, ref int Errors, ref int Warnings);
    }
}
