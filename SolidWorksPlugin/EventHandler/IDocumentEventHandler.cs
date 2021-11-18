﻿// <copyright file="IDocumentEventHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// Declaration if an event handler.
    /// </summary>
    public interface IDocumentEventHandler
    {
        /// <summary>
        /// Attaches all events to the document event handler.
        /// </summary>
        /// <param name="document">Document to attach events to.</param>
        /// <returns>True if connected successfully.</returns>
        bool AttachDocumentEvents(SwDocument document);

        /// <summary>
        /// Detaches all events from the document event handler.
        /// </summary>
        /// <param name="document">Document to detach events from.</param>
        void DetachDocumentEvents(SwDocument document);
    }
}
