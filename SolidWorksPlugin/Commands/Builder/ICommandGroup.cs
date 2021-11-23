// <copyright file="ICommandGroup.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Interface to allow access to commands.
    /// </summary>
    internal interface ICommandGroup : IDisposable
    {
        /// <summary>
        /// Gets the command based on its name.
        /// </summary>
        /// <param name="userId">User Id of the command.</param>
        /// <returns>The command if defined, or null.</returns>
        ICommandInfo? GetCommand(int userId);
    }
}
