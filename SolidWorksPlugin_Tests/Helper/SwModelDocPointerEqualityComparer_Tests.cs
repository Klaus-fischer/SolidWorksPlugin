namespace SIM.SolidWorksPlugin.Tests.Helper
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class SwModelDocPointerEqualityComparer_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var swApp = new Mock<ISldWorks>();
            var equalityComparer = new SwModelDocPointerEqualityComparer(swApp.Object);
            Assert.IsNotNull(equalityComparer);
            var swApplication = typeof(SwPointerEqualityComparer)
                .GetField("swApplication", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(equalityComparer);

            Assert.AreSame(swApp.Object, swApplication);
        }

        [TestMethod]
        public void Equals_Test()
        {
            #region Setup Mock

            var swModel1 = new Mock<IModelDoc2>();
            var swModel2 = new Mock<IModelDoc2>();
            var swModel3 = new Mock<IModelDoc2>();

            swModel1.Setup(o => o.GetTitle()).Returns("Titel 1");
            swModel2.Setup(o => o.GetTitle()).Returns("Titel 2");
            swModel3.Setup(o => o.GetTitle()).Returns("Titel 3");

            var swApp = new Mock<ISldWorks>();
            swApp.Setup(o =>
                o.IsSame(
                    It.Is<object>(o => o == swModel1.Object),
                    It.Is<object>(o => o == swModel2.Object)))
                .Returns((int)swObjectEquality.swObjectNotSame); // not same

            swApp.Setup(o =>
                o.IsSame(
                    It.Is<object>(o => o == swModel1.Object),
                    It.Is<object>(o => o == swModel1.Object)))
                .Returns((int)swObjectEquality.swObjectSame); // same

            swApp.Setup(o =>
               o.IsSame(
                   It.Is<object>(o => o == swModel1.Object),
                   It.Is<object>(o => o == swModel3.Object)))
               .Returns((int)swObjectEquality.swObjectSame); // same 

            #endregion

            var equalityComparer = new SwModelDocPointerEqualityComparer(swApp.Object);

            Assert.IsTrue(equalityComparer.Equals(null, null));

            Assert.IsFalse(equalityComparer.Equals(swModel1.Object, null));
            Assert.IsFalse(equalityComparer.Equals(null, swModel2.Object));

            Assert.IsTrue(equalityComparer.Equals(swModel1.Object, swModel1.Object));
            Assert.IsFalse(equalityComparer.Equals(swModel1.Object, swModel2.Object));

            Assert.IsTrue(equalityComparer.Equals(swModel1.Object, swModel3.Object));
        }

        [TestMethod]
        public void EqualsNotAlive_Test()
        {
            #region Setup Mock

            var swModel1 = new Mock<IModelDoc2>();
            var swModel2 = new Mock<IModelDoc2>();

            swModel1.Setup(o => o.GetTitle()).Throws(new Exception());
            swModel2.Setup(o => o.GetTitle()).Returns("Titel 2");

            var swApp = new Mock<ISldWorks>();

            #endregion

            var equalityComparer = new SwModelDocPointerEqualityComparer(swApp.Object);

            Assert.IsFalse(equalityComparer.Equals(swModel1.Object, swModel2.Object));

            Assert.IsFalse(equalityComparer.Equals(swModel2.Object, swModel1.Object));
        }

        [TestMethod]
        public void EqualsIsSameCrash_Test()
        {
            #region Setup Mock

            var swModel1 = new Mock<IModelDoc2>();
            var swModel2 = new Mock<IModelDoc2>();

            swModel1.Setup(o => o.GetTitle()).Returns("Titel 1");
            swModel2.Setup(o => o.GetTitle()).Returns("Titel 2");

            var swApp = new Mock<ISldWorks>();
            swApp.Setup(o => o.IsSame(It.IsAny<object>(), It.IsAny<object>())).Throws(new Exception());

            #endregion

            var equalityComparer = new SwModelDocPointerEqualityComparer(swApp.Object);

            Assert.IsFalse(equalityComparer.Equals(swModel1.Object, swModel2.Object));
        }

        [TestMethod]
        public void EqualsNoTypeMatch_Test()
        {
            #region Setup Mock

            var swModel1 = new Mock<IModelDoc2>();
            swModel1.Setup(o => o.GetTitle()).Returns("Titel 1");

            var swApp = new Mock<ISldWorks>();

            #endregion

            var equalityComparer = new SwModelDocPointerEqualityComparer(swApp.Object);

            Assert.IsFalse(equalityComparer.Equals(swModel1.Object, 1234));
        }

        [TestMethod]
        public void GetHashCodeTest_Test()
        {
            #region Setup Mock

            var swModel1 = new Mock<IModelDoc2>();
            swModel1.Setup(o => o.GetHashCode()).Returns(12345);

            var swApp = new Mock<ISldWorks>();

            #endregion

            var equalityComparer = new SwModelDocPointerEqualityComparer(swApp.Object);

            Assert.AreEqual(12345, equalityComparer.GetHashCode(swModel1.Object));
            Assert.AreEqual(0, equalityComparer.GetHashCode(null));
        }
    }
}
