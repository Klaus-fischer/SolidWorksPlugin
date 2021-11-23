// <copyright file="CommandHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Manages commands and their behavior.
    /// </summary>
    public class CommandHandler : IInternalCommandHandler, ICommandHandler, ICommandGroupHandler, IDisposable
    {
        private readonly Dictionary<int, ICommandGroup> commandHandlers = new();
        private readonly IDocumentManager documentManager;
        private ICommandManager swCommandManager;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler"/> class.
        /// </summary>
        /// <param name="swApplication">The current solid works application.</param>
        /// <param name="documentManager">The current used document manager.</param>
        /// <param name="cookie">The cookie of the add-in.</param>
        internal CommandHandler(ISldWorks swApplication, IDocumentManager documentManager, Cookie cookie)
        {
            this.documentManager = documentManager;
            this.swCommandManager = swApplication.GetCommandManager(cookie);
            swApplication.SetAddinCallbackInfo2(0, this, cookie);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            foreach (var handler in this.commandHandlers.Values)
            {
                handler.Dispose();
            }

            this.commandHandlers.Clear();

            Marshal.FinalReleaseComObject(this.swCommandManager);
            this.swCommandManager = default;

            this.disposed = true;
        }

        /// <inheritdoc/>
        public ICommandInfo? GetCommand(int commandGroupId, int commandUserId)
        {
            if (this.commandHandlers.TryGetValue(commandGroupId, out var handler) &&
                handler.GetCommand(commandUserId) is ICommandInfo cmd)
            {
                return cmd;
            }

            return null;
        }

        /// <inheritdoc/>
        public void AddCommandGroup(CommandGroupInfo commandGroupInfo, CommandGroupBuilderDelegate factoryMethod)
        {
            if (this.commandHandlers.ContainsKey(commandGroupInfo.Id))
            {
                throw new InvalidOperationException($"Command group with id {commandGroupInfo.Id} id already defined.");
            }

            var commandHandler = new CommandGroup(this.swCommandManager, commandGroupInfo);

            factoryMethod.Invoke(commandHandler);

            commandHandler.Activate();

            this.commandHandlers.Add(commandGroupInfo.Id, commandHandler);
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

        private bool TryGetCommandFromHandler(string handlerAndCommandName, [NotNullWhen(true)] out ICommandInfo? command)
        {
            if (this.SplitHandlerAndCommandName(handlerAndCommandName, out int handlerId, out int commandId))
            {
                if (this.commandHandlers.TryGetValue(handlerId, out var handler) &&
                    handler.GetCommand(commandId) is ICommandInfo cmd)
                {
                    command = cmd;
                    return true;
                }
            }

            command = null;
            return false;
        }

        private bool SplitHandlerAndCommandName(string handlerAndCommandName, out int handlerId, out int commandId)
        {
            handlerId = -1;
            commandId = -1;
            int indexOfColon = handlerAndCommandName.IndexOf(':');

            // abort if index not found.
            if (indexOfColon == -1)
            {
                return false;
            }

            if (int.TryParse(handlerAndCommandName.AsSpan(0, indexOfColon), out handlerId) &&
                int.TryParse(handlerAndCommandName.AsSpan(indexOfColon + 1), out commandId))
            {
                return true;
            }

            return false;
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
    }
}
