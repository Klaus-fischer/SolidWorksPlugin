// <copyright file="CommandGroupSpecExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Extensions for <see cref="CommandGroupSpec"/> objects.
    /// </summary>
    internal static class CommandGroupSpecExtensions
    {
        private static readonly int[] IconSizes = { 20, 32, 40, 64, 96, 128 };

        /// <summary>
        /// Gets the icons list based on the <see cref="CommandGroupSpec.IconsPath"/> property.
        /// </summary>
        /// <param name="spec">The command group specification to extend.</param>
        /// <returns>Array of string.</returns>
        public static string[] GetIconsList(this CommandGroupSpec spec) => GetFiles(spec.IconsPath).ToArray();

        /// <summary>
        /// Gets the icons list based on the <see cref="CommandGroupSpec.MainIconPath"/> property.
        /// </summary>
        /// <param name="spec">The command group specification to extend.</param>
        /// <returns>Array of string.</returns>
        internal static string[] GetMainIconList(this CommandGroupSpec spec) => GetFiles(spec.MainIconPath).ToArray();

        private static IEnumerable<string> GetFiles(string formatedInput)
        {
            var icons = IconSizes.Select(o => string.Format(formatedInput, o));

            foreach (var icon in icons)
            {
                string iconPath = icon;

                // combine relative path with path to assembly.
                if (!Path.IsPathRooted(icon))
                {
                    iconPath = Extensions.FilePathExtensions.GetAbsolutePath(SolidWorksAddin.AssemblyPath, icon);
                }

                if (File.Exists(iconPath))
                {
                    yield return iconPath;
                }
            }
        }
    }
}
