// <copyright file="CommandInfoAttribute.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class CommandInfoAttribute : Attribute
    {
        public int ImageIndex { get; set; }

        public bool HasMenu { get; set; }

        public bool HasToolbar { get; set; }
        
        public string? Text { get; set; }
    }
}
