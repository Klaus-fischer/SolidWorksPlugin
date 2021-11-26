// <copyright file="DocumentManagerFactory.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin.Helper
{
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Factory class for <see cref="IDocumentManager"/> instances.
    /// </summary>
    public class DocumentManagerFactory
    {
        /// <summary>
        /// Creates an <see cref="IDocumentManager"/>.
        /// </summary>
        /// <param name="swApplication">The current solid works application.</param>
        /// <returns>The build document manager.</returns>
        public IDocumentManager Build(ISldWorks swApplication)
        {
            var documentFactory = new SwDocumentFactory();
            var iModelDocComparer = new SwModelDocPointerEqualityComparer(swApplication);

            var documentManager = new DocumentManager(swApplication, documentFactory, iModelDocComparer);

            documentFactory.OpenDocumentCallBack = s => documentManager.OpenDocument(s, out _);
            documentFactory.GetDocumentCallBack = documentManager.GetDocument;

            return documentManager;
        }
    }
}
