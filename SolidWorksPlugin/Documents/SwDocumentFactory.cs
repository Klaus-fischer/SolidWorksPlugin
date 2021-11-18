// <copyright file="SwDocumentFactory.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.sldworks;

    internal class SwDocumentFactory
    {
        public SwDocument Create(IModelDoc2 model)
        {
            switch (model)
            {
                //case IPartDoc part:
                //    return new SwPart(part, manager);

                //case IAssemblyDoc assembly:
                //    return new SwAssembly(assembly, manager);

                //case IDrawingDoc drawing:
                //    return new SwDrawing(drawing, manager);

                default:
                    throw new InvalidCastException();
            }
        }
    }
}
