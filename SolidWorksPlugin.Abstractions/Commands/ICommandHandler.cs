// <copyright file="ICommandHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// Extends <see cref="ICommandHandler"/> for internal use.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Gets the command info by group id and command user id.
        /// </summary>
        /// <param name="commandGroupId">The user id of the command group.</param>
        /// <param name="commandUserId">The user id of the command.</param>
        /// <returns>The command group.</returns>
        ICommandInfo? GetCommand(int commandGroupId, int commandUserId);

        /// <summary>
        /// Gets the command group by user id.
        /// </summary>
        /// <param name="commandGroupId">The user id of the command group.</param>
        /// <returns>The command group informations.</returns>
        ICommandGroupInfo? GetCommandGroup(int commandGroupId);
    }
}