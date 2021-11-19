// <copyright file="IEventHandlerManagerInternals.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Extends <see cref="IEventHandlerManager"/> for internal use.
    /// </summary>
    internal interface IEventHandlerManagerInternals : IEventHandlerManager, IDisposable
    {
    }
}