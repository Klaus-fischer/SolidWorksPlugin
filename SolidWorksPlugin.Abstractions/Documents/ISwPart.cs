// <copyright file="SwPart.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using SolidWorks.Interop.sldworks;

namespace SIM.SolidWorksPlugin
{
    public interface ISwPart
    {
        IPartDoc Part { get; }
    }
}