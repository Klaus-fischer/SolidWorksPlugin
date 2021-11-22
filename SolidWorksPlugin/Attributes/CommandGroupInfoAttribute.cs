// <copyright file="CommandGroupInfoAttribute.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Attribute to describe a command enumeration.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class CommandGroupInfoAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandGroupInfoAttribute"/> class.
        /// </summary>
        /// <param name="commandGroupId">The Id of the command group.</param>
        /// <param name="title">The title of the command group.</param>
        public CommandGroupInfoAttribute(int commandGroupId, string title)
        {
            this.CommandGroupId = commandGroupId;
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        /// <summary>
        /// Gets the command group id.
        /// </summary>
        public int CommandGroupId { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets or sets the tool tip.
        /// </summary>
        public string ToolTip { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the hint.
        /// </summary>
        public string Hint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the relative position in the command group.
        /// </summary>
        public int Position { get; set; } = -1;
    }
}
