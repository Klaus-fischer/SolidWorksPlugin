// <copyright file="ICommandHandlerInternals.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Extends <see cref="ICommandGroupHandler"/> for internal use.
    /// </summary>
    internal interface ICommandHandlerInternals : ICommandGroupHandler, IDisposable
    {
        /// <summary>
        /// Gets the command manager.
        /// </summary>
        ICommandManager SwCommandManager { get; }

        /// <summary>
        /// Adds an command group to the command handler.
        /// </summary>
        /// <typeparam name="T">Type of the command enumeration.</typeparam>
        /// <param name="factoryMethod">Method to add all commands.</param>
        /// <param name="path">relative path to build sub menus.</param>
        void AddCommandGroup<T>(Action<ICommandHandler<T>> factoryMethod, string path)
            where T : struct, Enum;

        /// <summary>
        /// Generates the callback method names.
        /// </summary>
        /// <typeparam name="T">Type of the enumeration.</typeparam>
        /// <param name="id">Value of the enumeration.</param>
        /// <returns>both method names.</returns>
        (string OnExecute, string CanExecute) GetCallbackNames<T>(T id);
    }
}