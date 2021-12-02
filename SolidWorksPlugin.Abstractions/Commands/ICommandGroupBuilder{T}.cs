// <copyright file="ICommandGroupBuilder{T}.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Declares command builder with for command enumerations.
    /// </summary>
    /// <typeparam name="T">Type of the command enumeration.</typeparam>
    public interface ICommandGroupBuilder<T>
        where T : struct, Enum
    {
        /// <summary>
        /// Registers a command to the add-in.
        /// </summary>
        /// <param name="id">The unique command id. (Must be an enumeration).</param>
        /// <param name="command">The command to register.</param>
        /// <returns>The command info of the entry.</returns>
        ICommandInfo AddCommand(T id, ISwCommand command);

        /// <summary>
        /// Adds a separator to a command group.
        /// </summary>
        void AddSeparator();
    }
}
