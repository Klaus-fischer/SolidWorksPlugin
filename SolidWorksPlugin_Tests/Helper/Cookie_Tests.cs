namespace SIM.SolidWorksPlugin.Tests.Helper
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Cookie_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var cookie = new Cookie(125);

            Assert.IsNotNull(cookie);
            Assert.AreEqual(125, cookie.Value);

            // test implicit value converter.
            Assert.AreEqual(125, cookie);
        }
    }
}
