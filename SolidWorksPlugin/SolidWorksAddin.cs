// <copyright file="SolidWorksAddin.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swpublished;

    /// <summary>
    /// The base class for a solid works add-in.
    /// </summary>
    public abstract partial class SolidWorksAddin : ISwAddin
    {
        /// <summary>
        /// Gets the name of the <see cref="memberInstanceFactory"/> field.
        /// </summary>
        internal const string NameOfMemberInstanceFactory = nameof(memberInstanceFactory);

        private readonly ISolidworksAddinMemberInstanceFactory memberInstanceFactory;
        private Cookie addInCookie;
        private IInternalCommandHandler? commandHandler;
        private IEventHandlerManagerInternals? eventHandlerManager;
        private IDocumentManagerInternals? documentManager;
        private SldWorks? swApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidWorksAddin"/> class.
        /// </summary>
        /// <param name="factory">Factory to create member instances.</param>
        internal SolidWorksAddin(ISolidworksAddinMemberInstanceFactory factory)
        {
            this.memberInstanceFactory = factory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidWorksAddin"/> class.
        /// </summary>
        protected SolidWorksAddin()
            : this(new SolidworksAddinMemberInstanceFactory())
        {
        }

        /// <summary>
        /// Gets the path to the current assembly directory.
        /// </summary>
        public static string AssemblyPath { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        /// <summary>
        /// Gets the reference to the current Solid-Works application.
        /// </summary>
        public SldWorks SwApplication => this.swApplication
            ?? throw new NullReferenceException($"SwApplication is not defined. Call {nameof(this.ConnectToSW)} first.");

        /// <summary>
        /// Gets the document manager.
        /// </summary>
        public IDocumentManager DocumentManager => this.documentManager
            ?? throw new NullReferenceException($"DocumentManager is not defined. Call {nameof(this.ConnectToSW)} first.");

        /// <summary>
        /// Gets the command handler.
        /// </summary>
        public ICommandHandler CommandHandler => this.commandHandler
            ?? throw new NullReferenceException($"CommandHandler is not defined. Call {nameof(this.ConnectToSW)} first.");

        /// <summary>
        /// Com register function for types derived from <see cref="SolidWorksAddin"/>.
        /// </summary>
        /// <param name="t">Type to register.</param>
        [ComRegisterFunction]
        public static void RegisterFunction(Type t) => SwComInterop.RegisterFunction(t);

        /// <summary>
        /// Com unregister function for types derived from <see cref="SolidWorksAddin"/>.
        /// </summary>
        /// <param name="t">Type to unregister.</param>
        [ComUnregisterFunction]
        public static void UnregisterFunction(Type t) => SwComInterop.UnregisterFunction(t);

        TabCommandManager? commandTabBuilder;

        /// <inheritdoc/>
        public bool ConnectToSW(object ThisSW, int cookie)
        {
            try
            {
                this.swApplication = (SldWorks)ThisSW;
                this.addInCookie = new Cookie(cookie);

                (this.documentManager, this.commandHandler, this.eventHandlerManager) =
                    this.memberInstanceFactory.CreateInstances(this.swApplication, this.addInCookie);

                this.RegisterCommands(this.commandHandler);

                this.commandTabBuilder = new TabCommandManager(this.commandHandler.SwCommandManager);

                this.AddCommandTabMenu(this.commandTabBuilder);

                this.RegisterEventHandler(this.eventHandlerManager);

                this.OnConnectToSW(this.swApplication, this.addInCookie);
            }
            catch (Exception)
            {
                this.DisconnectFromSW();
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public bool DisconnectFromSW()
        {
            this.OnDisconnectFromSW();

            this.eventHandlerManager?.Dispose();
            this.eventHandlerManager = null;

            this.commandTabBuilder?.Dispose();
            this.commandTabBuilder = null;

            this.commandHandler?.Dispose();
            this.commandHandler = null;

            this.documentManager?.Dispose();
            this.documentManager = null;

            this.swApplication = null;

            // The add-in _must_ call GC.Collect() here in order to retrieve all managed code pointers.
            GC.Collect();
            GC.WaitForPendingFinalizers();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return true;
        }

        /// <summary>
        /// Register all command groups to the command group manager.
        /// </summary>
        /// <param name="commandGroupHandler">The command group manager.</param>
        protected virtual void RegisterCommands(ICommandGroupHandler commandGroupHandler)
        {
        }

        protected virtual void AddCommandTabMenu(ICommandTabManager tabManager)
        {
        }

        /// <summary>
        /// Register all events to the event handler manager.
        /// </summary>
        /// <param name="eventHandlerManager">The event handler manager.</param>
        protected virtual void RegisterEventHandler(IEventHandlerManager eventHandlerManager)
        {
        }

        /// <summary>
        /// Callback for user methods called at the end of <see cref="ConnectToSW(object, int)"/>.
        /// </summary>
        /// <param name="swApplication">The SolidWorks application.</param>
        /// <param name="addInCookie">The Add-In cookie.</param>
        protected virtual void OnConnectToSW(SldWorks swApplication, Cookie addInCookie)
        {
        }

        /// <summary>
        /// Callback for user methods called at the beginning of <see cref="DisconnectFromSW()"/>.
        /// </summary>
        protected virtual void OnDisconnectFromSW()
        {
        }
    }
}
