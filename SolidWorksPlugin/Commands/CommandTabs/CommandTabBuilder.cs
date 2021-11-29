﻿// <copyright file="CommandTabBuilder.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// The concrete command tab builder.
    /// </summary>
    internal class CommandTabBuilder : ICommandTabBuilder, IDisposable
    {
        private readonly ICommandManager swCommandManager;
        private readonly ICommandHandler commandHandler;
        private readonly Stack<CommandTabBox> swCommandTabBoxes = new();
        private CommandTab swCommandTab;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandTabBuilder"/> class.
        /// </summary>
        /// <param name="commandHandler">The assigned command handler.</param>
        /// <param name="title">The title of the tab.</param>
        /// <param name="swDocumentType">The document type.</param>
        public CommandTabBuilder(IInternalCommandHandler commandHandler, string title, swDocumentTypes_e swDocumentType)
        {
            this.commandHandler = commandHandler;
            this.swCommandManager = commandHandler.SwCommandManager;
            this.CurrentDocumentType = swDocumentType;

            if (this.swCommandManager.GetCommandTab((int)swDocumentType, title) is CommandTab commandTab)
            {
                this.swCommandManager.RemoveCommandTab(commandTab);
            }

            this.swCommandTab = this.swCommandManager.AddCommandTab((int)swDocumentType, title);

            this.swCommandTabBoxes.Push(this.swCommandTab.AddCommandTabBox());
        }

        /// <inheritdoc/>
        ICommandHandler ICommandTabBuilder.CommandHandler => this.commandHandler;

        /// <inheritdoc/>
        public swDocumentTypes_e CurrentDocumentType { get; }

        internal ICommandTabBox SwCommandTabBox => this.swCommandTabBoxes.Peek();

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            while (this.swCommandTabBoxes.Any())
            {
                var last = this.swCommandTabBoxes.Pop();

                this.swCommandTab.RemoveCommandTabBox(last);

                if (Marshal.IsComObject(last))
                {
                    Marshal.FinalReleaseComObject(last);
                }
            }

            this.swCommandManager.RemoveCommandTab(this.swCommandTab);
            if (Marshal.IsComObject(this.swCommandTab))
            {
                Marshal.FinalReleaseComObject(this.swCommandTab);
            }

            this.disposed = true;
        }

        /// <inheritdoc/>
        public void AddCommand(ICommandInfo commandInfo, swCommandTabButtonTextDisplay_e textDisplayStyle)
        {
            this.SwCommandTabBox.AddCommands(new int[] { commandInfo.Id }, new int[] { (int)textDisplayStyle });
        }

        /// <inheritdoc/>
        public void AddSpacer()
        {
            this.swCommandTabBoxes.Push(this.swCommandTab.AddCommandTabBox());
        }

        /// <inheritdoc/>
        public void AddFlyout(
            ICommandGroupInfo commandGroupInfo,
            swCommandTabButtonTextDisplay_e textDisplay,
            swCommandTabButtonFlyoutStyle_e flyoutStyle = swCommandTabButtonFlyoutStyle_e.swCommandTabButton_ActionFlyout)
        {
            int commandId = commandGroupInfo.ToolbarId;
            this.SwCommandTabBox.AddCommands(new int[] { commandId }, new int[] { (int)textDisplay | (int)flyoutStyle });
        }
    }
}
