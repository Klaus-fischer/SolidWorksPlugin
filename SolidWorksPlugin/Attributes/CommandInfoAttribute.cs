// <copyright file="CommandInfoAttribute.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.swconst;
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class CommandInfoAttribute : Attribute
    {
        public CommandInfoAttribute(string title)
        {
            this.Name = title;
        }

        public int ImageIndex { get; set; } = -1;

        public bool HasMenu { get; set; } = true;

        public bool HasToolbar { get; set; } = true;

        public string Name { get; }

        public int Position { get; set; } = -1;

        /// <summary>
        /// Gets or sets the tool-tip of the command.
        /// </summary>
        public string Tooltip { get; set; }

        public string Hint { get; set; }

        internal int GetSwCommandItemType_e()
        {
            swCommandItemType_e menuOptions = 0;
            if (this.HasMenu == true)
            {
                menuOptions |= swCommandItemType_e.swMenuItem;
            }

            if (this.HasToolbar == true)
            {
                menuOptions |= swCommandItemType_e.swToolbarItem;
            }

            return (int)menuOptions;
        }
    }
}
