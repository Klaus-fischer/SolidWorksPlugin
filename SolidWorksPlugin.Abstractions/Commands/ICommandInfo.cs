// <copyright file="ICommandInfo.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// The read only command info.
    /// </summary>
    public interface ICommandInfo
    {
        /// <summary>
        /// Gets the order of the command.
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the command of this entry.
        /// </summary>
        ISwCommand Command { get; }

        /// <summary>
        /// Gets the command id.
        /// </summary>
        int Id { get; }
    }
}
