// <copyright file="CommandGroupInfo.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Info class of a command group.
    /// </summary>
    public class CommandGroupInfo
    {
        private string? tooltip;
        private string? hint;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandGroupInfo"/> class.
        /// </summary>
        /// <param name="id">The unique user id of the command group.</param>
        /// <param name="title">The title of the command group.</param>
        public CommandGroupInfo(int id, string title)
        {
            this.Id = id;
            this.Title = title;
        }

        /// <summary>
        /// Gets the unique user id of the command group.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the title of the command group.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets or sets the collection of file path to the tool bar icons files.
        /// </summary>
        public string[] Icons { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Gets or sets the collection of file path to the main icon files.
        /// </summary>
        public string[] MainIcon { get; set; } = Array.Empty<string>();

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
        public string Hint { get => this.hint ?? this.Tooltip; set => this.hint = value; }

        /// <summary>
        /// Gets or sets the position of the command group.
        /// </summary>
        public int Position { get; set; } = -1;
    }
}
