// <copyright file="CommandGroup.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Manages commands of an single command group and their behavior.
    /// </summary>
    internal class CommandGroup : ICommandGroupBuilder, ICommandGroup
    {
        private readonly Dictionary<int, ICommandInfo> registeredCommands = new();

        private readonly SolidWorks.Interop.sldworks.CommandGroup swCommandGroup;
        private readonly int id;
        private ICommandManager commandManager;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandGroup"/> class.
        /// </summary>
        /// <param name="commandManager">The command manager.</param>
        /// <param name="commandGroupInfo">Th command group infos.</param>
        public CommandGroup(ICommandManager commandManager, CommandGroupInfo commandGroupInfo)
        {
            this.id = commandGroupInfo.Id;
            this.commandManager = commandManager;

            var cmdGroupErr = 0;
            this.swCommandGroup = commandManager.CreateCommandGroup2(
                UserID: commandGroupInfo.Id,
                Title: commandGroupInfo.Title,
                ToolTip: commandGroupInfo.Tooltip,
                Hint: commandGroupInfo.Hint,
                Position: commandGroupInfo.Position,
                IgnorePreviousVersion: true,
                Errors: ref cmdGroupErr);

            this.swCommandGroup.IconList = commandGroupInfo.Icons;
            this.swCommandGroup.MainIconList = commandGroupInfo.MainIcon;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.registeredCommands.Clear();
            this.commandManager.RemoveCommandGroup2(this.id, true);

            Marshal.FinalReleaseComObject(this.commandManager);
            this.commandManager = default;
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
            if (this.registeredCommands.ContainsKey(commandInfo.UserId))
            {
                throw new ArgumentException($"Command with id '{commandInfo.UserId}' is already defined.", nameof(commandInfo));
            }

            var callBackNames = this.GetCallbackNames(commandInfo);

            var cmdId = this.swCommandGroup.AddCommandItem2(
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

        /// <summary>
        /// Activates the command group.
        /// </summary>
        internal void Activate() => this.swCommandGroup.Activate();

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