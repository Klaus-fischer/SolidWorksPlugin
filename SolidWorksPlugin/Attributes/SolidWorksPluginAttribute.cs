// <copyright file="SolidWorksPluginAttribute.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    public class SolidWorksPluginAttribute : Attribute
    {
        public SolidWorksPluginAttribute(string title, string description)
        {
            this.Title = title;
            this.Description = description;
        }

        public string Title { get; }
        public string Description { get;  }
    }
}
