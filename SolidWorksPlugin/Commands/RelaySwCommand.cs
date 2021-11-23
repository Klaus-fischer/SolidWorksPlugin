// <copyright file="RelaySwCommand.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin.Commands
{
    using System;

    /// <summary>
    /// Relay command for easy access.
    /// </summary>
    public class RelaySwCommand : ISwCommand
    {
        private readonly Func<ISwDocument?, bool> canExecute;
        private readonly Action<ISwDocument?> onExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelaySwCommand"/> class.
        /// </summary>
        /// <param name="onExecute">Handler to execute command.</param>
        /// <param name="canExecute">Handler to validate command state.</param>
        public RelaySwCommand(Action<ISwDocument?> onExecute, Func<ISwDocument?, bool>? canExecute = null)
        {
            this.onExecute = onExecute ?? throw new ArgumentNullException(nameof(onExecute));
            this.canExecute = canExecute ?? new Func<ISwDocument?, bool>(d => true);
        }

        /// <inheritdoc/>
        public bool CanExecute(ISwDocument? document) => this.canExecute(document);

        /// <inheritdoc/>
        public void Execute(ISwDocument? document) => this.onExecute(document);
    }
}
