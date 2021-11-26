// <copyright file="SolidWorksAddin.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using Microsoft.Win32;
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
        private IInternalCommandTabManager? commandTabManager;
        private SldWorks? swApplication;
        private ISolidWorksDelegateHandler? delegates;

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
        /// Callback for user methods called at the end of <see cref="ConnectToSW(object, int)"/>.
        /// </summary>
        protected event EventHandler<ConnectEventArgs>? OnConnecting;

        /// <summary>
        /// Callback for user methods called at the end of <see cref="ConnectToSW(object, int)"/>.
        /// </summary>
        protected event EventHandler<ConnectEventArgs>? OnConnected;

        /// <summary>
        /// Callback for user methods called at the beginning of <see cref="DisconnectFromSW()"/>.
        /// </summary>
        protected event EventHandler? OnDisconnect;

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
        /// Gets the <see cref="ISolidWorksDelegateHandler"/>.
        /// </summary>
        public ISolidWorksDelegateHandler Delegates => this.delegates
            ?? throw new NullReferenceException($"Delegates is not defined. Call {nameof(this.ConnectToSW)} first.");

        /// <summary>
        /// Com register function for types derived from <see cref="SolidWorksAddin"/>.
        /// </summary>
        /// <param name="t">Type to register.</param>
        [ComRegisterFunction]
        public static void RegisterFunction(Type t) => SwComInterop.RegisterToKey(Registry.LocalMachine, t);

        /// <summary>
        /// Com unregister function for types derived from <see cref="SolidWorksAddin"/>.
        /// </summary>
        /// <param name="t">Type to unregister.</param>
        [ComUnregisterFunction]
        public static void UnregisterFunction(Type t) => SwComInterop.UnregisterFromKey(Registry.LocalMachine, t);

        /// <inheritdoc/>
        public bool ConnectToSW(object ThisSW, int cookie)
        {
            try
            {
                this.swApplication = (SldWorks)ThisSW;
                this.addInCookie = new Cookie(cookie);

                this.delegates = new SolidWorksDelegateHandler(this.swApplication);

                this.OnConnecting?.Invoke(this, new ConnectEventArgs(this.swApplication, this.addInCookie));

                (this.documentManager, this.commandHandler, this.eventHandlerManager, this.commandTabManager) =
                    this.memberInstanceFactory.CreateInstances(this.swApplication, this.addInCookie);

                this.RegisterCommands(this.commandHandler);

                this.AddCommandTabMenu(this.commandTabManager);

                this.RegisterEventHandler(this.eventHandlerManager);

                this.OnConnected?.Invoke(this, new ConnectEventArgs(this.swApplication, this.addInCookie));
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
            this.OnDisconnect?.Invoke(this, EventArgs.Empty);

            this.eventHandlerManager?.Dispose();
            this.eventHandlerManager = null;

            this.commandTabManager?.Dispose();
            this.commandTabManager = null;

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
        protected abstract void RegisterCommands(ICommandGroupHandler commandGroupHandler);

        /// <summary>
        /// Builds the tab menu for the command manager.
        /// </summary>
        /// <param name="tabManager">The tab manager.</param>
        protected abstract void AddCommandTabMenu(ICommandTabManager tabManager);

        /// <summary>
        /// Register all events to the event handler manager.
        /// </summary>
        /// <param name="eventHandlerManager">The event handler manager.</param>
        protected abstract void RegisterEventHandler(IEventHandlerManager eventHandlerManager);
    }
}
