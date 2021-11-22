﻿// <copyright file="SwDocumentFactory.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Factory of <see cref="SwDocument"/> classes.
    /// </summary>
    internal class SwDocumentFactory : ISwDocumentFactory
    {
        private readonly PropertyManager propertyManager = new();

        /// <summary>
        /// Creates a <see cref="SwDocument"/> based on the model type.
        /// </summary>
        /// <param name="model">SolidWorks model to check.</param>
        /// <returns>The created document.</returns>
        public SwDocument Create(IModelDoc2 model)
        {
            return model switch
            {
                PartDoc part => new SwPart(part)
                {
                    PropertyManagerCallBack = this.GetPropertyManager,
                },

                AssemblyDoc assembly => new SwAssembly(assembly)
                {
                    PropertyManagerCallBack = this.GetPropertyManager,
                },

                DrawingDoc drawing => new SwDrawing(drawing)
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
