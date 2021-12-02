// <copyright file="ICommandGroupHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Allows to add commands to command group.
    /// </summary>
    public interface ICommandGroupHandler
    {
        /// <summary>
        /// Adds an command group to the command handler.
        /// </summary>
        /// <param name="commandGroupSpec">The command group info.</param>
        /// <param name="factoryMethod">Method to add all commands.</param>
        void AddCommandGroup(CommandGroupSpec commandGroupSpec, CommandGroupBuilderDelegate factoryMethod);
    }
}
