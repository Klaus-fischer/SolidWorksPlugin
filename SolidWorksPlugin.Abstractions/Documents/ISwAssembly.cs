// <copyright file="ISwAssembly.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System.Collections.Generic;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Document class declaration for <see cref="AssemblyDoc"/> models.
    /// </summary>
    public interface ISwAssembly : ISwDocument
    {
        /// <summary>
        /// Gets the model of the document as <see cref="IAssemblyDoc"/>.
        /// </summary>
        AssemblyDoc Assembly { get; }

        /// <summary>
        /// Returns all models of the components used in this assembly.
        /// </summary>
        /// <param name="topLevelOnly">Flag to get only the first level of items, or all sub items.</param>
        /// <returns>Collection of <see cref="SwDocument"/>.</returns>
        IEnumerable<ISwDocument> GetReferencedDocuments(bool topLevelOnly);
    }
}