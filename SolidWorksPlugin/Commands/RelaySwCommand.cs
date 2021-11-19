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
        private readonly Func<SwDocument?, bool> canExecute;
        private readonly Action<SwDocument?> onExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelaySwCommand"/> class.
        /// </summary>
        /// <param name="onExecute">Handler to execute command.</param>
        /// <param name="canExecute">Handler to validate command state.</param>
        public RelaySwCommand(Action<SwDocument?> onExecute, Func<SwDocument?, bool>? canExecute = null)
        {
            this.onExecute = onExecute ?? throw new ArgumentNullException(nameof(onExecute));
            this.canExecute = canExecute ?? new Func<SwDocument?, bool>(d => true);
        }

        /// <inheritdoc/>
        public bool CanExecute(SwDocument? document) => this.canExecute(document);

        /// <inheritdoc/>
        public void Execute(SwDocument? document) => this.onExecute(document);
    }
}
