// <copyright file="ICommandGroupInfo.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// The read only command group info.
    /// </summary>
    public interface ICommandGroupInfo
    {
        /// <summary>
        /// Gets the title of the command group.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the unique user id of the command group.
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// Gets the tool bar id.
        /// </summary>
        int ToolbarId { get; }
    }
}