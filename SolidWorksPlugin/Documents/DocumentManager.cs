// <copyright file="DocumentManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// The Document manager holds a reference to all open documents.
    /// It handles all Open Save Close operations.
    /// </summary>
    internal class DocumentManager : IDocumentManagerInternals, IDocumentManager, IDisposable
    {
        private readonly ISldWorks swApplication;
        private readonly ISwDocumentFactory documentFactory;
        private readonly Dictionary<IModelDoc2, ISwDocument> openDocuments;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentManager"/> class.
        /// </summary>
        /// <param name="swApplication">The current solid works application.</param>
        /// <param name="factory">The <see cref="ISwDocumentFactory"/> to use.</param>
        /// <param name="comparer">The comparer to determine equal <see cref="IModelDoc2"/> objects.</param>
        public DocumentManager(ISldWorks swApplication, ISwDocumentFactory factory, IEqualityComparer<IModelDoc2> comparer)
        {
            this.swApplication = swApplication;
            this.documentFactory = factory;
            this.openDocuments = new Dictionary<IModelDoc2, ISwDocument>(comparer);

            // loop through all unknown documents to add them to the dictionary.
            foreach (var doc in this.GetAllUnknownDocuments())
            {
            }
        }

        /// <summary>
        /// Raises an event, if a Document was created on <see cref="GetDocument"/> call.
        /// </summary>
        public event EventHandler<ISwDocument>? OnDocumentAdded;

        /// <inheritdoc/>
        public ISwDocument? ActiveDocument
        {
            get
            {
                if (this.swApplication.ActiveDoc is IModelDoc2 model)
                {
                    if (this.openDocuments.TryGetValue(model, out var value))
                    {
                        return value;
                    }
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
        public ILogger<DocumentManager>? Logger { get; set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.openDocuments.Any())
            {
                this.openDocuments.Clear();
            }
        }

        /// <inheritdoc/>
        public ISwDocument OpenDocument(string filename, out bool wasOpen, swOpenDocOptions_e options = swOpenDocOptions_e.swOpenDocOptions_Silent)
        {
            var modelDoc = this.swApplication.GetOpenDocumentByName(filename) as IModelDoc2;
            wasOpen = true;

            if (modelDoc == null)
            {
                wasOpen = false;

                try
                {
                    modelDoc = this.OpenDocument(filename, options);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex);
                    throw;
                }
            }

            this.Logger?.LogDebug($"File {filename} opened successfully.");

            return this.GetDocument(modelDoc);
        }

        /// <inheritdoc/>
        public void SaveDocument(ISwDocument document, string? filename = null, bool saveAsCopy = false, object? exportData = null)
        {
            int error = 0, warnings = 0;
            var options = saveAsCopy
                ? swSaveAsOptions_e.swSaveAsOptions_Copy | swSaveAsOptions_e.swSaveAsOptions_Silent
                : swSaveAsOptions_e.swSaveAsOptions_Silent;

            if (filename is null)
            {
                document.Model.Save3((int)options, ref error, ref warnings);
                this.Logger?.LogDebug($"File {document.Filename} saved.");
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

                this.Logger?.LogDebug($"File {document.Filename} saved as {filename}.");
            }
        }

        /// <inheritdoc/>
        public void ReloadDocument(ISwDocument document, bool readOnly)
        {
            document.Model.ReloadOrReplace(readOnly, document.FilePath, true);
            this.Logger?.LogDebug($"File {document.Filename} reloaded.");
        }

        /// <inheritdoc/>
        public void CloseDocument(ISwDocument document)
        {
            try
            {
                this.swApplication.CloseDoc(document.FilePath);
                this.Logger?.LogDebug($"File {document.Filename} reloaded.");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Error on closing {document.Filename}.");
                throw;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<ISwDocument> GetAllUnknownDocuments()
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
        public ISwDocument GetDocument(IModelDoc2 model)
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

        /// <inheritdoc/>
        public IEnumerable<ISwDocument> GetOpenDocuments(bool all)
            => all ? this.openDocuments.Values : this.GetApplicationDocuments();

        /// <inheritdoc/>
        public void DisposeDocument(ISwDocument swDocument)
        {
            this.openDocuments.Remove(swDocument.Model);
        }

        private IEnumerable<ISwDocument> GetApplicationDocuments()
        {
            var swDocument = this.swApplication.GetFirstDocument();

            while (swDocument is IModelDoc2 modelDocument)
            {
                yield return this.GetDocument(modelDocument);

                swDocument = modelDocument.GetNext();
            }
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

            if (doc == null)
            {
                throw new InvalidOperationException($"Document could not be opened.\n Error:{(swFileLoadError_e)error}\n Warning:{(swFileLoadWarning_e)warning}");
            }

            return doc;
        }
    }
}
