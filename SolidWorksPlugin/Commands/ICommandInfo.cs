// <copyright file="ICommandInfo.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    public interface ICommandInfo
    {
        /// <summary>
        /// Gets the order of the command.
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// Gets the image index.
        /// </summary>
        int ImageIndex { get; }

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
