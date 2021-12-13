// <copyright file="IInternalCommandHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using Microsoft.Extensions.Logging;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Defines the signature for <see cref="ICommandHandler"/> interface.
    /// </summary>
    internal interface IInternalCommandHandler : ICommandHandler, ICommandGroupHandler, IDisposable
    {
        /// <summary>
        /// Gets the SolidWorks command manager.
        /// </summary>
        ICommandManager SwCommandManager { get; }

        /// <summary>
        /// Gets or sets the logger to log to.
        /// </summary>
        ILogger<CommandHandler>? Logger { get; set; }
    }
}
