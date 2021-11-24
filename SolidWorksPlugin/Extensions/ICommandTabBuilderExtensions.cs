// <copyright file="ICommandTabBuilderExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// Extensions for <see cref="ICommandTabBuilder"/> instances.
    /// </summary>
    public static class ICommandTabBuilderExtensions
    {
        /// <summary>
        /// Adds a command based on the command enumeration to the current tab page.
        /// </summary>
        /// <typeparam name="T">Type of the command enumeration.</typeparam>
        /// <param name="tabBuilder">The command tab builder to extend.</param>
        /// <param name="id">The command id.</param>
        /// <param name="textDisplay">The text display options.</param>
        public static void AddCommand<T>(this ICommandTabBuilder tabBuilder, T id, swCommandTabButtonTextDisplay_e textDisplay)
            where T : struct, Enum
        {
            var cmdInfo = tabBuilder.CommandHandler.GetCommand(id)
                ?? throw new ArgumentNullException(nameof(id), "Command is not defined in the Command Handler.");

            tabBuilder.AddCommand(cmdInfo, textDisplay);
        }

        /// <summary>
        /// Adds a tool bar as a fly out menu to the current tab page.
        /// </summary>
        /// <typeparam name="T">Type of the command enumeration.</typeparam>
        /// <param name="tabBuilder">The command tab builder to extend.</param>
        /// <param name="textDisplay">The text display style.</param>
        /// <param name="flyoutStyle">The fly out display style.</param>
        public static void AddFlyout<T>(
            this ICommandTabBuilder tabBuilder,
            swCommandTabButtonTextDisplay_e textDisplay,
            swCommandTabButtonFlyoutStyle_e flyoutStyle = swCommandTabButtonFlyoutStyle_e.swCommandTabButton_ActionFlyout)
            where T : struct, Enum
        {
            var cmdGroupInfo = tabBuilder.CommandHandler.GetCommandGroup<T>()
            ?? throw new ArgumentNullException(nameof(T), $"Command group {typeof(T).Name} is not defined in the Command Handler.");

            tabBuilder.AddFlyout(cmdGroupInfo, textDisplay, flyoutStyle);
        }
    }
}
