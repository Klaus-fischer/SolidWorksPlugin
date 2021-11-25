// <copyright file="Delegates.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

#pragma warning disable SA1649 // File name should match first type name

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.swconst;
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
    /// Delegate for <see cref="ICommandGroupHandler"/> factory method.
    /// </summary>
    /// <param name="builder">The command group builder.</param>
    public delegate void CommandGroupBuilderDelegate(ICommandGroupBuilder builder);

    /// <summary>
    /// Delegate for <see cref="ICommandGroupHandlerExtensions"/> factory method.
    /// </summary>
    /// <typeparam name="TArgs">Type of the command enumeration.</typeparam>
    /// <param name="builder">The command group builder.</param>
    public delegate void CommandGroupBuilderDelegate<TArgs>(ICommandGroupBuilder<TArgs> builder)
        where TArgs : struct, Enum;

    /// <summary>
    /// Delegate for <see cref="ICommandTabManager"/> factory method.
    /// </summary>
    /// <param name="builder">The command tab builder.</param>
    public delegate void CommandTabBuilderDelegate(ICommandTabBuilder builder);

    /// <summary>
    /// Defines a callback for user messages.
    /// </summary>
    /// <param name="message">The message to submit.</param>
    /// <param name="icon">The icon to display.</param>
    /// <param name="buttons">The message buttons.</param>
    /// <returns>Result of the dialog.</returns>
    public delegate swMessageBoxResult_e MessageToUserCallback(
        string message,
        swMessageBoxIcon_e icon = swMessageBoxIcon_e.swMbInformation,
        swMessageBoxBtn_e buttons = swMessageBoxBtn_e.swMbOk);
}
