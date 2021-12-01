namespace SIM.SolidWorksPlugin.Tests.Documents.DocumentsTypes
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    [TestClass]
    public class SwAssembly_Tests
    {
        public interface AsmDoc : AssemblyDoc, IModelDoc2 { };

        [TestMethod]
        public void Constructor()
        {
            var asmModel = new Mock<AsmDoc>();
            var asm = new SwAssembly(asmModel.Object, null);
            Assert.IsNotNull(asm);
            Assert.AreEqual(asmModel.Object, asm.Model);
            Assert.AreEqual(asmModel.Object, asm.Assembly);
        }

        [TestMethod]
        public void OnFileSaveNotify_Test()
        {
            bool invoked = false;
            var asmModel = new Mock<AsmDoc>();

            var asm = new SwAssembly(asmModel.Object, null);
            asm.OnFileSave += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(asm, s);
                Assert.AreEqual("filename", a);

                return 0;
            };

            asmModel.Raise(o => o.FileSaveNotify += null, "filename");
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnFileSaveAsNotify2_Test()
        {
            bool invoked = false;
            var asmModel = new Mock<AsmDoc>();
            var asm = new SwAssembly(asmModel.Object, null);

            asm.OnPreviewSaveAs += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(asm, s);
                Assert.AreEqual("filename", a);

                return 0;
            };

            asmModel.Raise(o => o.FileSaveAsNotify2 += null, "filename");
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnFileSavePostNotify_Test()
        {
            bool invoked = false;
            var asmModel = new Mock<AsmDoc>();
            var asm = new SwAssembly(asmModel.Object, null);
            asm.OnPostSaveAs += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(asm, s);
                Assert.AreEqual("filename", a.Filename);
                Assert.AreEqual(swFileSaveTypes_e.swFileSaveAsCopy, a.SaveType);

                return 0;
            };

            asmModel.Raise(o => o.FileSavePostNotify += null, (int)swFileSaveTypes_e.swFileSaveAsCopy, "filename");
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnAddCustomPropertyNotify_Test()
        {
            bool invoked = false;

            var asmModel = new Mock<AsmDoc>();
            var asm = new SwAssembly(asmModel.Object, null);
            asm.OnPropertyChanged += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(asm, s);
                Assert.AreEqual("propName", a.PropertyName);
                Assert.AreEqual("configuration", a.Configuration);
                Assert.AreEqual("newValue", a.NewValue);
                Assert.AreEqual(null, a.OldValue);
                Assert.AreEqual(swCustomInfoType_e.swCustomInfoText, a.ValueType);

                return 0;
            };

            asmModel.Raise(o => o.AddCustomPropertyNotify += null, "propName", "configuration", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnChangeCustomPropertyNotify_Test()
        {
            bool invoked = false;
            var asmModel = new Mock<AsmDoc>();
            var asm = new SwAssembly(asmModel.Object, null);
            asm.OnPropertyChanged += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(asm, s);
                Assert.AreEqual("propName", a.PropertyName);
                Assert.AreEqual("configuration", a.Configuration);
                Assert.AreEqual("newValue", a.NewValue);
                Assert.AreEqual("oldValue", a.OldValue);
                Assert.AreEqual(swCustomInfoType_e.swCustomInfoText, a.ValueType);

                return 0;
            };

            asmModel.Raise(o => o.ChangeCustomPropertyNotify += null, "propName", "configuration", "oldValue", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnDeleteCustomPropertyNotify_Test()
        {
            bool invoked = false;
            var asmModel = new Mock<AsmDoc>();
            var asm = new SwAssembly(asmModel.Object, null);
            asm.OnPropertyChanged += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(asm, s);
                Assert.AreEqual("propName", a.PropertyName);
                Assert.AreEqual("configuration", a.Configuration);
                Assert.AreEqual(null, a.NewValue);
                Assert.AreEqual("oldValue", a.OldValue);
                Assert.AreEqual(swCustomInfoType_e.swCustomInfoText, a.ValueType);

                return 0;
            };

            asmModel.Raise(o => o.DeleteCustomPropertyNotify += null, "propName", "configuration", "oldValue", (int)swCustomInfoType_e.swCustomInfoText);

            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void OnDestroyNotify2_Test()
        {
            bool invoked = false;
            var asmModel = new Mock<AsmDoc>();
            var asm = new SwAssembly(asmModel.Object, null);
            asm.OnDestroy += (s, a) =>
            {
                invoked = true;
                Assert.AreSame(asm, s);
                Assert.AreEqual(swDestroyNotifyType_e.swDestroyNotifyHidden, a);

                return 0;
            };

            asmModel.Raise(o => o.DestroyNotify2 += null, (int)swDestroyNotifyType_e.swDestroyNotifyHidden);
            Assert.AreEqual(true, invoked);
        }

        [TestMethod]
        public void Dispose_Test()
        {
            bool invoked = false;
            var asmModel = new Mock<AsmDoc>();

            var asm = new SwAssembly(asmModel.Object, null);

            asmModel.Raise(o => o.FileSaveNotify += null, "filename");
            asmModel.Raise(o => o.FileSaveAsNotify2 += null, "filename");
            asmModel.Raise(o => o.FileSavePostNotify += null, (int)swFileSaveTypes_e.swFileSaveAsCopy, "filename");
            asmModel.Raise(o => o.AddCustomPropertyNotify += null, "propName", "configuration", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            asmModel.Raise(o => o.ChangeCustomPropertyNotify += null, "propName", "configuration", "oldValue", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            asmModel.Raise(o => o.DeleteCustomPropertyNotify += null, "propName", "configuration", "oldValue", (int)swCustomInfoType_e.swCustomInfoText);
            asmModel.Raise(o => o.DestroyNotify2 += null, (int)swDestroyNotifyType_e.swDestroyNotifyHidden);

            asm.OnFileSave += (s, a) => { invoked = true; return 0; };
            asm.OnPreviewSaveAs += (s, a) => { invoked = true; return 0; };
            asm.OnPostSaveAs += (s, a) => { invoked = true; return 0; };
            asm.OnPropertyChanged += (s, a) => { invoked = true; return 0; };
            asm.OnPropertyChanged += (s, a) => { invoked = true; return 0; };
            asm.OnPropertyChanged += (s, a) => { invoked = true; return 0; };
            asm.OnDestroy += (s, a) => { invoked = true; return 0; };

            asm.Dispose();

            asmModel.Raise(o => o.FileSaveNotify += null, "filename");
            asmModel.Raise(o => o.FileSaveAsNotify2 += null, "filename");
            asmModel.Raise(o => o.FileSavePostNotify += null, (int)swFileSaveTypes_e.swFileSaveAsCopy, "filename");
            asmModel.Raise(o => o.AddCustomPropertyNotify += null, "propName", "configuration", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            asmModel.Raise(o => o.ChangeCustomPropertyNotify += null, "propName", "configuration", "oldValue", "newValue", (int)swCustomInfoType_e.swCustomInfoText);
            asmModel.Raise(o => o.DeleteCustomPropertyNotify += null, "propName", "configuration", "oldValue", (int)swCustomInfoType_e.swCustomInfoText);
            asmModel.Raise(o => o.DestroyNotify2 += null, (int)swDestroyNotifyType_e.swDestroyNotifyHidden);

            Assert.IsFalse(invoked);
        }
    }
}
