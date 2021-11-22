// <copyright file="Delegates.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

#pragma warning disable SA1649 // File name should match first type name

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// Generic Event handler for <see cref="ISwDocument"/> class.
    /// </summary>
    /// <typeparam name="TArgs">Type of the event arguments.</typeparam>
    /// <param name="document">The document that raises the event.</param>
    /// <param name="args">The event arguments.</param>
    /// <returns>SolidWorks expects an integer result. Should be 0 if everything is OK.</returns>
    public delegate int DocumentEventHandler<TArgs>(ISwDocument document, TArgs args);
}
