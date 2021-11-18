// <copyright file="CommandHandler{T}.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Manages commands of an single command group and their behavior.
    /// </summary>
    /// <typeparam name="T">Type of the command enumeration.</typeparam>
    internal class CommandHandler<T> : ICommandHandler<T>, ICommandHandler
        where T : struct, Enum
    {
        private readonly Dictionary<T, ISwCommand> registeredCommands = new Dictionary<T, ISwCommand>();

        private readonly CommandHandler commandHandler;
        private readonly CommandGroup swCommandGroup;
        private readonly int id;
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler{T}"/> class.
        /// </summary>
        /// <param name="commandHandler">The main command handler.</param>
        /// <param name="swCommandGroup">The current command group.</param>
        /// <param name="path">The path of command group.</param>
        /// <param name="id">The command group id.</param>
        public CommandHandler(CommandHandler commandHandler, CommandGroup swCommandGroup, string path, int id)
        {
            this.swCommandGroup = swCommandGroup;
            this.id = id;
            this.commandHandler = commandHandler;
            this.path = $"{path}\\";
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!this.registeredCommands.Any())
            {
                return;
            }

            this.registeredCommands.Clear();
            this.commandHandler.SwCommandManager.RemoveCommandGroup2(this.id, true);
        }

        /// <inheritdoc/>
        public ISwCommand? GetCommand(string name)
        {
            if (Enum.TryParse(name, out T commandKey))
            {
                if (this.registeredCommands.TryGetValue(commandKey, out var command))
                {
                    return command;
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public CommandInfo RegisterCommand(T id, ISwCommand command)
        {
            if (this.registeredCommands.ContainsKey(id))
            {
                throw new ArgumentException($"Command with id '{id}' is already defined.", nameof(id));
            }

            var info = this.GetCommandInfo(id);

            var callBackNames = this.commandHandler.GetCallbackNames(id);

            var cmdId = this.swCommandGroup.AddCommandItem2(
                info.Name,
                info.Position,
                info.Hint,
                info.Tooltip,
                info.ImageIndex,
                callBackNames.OnExecute,
                callBackNames.CanExecute,
                (int)(object)id,
                info.GetSwCommandItemType_e());

            this.EnableMenuOrToolbar(info);

            this.registeredCommands.Add(id, command);

            return new CommandInfo(command, cmdId, this.id, info.Name, (int)(object)id, info.ImageIndex);
        }

        /// <inheritdoc/>
        public void AddCommandGroup<TCommand>(Action<ICommandHandler<TCommand>> factoryMethod)
            where TCommand : struct, Enum
             => this.commandHandler.AddCommandGroup(factoryMethod, this.path);

        private CommandInfoAttribute GetCommandInfo(T id)
        {
            if (typeof(T).GetField($"{id}")?.GetCustomAttribute<CommandInfoAttribute>() is not CommandInfoAttribute info)
            {
                info = new CommandInfoAttribute($"{id}");
            }

            return info;
        }

        private void EnableMenuOrToolbar(CommandInfoAttribute info)
        {
            if (info.HasMenu)
            {
                this.swCommandGroup.HasMenu = true;
            }

            if (info.HasToolbar)
            {
                this.swCommandGroup.HasToolbar = true;
            }
        }
    }
}