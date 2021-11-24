// <copyright file="CommandGroupInfo.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// The concrete implementation of the <see cref="ICommandGroupInfo"/> interface.
    /// </summary>
    internal class CommandGroupInfo : ICommandGroupInfo
    {
        /// <inheritdoc/>
        public int UserId { get; set; }

        /// <inheritdoc/>
        public int ToolbarId { get; set; }

        /// <inheritdoc/>
        public string Title { get; set; } = string.Empty;
    }
}
