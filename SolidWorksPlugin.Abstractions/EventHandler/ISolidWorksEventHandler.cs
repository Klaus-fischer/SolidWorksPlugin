// <copyright file="ISolidWorksEventHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Declaration if an event handler.
    /// </summary>
    public interface ISolidWorksEventHandler
    {
        /// <summary>
        /// Attaches all events to the document event handler.
        /// </summary>
        /// <param name="sldWorks">Document to attach events to.</param>
        void AttachSwEvents(SldWorks sldWorks);

        /// <summary>
        /// Detaches all events from the document event handler.
        /// </summary>
        /// <param name="sldWorks">Document to detach events from.</param>
        void DetachSwEvents(SldWorks sldWorks);
    }
}
