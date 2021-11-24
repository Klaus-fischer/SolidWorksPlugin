// <copyright file="ICommandTabManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// The command tab manager declaration used in <see cref="SolidWorksAddin.AddCommandTabMenu(ICommandTabManager)"/> call.
    /// </summary>
    public interface ICommandTabManager
    {
        /// <summary>
        /// Builds an command tab by factory method.
        /// </summary>
        /// <param name="title">Title of the command tab page.</param>
        /// <param name="factoryMethod">The factory method.</param>
        /// <param name="documentTypes">The valid document types.</param>
        void BuildCommandTab(string title, Action<ICommandTabBuilder> factoryMethod, params swDocumentTypes_e[] documentTypes);
    }
}
