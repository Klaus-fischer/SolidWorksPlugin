// <copyright file="PostSaveAsEventArgs.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.swconst;

    public class PostSaveAsEventArgs : EventArgs
    {
        public PostSaveAsEventArgs(string filename, swFileSaveTypes_e options)
        {
            this.Filename = filename;
            this.SaveType = options;
        }

        public string Filename { get; }

        public swFileSaveTypes_e SaveType { get; }
    }
}
