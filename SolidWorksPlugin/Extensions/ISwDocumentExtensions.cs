// <copyright file="ISwDocumentExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// Extensions for <see cref="ISwDocument"/> class.
    /// </summary>
    public static class ISwDocumentExtensions
    {
        /// <summary>
        /// Rebuilds the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="topOnly">True to rebuild all sub documents.</param>
        public static void RebuildDocument(ISwDocument document, bool topOnly)
            => document.Model.ForceRebuild3(topOnly);

        /// <summary>
        /// Sets the save flag for the current document.
        /// </summary>
        /// <param name="document">The document.</param>
        public static void SetSaveIndicatorFlag(ISwDocument document)
            => document.Model.SetSaveFlag();
    }
}
