// <copyright file="IInternalCommandHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;
    using System;

    /// <summary>
    /// Defines the signature for <see cref="ICommandHandler"/> interface.
    /// </summary>
    internal interface IInternalCommandHandler : ICommandHandler, ICommandGroupHandler, IDisposable
    {
        ICommandManager SwCommandManager { get; }
    }
}
