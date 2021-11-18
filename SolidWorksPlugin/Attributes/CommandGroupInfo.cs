// <copyright file="CommandGroupInfo.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

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

        public int CommandGroupId { get; }

        public string Title { get; }

        public string ToolTip { get; set; } = string.Empty;

        public string Hint { get; set; } = string.Empty;

        public int Position { get; set; } = -1;
    }

    public class CommandGroupIconsAttribute : Attribute
    {
        public string IconList { get; set; } = string.Empty;

        public string MainIconsList { get; set; } = string.Empty;
    }
}
