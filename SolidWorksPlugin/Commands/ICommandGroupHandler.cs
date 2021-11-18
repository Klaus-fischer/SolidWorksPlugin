// <copyright file="ICommandGroupHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Declares the command manager for add in callback.
    /// </summary>
    /// <typeparam name="T">Type of the command enumeration.</typeparam>
    public interface ICommandGroupHandler
    {
        /// <summary>
        /// Adds an command group to the command handler.
        /// </summary>
        /// <typeparam name="T">Type of the command enumeration.</typeparam>
        /// <param name="factoryMethod">Method to add all commands.</param>
        void AddCommandGroup<T>(Action<ICommandHandler<T>> factoryMethod)
            where T : struct, Enum;
    }
}
