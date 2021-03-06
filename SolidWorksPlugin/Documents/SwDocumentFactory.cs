// <copyright file="SwDocumentFactory.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Factory of <see cref="ISwDocument"/> classes.
    /// </summary>
    internal class SwDocumentFactory : ISwDocumentFactory
    {
        private readonly PropertyManager propertyManager = new();

        /// <summary>
        /// Gets or sets the callback to open a document.
        /// </summary>
        public Func<string, ISwDocument>? OpenDocumentCallBack { get; set; }

        /// <summary>
        /// Gets or sets the callback to get a open document.
        /// </summary>
        public Func<IModelDoc2, ISwDocument>? GetDocumentCallBack { get; set; }

        /// <summary>
        /// Creates a <see cref="ISwDocument"/> based on the model type.
        /// </summary>
        /// <param name="model">SolidWorks model to check.</param>
        /// <returns>The created document.</returns>
        public ISwDocument Create(IModelDoc2 model)
        {
            return model switch
            {
                PartDoc part => new SwPart(part)
                {
                    PropertyManagerCallBack = this.GetPropertyManager,
                },

                AssemblyDoc assembly => new SwAssembly(assembly, this.GetDocumentCallBack)
                {
                    PropertyManagerCallBack = this.GetPropertyManager,
                },

                DrawingDoc drawing => new SwDrawing(drawing, this.OpenDocumentCallBack)
                {
                    PropertyManagerCallBack = this.GetPropertyManager,
                },

                _ => throw new InvalidCastException(),
            };
        }

        private IPropertyManager GetPropertyManager(IModelDoc2 model)
        {
            this.propertyManager.ActiveModel = model;
            return this.propertyManager;
        }
    }
}
