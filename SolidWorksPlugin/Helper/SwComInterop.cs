// <copyright file="SwComInterop.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using Microsoft.Win32;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Helper class that handles the com registration.
    /// </summary>
    public class SwComInterop
    {
        private const string Key = @"SOFTWARE\SolidWorks\Addins\";

        /// <summary>
        /// Allows testing the registration to an different root key.
        /// </summary>
        /// <param name="hklm">The <see cref="Registry.LocalMachine"/> key by default.</param>
        /// <param name="t">Type to register.</param>
        internal static void RegisterToKey(RegistryKey hklm, Type t)
        {
            if (hklm.OpenSubKey(Key, true)?.CreateSubKey($"{{{t.GUID}}}", true) is not RegistryKey addinKey)
            {
                return;
            }

            addinKey.SetValue(null, 0);

            var description = string.Empty;
            var title = t.Name;
            var loadAtStartUp = true;

            if (t.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute desc)
            {
                description = desc.Description;
            }

            if (t.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute name)
            {
                title = name.DisplayName;
            }

            var attibute = t.GetCustomAttributes().OfType<SolidWorksPluginAttribute>().FirstOrDefault();

            if (attibute is SolidWorksPluginAttribute swAttr)
            {
                description = swAttr.Description ?? description;
                title = swAttr.Title;
                loadAtStartUp = swAttr.LoadAtStartupByDefault;
            }

            addinKey.SetValue(null, loadAtStartUp ? 1 : 0, RegistryValueKind.DWord);
            addinKey.SetValue("Description", description);
            addinKey.SetValue("Title", title);
        }

        /// <summary>
        /// Allows testing undoing the registration from an different root key.
        /// </summary>
        /// <param name="hklm">The <see cref="Registry.LocalMachine"/> key by default.</param>
        /// <param name="t">Type to register.</param>
        internal static void UnregisterFromKey(RegistryKey hklm, Type t)
        {
            hklm.OpenSubKey(Key, true)?.DeleteSubKeyTree($"{{{t.GUID}}}", false);
        }

        /// <summary>
        /// Checks if object is a com object, and calls <see cref="Marshal.FinalReleaseComObject(object)"/>.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="comObject">The object to finalize.</param>
        /// <returns>Null by default.</returns>
        internal static T ReleaseComObject<T>(T comObject)
            where T : class
        {
            if (Marshal.IsComObject(comObject))
            {
                Marshal.FinalReleaseComObject(comObject);
            }

#pragma warning disable CS8603 // Mögliche Nullverweis Rückgabe.
            return null;
#pragma warning restore CS8603 // Mögliche Nullverweis Rückgabe.
        }
    }
}
