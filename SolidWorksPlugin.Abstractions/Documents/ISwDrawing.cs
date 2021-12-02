// <copyright file="ISwDrawing.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Document class declaration for <see cref="DrawingDoc"/> models.
    /// </summary>
    public interface ISwDrawing : ISwDocument
    {
        /// <summary>
        /// Gets the draw document.
        /// </summary>
        ISwDocument? Document { get; }

        /// <summary>
        /// Gets the model of the document as <see cref="IDrawingDoc"/>.
        /// </summary>
        IDrawingDoc Drawing { get; }
    }
}