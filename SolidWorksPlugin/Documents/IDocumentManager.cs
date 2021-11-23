// <copyright file="IDocumentManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// The document manager allows to open / save / close filed.
    /// It handles with <see cref="ISwDocument"/> wrapper.
    /// </summary>
    public interface IDocumentManager
    {
        /// <summary>
        /// Gets or sets the active document in application.
        /// </summary>
        ISwDocument? ActiveDocument { get; set; }

        /// <summary>
        /// Opens the document from file system.
        /// </summary>
        /// <param name="filename">The full file name to open.</param>
        /// <param name="wasOpen">a flag that indicates if document was already open.</param>
        /// <param name="options">The <see cref="swOpenDocOptions_e"/>.</param>
        /// <returns>The loaded document wrapped in <see cref="ISwDocument"/> class.</returns>
        /// <exception cref="InvalidOperationException">If Application is busy over 10 trials.</exception>
        ISwDocument OpenDocument(string filename, out bool wasOpen, swOpenDocOptions_e options = swOpenDocOptions_e.swOpenDocOptions_Silent);

        /// <summary>
        /// Saves the document to the file system.
        /// </summary>
        /// <param name="document">The document to save.</param>
        /// <param name="filename">Target filename (if null, known filename will be taken).</param>
        /// <param name="saveAsCopy">Flag to save document as copy (only if filename specified).</param>
        /// <param name="exportData">The export data (PDF export).</param>
        void SaveDocument(ISwDocument document, string? filename = null, bool saveAsCopy = false, object? exportData = null);

        /// <summary>
        /// Discards all changes an reloads the document.
        /// </summary>
        /// <param name="document">The document to reload.</param>
        /// <param name="readOnly">True if file should be reloaded as read only.</param>
        void ReloadDocument(ISwDocument document, bool readOnly);

        /// <summary>
        /// Closes the document in SolidWorks application.
        /// </summary>
        /// <param name="document">The document to close.</param>
        void CloseDocument(ISwDocument document);
    }
}
