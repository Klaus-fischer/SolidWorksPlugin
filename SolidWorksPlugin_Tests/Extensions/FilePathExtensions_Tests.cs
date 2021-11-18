namespace SIM.SolidWorksPlugin.Tests.Extensions
{
    using BenchmarkDotNet.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SIM.SolidWorksPlugin.Extensions;

    [TestClass]
    [MemoryDiagnoser]
    public class FilePathExtensions_Tests
    {
        [TestMethod]
        [Benchmark]
        public void GetAbsolutePath_Test()
        {
            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\", @"Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test", @"\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\", @"\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test", @".\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\", @".\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\SubDir", @"..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\SubDir\", @"..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\SubDir", @"\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\SubDir\", @"\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\SubDir", @".\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\SubDir\", @".\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\SubDir\SubSubDir", @"..\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\SubDir\SubSubDir\", @"..\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\SubDir\SubSubDir", @"\..\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\SubDir\SubSubDir\", @"\..\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\SubDir\SubSubDir", @".\..\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePath(@"C:\Test\SubDir\SubSubDir\", @".\..\..\Class.cs"));
        }

        [TestMethod]
        [Benchmark]
        public void GetAbsolutePathSpan_Test()
        {
            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\", @"Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test", @"\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\", @"\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test", @".\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\", @".\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\SubDir", @"..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\SubDir\", @"..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\SubDir", @"\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\SubDir\", @"\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\SubDir", @".\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\SubDir\", @".\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\SubDir\SubSubDir", @"..\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\SubDir\SubSubDir\", @"..\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\SubDir\SubSubDir", @"\..\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\SubDir\SubSubDir\", @"\..\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\SubDir\SubSubDir", @".\..\..\Class.cs"));

            Assert.AreEqual(@"C:\Test\Class.cs",
                FilePathExtensions.GetAbsolutePathSpan(@"C:\Test\SubDir\SubSubDir\", @".\..\..\Class.cs"));
        }
    }
}
