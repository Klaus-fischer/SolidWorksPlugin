// <copyright file="SwAssembly.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace SIM.SolidWorksPlugin
{
    public interface ISwAssembly
    {
        AssemblyDoc Assembly { get; }

        IEnumerable<ISwDocument> GetReferencedDocuments(bool topLevelOnly);
    }
}