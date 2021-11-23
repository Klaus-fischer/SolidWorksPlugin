﻿// <copyright file="CommandHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Manages commands and their behavior.
    /// </summary>
    public class CommandHandler : ICommandHandlerInternals, ICommandGroupHandler, IDisposable
    {
        private readonly Dictionary<string, ICommandHandler> commandHandlers;
        private readonly IDocumentManager documentManager;
        private readonly ICommandManager swCommandManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler"/> class.
        /// </summary>
        /// <param name="swApplication">The current solid works application.</param>
        /// <param name="documentManager">The current used document manager.</param>
        /// <param name="cookie">The cookie of the add-in.</param>
        internal CommandHandler(ISldWorks swApplication, IDocumentManager documentManager, Cookie cookie)
        {
            this.commandHandlers = new Dictionary<string, ICommandHandler>();
            this.documentManager = documentManager;
            this.swCommandManager = swApplication.GetCommandManager(cookie);
            swApplication.SetAddinCallbackInfo2(0, this, cookie);
        }

        /// <inheritdoc/>
        ICommandManager ICommandHandlerInternals.SwCommandManager => this.swCommandManager;

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!this.commandHandlers.Any())
            {
                return;
            }

            foreach (var handler in this.commandHandlers.Values)
            {
                handler.Dispose();
            }

            this.commandHandlers.Clear();
        }

        /// <summary>
        /// Callback method to check if command is valid in current context.
        /// </summary>
        /// <param name="commandName">Name of the target command.</param>
        /// <returns>1 for valid call, otherwise 0.</returns>
        public int CanExecute(string commandName)
        {
            var result = CommandCanExecuteState.Disabled;
            if (!this.TryGetCommandFromHandler(commandName, out var commandInfo) || commandInfo.Command is not ISwCommand command)
            {
                return (int)result;
            }

            if (command is IToggleCommand toggleCommand)
            {
                if (toggleCommand.IsActive)
                {
                    result |= CommandCanExecuteState.Selected;
                }
            }

            var activeDoc = this.documentManager.ActiveDocument;
            if (command.CanExecute(activeDoc))
            {
                result |= CommandCanExecuteState.Enabled;
            }

            return (int)result;
        }

        /// <summary>
        /// Callback method to execute the command.
        /// </summary>
        /// <param name="commandName">Name of the target command.</param>
        public void OnExecute(string commandName)
        {
            if (!this.TryGetCommandFromHandler(commandName, out var commandInfo) || commandInfo.Command is not ISwCommand command)
            {
                throw new ArgumentException($"Command '{commandName}' not defined.");
            }

            var activeDoc = this.documentManager.ActiveDocument;
            if (command.CanExecute(activeDoc))
            {
                command.Execute(activeDoc);
            }
        }

        /// <inheritdoc/>
        public void AddCommandGroup<T>(Action<ICommandHandler<T>> factoryMethod)
            where T : struct, Enum => ((ICommandHandlerInternals)this).AddCommandGroup(factoryMethod, string.Empty);

        /// <summary>
        /// Adds an command group to the command handler.
        /// </summary>
        /// <typeparam name="T">Type of the command enumeration.</typeparam>
        /// <param name="factoryMethod">Method to add all commands.</param>
        /// <param name="path">relative path to build sub menus.</param>
        void ICommandHandlerInternals.AddCommandGroup<T>(Action<ICommandHandler<T>> factoryMethod, string path)
        {
            (var info, var icons) = this.GetIconsAndInfo(typeof(T));

            if (this.commandHandlers.ContainsKey(typeof(T).Name))
            {
                throw new InvalidOperationException("CommandHandler for this type of Enumeration is already defined.");
            }

            var title = $"{path}{info.Title}";
            var cmdGroupErr = 0;
            var swCommandGroup = this.swCommandManager.CreateCommandGroup2(
                UserID: info.CommandGroupId,
                Title: title,
                ToolTip: info.ToolTip,
                Hint: info.Hint,
                Position: info.Position,
                IgnorePreviousVersion: true,
                Errors: ref cmdGroupErr);

            if (icons is not null)
            {
                swCommandGroup.IconList = icons.GetIconsList();
                swCommandGroup.MainIconList = icons.GetMainIconList();
            }

            var commandHandler = new CommandHandler<T>(this, swCommandGroup, title, info.CommandGroupId);

            factoryMethod.Invoke(commandHandler);

            swCommandGroup.Activate();

            this.commandHandlers.Add(typeof(T).Name, commandHandler);

        }

        /// <summary>
        /// Generates the callback method names.
        /// </summary>
        /// <typeparam name="T">Type of the enumeration.</typeparam>
        /// <param name="id">Value of the enumeration.</param>
        /// <returns>both method names.</returns>
        (string OnExecute, string CanExecute) ICommandHandlerInternals.GetCallbackNames<T>(T id)
        {
            return ($"{nameof(this.OnExecute)}({typeof(T).Name}:{id})",
                    $"{nameof(this.CanExecute)}({typeof(T).Name}:{id})");
        }

        private bool TryGetCommandFromHandler(string handlerAndCommandName, [NotNullWhen(true)] out ICommandInfo? command)
        {
            (var handlerName, var commandName) = this.SplitHandlerAndCommandName(handlerAndCommandName);

            if (this.commandHandlers.TryGetValue(handlerName, out var handler) &&
                handler.GetCommand(commandName) is ICommandInfo cmd)
            {
                command = cmd;
                return true;
            }

            command = null;
            return false;
        }

        private (string Handler, string Command) SplitHandlerAndCommandName(string handlerAndCommandName)
        {
            int indexOfColon = handlerAndCommandName.IndexOf(':');

            // abort if index not found.
            if (indexOfColon == -1)
            {
                return (string.Empty, string.Empty);
            }

            string handler = new string(handlerAndCommandName.AsSpan(0, indexOfColon));
            string command = new string(handlerAndCommandName.AsSpan(indexOfColon + 1));

            return (handler, command);
        }

        private (CommandGroupInfoAttribute Info, CommandGroupIconsAttribute? Icons) GetIconsAndInfo(Type enumType)
        {
            if (enumType.GetCustomAttribute<CommandGroupInfoAttribute>() is not CommandGroupInfoAttribute info)
            {
                throw new ArgumentException($"Attribute {nameof(CommandGroupInfoAttribute)} is not defined on Enum '{enumType.Name}'");
            }

            var icons = enumType.GetCustomAttribute<CommandGroupIconsAttribute>();

            return (info, icons);
        }

        /// <inheritdoc/>
        public ICommandInfo? GetCommandInfo<T>(T id)
            where T : struct, Enum
        {
            if (this.commandHandlers.TryGetValue(typeof(T).Name, out var handler))
            {
                return handler.GetCommand(id);
            }

            return null;
        }
    }
}
