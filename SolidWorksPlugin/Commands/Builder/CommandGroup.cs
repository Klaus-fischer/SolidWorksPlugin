// <copyright file="CommandGroup.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
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
        /// <param name="commandGroupInfo">Th command group infos.</param>
        public CommandGroup(SW.ICommandManager commandManager, CommandGroupInfo commandGroupInfo)
        {
            this.swCommandManager = commandManager;
            this.commandGroupInfo = commandGroupInfo;

            var cmdGroupErr = 0;

            this.swCommandGroup = commandManager.CreateCommandGroup2(
                UserID: commandGroupInfo.UserId,
                Title: commandGroupInfo.Title,
                ToolTip: commandGroupInfo.Tooltip,
                Hint: commandGroupInfo.Hint,
                Position: commandGroupInfo.Position,
                IgnorePreviousVersion: true,
                Errors: ref cmdGroupErr);

            if (!string.IsNullOrWhiteSpace(commandGroupInfo.IconsPath))
            {
                this.swCommandGroup.IconList = commandGroupInfo.GetIconsList();
            }

            if (!string.IsNullOrWhiteSpace(commandGroupInfo.MainIconPath))
            {
                this.swCommandGroup.MainIconList = commandGroupInfo.GetMainIconList();
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

            if (Marshal.IsComObject(this.swCommandGroup))
            {
                Marshal.FinalReleaseComObject(this.swCommandGroup);
            }

            this.swCommandGroup = default;

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
        public ICommandInfo AddCommand(CommandInfo commandInfo, ISwCommand command)
        {
            if (commandInfo is null)
            {
                throw new ArgumentNullException(nameof(commandInfo));
            }

            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (this.registeredCommands.ContainsKey(commandInfo.UserId))
            {
                throw new ArgumentException($"Command with id '{commandInfo.UserId}' is already defined.", nameof(commandInfo));
            }

            return this.AddCommandToCommandGroup(commandInfo, command);
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

        private ICommandInfo AddCommandToCommandGroup(CommandInfo commandInfo, ISwCommand command)
        {
            var callBackNames = this.GetCallbackNames(commandInfo);

            int cmdId;

            cmdId = this.swCommandGroup.AddCommandItem2(
                Name: commandInfo.Name,
                Position: commandInfo.Position,
                HintString: commandInfo.Hint,
                ToolTip: commandInfo.Tooltip,
                ImageListIndex: commandInfo.ImageIndex,
                CallbackFunction: callBackNames.OnExecute,
                EnableMethod: callBackNames.CanExecute,
                UserID: commandInfo.UserId,
                MenuTBOption: commandInfo.GetSwCommandItemType_e());

            commandInfo.Id = cmdId;
            commandInfo.Command = command;

            this.EnableMenuOrToolbar(commandInfo);

            this.registeredCommands.Add(commandInfo.UserId, commandInfo);

            return commandInfo;
        }

        private (string OnExecute, string CanExecute) GetCallbackNames(CommandInfo commandInfo)
        {
            return ($"{nameof(CommandHandler.OnExecute)}({commandInfo.CommandGroupId}:{commandInfo.UserId})",
                    $"{nameof(CommandHandler.CanExecute)}({commandInfo.CommandGroupId}:{commandInfo.UserId})");
        }

        private void EnableMenuOrToolbar(CommandInfo info)
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