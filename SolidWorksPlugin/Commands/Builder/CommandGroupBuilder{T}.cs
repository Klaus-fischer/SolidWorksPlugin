// <copyright file="CommandGroupBuilder{T}.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Reflection;

    /// <summary>
    /// wrapper class to implement <see cref="ICommandGroupBuilder{T}"/> functionality to <see cref="ICommandGroupBuilder"/> behavior.
    /// </summary>
    /// <typeparam name="T">Typ of the command enumeration.</typeparam>
    internal class CommandGroupBuilder<T> : ICommandGroupBuilder<T>
        where T : struct, Enum
    {
        private readonly ICommandGroupBuilder commandGroupBuilder;
        private readonly int commandGroupId;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandGroupBuilder{T}"/> class.
        /// </summary>
        /// <param name="commandGroupBuilder">The command group builder to add commands to.</param>
        /// <param name="commandGroupId">The base command group id.</param>
        public CommandGroupBuilder(ICommandGroupBuilder commandGroupBuilder, int commandGroupId)
        {
            this.commandGroupBuilder = commandGroupBuilder;
            this.commandGroupId = commandGroupId;
        }

        /// <inheritdoc/>
        public ICommandInfo AddCommand(T id, ISwCommand command)
        {
            var info = this.GetCommandInfoAttribute(id);

            var commandSpec = new CommandSpec((int)(object)id, info.Name, this.commandGroupId)
            {
                ImageIndex = info.ImageIndex,
                Position = info.Position,
                HasMenu = info.HasMenu,
                HasToolbar = info.HasToolbar,
                Hint = info.Hint,
                Tooltip = info.Tooltip,
            };

            return this.commandGroupBuilder.AddCommand(commandSpec, command);
        }

        /// <inheritdoc/>
        public void AddSeparator() => this.commandGroupBuilder.AddSeparator();

        private CommandSpecAttribute GetCommandInfoAttribute(T id)
        {
            if (typeof(T).GetField($"{id}")!.GetCustomAttribute<CommandSpecAttribute>() is not CommandSpecAttribute info)
            {
                info = new CommandSpecAttribute($"{id}");
            }

            return info;
        }
    }
}
