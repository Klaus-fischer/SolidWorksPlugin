// <copyright file="CommandHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
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
        public ICommandManager SwCommandManager => this.swCommandManager;

        /// <inheritdoc/>
        public ILogger<CommandHandler>? Logger { get; set; }

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

            this.swCommandManager = SwComInterop.ReleaseComObject(this.swCommandManager);

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
        public ICommandGroupInfo? GetCommandGroup(int commandGroupId)
        {
            if (this.commandHandlers.TryGetValue(commandGroupId, out var handler))
            {
                return handler.Info;
            }

            return null;
        }

        /// <inheritdoc/>
        public void AddCommandGroup(CommandGroupSpec commandGroupSpec, CommandGroupBuilderDelegate factoryMethod)
        {
            if (commandGroupSpec is null)
            {
                throw new ArgumentNullException(nameof(commandGroupSpec));
            }

            if (this.commandHandlers.ContainsKey(commandGroupSpec.UserId))
            {
                throw new InvalidOperationException($"Command group with id {commandGroupSpec.UserId} id already defined.");
            }

            var commandHandler = new CommandGroup(this.swCommandManager, commandGroupSpec);

            factoryMethod.Invoke(commandHandler);

            commandHandler.Activate();

            this.commandHandlers.Add(commandGroupSpec.UserId, commandHandler);
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
                this.Logger.LogDebug($"Command {commandName} can't be executed cause it's not defined.");
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

            try
            {
                var activeDoc = this.documentManager.ActiveDocument;
                if (command.CanExecute(activeDoc))
                {
                    command.Execute(activeDoc);
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Error on executing {commandInfo.Name} command.");
                throw ex;
            }
        }

#if NETSTANDARD2_1
        private bool TryGetCommandFromHandler(string handlerAndCommandName, [NotNullWhen(true)] out ICommandInfo? command)
#else
        private bool TryGetCommandFromHandler(string handlerAndCommandName, out ICommandInfo? command)
#endif
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
#if NETSTANDARD2_1
            if (int.TryParse(handlerAndCommandName.AsSpan(0, indexOfColon), out handlerId) &&
                int.TryParse(handlerAndCommandName.AsSpan(indexOfColon + 1), out commandId))
            {
                return true;
            }
#else
            if (int.TryParse(handlerAndCommandName.Substring(0, indexOfColon), out handlerId) &&
                int.TryParse(handlerAndCommandName.Substring(indexOfColon + 1), out commandId))
            {
                return true;
            }
#endif

            return false;
        }
    }
}
