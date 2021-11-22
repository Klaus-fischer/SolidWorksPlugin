// <copyright file="EventHandlerManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// The event manager holds all event handlers.
    /// It will register each document at the document event handler at creation time.
    /// </summary>
    internal class EventHandlerManager : IEventHandlerManagerInternals, IEventHandlerManager, IDisposable
    {
        private readonly ICollection<IDocumentEventHandler> documentEventHandlers
            = new Collection<IDocumentEventHandler>();

        private readonly ICollection<ISolidWorksEventHandler> solidWorksEventHandlers
            = new Collection<ISolidWorksEventHandler>();

        private readonly SldWorks swApplication;
        private readonly IDocumentManagerInternals documentManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerManager"/> class.
        /// </summary>
        /// <param name="swApplication">The current solid works application.</param>
        /// <param name="documentManager">The current used document manager.</param>
        public EventHandlerManager(SldWorks swApplication, IDocumentManagerInternals documentManager)
        {
            this.swApplication = swApplication;
            this.documentManager = documentManager;
            this.AttachAllEvents();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.DetachAllEvents();
        }

        /// <inheritdoc/>
        public void RegisterDocumentEventHandler(IDocumentEventHandler eventHandler)
        {
            // attach events to all open documents.
            foreach (var document in this.documentManager.GetOpenDocuments())
            {
                eventHandler.AttachDocumentEvents(document);
            }

            this.documentEventHandlers.Add(eventHandler);
        }

        /// <inheritdoc/>
        public void RegisterSolidWorksEventHandler(ISolidWorksEventHandler eventHandler)
        {
            eventHandler.AttachSwEvents(this.swApplication);
            this.solidWorksEventHandlers.Add(eventHandler);
        }

        private void AttachAllEvents()
        {
            this.documentManager.OnDocumentAdded += this.OnOpenDocumentAdded;
            this.swApplication.FileNewNotify2 += this.OnNewFile;
            this.swApplication.FileOpenPostNotify += this.OnFileOpen;

            foreach (var document in this.documentManager.GetOpenDocuments())
            {
                document.OnDestroy += this.DetachEventsFromDocument;
            }
        }

        private void OnOpenDocumentAdded(object? sender, ISwDocument document)
        {
            this.AttachEventsToDocument(document);
        }

        private void AttachEventsToAllOpenedDocument()
        {
            foreach (var document in this.documentManager.GetAllUnknownDocuments())
            {
                this.AttachEventsToDocument(document);
            }
        }

        private void AttachEventsToDocument(ISwDocument document)
        {
            foreach (var docEventHandler in this.documentEventHandlers)
            {
                docEventHandler.AttachDocumentEvents(document);
            }

            document.OnDestroy += this.DetachEventsFromDocument;
        }

        private int DetachEventsFromDocument(ISwDocument document, swDestroyNotifyType_e destroyType)
        {
            if (destroyType == swDestroyNotifyType_e.swDestroyNotifyDestroy)
            {
                foreach (var docEventHandler in this.documentEventHandlers)
                {
                    docEventHandler.DetachDocumentEvents(document);
                }

                document.OnDestroy -= this.DetachEventsFromDocument;
                this.documentManager.DisposeDocument(document);
            }

            return 0;
        }

        private int OnNewFile(object newDoc, int docType, string templateName)
        {
            this.AttachEventsToAllOpenedDocument();
            return 0;
        }

        private int OnFileOpen(string fileName)
        {
            this.AttachEventsToAllOpenedDocument();
            return 0;
        }

        private void DetachAllEvents()
        {
            this.swApplication.FileNewNotify2 -= this.OnNewFile;
            this.swApplication.FileOpenPostNotify -= this.OnFileOpen;
            this.documentManager.OnDocumentAdded -= this.OnOpenDocumentAdded;

            foreach (var swEventHandler in this.solidWorksEventHandlers)
            {
                swEventHandler.DetachSwEvents(this.swApplication);
            }

            this.solidWorksEventHandlers.Clear();

            foreach (var document in this.documentManager.GetOpenDocuments())
            {
                foreach (var docEventHandler in this.documentEventHandlers)
                {
                    docEventHandler.DetachDocumentEvents(document);
                }

                document.OnDestroy -= this.DetachEventsFromDocument;
            }

            this.documentEventHandlers.Clear();
        }
    }
}
