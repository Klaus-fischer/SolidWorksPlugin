// <copyright file="FileExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System.IO;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// Extensions for filenames.
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// Extension for assembly documents.
        /// </summary>
        public const string AsmExt = ".SLDASM";

        /// <summary>
        /// Extension for part documents.
        /// </summary>
        public const string PrtExt = ".SLDPRT";

        /// <summary>
        /// Extension for drawing documents.
        /// </summary>
        public const string DrwExt = ".SLDDRW";

        /// <summary>
        /// Gets the <see cref="swDocumentTypes_e"/> based on the filename extension.
        /// </summary>
        /// <param name="filename">Filename to check.</param>
        /// <returns>The extension.</returns>
        internal static int GetDocumentType(string filename)
        {
            switch (Path.GetExtension(filename).ToUpper())
            {
                case DrwExt:
                case ".DRWDOT":
                    return (int)swDocumentTypes_e.swDocDRAWING;

                case AsmExt:
                case ".ASMDOT":
                    return (int)swDocumentTypes_e.swDocASSEMBLY;

                case PrtExt:
                case ".SLDLFT":
                    return (int)swDocumentTypes_e.swDocPART;

                default:
                    return (int)swDocumentTypes_e.swDocPART;
            }
        }
    }
}
