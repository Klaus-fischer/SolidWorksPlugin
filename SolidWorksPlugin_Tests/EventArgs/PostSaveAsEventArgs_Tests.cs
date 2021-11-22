namespace SIM.SolidWorksPlugin.Tests.EventArgs
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolidWorks.Interop.swconst;
    using System;

    [TestClass]
    public class PostSaveAsEventArgs_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var args = new PostSaveAsEventArgs("filename", swFileSaveTypes_e.swFileSaveAsCopy);
            Assert.IsNotNull(args);
            Assert.AreEqual("filename", args.Filename);
            Assert.AreEqual(swFileSaveTypes_e.swFileSaveAsCopy, args.SaveType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Fail()
        {
            new PostSaveAsEventArgs(null, swFileSaveTypes_e.swFileSaveAsCopy);
        }
    }
}
