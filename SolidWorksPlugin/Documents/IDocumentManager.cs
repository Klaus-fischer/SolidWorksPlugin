// <copyright file="IDocumentManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.swconst;
    using System.Collections.Generic;

    public interface IDocumentManager
    {
        IEnumerable<SwDocument> GetOpenDocuments();

        SwDocument OpenDocument(string filename, out bool wasOpen, swOpenDocOptions_e options = swOpenDocOptions_e.swOpenDocOptions_Silent);

        bool TryOpenDocument(string filename, out SwDocument swDocument);

        SwDocument SaveDocument(SwDocument document, string? filename = null, bool saveAsCopy = false);

        void CloseDocument(SwDocument document);

        SwDocument? ActiveDocument { get; set; }
    }
}
