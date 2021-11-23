// <copyright file="ICommandHandler{T}.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Declares command manager for commands.
    /// </summary>
    /// <typeparam name="T">Type of the command enumeration.</typeparam>
    public interface ICommandHandler<T>
        where T : struct, Enum
    {
        /// <summary>
        /// Registers a command to the add-in.
        /// </summary>
        /// <param name="id">The unique command id. (Must be an enumeration).</param>
        /// <param name="command">The command to register.</param>
        /// <returns>The command info of the entry.</returns>
        ICommandInfo? RegisterCommand(T id, ISwCommand command);
    }
}
