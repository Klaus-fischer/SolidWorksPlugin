// <copyright file="ISolidworksAddinMemberInstanceFactory.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// declaration of factory class for <see cref="SolidWorksAddin"/> member.
    /// </summary>
    internal interface ISolidworksAddinMemberInstanceFactory
    {
        /// <summary>
        /// Creates member instances for <see cref="SolidWorksAddin"/> class.
        /// </summary>
        /// <param name="swApplication">The current solid works application.</param>
        /// <param name="cookie">The cookie of the add-in.</param>
        /// <returns>The created instances.</returns>
        (
            IDocumentManagerInternals DocumentManager,
            ICommandHandlerInternals CommandManager,
            IEventHandlerManagerInternals EventHandler
        ) CreateInstances(SldWorks swApplication, Cookie cookie);
    }
}