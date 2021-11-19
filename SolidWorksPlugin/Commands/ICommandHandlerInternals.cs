// <copyright file="ICommandHandlerInternals.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Extends <see cref="ICommandGroupHandler"/> for internal use.
    /// </summary>
    internal interface ICommandHandlerInternals : ICommandGroupHandler, IDisposable
    {
    }
}