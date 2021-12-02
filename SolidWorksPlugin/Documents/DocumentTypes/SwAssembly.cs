// <copyright file="SwAssembly.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Document class for <see cref="AssemblyDoc"/> models.
    /// </summary>
    internal sealed class SwAssembly : SwDocument, IDisposable, ISwAssembly
    {
        private readonly AssemblyDoc model;
        private readonly Func<IModelDoc2, ISwDocument>? getDocumentCallBack;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwAssembly"/> class.
        /// </summary>
        /// <param name="model">Part Model of the document.</param>
        /// <param name="getDocumentCallBack">Callback to open a document.</param>
        internal SwAssembly(AssemblyDoc model, Func<IModelDoc2, ISwDocument>? getDocumentCallBack)
            : base((IModelDoc2)model)
        {
            this.model = model;
            this.getDocumentCallBack = getDocumentCallBack;

            this.model.FileSaveNotify += this.OnFileSaveNotify;
            this.model.FileSaveAsNotify2 += this.OnFileSaveAsNotify2;
            this.model.FileSavePostNotify += this.OnFileSavePostNotify;
            this.model.AddCustomPropertyNotify += this.OnAddCustomPropertyNotify;
            this.model.ChangeCustomPropertyNotify += this.OnChangeCustomPropertyNotify;
            this.model.DeleteCustomPropertyNotify += this.OnDeleteCustomPropertyNotify;
            this.model.DestroyNotify2 += this.OnDestroyNotify2;
        }

        /// <summary>
        /// Gets the model of the document as <see cref="IAssemblyDoc"/>.
        /// </summary>
        public AssemblyDoc Assembly => this.model;

        /// <inheritdoc/>
        public void Dispose()
        {
            this.model.FileSaveNotify -= this.OnFileSaveNotify;
            this.model.FileSaveAsNotify2 -= this.OnFileSaveAsNotify2;
            this.model.FileSavePostNotify -= this.OnFileSavePostNotify;
            this.model.AddCustomPropertyNotify -= this.OnAddCustomPropertyNotify;
            this.model.ChangeCustomPropertyNotify -= this.OnChangeCustomPropertyNotify;
            this.model.DeleteCustomPropertyNotify -= this.OnDeleteCustomPropertyNotify;
            this.model.DestroyNotify2 -= this.OnDestroyNotify2;
        }

        /// <summary>
        /// Returns all models of the components used in this assembly.
        /// </summary>
        /// <param name="topLevelOnly">Flag to get only the first level of items, or all sub items.</param>
        /// <returns>Collection of <see cref="SwDocument"/>.</returns>
        public IEnumerable<ISwDocument> GetReferencedDocuments(bool topLevelOnly)
        {
            if (this.Assembly.GetComponents(topLevelOnly) is object[] components)
            {
                foreach (Component2 comp in components
                    .OfType<Component2>()
                    .GroupBy(c => c.GetPathName())
                    .Select(g => g.First()))
                {
                    if (comp.GetModelDoc2() is ModelDoc2 subPart)
                    {
                        if (this.getDocumentCallBack?.Invoke(subPart) is ISwDocument document)
                        {
                            yield return document;
                        }
                    }
                }
            }
        }
    }
}
