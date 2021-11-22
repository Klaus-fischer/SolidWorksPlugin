namespace SIM.SolidWorksPlugin.Tests.Documents.DocumentsTypes
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.sldworks;
    using System;

    [TestClass]
    public class SwDocument_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var model = new Mock<IModelDoc2>();
            var mock = new SwMockDocument(model.Object);

            Assert.IsNotNull(mock);
            Assert.AreSame(model.Object, mock.Model);
        }

        [TestMethod]
        public void FilePath_Test()
        {
            var model = new Mock<IModelDoc2>();
            model.Setup(o => o.GetPathName()).Returns(@"C:\Users\TestUser\Desktop\MyFile.moq");

            var mock = new SwMockDocument(model.Object);
            Assert.AreEqual(@"C:\Users\TestUser\Desktop\MyFile.moq", mock.FilePath);
            Assert.AreEqual(@"MyFile", mock.Filename);
            Assert.AreEqual(@".moq", mock.FileExtension);
        }

        [TestMethod]
        public void PropertyCallback_Test()
        {
            bool invoked = false;

            var model = new Mock<IModelDoc2>();
            var propMan = new Mock<IPropertyManager>();

            Func<IModelDoc2, IPropertyManager> callback = m =>
            {
                invoked = true;
                Assert.AreSame(model.Object, m);
                return propMan.Object;
            };

            var mock = new SwMockDocument(model.Object);
            mock.PropertyManagerCallBack = callback;

            Assert.IsFalse(invoked);
            var manager = mock.Properties;
            Assert.IsTrue(invoked);

            Assert.AreSame(propMan.Object, manager);
        }

        internal class SwMockDocument : SwDocument
        {
            public SwMockDocument(IModelDoc2 model) : base(model)
            {
            }
        }
    }
}
