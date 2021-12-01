// <copyright file="CommandGroup.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.swconst;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using SW = SolidWorks.Interop.sldworks;

    /// <summary>
    /// Manages commands of an single command group and their behavior.
    /// </summary>
    internal class CommandGroup : ICommandGroupBuilder, ICommandGroup
    {
        private readonly Dictionary<int, ICommandInfo> registeredCommands = new();

        private readonly SW.ICommandManager swCommandManager;
        private readonly CommandGroupInfo commandGroupInfo;
        private SW.CommandGroup swCommandGroup;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandGroup"/> class.
        /// </summary>
        /// <param name="commandManager">The command manager.</param>
        /// <param name="commandGroupSpec">Th command group infos.</param>
        public CommandGroup(SW.ICommandManager commandManager, CommandGroupSpec commandGroupSpec)
        {
            this.swCommandManager = commandManager;
            this.commandGroupInfo = new CommandGroupInfo()
            {
                Title = commandGroupSpec.Title,
                UserId = commandGroupSpec.UserId,
            };

            var cmdGroupErr = 0;

            this.swCommandGroup = commandManager.CreateCommandGroup2(
                UserID: commandGroupSpec.UserId,
                Title: commandGroupSpec.Title,
                ToolTip: commandGroupSpec.Tooltip,
                Hint: commandGroupSpec.Hint,
                Position: commandGroupSpec.Position,
                IgnorePreviousVersion: true,
                Errors: ref cmdGroupErr);

            if (!string.IsNullOrWhiteSpace(commandGroupSpec.IconsPath))
            {
                this.swCommandGroup.IconList = commandGroupSpec.GetIconsList();
            }

            if (!string.IsNullOrWhiteSpace(commandGroupSpec.MainIconPath))
            {
                this.swCommandGroup.MainIconList = commandGroupSpec.GetMainIconList();
            }
        }

        /// <inheritdoc/>
        public ICommandGroupInfo Info => this.commandGroupInfo;



        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.registeredCommands.Clear();
            this.swCommandManager.RemoveCommandGroup2(this.commandGroupInfo.UserId, true);

            this.swCommandGroup = SwComInterop.ReleaseComObject(this.swCommandGroup);

            this.disposed = true;
        }

        /// <inheritdoc/>
        public ICommandInfo? GetCommand(int userId)
        {
            if (this.registeredCommands.TryGetValue(userId, out var cmdInfo))
            {
                return cmdInfo;
            }

            return null;
        }

        /// <inheritdoc/>
        public ICommandInfo AddCommand(CommandSpec commandSpec, ISwCommand command)
        {
            if (commandSpec is null)
            {
                throw new ArgumentNullException(nameof(commandSpec));
            }

            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (this.registeredCommands.ContainsKey(commandSpec.UserId))
            {
                throw new ArgumentException($"Command with id '{commandSpec.UserId}' is already defined.", nameof(commandSpec));
            }

            return this.AddCommandToCommandGroup(commandSpec, command);
        }

        /// <inheritdoc/>
        public void AddSeparator()
        {
            this.swCommandGroup.AddSpacer2(
                -1,
                (int)(swCommandItemType_e.swMenuItem | swCommandItemType_e.swToolbarItem));
        }

        /// <summary>
        /// Activates the command group.
        /// </summary>
        internal void Activate()
        {
            this.swCommandGroup.Activate();

            foreach (var command in this.registeredCommands.Values.OfType<CommandInfo>())
            {
                command.Id = this.swCommandGroup.CommandID[command.Id];
            }

            this.commandGroupInfo.ToolbarId = this.swCommandGroup.ToolbarId;
        }

        private ICommandInfo AddCommandToCommandGroup(CommandSpec commandSpec, ISwCommand command)
        {
            var callBackNames = this.GetCallbackNames(commandSpec);

            int cmdId;

            cmdId = this.swCommandGroup.AddCommandItem2(
                Name: commandSpec.Name,
                Position: commandSpec.Position,
                HintString: commandSpec.Hint,
                ToolTip: commandSpec.Tooltip,
                ImageListIndex: commandSpec.ImageIndex,
                CallbackFunction: callBackNames.OnExecute,
                EnableMethod: callBackNames.CanExecute,
                UserID: commandSpec.UserId,
                MenuTBOption: commandSpec.GetSwCommandItemType_e());

            var commandInfo = new CommandInfo(commandSpec.Name, command)
            {
                Id = cmdId,
                UserId = commandSpec.UserId,
            };

            this.EnableMenuOrToolbar(commandSpec);

            this.registeredCommands.Add(commandSpec.UserId, commandInfo);

            return commandInfo;
        }



        private (string OnExecute, string CanExecute) GetCallbackNames(CommandSpec commandSpec)
        {
            return ($"{nameof(CommandHandler.OnExecute)}({commandSpec.CommandGroupId}:{commandSpec.UserId})",
                    $"{nameof(CommandHandler.CanExecute)}({commandSpec.CommandGroupId}:{commandSpec.UserId})");
        }

        private void EnableMenuOrToolbar(CommandSpec commandSpec)
        {
            if (commandSpec.HasMenu)
            {
                this.swCommandGroup.HasMenu = true;
            }

            if (commandSpec.HasToolbar)
            {
                this.swCommandGroup.HasToolbar = true;
            }
        }
    }
}