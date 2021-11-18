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

    internal class EventHandlerManager : IEventHandlerManager, IDisposable
    {
        private readonly ICollection<IDocumentEventHandler> documentEventHandlers
            = new Collection<IDocumentEventHandler>();

        private readonly ICollection<ISolidWorksEventHandler> solidWorksEventHandlers
            = new Collection<ISolidWorksEventHandler>();

        private readonly SldWorks swApplication;
        private readonly DocumentManager documentManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerManager"/> class.
        /// </summary>
        /// <param name="swApplication">The current solid works application.</param>
        /// <param name="documentManager">The current used document manager.</param>
        public EventHandlerManager(SldWorks swApplication, DocumentManager documentManager)
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
            this.documentManager.OnDocumentCreated += this.OnOpenDocumentAdded;
            this.swApplication.FileNewNotify2 += this.OnNewFile;
            this.swApplication.FileOpenPostNotify += this.OnFileOpen;
        }

        private void OnOpenDocumentAdded(object? sender, SwDocument document)
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

        private void AttachEventsToDocument(SwDocument document)
        {
            foreach (var docEventHandler in this.documentEventHandlers)
            {
                docEventHandler.AttachDocumentEvents(document);
            }

            document.OnDestroy += this.DetachEventsFromDocument;
        }

        private int DetachEventsFromDocument(SwDocument document, swDestroyNotifyType_e destroyType)
        {
            if (destroyType == swDestroyNotifyType_e.swDestroyNotifyDestroy)
            {
                foreach (var docEventHandler in this.documentEventHandlers)
                {
                    docEventHandler.DetachDocumentEvents(document);
                }
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
