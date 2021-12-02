// <copyright file="SwDrawing.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using SolidWorks.Interop.sldworks;

namespace SIM.SolidWorksPlugin
{
    public interface ISwDrawing
    {
        ISwDocument? Document { get; }
        IDrawingDoc Drawing { get; }
    }
}