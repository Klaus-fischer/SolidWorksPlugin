// <copyright file="ICommandHandlerExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Extensions for <see cref="ICommandHandler"/> instances.
    /// </summary>
    public static class ICommandHandlerExtensions
    {
        /// <summary>
        /// Gets the command by the command enumeration.
        /// </summary>
        /// <typeparam name="T">Type of the command enumeration.</typeparam>
        /// <param name="commandHandler">The command handler to extend.</param>
        /// <param name="id">The command id.</param>
        /// <returns>The command, if registered, otherwise null.</returns>
        public static ICommandInfo? GetCommand<T>(this ICommandHandler commandHandler, T id)
            where T : struct, Enum
        {
            (var info, _) = typeof(T).GetSpecAndIconsAttribute();

            return commandHandler.GetCommand(info.CommandGroupId, (int)(object)id);
        }

        /// <summary>
        /// Gets the command group by the command enumeration.
        /// </summary>
        /// <typeparam name="T">Type of the command enumeration.</typeparam>
        /// <param name="commandHandler">The command handler to extend.</param>
        /// <returns>The command group informations.</returns>
        public static ICommandGroupInfo? GetCommandGroup<T>(this ICommandHandler commandHandler)
        {
            (var info, _) = typeof(T).GetSpecAndIconsAttribute();

            return commandHandler.GetCommandGroup(info.CommandGroupId);
        }
    }
}
