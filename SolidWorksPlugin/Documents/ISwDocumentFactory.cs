// <copyright file="ISwDocumentFactory.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Declaration of the <see cref="SwDocumentFactory"/> used in <see cref="DocumentManager"/>.
    /// </summary>
    internal interface ISwDocumentFactory
    {
        /// <summary>
        /// Creates a <see cref="SwDocument"/> based on the model type.
        /// </summary>
        /// <param name="model">SolidWorks model to check.</param>
        /// <returns>The created document.</returns>
        SwDocument Create(IModelDoc2 model);
    }
}