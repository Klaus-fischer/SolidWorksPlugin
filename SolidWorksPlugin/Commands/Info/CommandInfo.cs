// <copyright file="CommandInfo.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Diagnostics;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// The return value of the <see cref="ICommandGroupBuilder{T}.AddCommand(T, ISwCommand)"/> method.
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class CommandInfo : ICommandInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInfo"/> class.
        /// </summary>
        /// <param name="commandGroupId">The <see cref="CommandGroupId"/>.</param>
        /// <param name="name">The <see cref="Name"/>.</param>
        /// <param name="userId">The <see cref="UserId"/>.</param>
        public CommandInfo(int userId, string name, int commandGroupId)
        {
            this.CommandGroupId = commandGroupId;
            this.Name = name;
            this.UserId = userId;
        }

        /// <summary>
        /// Gets the command id.
        /// </summary>
        int ICommandInfo.Id => this.Id;

        /// <summary>
        /// Gets the command.
        /// </summary>
        ISwCommand ICommandInfo.Command => this.Command;

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
        /// Gets or sets the tool-tip of the command.
        /// </summary>
        public string? Tooltip { get; set; }

        /// <summary>
        /// Gets or sets the hint of the command.
        /// </summary>
        public string? Hint { get; set; }

        /// <summary>
        /// Gets or sets the command id.
        /// </summary>
        internal int Id { get; set; }

        /// <summary>
        /// Gets or sets the command id.
        /// </summary>
        internal ISwCommand Command { get; set; }

        /// <summary>
        /// Converts the <see cref="HasMenu"/> and <see cref="Tooltip"/> values to an <see cref="swCommandItemType_e"/> value.
        /// </summary>
        /// <returns>The command item type.</returns>
        internal int GetSwCommandItemType_e()
        {
            swCommandItemType_e menuOptions = 0;
            if (this.HasMenu == true)
            {
                menuOptions |= swCommandItemType_e.swMenuItem;
            }

            if (this.HasToolbar == true)
            {
                menuOptions |= swCommandItemType_e.swToolbarItem;
            }

            return (int)menuOptions;
        }
    }
}
