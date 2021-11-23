// <copyright file="CommandGroupInfo.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Info class of a command group.
    /// </summary>
    public class CommandGroupInfo
    {
        private static readonly int[] IconSizes = { 20, 32, 40, 64, 96, 128 };

        private string? tooltip;
        private string? hint;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandGroupInfo"/> class.
        /// </summary>
        /// <param name="id">The unique user id of the command group.</param>
        /// <param name="title">The title of the command group.</param>
        public CommandGroupInfo(int id, string title)
        {
            this.Id = id;
            this.Title = title;
        }

        /// <summary>
        /// Gets the unique user id of the command group.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the title of the command group.
        /// </summary>
        public string Title { get; }

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
        /// Gets or sets the tool tip of the command group.
        /// If default, the title will be returned.
        /// </summary>
        public string Tooltip
        {
            get => this.tooltip ?? this.Title;
            set => this.tooltip = value;
        }

        /// <summary>
        /// Gets or sets the hint of the command group.
        /// If default, the tool tip will be returned.
        /// </summary>
        public string Hint { get => this.hint ?? this.Tooltip; set => this.hint = value; }

        /// <summary>
        /// Gets or sets the position of the command group.
        /// </summary>
        public int Position { get; set; } = -1;

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
