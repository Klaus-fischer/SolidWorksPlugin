namespace SIM.SolidWorksPlugin.Tests.Attributes
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SolidWorksPluginAttribute_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var spa = new SolidWorksPluginAttribute("title");
            Assert.IsNotNull(spa);
            Assert.AreEqual("title", spa.Title);
            Assert.IsNull(spa.Description);
        }

        [TestMethod]
        public void ConstructorAndParamterAssignment()
        {
            var spa = new SolidWorksPluginAttribute("title")
            {
                Description = "Description",
            };

            Assert.IsNotNull(spa);
            Assert.AreEqual("title", spa.Title);
            Assert.AreEqual("Description", spa.Description);
        }
    }
}
