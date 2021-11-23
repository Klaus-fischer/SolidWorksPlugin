// <copyright file="ICommandHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Interface to allow access to commands.
    /// </summary>
    internal interface ICommandHandler : IDisposable
    {
        /// <summary>
        /// Gets the command based on its name.
        /// </summary>
        /// <param name="name">Name of the command.</param>
        /// <returns>The command if defined, or null.</returns>
        ICommandInfo? GetCommand(string name);

        ICommandInfo? GetCommand<T>(T id)
            where T : struct, Enum;
    }
}
