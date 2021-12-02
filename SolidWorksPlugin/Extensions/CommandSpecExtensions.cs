// <copyright file="CommandSpecExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// Extensions for <see cref="CommandSpec"/> objects.
    /// </summary>
    internal static class CommandSpecExtensions
    {
        /// <summary>
        /// Converts the <see cref="CommandSpec.HasMenu"/> and <see cref="CommandSpec.Tooltip"/> values to an <see cref="swCommandItemType_e"/> value.
        /// </summary>
        /// <param name="spec">The command specification to extend.</param>
        /// <returns>The command item type.</returns>
        internal static int GetSwCommandItemType_e(this CommandSpec spec)
        {
            swCommandItemType_e menuOptions = 0;
            if (spec.HasMenu == true)
            {
                menuOptions |= swCommandItemType_e.swMenuItem;
            }

            if (spec.HasToolbar == true)
            {
                menuOptions |= swCommandItemType_e.swToolbarItem;
            }

            return (int)menuOptions;
        }
    }
}
