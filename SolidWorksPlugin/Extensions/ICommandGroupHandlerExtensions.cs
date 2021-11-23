// <copyright file="ICommandGroupHandlerExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Extends <see cref="ICommandGroupHandler"/> by command enumeration overloading.
    /// </summary>
    public static class ICommandGroupHandlerExtensions
    {
        /// <summary>
        /// Adds an command group to the command handler based on a command enumeration.
        /// </summary>
        /// <typeparam name="T">Type of the command enumeration.</typeparam>
        /// <param name="handler">The command group handler to extend.</param>
        /// <param name="factoryMethod">Method to add all commands.</param>
        public static void AddCommandGroup<T>(this ICommandGroupHandler handler, CommandGroupBuilderDelegate<T> factoryMethod)
         where T : struct, Enum
        {
            (var info, var icons) = GetIconsAndInfo(typeof(T));

            var commandGroupInfo = new CommandGroupInfo(
                userId: info.CommandGroupId,
                title: info.Title)
            {
                Position = info.Position,
                Hint = info.Hint,
                Tooltip = info.ToolTip,
            };

            CommandGroupBuilderDelegate factoryAction = d => AddCommands(d, info.CommandGroupId, factoryMethod);

            if (icons is not null)
            {
                commandGroupInfo.IconsPath = icons.IconsPath;
                commandGroupInfo.MainIconPath = icons.MainIconPath;
            }

            handler.AddCommandGroup(commandGroupInfo, factoryAction);
        }

        private static void AddCommands<T>(ICommandGroupBuilder d, int commandGroupId, CommandGroupBuilderDelegate<T> factoryMethod)
            where T : struct, Enum
        {
            var cmdBuilder = new CommandGroupBuilder<T>(d, commandGroupId);

            factoryMethod(cmdBuilder);
        }

        internal static (CommandGroupInfoAttribute Info, CommandGroupIconsAttribute? Icons) GetIconsAndInfo(this Type enumType)
        {
            if (enumType.GetCustomAttribute<CommandGroupInfoAttribute>() is not CommandGroupInfoAttribute info)
            {
                throw new ArgumentException($"Attribute {nameof(CommandGroupInfoAttribute)} is not defined on Enum '{enumType.Name}'");
            }

            var icons = enumType.GetCustomAttribute<CommandGroupIconsAttribute>();

            return (info, icons);
        }
    }
}
