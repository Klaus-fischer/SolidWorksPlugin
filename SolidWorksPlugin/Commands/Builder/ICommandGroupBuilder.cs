// <copyright file="ICommandGroupBuilder.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// Allows to add commands to a command group.
    /// </summary>
    public interface ICommandGroupBuilder
    {
        /// <summary>
        /// Adds a command to a command group.
        /// </summary>
        /// <param name="commandSpec">The command informations.</param>
        /// <param name="command">The command to add.</param>
        /// <returns>The command info including command id.</returns>
        ICommandInfo AddCommand(CommandSpec commandSpec, ISwCommand command);

        /// <summary>
        /// Adds a separator to a command group.
        /// </summary>
        void AddSeparator();
    }
}
