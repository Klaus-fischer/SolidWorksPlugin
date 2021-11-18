namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CommandTarget
    {
        private readonly Func<string, int> canExecuteHandler;
        private readonly Action<string> onExecuteHandler;

        internal CommandTarget(Action<string> onExecuteHandler, Func<string, int> canExecuteHandler)
        {
            this.canExecuteHandler = canExecuteHandler;
            this.onExecuteHandler = onExecuteHandler;
        }

        public int CanExecute(string commandName) => this.canExecuteHandler(commandName);

        public void OnExecute(string commandName) => this.onExecuteHandler(commandName);
    }
}
