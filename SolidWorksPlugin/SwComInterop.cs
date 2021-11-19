// <copyright file="SwComInterop.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using Microsoft.Win32;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Helper class that handles the com registration.
    /// </summary>
    public class SwComInterop
    {
        /// <summary>
        /// To register <see cref="ISldWorks"/> class.
        /// </summary>
        /// <param name="t">Type to register.</param>
        public static void RegisterFunction(Type t)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey addinKey = hklm.CreateSubKey(GetAddinKey(t.GUID));
            addinKey.SetValue(null, 0);

            var description = string.Empty;
            var title = t.Name;

            if (t.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute desc)
            {
                description = desc.Description;
            }

            if (t.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute name)
            {
                title = name.DisplayName;
            }

            addinKey.SetValue("Description", description);
            addinKey.SetValue("Title", title);
        }

        /// <summary>
        /// To unregister <see cref="ISldWorks"/> class.
        /// </summary>
        /// <param name="t">Type to unregister.</param>
        public static void UnregisterFunction(Type t)
        {
            RegistryKey hklm = Registry.LocalMachine;
            hklm.DeleteSubKeyTree(GetAddinKey(t.GUID));
        }

        private static string GetAddinKey(Guid guid) => @$"SOFTWARE\SolidWorks\Addins\{{{guid}}}";
    }
}
