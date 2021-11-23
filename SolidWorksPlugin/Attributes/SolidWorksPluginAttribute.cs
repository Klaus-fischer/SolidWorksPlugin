// <copyright file="SolidWorksPluginAttribute.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// The attribute that describes the title and description of an add-in.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SolidWorksPluginAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolidWorksPluginAttribute"/> class.
        /// </summary>
        /// <param name="title">The title of the add-in.</param>
        public SolidWorksPluginAttribute(string title)
        {
            this.Title = title;
        }

        /// <summary>
        /// Gets the title of the add-in.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets or sets the description of the add-in.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether add-in should be load at startup.
        /// </summary>
        public bool LoadAtStartupByDefault { get; set; } = true;
    }
}
