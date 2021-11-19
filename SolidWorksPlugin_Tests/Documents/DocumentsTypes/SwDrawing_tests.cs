namespace SIM.SolidWorksPlugin.Tests.Documents.DocumentsTypes
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    [TestClass]
    public class SwDrawing_Tests
    {
        public interface DrwDoc : DrawingDoc, ModelDoc2 { };

        [TestMethod]
        public void Constructor()
        {
            var drwModel = new Mock<DrwDoc>();
            var drw = new SwDrawing(drwModel.Object);
            Assert.IsNotNull(drw);
            Assert.AreEqual(drwModel.Object, drw.Model);
            Assert.AreEqual(drwModel.Object, drw.Drawing);
        }

        [TestMethod]
        public void OnFileSaveNotify_Test()
        {
            bool invoked = false;
            var drwModel = new Mock<DrwDoc>();

            var drw = new SwDrawing(drwModel.Object);
            drw.OnFileSave += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(drw, s);
                Assert.AreEqual("filename", a);

                return 0;
            };

            drwModel.Raise(o => o.FileSaveNotify += null, "filename");
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnFileSaveAsNotify2_Test()
        {
            bool invoked = false;
            var drwModel = new Mock<DrwDoc>();
            var drw = new SwDrawing(drwModel.Object);

            drw.OnPreviewSaveAs += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(drw, s);
                Assert.AreEqual("filename", a);

                return 0;
            };

            drwModel.Raise(o => o.FileSaveAsNotify2 += null, "filename");
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnFileSavePostNotify_Test()
        {
            bool invoked = false;
            var drwModel = new Mock<DrwDoc>();
            var drw = new SwDrawing(drwModel.Object);
            drw.OnPostSaveAs += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(drw, s);
                Assert.AreEqual("filename", a.Filename);
                Assert.AreEqual(swFileSaveTypes_e.swFileSaveAsCopy, a.SaveType);

                return 0;
            };

            drwModel.Raise(o => o.FileSavePostNotify += null, (int)swFileSaveTypes_e.swFileSaveAsCopy, "filename");
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnAddCustomPropertyNotify_Test()
        {
            bool invoked = false;

            var drwModel = new Mock<DrwDoc>();
            var drw = new SwDrawing(drwModel.Object);
            drw.OnPropertyChanged += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(drw, s);
                Assert.AreEqual("propName", a.PropertyName);
                Assert.AreEqual("configuration", a.Configuration);
                Assert.AreEqual("newValue", a.NewValue);
                Assert.AreEqual(null, a.OldValue);
                Assert.AreEqual(swCustomInfoType_e.swCustomInfoText, a.ValueType);

                return 0;
            };

            drwModel.Raise(o => o.AddCustomPropertyNotify += null, "propName", "configuration", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnChangeCustomPropertyNotify_Test()
        {
            bool invoked = false;
            var drwModel = new Mock<DrwDoc>();
            var drw = new SwDrawing(drwModel.Object);
            drw.OnPropertyChanged += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(drw, s);
                Assert.AreEqual("propName", a.PropertyName);
                Assert.AreEqual("configuration", a.Configuration);
                Assert.AreEqual("newValue", a.NewValue);
                Assert.AreEqual("oldValue", a.OldValue);
                Assert.AreEqual(swCustomInfoType_e.swCustomInfoText, a.ValueType);

                return 0;
            };

            drwModel.Raise(o => o.ChangeCustomPropertyNotify += null, "propName", "configuration", "oldValue", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnDeleteCustomPropertyNotify_Test()
        {
            bool invoked = false;
            var drwModel = new Mock<DrwDoc>();
            var drw = new SwDrawing(drwModel.Object);
            drw.OnPropertyChanged += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(drw, s);
                Assert.AreEqual("propName", a.PropertyName);
                Assert.AreEqual("configuration", a.Configuration);
                Assert.AreEqual(null, a.NewValue);
                Assert.AreEqual("oldValue", a.OldValue);
                Assert.AreEqual(swCustomInfoType_e.swCustomInfoText, a.ValueType);

                return 0;
            };

            drwModel.Raise(o => o.DeleteCustomPropertyNotify += null, "propName", "configuration", "oldValue", (int)swCustomInfoType_e.swCustomInfoText);

            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnDestroyNotify2_Test()
        {
            bool invoked = false;
            var drwModel = new Mock<DrwDoc>();
            var drw = new SwDrawing(drwModel.Object);
            drw.OnDestroy += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(drw, s);
                Assert.AreEqual(swDestroyNotifyType_e.swDestroyNotifyHidden, a);

                return 0;
            };

            drwModel.Raise(o => o.DestroyNotify2 += null, (int)swDestroyNotifyType_e.swDestroyNotifyHidden);
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void Dispose_Test()
        {
            bool invoked = false;
            var drwModel = new Mock<DrwDoc>();

            var drw = new SwDrawing(drwModel.Object);

            drwModel.Raise(o => o.FileSaveNotify += null, "filename");
            drwModel.Raise(o => o.FileSaveAsNotify2 += null, "filename");
            drwModel.Raise(o => o.FileSavePostNotify += null, (int)swFileSaveTypes_e.swFileSaveAsCopy, "filename");
            drwModel.Raise(o => o.AddCustomPropertyNotify += null, "propName", "configuration", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            drwModel.Raise(o => o.ChangeCustomPropertyNotify += null, "propName", "configuration", "oldValue", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            drwModel.Raise(o => o.DeleteCustomPropertyNotify += null, "propName", "configuration", "oldValue", (int)swCustomInfoType_e.swCustomInfoText);
            drwModel.Raise(o => o.DestroyNotify2 += null, (int)swDestroyNotifyType_e.swDestroyNotifyHidden);

            drw.OnFileSave += (s, a) => { invoked = true; return 0; };
            drw.OnPreviewSaveAs += (s, a) => { invoked = true; return 0; };
            drw.OnPostSaveAs += (s, a) => { invoked = true; return 0; };
            drw.OnPropertyChanged += (s, a) => { invoked = true; return 0; };
            drw.OnPropertyChanged += (s, a) => { invoked = true; return 0; };
            drw.OnPropertyChanged += (s, a) => { invoked = true; return 0; };
            drw.OnDestroy += (s, a) => { invoked = true; return 0; };

            drw.Dispose();

            drwModel.Raise(o => o.FileSaveNotify += null, "filename");
            drwModel.Raise(o => o.FileSaveAsNotify2 += null, "filename");
            drwModel.Raise(o => o.FileSavePostNotify += null, (int)swFileSaveTypes_e.swFileSaveAsCopy, "filename");
            drwModel.Raise(o => o.AddCustomPropertyNotify += null, "propName", "configuration", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            drwModel.Raise(o => o.ChangeCustomPropertyNotify += null, "propName", "configuration", "oldValue", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            drwModel.Raise(o => o.DeleteCustomPropertyNotify += null, "propName", "configuration", "oldValue", (int)swCustomInfoType_e.swCustomInfoText);
            drwModel.Raise(o => o.DestroyNotify2 += null, (int)swDestroyNotifyType_e.swDestroyNotifyHidden);

            Assert.IsFalse(invoked);
        }
    }
}
