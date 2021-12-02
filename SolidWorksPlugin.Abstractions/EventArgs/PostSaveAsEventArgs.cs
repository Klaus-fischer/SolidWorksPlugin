// <copyright file="PostSaveAsEventArgs.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// Event arguments for <see cref="ISwDocument.OnPostSaveAs"/> event.
    /// </summary>
    public class PostSaveAsEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostSaveAsEventArgs"/> class.
        /// </summary>
        /// <param name="filename">The filename of the document.</param>
        /// <param name="options">The options of the save operation.</param>
        public PostSaveAsEventArgs(string filename, swFileSaveTypes_e options)
        {
            this.Filename = filename ?? throw new ArgumentNullException(nameof(filename));
            this.SaveType = options;
        }

        /// <summary>
        /// Gets the filename of the Document.
        /// </summary>
        public string Filename { get; }

        /// <summary>
        /// Gets the save type options of the save operation.
        /// </summary>
        public swFileSaveTypes_e SaveType { get; }
    }
}
