// <copyright file="CommandGroupIconsAttribute.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Attribute to attach path to icon files to an command enumeration.
    /// </summary>
    public class CommandGroupIconsAttribute : Attribute
    {
        private static readonly int[] IconSizes = { 20, 32, 40, 64, 96, 128 };

        /// <summary>
        /// Gets or sets the path to the tool bar icons file.
        /// Use .\ or ..\ notation to navigate relative to path of the dll.
        /// Use {0} as placeholder for different sizes.
        /// </summary>
        /// <example>
        /// IconsPath = ".\Icons\Toolbar{0}.png"...
        /// </example>
        public string IconsPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the path to the main icons file.
        /// Use .\ or ..\ notation to navigate relative to path of the dll.
        /// Use {0} as placeholder for different sizes.
        /// </summary>
        /// <example>
        /// IconsPath = ".\Icons\Icon{0}.png"...
        /// </example>
        public string MainIconPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets the icons list based on the <see cref="IconsPath"/> property.
        /// </summary>
        /// <returns>Array of string.</returns>
        internal string[] GetIconsList() => GetFiles(this.IconsPath).ToArray();

        /// <summary>
        /// Gets the icons list based on the <see cref="MainIconPath"/> property.
        /// </summary>
        /// <returns>Array of string.</returns>
        internal string[] GetMainIconList() => GetFiles(this.MainIconPath).ToArray();

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
