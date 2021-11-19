namespace SIM.SolidWorksPlugin.Tests.EventArgs
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SolidWorks.Interop.swconst;
    using System;

    [TestClass]
    public class PropertyChangedEventArgs_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var args = new PropertyChangedEventArgs("propertyName", "config", "oldValue", "newValue", swCustomInfoType_e.swCustomInfoText);
            Assert.IsNotNull(args);
            Assert.AreEqual("propertyName", args.PropertyName);
            Assert.AreEqual("config", args.Configuration);
            Assert.AreEqual("oldValue", args.OldValue);
            Assert.AreEqual("newValue", args.NewValue);
            Assert.AreEqual(swCustomInfoType_e.swCustomInfoText, args.ValueType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Fail()
        {
            new PropertyChangedEventArgs("propertyName", null, "oldValue", "newValue", swCustomInfoType_e.swCustomInfoText);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Fail2()
        {
            new PropertyChangedEventArgs(null, "config", "oldValue", "newValue", swCustomInfoType_e.swCustomInfoText);
        }
    }
}
