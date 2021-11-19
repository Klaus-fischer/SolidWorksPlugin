namespace SIM.SolidWorksPlugin.Tests.Documents.DocumentsTypes
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    [TestClass]
    public class SwPart_Tests
    {
        public interface PrtDoc : PartDoc, ModelDoc2 { };

        [TestMethod]
        public void Constructor()
        {
            var prtModel = new Mock<PrtDoc>();
            var prt = new SwPart(prtModel.Object);
            Assert.IsNotNull(prt);
            Assert.AreEqual(prtModel.Object, prt.Model);
            Assert.AreEqual(prtModel.Object, prt.Part);
        }

        [TestMethod]
        public void OnFileSaveNotify_Test()
        {
            bool invoked = false;
            var prtModel = new Mock<PrtDoc>();

            var prt = new SwPart(prtModel.Object);
            prt.OnFileSave += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(prt, s);
                Assert.AreEqual("filename", a);

                return 0;
            };

            prtModel.Raise(o => o.FileSaveNotify += null, "filename");
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnFileSaveAsNotify2_Test()
        {
            bool invoked = false;
            var prtModel = new Mock<PrtDoc>();
            var prt = new SwPart(prtModel.Object);

            prt.OnPreviewSaveAs += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(prt, s);
                Assert.AreEqual("filename", a);

                return 0;
            };

            prtModel.Raise(o => o.FileSaveAsNotify2 += null, "filename");
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnFileSavePostNotify_Test()
        {
            bool invoked = false;
            var prtModel = new Mock<PrtDoc>();
            var prt = new SwPart(prtModel.Object);
            prt.OnPostSaveAs += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(prt, s);
                Assert.AreEqual("filename", a.Filename);
                Assert.AreEqual(swFileSaveTypes_e.swFileSaveAsCopy, a.SaveType);

                return 0;
            };

            prtModel.Raise(o => o.FileSavePostNotify += null, (int)swFileSaveTypes_e.swFileSaveAsCopy, "filename");
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnAddCustomPropertyNotify_Test()
        {
            bool invoked = false;

            var prtModel = new Mock<PrtDoc>();
            var prt = new SwPart(prtModel.Object);
            prt.OnPropertyChanged += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(prt, s);
                Assert.AreEqual("propName", a.PropertyName);
                Assert.AreEqual("configuration", a.Configuration);
                Assert.AreEqual("newValue", a.NewValue);
                Assert.AreEqual(null, a.OldValue);
                Assert.AreEqual(swCustomInfoType_e.swCustomInfoText, a.ValueType);

                return 0;
            };

            prtModel.Raise(o => o.AddCustomPropertyNotify += null, "propName", "configuration", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnChangeCustomPropertyNotify_Test()
        {
            bool invoked = false;
            var prtModel = new Mock<PrtDoc>();
            var prt = new SwPart(prtModel.Object);
            prt.OnPropertyChanged += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(prt, s);
                Assert.AreEqual("propName", a.PropertyName);
                Assert.AreEqual("configuration", a.Configuration);
                Assert.AreEqual("newValue", a.NewValue);
                Assert.AreEqual("oldValue", a.OldValue);
                Assert.AreEqual(swCustomInfoType_e.swCustomInfoText, a.ValueType);

                return 0;
            };

            prtModel.Raise(o => o.ChangeCustomPropertyNotify += null, "propName", "configuration", "oldValue", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnDeleteCustomPropertyNotify_Test()
        {
            bool invoked = false;
            var prtModel = new Mock<PrtDoc>();
            var prt = new SwPart(prtModel.Object);
            prt.OnPropertyChanged += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(prt, s);
                Assert.AreEqual("propName", a.PropertyName);
                Assert.AreEqual("configuration", a.Configuration);
                Assert.AreEqual(null, a.NewValue);
                Assert.AreEqual("oldValue", a.OldValue);
                Assert.AreEqual(swCustomInfoType_e.swCustomInfoText, a.ValueType);

                return 0;
            };

            prtModel.Raise(o => o.DeleteCustomPropertyNotify += null, "propName", "configuration", "oldValue", (int)swCustomInfoType_e.swCustomInfoText);

            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnDestroyNotify2_Test()
        {
            bool invoked = false;
            var prtModel = new Mock<PrtDoc>();
            var prt = new SwPart(prtModel.Object);
            prt.OnDestroy += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(prt, s);
                Assert.AreEqual(swDestroyNotifyType_e.swDestroyNotifyHidden, a);

                return 0;
            };

            prtModel.Raise(o => o.DestroyNotify2 += null, (int)swDestroyNotifyType_e.swDestroyNotifyHidden);
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void Dispose_Test()
        {
            bool invoked = false;
            var prtModel = new Mock<PrtDoc>();

            var prt = new SwPart(prtModel.Object);

            prtModel.Raise(o => o.FileSaveNotify += null, "filename");
            prtModel.Raise(o => o.FileSaveAsNotify2 += null, "filename");
            prtModel.Raise(o => o.FileSavePostNotify += null, (int)swFileSaveTypes_e.swFileSaveAsCopy, "filename");
            prtModel.Raise(o => o.AddCustomPropertyNotify += null, "propName", "configuration", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            prtModel.Raise(o => o.ChangeCustomPropertyNotify += null, "propName", "configuration", "oldValue", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            prtModel.Raise(o => o.DeleteCustomPropertyNotify += null, "propName", "configuration", "oldValue", (int)swCustomInfoType_e.swCustomInfoText);
            prtModel.Raise(o => o.DestroyNotify2 += null, (int)swDestroyNotifyType_e.swDestroyNotifyHidden);

            prt.OnFileSave += (s, a) => { invoked = true; return 0; };
            prt.OnPreviewSaveAs += (s, a) => { invoked = true; return 0; };
            prt.OnPostSaveAs += (s, a) => { invoked = true; return 0; };
            prt.OnPropertyChanged += (s, a) => { invoked = true; return 0; };
            prt.OnPropertyChanged += (s, a) => { invoked = true; return 0; };
            prt.OnPropertyChanged += (s, a) => { invoked = true; return 0; };
            prt.OnDestroy += (s, a) => { invoked = true; return 0; };

            prt.Dispose();

            prtModel.Raise(o => o.FileSaveNotify += null, "filename");
            prtModel.Raise(o => o.FileSaveAsNotify2 += null, "filename");
            prtModel.Raise(o => o.FileSavePostNotify += null, (int)swFileSaveTypes_e.swFileSaveAsCopy, "filename");
            prtModel.Raise(o => o.AddCustomPropertyNotify += null, "propName", "configuration", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            prtModel.Raise(o => o.ChangeCustomPropertyNotify += null, "propName", "configuration", "oldValue", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            prtModel.Raise(o => o.DeleteCustomPropertyNotify += null, "propName", "configuration", "oldValue", (int)swCustomInfoType_e.swCustomInfoText);
            prtModel.Raise(o => o.DestroyNotify2 += null, (int)swDestroyNotifyType_e.swDestroyNotifyHidden);

            Assert.IsFalse(invoked);
        }
    }
}
