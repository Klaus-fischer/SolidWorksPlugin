// <copyright file="CommandGroupIconsAttribute.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Attribute to attach path to icon files to an command enumeration.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class CommandGroupIconsAttribute : Attribute
    {
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
    }
}
