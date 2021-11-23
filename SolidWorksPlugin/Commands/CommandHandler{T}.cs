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
        private readonly Dictionary<T, ICommandInfo> registeredCommands = new Dictionary<T, ICommandInfo>();

        private readonly ICommandHandlerInternals commandHandler;
        private readonly CommandGroup swCommandGroup;
        private readonly int id;
        private readonly string path;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler{T}"/> class.
        /// </summary>
        /// <param name="commandHandler">The main command handler.</param>
        /// <param name="swCommandGroup">The current command group.</param>
        /// <param name="path">The path of command group.</param>
        /// <param name="id">The command group id.</param>
        public CommandHandler(ICommandHandlerInternals commandHandler, CommandGroup swCommandGroup, string path, int id)
        {
            this.swCommandGroup = swCommandGroup;
            this.id = id;
            this.commandHandler = commandHandler;
            this.path = $"{path}\\";
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.registeredCommands.Clear();
            this.commandHandler.SwCommandManager.RemoveCommandGroup2(this.id, true);
            this.disposed = true;
        }

        /// <inheritdoc/>
        public ICommandInfo? GetCommand(string name)
        {
            if (Enum.TryParse(name, out T id))
            {
                return this.GetCommand(id);
            }

            return null;
        }

        /// <inheritdoc/>
        public ICommandInfo? GetCommand<Tid>(Tid id)
            where Tid : struct, Enum
        {
            if (id is T key && this.registeredCommands.TryGetValue(key, out var commandInfo))
            {
                return commandInfo;
            }

            return null;
        }

        /// <inheritdoc/>
        public ICommandInfo RegisterCommand(T id, ISwCommand command)
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

            var cmdInfo = new CommandInfo(command, cmdId, this.id, info.Name, (int)(object)id, info.ImageIndex);

            this.registeredCommands.Add(id, cmdInfo);

            return cmdInfo;
        }

        ///// <inheritdoc/>
        //public void AddCommandGroup<TCommand>(Action<ICommandHandler<TCommand>> factoryMethod)
        //    where TCommand : struct, Enum
        //     => this.commandHandler.AddCommandGroup(factoryMethod, this.path);

        private CommandInfoAttribute GetCommandInfo(T id)
        {
            if (typeof(T).GetField($"{id}")!.GetCustomAttribute<CommandInfoAttribute>() is not CommandInfoAttribute info)
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