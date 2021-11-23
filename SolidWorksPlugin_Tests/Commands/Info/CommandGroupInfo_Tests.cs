namespace SIM.SolidWorksPlugin.Tests.Attributes
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;

    [TestClass]
    public class CommandGroupInfo_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var cgi = new CommandGroupInfo(42, "MyCommandGroup");
            Assert.IsNotNull(cgi);
            Assert.AreEqual(42, cgi.Id);
            Assert.AreEqual("MyCommandGroup", cgi.Title);
            Assert.IsNotNull(cgi.Hint);
            Assert.IsNotNull(cgi.Tooltip);
            Assert.AreEqual(-1, cgi.Position);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Fail()
        {
            new CommandGroupInfo(42, null);
        }

        [TestMethod]
        public void ConstructorAndParamterAssignment()
        {
            var cgi = new CommandGroupInfo(41, "MyCommandGrou1p")
            {
                Hint = "hint",
                Position = 25,
                Tooltip = "tooltip",
            };

            Assert.IsNotNull(cgi);
            Assert.AreEqual(41, cgi.Id);
            Assert.AreEqual("MyCommandGrou1p", cgi.Title);
            Assert.AreEqual("hint", cgi.Hint);
            Assert.AreEqual("tooltip", cgi.Tooltip);
            Assert.AreEqual(25, cgi.Position);
        }

        [TestMethod]
        public void ToolTipAndHint_Test()
        {
            var commandInfo = new CommandGroupInfo(1, "MyCommand");

            Assert.AreEqual("MyCommand", commandInfo.Tooltip);
            Assert.AreEqual("MyCommand", commandInfo.Hint);

            commandInfo.Tooltip = "ToolTip";

            Assert.AreEqual("ToolTip", commandInfo.Tooltip);
            Assert.AreEqual("ToolTip", commandInfo.Hint);

            commandInfo.Hint = "Hint";

            Assert.AreEqual("ToolTip", commandInfo.Tooltip);
            Assert.AreEqual("Hint", commandInfo.Hint);
        }


        [TestMethod]
        public void GetIconsList_Test()
        {
            var tempDir = this.GenerateTempIconFiles();
            try
            {
                var cgi = new CommandGroupInfo(1, "MyCommand")
                {
                    IconsPath = @$"{tempDir}\Icon{{0}}.png",
                };

                var icons = cgi.GetIconsList();
                Assert.AreEqual(5, icons.Length);

                // 16 should not be used.
                Assert.AreEqual(@$"{tempDir}\Icon20.png", icons[0]);
                Assert.AreEqual(@$"{tempDir}\Icon32.png", icons[1]);
                // 40 should be skipped
                Assert.AreEqual(@$"{tempDir}\Icon64.png", icons[2]);
                Assert.AreEqual(@$"{tempDir}\Icon96.png", icons[3]);
                Assert.AreEqual(@$"{tempDir}\Icon128.png", icons[4]);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [TestMethod]
        public void GetMainIconList_Test()
        {
            var tempDir = this.GenerateTempIconFiles();
            try
            {
                var cgi = new CommandGroupInfo(1, "MyCommand")
                {
                    MainIconPath = @$"{tempDir}\MainIcon{{0}}.png",
                };

                var icons = cgi.GetMainIconList();

                Assert.AreEqual(5, icons.Length);
                Assert.AreEqual(@$"{tempDir}\MainIcon20.png", icons[0]);
                Assert.AreEqual(@$"{tempDir}\MainIcon32.png", icons[1]);
                Assert.AreEqual(@$"{tempDir}\MainIcon40.png", icons[2]);
                // 64 should be skipped
                Assert.AreEqual(@$"{tempDir}\MainIcon96.png", icons[3]);
                Assert.AreEqual(@$"{tempDir}\MainIcon128.png", icons[4]);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [TestMethod]
        public void GetIconsListRelative_Test()
        {
            var path = Path.Combine(SolidWorksAddin.AssemblyPath, "Icon20.png");
            File.WriteAllText(path, "");

            try
            {
                var cgi = new CommandGroupInfo(1, "MyCommand")
                {
                    IconsPath = @$".\Icon{{0}}.png",
                };

                var icons = cgi.GetIconsList();
                Assert.AreEqual(1, icons.Length);
                Assert.AreEqual(@$"{SolidWorksAddin.AssemblyPath}\Icon20.png", icons[0]);
            }
            finally
            {
                File.Delete(path);
            }
        }
        private string GenerateTempIconFiles()
        {
            var dir = Path.GetTempFileName();
            File.Delete(dir);
            Directory.CreateDirectory(dir);

            // to simulate icon file that is not used.
            File.WriteAllText(Path.Combine(dir, "Icon16.png"), string.Empty);

            File.WriteAllText(Path.Combine(dir, "Icon20.png"), string.Empty);
            File.WriteAllText(Path.Combine(dir, "Icon32.png"), string.Empty);

            // to simulate not existing file.
            // File.WriteAllText(Path.Combine(dir, "Icon40.png"), string.Empty);
            File.WriteAllText(Path.Combine(dir, "Icon64.png"), string.Empty);
            File.WriteAllText(Path.Combine(dir, "Icon96.png"), string.Empty);
            File.WriteAllText(Path.Combine(dir, "Icon128.png"), string.Empty);

            File.WriteAllText(Path.Combine(dir, "MainIcon20.png"), string.Empty);
            File.WriteAllText(Path.Combine(dir, "MainIcon32.png"), string.Empty);
            File.WriteAllText(Path.Combine(dir, "MainIcon40.png"), string.Empty);

            // to simulate not existing file.
            //File.WriteAllText(Path.Combine(dir, "MainIcon64.png"), string.Empty);
            File.WriteAllText(Path.Combine(dir, "MainIcon96.png"), string.Empty);
            File.WriteAllText(Path.Combine(dir, "MainIcon128.png"), string.Empty);

            return dir;
        }
    }
}
