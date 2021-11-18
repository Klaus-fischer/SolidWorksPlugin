// <copyright file="ICommand.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    public interface ISwCommand
    {
        /// <summary>
        /// Gets the command title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the tool-tip of the command.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Validates if command can be executed.
        /// </summary>
        /// <returns>True on valid call.</returns>
        /// <param name="document">The current active document.</param>
        bool CanExecute(SwDocument? document);

        /// <summary>
        /// Execute method of the command.
        /// </summary>
        /// <param name="document">The current active document.</param>
        void Execute(SwDocument? document);
    }
}
