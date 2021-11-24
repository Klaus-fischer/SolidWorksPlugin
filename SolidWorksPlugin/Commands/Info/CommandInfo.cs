// <copyright file="CommandInfo.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// The concrete implementation of the <see cref="ICommandGroupInfo"/> interface.
    /// </summary>
    internal class CommandInfo : ICommandInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInfo"/> class.
        /// </summary>
        /// <param name="name">Name of the command.</param>
        /// <param name="command">The command.</param>
        public CommandInfo(string name, ISwCommand command)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public ISwCommand Command { get; }

        /// <inheritdoc/>
        public int Id { get; set; }

        /// <inheritdoc/>
        public int UserId { get; set; }
    }
}
