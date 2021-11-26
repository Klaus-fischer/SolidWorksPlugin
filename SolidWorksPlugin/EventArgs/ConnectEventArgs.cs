// <copyright file="ConnectEventArgs.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Event arguments used in <see cref="SolidWorksAddin.OnConnecting"/> and <see cref="SolidWorksAddin.OnConnected"/> events.
    /// </summary>
    public class ConnectEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectEventArgs"/> class.
        /// </summary>
        /// <param name="swApplication">The SolidWorks application.</param>
        /// <param name="cookie">The cookie of the add-in.</param>
        internal ConnectEventArgs(SldWorks swApplication, Cookie cookie)
        {
            this.SwApplication = swApplication;
            this.Cookie = cookie;
        }

        /// <summary>
        /// Gets the SolidWorks application.
        /// </summary>
        public SldWorks SwApplication { get; }

        /// <summary>
        /// Gets the cookie of the add-in.
        /// </summary>
        public Cookie Cookie { get; }
    }
}
