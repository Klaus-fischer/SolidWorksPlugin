namespace SIM.SolidWorksPlugin.Tests.Commands
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SIM.SolidWorksPlugin.Commands;
    using System;

    [TestClass]
    public class RelaySwCommand_Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var cmd = new RelaySwCommand(d => { }, d => true);
            Assert.IsNotNull(cmd);

            cmd = new RelaySwCommand(d => { }, null);
            Assert.IsNotNull(cmd);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Fail()
        {
            new RelaySwCommand(null, null);
        }

        [TestMethod]
        public void CanExecuteTest()
        {
            var cmd = new RelaySwCommand(d => { });

            Assert.IsTrue(cmd.CanExecute(null));

            bool invoked = false;
            Func<SwDocument?, bool> canExecuteCallback = d =>
            {
                invoked = true;
                return false;
            };

            cmd = new RelaySwCommand(d => { }, canExecuteCallback);
            Assert.IsFalse(cmd.CanExecute(null));
            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void OnExecuteTest()
        {
            bool invoked = false;

            Action<SwDocument?> onExecute = d => invoked = true;

            var cmd = new RelaySwCommand(onExecute);

            cmd.Execute(null);
            Assert.IsTrue(invoked);
        }
    }
}
