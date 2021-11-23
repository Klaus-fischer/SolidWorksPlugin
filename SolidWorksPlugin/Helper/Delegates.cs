// <copyright file="Delegates.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

#pragma warning disable SA1649 // File name should match first type name

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Generic Event handler for <see cref="ISwDocument"/> class.
    /// </summary>
    /// <typeparam name="TArgs">Type of the event arguments.</typeparam>
    /// <param name="document">The document that raises the event.</param>
    /// <param name="args">The event arguments.</param>
    /// <returns>SolidWorks expects an integer result. Should be 0 if everything is OK.</returns>
    public delegate int DocumentEventHandler<TArgs>(ISwDocument document, TArgs args);

    /// <summary>
    /// Delegate for <see cref="CommandHandler"/> factory method.
    /// </summary>
    /// <param name="commandGroupBuilder">The command group builder.</param>
    public delegate void CommandGroupBuilderDelegate(ICommandGroupBuilder commandGroupBuilder);

    /// <summary>
    /// Delegate for <see cref="ICommandGroupHandlerExtensions"/> factory method.
    /// </summary>
    /// <typeparam name="TArgs">Type of the command enumeration.</typeparam>
    /// <param name="commandGroupBuilder">The command group builder.</param>
    public delegate void CommandGroupBuilderDelegate<TArgs>(ICommandGroupBuilder<TArgs> commandGroupBuilder)
        where TArgs : struct, Enum;
}
