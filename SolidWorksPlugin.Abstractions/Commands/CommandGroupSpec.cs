// <copyright file="CommandGroupSpec.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Info class of a command group.
    /// </summary>
    public class CommandGroupSpec
    {
        private string? tooltip;
        private string? hint;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandGroupSpec"/> class.
        /// </summary>
        /// <param name="userId">The unique user id of the command group.</param>
        /// <param name="title">The title of the command group.</param>
        public CommandGroupSpec(int userId, string title)
        {
            this.UserId = userId;
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        /// <summary>
        /// Gets the unique user id of the command group.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Gets the tool bar id.
        /// </summary>
        public int ToolbarId { get; internal set; }

        /// <summary>
        /// Gets the title of the command group.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets or sets the path to the tool bar icons file.
        /// Use .\ or ..\ notation to navigate relative to path of the dll.
        /// Use {0} as placeholder for different sizes.
        /// </summary>
        /// <example>
        /// IconsPath = ".\Icons\Toolbar{0}.png"...
        /// </example>
        public string IconsPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the path to the main icons file.
        /// Use .\ or ..\ notation to navigate relative to path of the dll.
        /// Use {0} as placeholder for different sizes.
        /// </summary>
        /// <example>
        /// IconsPath = ".\Icons\Icon{0}.png"...
        /// </example>
        public string MainIconPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tool tip of the command group.
        /// If default, the title will be returned.
        /// </summary>
        public string Tooltip
        {
            get => this.tooltip ?? this.Title;
            set => this.tooltip = value;
        }

        /// <summary>
        /// Gets or sets the hint of the command group.
        /// If default, the tool tip will be returned.
        /// </summary>
        public string Hint
        {
            get => this.hint ?? this.Tooltip;
            set => this.hint = value;
        }

        /// <summary>
        /// Gets or sets the position of the command group.
        /// </summary>
        public int Position { get; set; } = -1;
    }
}
