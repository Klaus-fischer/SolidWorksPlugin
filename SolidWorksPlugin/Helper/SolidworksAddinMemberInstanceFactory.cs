// <copyright file="SolidworksAddinMemberInstanceFactory.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Factory class for <see cref="SolidWorksAddin"/> member.
    /// </summary>
    internal class SolidworksAddinMemberInstanceFactory : ISolidworksAddinMemberInstanceFactory
    {
        /// <summary>
        /// Creates member instances for <see cref="SolidWorksAddin"/> class.
        /// </summary>
        /// <param name="swApplication">The current solid works application.</param>
        /// <param name="cookie">The cookie of the add-in.</param>
        /// <returns>The created instances.</returns>
        public (IDocumentManagerInternals DocumentManager,
            IInternalCommandHandler CommandManager,
            IEventHandlerManagerInternals EventHandler,
            IInternalCommandTabManager TabManager) CreateInstances(SldWorks swApplication, Cookie cookie)
        {
            var documentFactory = new SwDocumentFactory();
            var iModelDocComparer = new SwModelDocPointerEqualityComparer(swApplication);

            var documentManager = new DocumentManager(swApplication, documentFactory, iModelDocComparer);
            var commandHandler = new CommandHandler(swApplication, documentManager, cookie);
            var eventHandlerManager = new EventHandlerManager(swApplication, documentManager);
            var commandTabManager = new TabCommandManager(commandHandler);

            documentFactory.OpenDocumentCallBack = s => documentManager.OpenDocument(s, out _);
            documentFactory.GetDocumentCallBack = documentManager.GetDocument;

            return (documentManager, commandHandler, eventHandlerManager, commandTabManager);
        }
    }
}
