// <copyright file="CommandSpec.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System.Diagnostics;

    /// <summary>
    /// The return value of the <see cref="ICommandGroupBuilder{T}.AddCommand(T, ISwCommand)"/> method.
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class CommandSpec
    {
        private string? tooltip;
        private string? hint;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandSpec"/> class.
        /// </summary>
        /// <param name="commandGroupId">The <see cref="CommandGroupId"/>.</param>
        /// <param name="name">The <see cref="Name"/>.</param>
        /// <param name="userId">The <see cref="UserId"/>.</param>
        public CommandSpec(int userId, string name, int commandGroupId)
        {
            this.CommandGroupId = commandGroupId;
            this.Name = name;
            this.UserId = userId;
        }

        /// <summary>
        /// Gets the user id of the command.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the command group.
        /// </summary>
        public int CommandGroupId { get; }

        /// <summary>
        /// Gets or sets the image index.
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
        /// Gets or sets the position in the tool bar.
        /// Default is -1, for appending the tool bar.
        /// </summary>
        public int Position { get; set; } = -1;

        /// <summary>
        /// Gets or sets the tool tip of the command group.
        /// If default, the title will be returned.
        /// </summary>
        public string Tooltip
        {
            get => this.tooltip ?? this.Name;
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
    }
}
