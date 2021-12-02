// <copyright file="IEventHandlerManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// Manager to register event handler to.
    /// </summary>
    public interface IEventHandlerManager
    {
        /// <summary>
        /// Registers a <see cref="ISolidWorksEventHandler"/> to the manager.
        /// </summary>
        /// <param name="eventHandler">The event handler to register.</param>
        void RegisterSolidWorksEventHandler(ISolidWorksEventHandler eventHandler);

        /// <summary>
        /// Registers a <see cref="IDocumentEventHandler"/> to the manager.
        /// </summary>
        /// <param name="eventHandler">The event handler to register.</param>
        void RegisterDocumentEventHandler(IDocumentEventHandler eventHandler);
    }
}
