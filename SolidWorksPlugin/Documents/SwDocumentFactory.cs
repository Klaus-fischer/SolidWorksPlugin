// <copyright file="SwDocumentFactory.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.sldworks;

    internal class SwDocumentFactory
    {
        /// <summary>
        /// Creates a <see cref="SwDocument"/> based on the model type.
        /// </summary>
        /// <param name="model">SolidWorks model to check.</param>
        /// <returns>The created document.</returns>
        public SwDocument Create(IModelDoc2 model)
        {
            switch (model)
            {
                case PartDoc part:
                    return new SwPart(part);

                case AssemblyDoc assembly:
                    return new SwAssembly(assembly);

                case DrawingDoc drawing:
                    return new SwDrawing(drawing);

                default:
                    throw new InvalidCastException();
            }
        }
    }
}
