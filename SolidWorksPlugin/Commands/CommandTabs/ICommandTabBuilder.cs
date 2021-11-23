// <copyright file="ICommandTabBuilder.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.InteropServices;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    public interface ICommandTabBuilder
    {
        void AddCommand(ICommandInfo commandInfom, swCommandTabButtonTextDisplay_e textDisplay);

        void AddSpacer();

        public void AddFlyout(
            ICommandGroupInfo commandGroupInfo,
            swCommandTabButtonTextDisplay_e textDisplay,
            swCommandTabButtonFlyoutStyle_e flyoutStyle);
    }

    public interface ICommandTabManager
    {
        void BuildCommandTab(string title, Action<ICommandTabBuilder> factoryMethod, params swDocumentTypes_e[] documentTypes);
    }

    public class TabCommandManager : ICommandTabManager, IDisposable
    {
        private readonly ICommandManager swCommandManager;
        Collection<IDisposable> disposables = new();
        private bool disposed;

        public TabCommandManager(ICommandManager swCommandManager)
        {
            this.swCommandManager = swCommandManager ?? throw new ArgumentNullException(nameof(swCommandManager));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            while (this.disposables.Any())
            {
                this.disposables[0].Dispose();
                this.disposables.RemoveAt(0);
            }

            this.disposed = true;
        }

        /// <inheritdoc/>
        public void BuildCommandTab(string title, Action<ICommandTabBuilder> factoryMethod, params swDocumentTypes_e[] documentTypes)
        {
            foreach (var documentType in documentTypes)
            {
                var cmdTabBuilder = new CommandTabBuilder(this.swCommandManager, title, documentType);

                factoryMethod(cmdTabBuilder);

                this.disposables.Add(cmdTabBuilder);
            }
        }
    }

    public class CommandTabBuilder : ICommandTabBuilder, IDisposable
    {
        private readonly ICommandManager swCommandManager;
        private readonly Stack<CommandTabBox> swCommandTabBoxes = new();
        private CommandTab swCommandTab;

        private bool disposed;

        public CommandTabBuilder(ICommandManager swCommandManager, string title, swDocumentTypes_e swDocumentType)
        {
            if (swCommandManager.GetCommandTab((int)swDocumentType, title) is CommandTab commandTab)
            {
                swCommandManager.RemoveCommandTab(commandTab);
            }

            this.swCommandTab = swCommandManager.AddCommandTab((int)swDocumentType, title);

            this.swCommandTabBoxes.Push(this.swCommandTab.AddCommandTabBox());
            this.swCommandManager = swCommandManager;
        }

        private ICommandTabBox SwCommandTabBox => this.swCommandTabBoxes.Peek();

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

        public void AddCommand(ICommandInfo commandInfo, swCommandTabButtonTextDisplay_e textDisplayStyle)
        {
            this.SwCommandTabBox.AddCommands(new int[] { commandInfo.Id }, new int[] { (int)textDisplayStyle });
        }

        public void AddSpacer()
        {
            this.swCommandTabBoxes.Push(this.swCommandTab.AddCommandTabBox());
        }

        public void AddFlyout(
            ICommandGroupInfo commandGroupInfo,
            swCommandTabButtonTextDisplay_e textDisplay,
            swCommandTabButtonFlyoutStyle_e flyoutStyle)
        {
            int commandId = commandGroupInfo.ToolbarId;
            this.SwCommandTabBox.AddCommands(new int[] { commandId }, new int[] { (int)textDisplay | (int)flyoutStyle });
        }
    }
}
