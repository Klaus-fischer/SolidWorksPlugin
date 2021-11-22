// <copyright file="SolidWorksAddinExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin.Windows
{
    using System.Windows;
    using System.Windows.Interop;

    /// <summary>
    /// Extension methods for <see cref="SolidWorksAddin"/> class.
    /// </summary>
    public static class SolidWorksAddinExtensions
    {
        /// <summary>
        /// Registers a window to be top most on the solid works screen.
        /// </summary>
        /// <param name="addIn">The add-in to extend.</param>
        /// <param name="window">The window.</param>
        public static void RegisterWindow(this SolidWorksAddin addIn, Window window)
        {
            new WindowInteropHelper(window)
            {
                Owner = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle,
            };
        }
    }
}
