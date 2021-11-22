namespace SIM.SolidWorksPlugin.Tests.Extensions
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.sldworks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class ISwDocumentExtensions_Tests
    {
        [TestMethod]
        public void RebuildDocument_Test()
        {
            var document = new Mock<ISwDocument>();
            var model = new Mock<IModelDoc2>();

            model.Setup(o => o.ForceRebuild3(true));
            model.Setup(o => o.ForceRebuild3(false));

            document.SetupGet(o => o.Model).Returns(model.Object);

            document.Object.RebuildDocument(true);
            model.Verify(o => o.ForceRebuild3(true), Times.Once);
            model.Verify(o => o.ForceRebuild3(false), Times.Never);

            document.Object.RebuildDocument(false);
            model.Verify(o => o.ForceRebuild3(false), Times.Once);
        }

        [TestMethod]
        public void SetSaveFlag_Test()
        {
            var document = new Mock<ISwDocument>();
            var model = new Mock<IModelDoc2>();

            model.Setup(o => o.SetSaveFlag());

            document.SetupGet(o => o.Model).Returns(model.Object);

            document.Object.SetSaveIndicatorFlag();

            model.Verify(o => o.SetSaveFlag(), Times.Once);
        }
    }
}
