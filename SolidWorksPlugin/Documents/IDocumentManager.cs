// <copyright file="Interface1.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.swconst;

    public interface IDocumentManager
    {
        SwDocument OpenDocument(string filename, out bool wasOpen, swOpenDocOptions_e options = swOpenDocOptions_e.swOpenDocOptions_Silent);

        bool TryOpenDocument(string filename, out SwDocument swDocument);

        SwDocument SaveDocument(SwDocument document, string? filename = null, bool saveAsCopy = false);

        void CloseDocument(SwDocument document);

        SwDocument? ActiveDocument { get; set; }
    }

    internal class DocumentManager : IDocumentManager
    {
        public SwDocument? ActiveDocument { get; set; }

        public void CloseDocument(SwDocument document)
        {
        }

        public SwDocument OpenDocument(string filename, out bool wasOpen, swOpenDocOptions_e options = swOpenDocOptions_e.swOpenDocOptions_Silent)
        {
            wasOpen = true;
            return new SwDocument();
        }

        public SwDocument SaveDocument(SwDocument document, string? filename = null, bool saveAsCopy = false)
        {
            return new SwDocument();
        }

        public bool TryOpenDocument(string filename, out SwDocument swDocument)
        {
            swDocument = new SwDocument();
            return false;
        }
    }

    public class SwDocument
    {

    }
}
