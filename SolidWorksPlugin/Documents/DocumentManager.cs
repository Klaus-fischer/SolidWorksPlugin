// <copyright file="DocumentManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// The Document manager holds a reference to all open documents.
    /// It handles all Open Save Close operations.
    /// </summary>
    internal class DocumentManager : IDocumentManagerInternals, IDocumentManager, IDisposable
    {
        private readonly SldWorks swApplication;
        private readonly SwDocumentFactory documentFactory;
        private readonly Dictionary<IModelDoc2, SwDocument> openDocuments;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentManager"/> class.
        /// </summary>
        /// <param name="swApplication">The current solid works application.</param>
        public DocumentManager(SldWorks swApplication)
        {
            this.swApplication = swApplication;
            this.documentFactory = new SwDocumentFactory();
            this.openDocuments = new Dictionary<IModelDoc2, SwDocument>(
                new SwModelDocPointerEqualityComparer(swApplication));
        }

        /// <summary>
        /// Raises an event, if a Document was created on <see cref="GetDocument"/> call.
        /// </summary>
        public event EventHandler<ISwDocumentEvents>? OnDocumentAdded;

        /// <inheritdoc/>
        public SwDocument? ActiveDocument
        {
            get
            {
                if (this.swApplication.ActiveDoc is IModelDoc2 model)
                {
                    return this.GetDocument(model);
                }

                return null;
            }

            set
            {
                if (value is not null)
                {
                    this.swApplication.ActivateDoc(value.Filename);
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.openDocuments.Any())
            {
                this.openDocuments.Clear();
            }
        }

        /// <inheritdoc/>
        public SwDocument OpenDocument(string filename, out bool wasOpen, swOpenDocOptions_e options = swOpenDocOptions_e.swOpenDocOptions_Silent)
        {
            var modelDoc = this.swApplication.GetOpenDocumentByName(filename) as IModelDoc2;
            wasOpen = true;

            if (modelDoc == null)
            {
                wasOpen = false;
                modelDoc = this.OpenDocument(filename, options);
            }

            return this.GetDocument(modelDoc);
        }

        /// <inheritdoc/>
        public void RebuildDocument(SwDocument document, bool topOnly)
            => document.Model.ForceRebuild3(topOnly);

        /// <inheritdoc/>
        public void SetSaveIndicatorFlag(SwDocument document)
            => document.Model.SetSaveFlag();

        /// <inheritdoc/>
        public void SaveDocument(SwDocument document, string? filename = null, bool saveAsCopy = false, object? exportData = null)
        {
            int error = 0, warnings = 0;
            var options = saveAsCopy
                ? swSaveAsOptions_e.swSaveAsOptions_Copy | swSaveAsOptions_e.swSaveAsOptions_Silent
                : swSaveAsOptions_e.swSaveAsOptions_Silent;

            if (filename is null)
            {
                document.Model.Save3((int)options, ref error, ref warnings);
            }
            else
            {
                document.Model.Extension.SaveAs2(
                    Name: filename,
                    Version: (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                    Options: (int)options,
                    ExportData: exportData,
                    ReferencePrefixOrSuffixText: null,
                    AddTextAsPrefix: false,
                    Errors: ref error,
                    Warnings: ref warnings);
            }
        }

        /// <inheritdoc/>
        public void Reload(SwDocument document, bool readOnly)
        {
            document.Model.ReloadOrReplace(readOnly, document.FilePath, true);
        }

        /// <inheritdoc/>
        public void CloseDocument(SwDocument document)
        {
            this.swApplication.CloseDoc(document.FilePath);
        }

        /// <inheritdoc/>
        public IEnumerable<ISwDocumentEvents> GetAllUnknownDocuments()
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

        /// <inheritdoc/>
        public IEnumerable<ISwDocumentEvents> GetOpenDocuments() => this.openDocuments.Values;

        private SwDocument GetDocument(IModelDoc2 model)
        {
            if (this.openDocuments.TryGetValue(model, out var document))
            {
                return document;
            }

            var result = this.documentFactory.Create(model: model);
            this.openDocuments.Add(model, result);
            this.OnDocumentAdded?.Invoke(this, result);

            return result;
        }

        private IModelDoc2 OpenDocument(string filename, swOpenDocOptions_e options)
        {
            int error = 0, warning = 0;
            int type = FileExtensions.GetDocumentType(filename);
            int loopCount = 0;

            ModelDoc2 doc;
            do
            {
                doc = this.swApplication.OpenDoc6(
                        filename,
                        type,
                        (int)options,
                        string.Empty,
                        ref error,
                        ref warning);

                if (++loopCount == 10)
                {
                    break;
                }
            }
            while (error == (int)swFileLoadError_e.swApplicationBusy);

            return doc;
        }
    }
}
