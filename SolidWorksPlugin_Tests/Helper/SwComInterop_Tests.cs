namespace SIM.SolidWorksPlugin.Tests.Helper
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Win32;
    using System;
    using System.Runtime.InteropServices;

    [TestClass]
    [Guid(GUID)]
    [System.ComponentModel.DisplayName(Name)]
    [System.ComponentModel.Description(Description)]
    public class SwComInterop_Tests
    {
        const string GUID = "4EE22462-7058-4A64-A492-0D19905BC783";
        const string Name = "RegisterTestClass";
        const string Description = "Description of TestClass";

        [TestMethod]
        public void Register_Test()
        {
            try
            {
                var rootKey = MockRegistry();

                SwComInterop.RegisterToKey(rootKey, typeof(SwComInterop_Tests));

                var key = rootKey.OpenSubKey(@$"SOFTWARE\SolidWorks\Addins\{{{GUID}}}");
                Assert.IsNotNull(key);
                Assert.AreEqual(Name, key.GetValue("Title"));
                Assert.AreEqual(Description, key.GetValue("Description"));
            }
            finally
            {
                this.CleanupRegistry();
            }
        }

        [TestMethod]
        public void Register_Fail()
        {
            try
            {
                var rootKey = MockRegistry(true);

                SwComInterop.RegisterToKey(rootKey, typeof(SwComInterop_Tests));

                var key = rootKey.OpenSubKey(@$"SOFTWARE\SolidWorks\Addins\{{{GUID}}}");
                Assert.IsNull(key);

            }
            finally
            {
                this.CleanupRegistry();
            }
        }

        [TestMethod]
        public void Unregister_Test()
        {
            try
            {
                var rootKey = MockRegistry();

                SwComInterop.RegisterToKey(rootKey, typeof(SwComInterop_Tests));

                var key = rootKey.OpenSubKey(@$"SOFTWARE\SolidWorks\Addins\{{{GUID}}}", true);

                Assert.IsNotNull(key);

                SwComInterop.UnregisterFromKey(rootKey, typeof(SwComInterop_Tests));

                key = rootKey.OpenSubKey(@$"SOFTWARE\SolidWorks\Addins\{{{GUID}}}");

                Assert.IsNull(key);
            }
            finally
            {
                this.CleanupRegistry();
            }
        }

        [TestMethod]
        public void UnregisterEmpty_Test()
        {
            try
            {
                var rootKey = MockRegistry();

                SwComInterop.UnregisterFromKey(rootKey, typeof(SwComInterop_Tests));

                rootKey.OpenSubKey("SOFTWARE").OpenSubKey("SolidWorks", true).DeleteSubKeyTree("Addins");

                SwComInterop.UnregisterFromKey(rootKey, typeof(SwComInterop_Tests));
            }
            finally
            {
                this.CleanupRegistry();
            }
        }


        private RegistryKey MockRegistry(bool withoutCreationSubKeys = false)
        {
            var root = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("SolidWorksPlugin_Tests");

            if (!withoutCreationSubKeys)
            {
                root.CreateSubKey("SOFTWARE", true).CreateSubKey("SolidWorks", true).CreateSubKey("Addins", true);
            }

            return root;
        }

        private void CleanupRegistry()
        {
            Registry.CurrentUser.OpenSubKey("SOFTWARE", true).DeleteSubKeyTree("SolidWorksPlugin_Tests");
        }
    }
}
