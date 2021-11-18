// <copyright file="ICommandManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// Declares command manager for add in callback.
    /// </summary>
    /// <typeparam name="T">Type of the command enumeration.</typeparam>
    public interface ICommandManager<T>
        where T : struct, Enum
    {
        /// <summary>
        /// Registers a command to the add-in.
        /// </summary>
        /// <param name="id">The unique command id. (Must be an enumeration).</param>
        /// <param name="command">The command to register.</param>
        /// <param name="imageIndex">The index of the image in icon stripe.</param>
        /// <param name="menuOptions">The menu options of the command.</param>
        /// <returns>The command info of the entry.</returns>
        ICommandInfo RegisterCommand(T id, ISwCommand command);
    }
}
