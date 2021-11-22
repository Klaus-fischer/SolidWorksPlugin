// <copyright file="SolidWorksAddinExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin.Extensions
{
    using System;
    using System.Reflection;

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
        public static void RegisterWindow(this SolidWorksAddin addIn, object window)
        {
            if (Type.GetType("System.Windows.Interop.WindowInteropHelper") is Type interopHelperType &&
                interopHelperType.GetProperty("Owner") is PropertyInfo pi)
            {
                var interopHelper = Activator.CreateInstance(interopHelperType, new object[] { window });
                pi.SetValue(interopHelper, System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
            }
        }
    }
}
