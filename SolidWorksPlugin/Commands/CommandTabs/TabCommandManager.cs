// <copyright file="TabCommandManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// The concrete tab command manager.
    /// </summary>
    internal class TabCommandManager : IInternalCommandTabManager, ICommandTabManager, IDisposable
    {
        private readonly IInternalCommandHandler commandHandler;
        private readonly Collection<IDisposable> disposables = new();
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabCommandManager"/> class.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        public TabCommandManager(IInternalCommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
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
        public void BuildCommandTab(string title, CommandTabBuilderDelegate factoryMethod, params swDocumentTypes_e[] documentTypes)
        {
            foreach (var documentType in documentTypes)
            {
                var cmdTabBuilder = new CommandTabBuilder(this.commandHandler, title, documentType);

                factoryMethod(cmdTabBuilder);

                this.disposables.Add(cmdTabBuilder);
            }
        }
    }
}
