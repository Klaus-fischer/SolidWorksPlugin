// <copyright file="CommandInfoAttribute.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// Attribute to describe a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class CommandInfoAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInfoAttribute"/> class.
        /// </summary>
        /// <param name="title">Title of the command.</param>
        public CommandInfoAttribute(string title)
        {
            this.Name = title ?? throw new ArgumentNullException(nameof(title));
        }

        /// <summary>
        /// Gets or sets the image index of the command.
        /// </summary>
        public int ImageIndex { get; set; } = -1;

        /// <summary>
        /// Gets or sets a value indicating whether command should be visible in the menu.
        /// </summary>
        public bool HasMenu { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether command should be visible on the tool bar.
        /// </summary>
        public bool HasToolbar { get; set; } = true;

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the position in the tool bar.
        /// Default is -1, for appending the tool bar.
        /// </summary>
        public int Position { get; set; } = -1;

        /// <summary>
        /// Gets or sets the tool-tip of the command.
        /// </summary>
        public string Tooltip { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the hint of the command.
        /// </summary>
        public string Hint { get; set; } = string.Empty;
    }
}
