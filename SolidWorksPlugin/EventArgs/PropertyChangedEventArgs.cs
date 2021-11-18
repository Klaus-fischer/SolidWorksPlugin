// <copyright file="PropertyChangedEventArgs.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.swconst;

    public class PropertyChangedEventArgs : EventArgs
    {
        public string PropertyName { get; set; }

        public string Configuration { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public swCustomInfoType_e ValueType { get; set; }
    }
}
