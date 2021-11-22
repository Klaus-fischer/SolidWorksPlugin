namespace SIM.SolidWorksPlugin.Tests.Documents
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.sldworks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class SwDocumentFactory_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var factory = new SwDocumentFactory();
            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void CreateAssembly()
        {
            var model = new Mock<DocumentsTypes.SwAssembly_Tests.AsmDoc>();
            var factory = new SwDocumentFactory();
            var document = factory.Create(model.Object);

            Assert.IsTrue(document is SwAssembly);
        }

        [TestMethod]
        public void CreateDrawing()
        {
            var model = new Mock<DocumentsTypes.SwDrawing_Tests.DrwDoc>();
            var factory = new SwDocumentFactory();
            var document = factory.Create(model.Object);

            Assert.IsTrue(document is SwDrawing);
        }

        [TestMethod]
        public void CreatePart()
        {
            var model = new Mock<DocumentsTypes.SwPart_Tests.PrtDoc>();
            var factory = new SwDocumentFactory();
            var document = factory.Create(model.Object);

            Assert.IsTrue(document is SwPart);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Create_Fail()
        {
            var model = new Mock<IModelDoc2>();
            var factory = new SwDocumentFactory();
            factory.Create(model.Object);
        }

        [TestMethod]
        public void PropertyManagerExchange_Test()
        {
            var asmModel = new Mock<DocumentsTypes.SwAssembly_Tests.AsmDoc>();
            asmModel.Setup(o => o.get_SummaryInfo(0)).Returns("Assembly");
            var prtModel = new Mock<DocumentsTypes.SwPart_Tests.PrtDoc>();
            prtModel.Setup(o => o.get_SummaryInfo(0)).Returns("Part");
            var drwModel = new Mock<DocumentsTypes.SwDrawing_Tests.DrwDoc>();
            drwModel.Setup(o => o.get_SummaryInfo(0)).Returns("Drawing");

            var factory = new SwDocumentFactory();

            var assembly = factory.Create(asmModel.Object);
            var part = factory.Create(prtModel.Object);
            var drawing = factory.Create(drwModel.Object);

            var propMan = typeof(SwDocumentFactory).GetField("propertyManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(factory);

            // compare accessors
            Assert.AreSame(propMan, assembly.Properties);
            Assert.AreSame(propMan, part.Properties);
            Assert.AreSame(propMan, drawing.Properties);

            // check random access.
            Assert.AreEqual("Assembly", assembly.Properties.Title);
            Assert.AreEqual("Part", part.Properties.Title);
            Assert.AreEqual("Drawing", drawing.Properties.Title);
        }
    }
}
