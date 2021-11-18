// <copyright file="SwAssembly.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.sldworks;

    public sealed class SwAssembly: SwDocument, IDisposable
    {
        private readonly AssemblyDoc model;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwAssembly"/> class.
        /// </summary>
        /// <param name="model">Part Model of the document.</param>
        internal SwAssembly(AssemblyDoc model)
            : base((IModelDoc2)model)
        {
            this.model = model;
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
    }
}
