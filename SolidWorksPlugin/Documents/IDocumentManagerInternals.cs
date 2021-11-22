// <copyright file="IDocumentManagerInternals.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extends <see cref="IDocumentManager"/> for internal use.
    /// </summary>
    internal interface IDocumentManagerInternals : IDocumentManager, IDisposable
    {
        /// <summary>
        /// Raises an event, if a Document was created by late access.
        /// </summary>
        event EventHandler<ISwDocumentEvents>? OnDocumentAdded;

        /// <summary>
        /// Look for all open documents in solid works and returns all, documents that are not the open list yet.
        /// After returning the item, it will be added to the open documents collection.
        /// </summary>
        /// <returns>The collection of all unknown documents.</returns>
        IEnumerable<ISwDocumentEvents> GetAllUnknownDocuments();

        /// <summary>
        /// Enumerates all open documents.
        /// </summary>
        /// <returns>Collection of open documents.</returns>
        IEnumerable<ISwDocumentEvents> GetOpenDocuments();
    }
}