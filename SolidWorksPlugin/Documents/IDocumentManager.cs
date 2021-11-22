// <copyright file="IDocumentManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.swconst;

    public interface IDocumentManager
    {
        SwDocument? ActiveDocument { get; set; }

        SwDocument OpenDocument(string filename, out bool wasOpen, swOpenDocOptions_e options = swOpenDocOptions_e.swOpenDocOptions_Silent);


        void SaveDocument(SwDocument document, string? filename = null, bool saveAsCopy = false, object? exportData = null);


        void CloseDocument(SwDocument document);
    }
}
