// <copyright file="CommandInfo.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Diagnostics;
    using SolidWorks.Interop.swconst;

    [DebuggerDisplay("{Command.Title}")]
    internal class CommandInfo : ICommandInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInfo"/> class.
        /// </summary>
        /// <param name="command">Command to point to.</param>
        /// <param name="id">Id of the target command.</param>
        public CommandInfo(ISwCommand command, int id)
        {
            this.Command = command ?? throw new ArgumentNullException(nameof(command));
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the order of the command.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the image index.
        /// </summary>
        public int ImageIndex { get; set; }

        /// <summary>
        /// Gets or sets <see cref="swCommandItemType_e"/> for item menu.
        /// 0 means no toolbox and menu entry.
        /// </summary>
        public swCommandItemType_e MenuOptions { get; set; }

        /// <inheritdoc/>
        public ISwCommand Command { get; }

        /// <inheritdoc/>
        public int Id { get; }
    }
}
