// <copyright file="IDocumentManagerInternals.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Extends <see cref="IDocumentManager"/> for internal use.
    /// </summary>
    internal interface IDocumentManagerInternals : IDocumentManager, IDisposable
    {
        /// <summary>
        /// Raises an event, if a Document was created by late access.
        /// </summary>
        event EventHandler<ISwDocument>? OnDocumentAdded;

        /// <summary>
        /// Gets or sets the logger to log to.
        /// </summary>
        ILogger? Logger { get; set; }

        /// <summary>
        /// Removes the document from the known documents list.
        /// </summary>
        /// <param name="document">The document to remove.</param>
        void DisposeDocument(ISwDocument document);

        /// <summary>
        /// Look for all open documents in solid works and returns all, documents that are not the open list yet.
        /// After returning the item, it will be added to the open documents collection.
        /// </summary>
        /// <returns>The collection of all unknown documents.</returns>
        IEnumerable<ISwDocument> GetAllUnknownDocuments();
    }
}