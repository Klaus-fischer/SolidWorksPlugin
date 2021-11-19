// <copyright file="CommandInfo.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The return value of the <see cref="ICommandHandler{T}.RegisterCommand(T, ISwCommand)"/> method.
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class CommandInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInfo"/> class.
        /// </summary>
        /// <param name="command">The <see cref="Command"/>.</param>
        /// <param name="id">The <see cref="Id"/>.</param>
        /// <param name="commandGroupId">The <see cref="CommandGroupId"/>.</param>
        /// <param name="name">The <see cref="Name"/>.</param>
        /// <param name="userId">The <see cref="UserId"/>.</param>
        /// <param name="imageIndex">The <see cref="ImageIndex"/>.</param>
        internal CommandInfo(ISwCommand command, int id, int commandGroupId, string name,  int userId, int imageIndex)
        {
            this.Command = command ?? throw new ArgumentNullException(nameof(command));
            this.Id = id;
            this.CommandGroupId = commandGroupId;
            this.Name = name;
            this.UserId = userId;
            this.ImageIndex = imageIndex;
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        public ISwCommand Command { get; }

        /// <summary>
        /// Gets the command id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the command group.
        /// </summary>
        public int CommandGroupId { get; }

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the user id of the command.
        /// </summary>
        public int UserId { get;  }

        /// <summary>
        /// Gets the image index.
        /// </summary>
        public int ImageIndex { get; }
    }
}
