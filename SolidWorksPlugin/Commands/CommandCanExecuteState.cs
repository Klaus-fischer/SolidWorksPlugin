// <copyright file="CommandCanExecuteState.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Defines return values of <see cref="CommandHandler.CanExecute(string)"/> method.
    /// </summary>
    [Flags]
    internal enum CommandCanExecuteState : int
    {
        /// <summary>
        /// De-selects and disables the item.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// De-selects and enables the item; this is the default state if no update function is specified.
        /// </summary>
        Enabled = 1,

        /// <summary>
        /// Selects and disables the item.
        /// </summary>
        Selected = 2,

        /// <summary>
        /// Selects and enables the item.
        /// </summary>
        SelectedAndEnabled = Selected | Enabled,
    }
}
