// <copyright file="Interface1.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class DocumentManager : IDocumentManager, IDisposable
    {
        private readonly SldWorks swApplication;
        private readonly SwDocumentFactory documentFactory;
        private readonly Dictionary<IModelDoc2, SwDocument> openDocuments;

        public DocumentManager(SldWorks swApplication)
        {
            this.swApplication = swApplication;
            this.documentFactory = new SwDocumentFactory();
            this.openDocuments = new Dictionary<IModelDoc2, SwDocument>(
                new SwModelPointerEqualityComparer(swApplication));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.openDocuments.Any())
            {
                this.openDocuments.Clear();
            }
        }

        public SwDocument? ActiveDocument { get; set; }

        public void CloseDocument(SwDocument document)
        {
        }

        public IEnumerable<SwDocument> GetOpenDocuments() => this.openDocuments.Values;

        public SwDocument OpenDocument(string filename, out bool wasOpen, swOpenDocOptions_e options = swOpenDocOptions_e.swOpenDocOptions_Silent)
        {
            wasOpen = true;
            return new SwDocument(null);
        }

        public SwDocument SaveDocument(SwDocument document, string? filename = null, bool saveAsCopy = false)
        {
            return new SwDocument(null);
        }

        public bool TryOpenDocument(string filename, out SwDocument swDocument)
        {
            swDocument = new SwDocument(null);
            return false;
        }

        internal IEnumerable<SwDocument> GetAllUnknownDocuments()
        {
            var swDocument = this.swApplication.GetFirstDocument();

            while (swDocument is IModelDoc2 modelDocument)
            {
                if (!this.openDocuments.ContainsKey(modelDocument))
                {
                    var document = this.documentFactory.Create(modelDocument);

                    this.openDocuments.Add(modelDocument, document);

                    yield return document;
                }

                swDocument = modelDocument.GetNext();
            }
        }
    }
}
