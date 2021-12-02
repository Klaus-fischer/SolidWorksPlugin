// <copyright file="SwDrawing.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Document class for <see cref="DrawingDoc"/> models.
    /// </summary>
    internal sealed class SwDrawing : SwDocument, IDisposable, ISwDrawing
    {
        private readonly DrawingDoc model;
        private readonly Func<string, ISwDocument>? openDocumentCallBack;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwDrawing"/> class.
        /// </summary>
        /// <param name="model">Part Model of the document.</param>
        /// <param name="openDocumentCallBack">Callback to open a document.</param>
        internal SwDrawing(DrawingDoc model, Func<string, ISwDocument>? openDocumentCallBack)
            : base((IModelDoc2)model)
        {
            this.model = model;
            this.openDocumentCallBack = openDocumentCallBack;

            this.model.FileSaveNotify += this.OnFileSaveNotify;
            this.model.FileSaveAsNotify2 += this.OnFileSaveAsNotify2;
            this.model.FileSavePostNotify += this.OnFileSavePostNotify;
            this.model.AddCustomPropertyNotify += this.OnAddCustomPropertyNotify;
            this.model.ChangeCustomPropertyNotify += this.OnChangeCustomPropertyNotify;
            this.model.DeleteCustomPropertyNotify += this.OnDeleteCustomPropertyNotify;
            this.model.DestroyNotify2 += this.OnDestroyNotify2;
        }

        /// <summary>
        /// Gets the model of the document as <see cref="IDrawingDoc"/>.
        /// </summary>
        public IDrawingDoc Drawing => this.model;

        /// <summary>
        /// Gets the drawed document.
        /// </summary>
        public ISwDocument? Document => this.GetDocument();

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

        private ISwDocument? GetDocument()
        {
            if ((this.Drawing.GetFirstView() as IView)?.GetNextView() is IView view)
            {
                return this.openDocumentCallBack?.Invoke(view.GetReferencedModelName());
            }

            return null;
        }
    }
}
