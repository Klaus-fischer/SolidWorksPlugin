// <copyright file="CommandManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// The command manager saves all commands of the plug-in and handles assigns the execute callbacks.
    /// </summary>
    /// <typeparam name="T">Type of the command enumeration.</typeparam>
    internal class CommandManager<T> : ICommandManager<T>, IDisposable
        where T : struct, Enum
    {
        private readonly int mainCommandId;
        private readonly IDocumentManager documentManager;
        private readonly Dictionary<T, CommandInfo> registeredCommands = new Dictionary<T, CommandInfo>();

        private ICommandManager? swCommandManager;
        private ICommandGroup? swCommandGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandManager{T}"/> class.
        /// </summary>
        /// <param name="swApplication">The current SolidWorks application.</param>
        /// <param name="cookie">The Cookie of the current add-in.</param>
        /// <param name="mainCommandId">The Main Command id of the add-in.</param>
        /// <param name="documentManager">The currently used document manager.</param>
        public CommandManager(SldWorks swApplication, int cookie, int mainCommandId, IDocumentManager documentManager)
        {
            this.documentManager = documentManager ?? throw new ArgumentNullException(nameof(documentManager));
            this.mainCommandId = mainCommandId;
            this.swCommandManager = swApplication.GetCommandManager(cookie);

            var commandTarget = new CommandTarget(this.OnExecute, this.CanExecute);

            // Setup callbacks to an public object that is not generic.
            // SolidWorks will crash otherwise.
            swApplication.SetAddinCallbackInfo2(0, commandTarget, cookie);
        }

        /// <summary>
        /// To Initialize the command manager.
        /// </summary>
        /// <param name="title">The title of the command group.</param>
        /// <param name="tooltip">The tool tip of the command group.</param>
        /// <param name="icons">Location of the icons in various resolutions.</param>
        /// <param name="mainIcon">Location of the main icon in various resolutions.</param>
        /// <param name="position">The position of the item.</param>
        public void InitializeCommandManager(string title, string tooltip, string[] icons, string[] mainIcon, int position = -1)
        {
            var ignorePrevious = false;
            var readFromRegistrySuccess = this.swCommandManager!.GetGroupDataFromRegistry(
                this.mainCommandId,
                out var _);
            if (readFromRegistrySuccess)
            {
                ignorePrevious = true;
            }

            var cmdGroupErr = 0;
            this.swCommandGroup = this.swCommandManager.CreateCommandGroup2(
                this.mainCommandId,
                $"SW-Addin\\{title}",
                tooltip,
                string.Empty,
                position,
                ignorePrevious,
                ref cmdGroupErr);

            this.swCommandGroup.IconList = icons;
            this.swCommandGroup.MainIconList = mainIcon;
        }

        /// <inheritdoc/>
        public ICommandInfo RegisterCommand(T id, ISwCommand command)
        {
            if (this.registeredCommands.ContainsKey(id))
            {
                throw new ArgumentException($"Command with id '{id}' is already defined.", nameof(id));
            }

            var executeMethodName = $"{nameof(CommandTarget.OnExecute)}({id})";
            var canExecuteMethodName = $"{nameof(CommandTarget.CanExecute)}({id})";

            int imgIndex = 0;
            swCommandItemType_e menuOptions = swCommandItemType_e.swMenuItem;
            string name = command.Title;

            if (typeof(T).GetField($"{id}").GetCustomAttribute<CommandInfoAttribute>() is CommandInfoAttribute commandInfo)
            {
                imgIndex = commandInfo.ImageIndex;
                menuOptions = 0;
                if (commandInfo.HasMenu == true)
                {
                    menuOptions |= swCommandItemType_e.swMenuItem;
                }

                if (commandInfo.HasToolbar == true)
                {
                    menuOptions |= swCommandItemType_e.swToolbarItem;
                }

                name = commandInfo.Text ?? name;
            }

            var cmdId = this.swCommandGroup!.AddCommandItem2(
                name,
                -1,
                command.Description,
                command.Title,
                imgIndex,
                executeMethodName,
                canExecuteMethodName,
                (int)(object)id,
                (int)menuOptions);

            var result = new CommandInfo(command, cmdId)
            {
                ImageIndex = imgIndex,
                UserId = (int)(object)id,
                MenuOptions = menuOptions,
            };

            this.registeredCommands.Add(id, result);

            return result;
        }

        /// <summary>
        /// Activates the command group.
        /// </summary>
        public void Activate()
        {
            this.swCommandGroup!.HasMenu = true;
            this.swCommandGroup!.HasToolbar = true;
            this.swCommandGroup!.Activate();
        }

        /// <summary>
        /// Callback method to check if command is valid in current context.
        /// </summary>
        /// <param name="commandName">Name of the target command.</param>
        /// <returns>1 for valid call, otherwise 0.</returns>
        public int CanExecute(string commandName)
        {
            if (!Enum.TryParse(commandName, out T commandKey))
            {
                return (int)CommandCanExecuteState.Disabled;
            }

            if (!this.registeredCommands.TryGetValue(commandKey, out var command))
            {
                return (int)CommandCanExecuteState.Disabled;
            }

            var result = CommandCanExecuteState.Disabled;
            if (command.Command is IToggleCommand toggleCommand)
            {
                if (toggleCommand.IsActive)
                {
                    result |= CommandCanExecuteState.Selected;
                }
            }

            var activeDoc = this.documentManager.ActiveDocument;
            if (command.Command.CanExecute(activeDoc))
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
            if (!Enum.TryParse(commandName, out T commandKey))
            {
                throw new ArgumentOutOfRangeException($"{commandName} is not valid", nameof(commandName));
            }

            if (!this.registeredCommands.TryGetValue(commandKey, out var command))
            {
                throw new ArgumentOutOfRangeException($"{commandName} is not defined.", nameof(commandName));
            }

            var activeDoc = this.documentManager.ActiveDocument;

            if (command.Command.CanExecute(activeDoc))
            {
                command.Command.Execute(activeDoc);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // skip if already disposed.
            if (this.swCommandManager == null)
            {
                return;
            }

            this.swCommandManager.RemoveCommandGroup2(this.mainCommandId, true);

            Marshal.ReleaseComObject(this.swCommandGroup);
            this.swCommandGroup = null;

            Marshal.ReleaseComObject(this.swCommandManager);
            this.swCommandManager = null;

            this.registeredCommands.Clear();
        }
    }
}
