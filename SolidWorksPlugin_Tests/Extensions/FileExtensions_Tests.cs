namespace SIM.SolidWorksPlugin.Tests.Extensions
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolidWorks.Interop.swconst;

    [TestClass]
    public class FileExtensions_Tests
    {
        [TestMethod]
        public void FileExtensionsValidation()
        {
            Assert.AreEqual(".SLDASM", FileExtensions.AsmExt);
            Assert.AreEqual(".SLDPRT", FileExtensions.PrtExt);
            Assert.AreEqual(".SLDDRW", FileExtensions.DrwExt);
        }

        [TestMethod]
        public void GetDocumentType_Test()
        {
            Assert.AreEqual(
                (int)swDocumentTypes_e.swDocDRAWING,
                FileExtensions.GetDocumentType("Test.slddrw"));

            Assert.AreEqual(
                (int)swDocumentTypes_e.swDocDRAWING,
                FileExtensions.GetDocumentType("Test.DRWDOT"));

            Assert.AreEqual(
                (int)swDocumentTypes_e.swDocASSEMBLY,
                FileExtensions.GetDocumentType("Test.sldasm"));
            Assert.AreEqual(
                (int)swDocumentTypes_e.swDocASSEMBLY,
                FileExtensions.GetDocumentType("Test.ASMDOT"));

            Assert.AreEqual(
                (int)swDocumentTypes_e.swDocPART,
                FileExtensions.GetDocumentType("Test.sldprt"));

            Assert.AreEqual(
                (int)swDocumentTypes_e.swDocPART,
                FileExtensions.GetDocumentType("Test.SLDLFT"));

            Assert.AreEqual(
                (int)swDocumentTypes_e.swDocPART,
                FileExtensions.GetDocumentType("Test.step"));
        }
    }
}
