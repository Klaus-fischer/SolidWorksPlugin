// <copyright file="SolidWorksAddin.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.IO;
    using System.Reflection;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swpublished;

#if DEPENDENCY_INJECTION
    using Microsoft.Extensions.DependencyInjection;
#endif

    /// <summary>
    /// The base class for a solid works add-in.
    /// </summary>
    public abstract partial class SolidWorksAddin : ISwAddin
    {
#if DEPENDENCY_INJECTION
        private readonly IServiceProvider? serviceProvider;
#endif
        private Cookie addInCookie;
        private ICommandHandlerInternals? commandHandler;
        private IEventHandlerManagerInternals? eventHandlerManager;
        private IDocumentManagerInternals? documentManager;
        private SldWorks? swApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidWorksAddin"/> class.
        /// </summary>
        public SolidWorksAddin()
        {
        }

#if DEPENDENCY_INJECTION
        /// <summary>
        /// Initializes a new instance of the <see cref="SolidWorksAddin"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider to inject dependencies.</param>
        internal SolidWorksAddin(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
#endif

        /// <summary>
        /// Gets the path to the current assembly directory.
        /// </summary>
        public static string AssemblyPath { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        /// <summary>
        /// Gets the reference to the current Solid-Works application.
        /// </summary>
        public SldWorks SwApplication => this.swApplication!;

        /// <summary>
        /// Gets the document manager.
        /// </summary>
        public IDocumentManager DocumentManager => this.documentManager!;

        /// <inheritdoc/>
        public bool ConnectToSW(object ThisSW, int cookie)
        {
            try
            {
                this.swApplication = (SldWorks)ThisSW;
                this.addInCookie = new Cookie(cookie);

#if DEPENDENCY_INJECTION
                if (this.serviceProvider is null)
#endif
                {
                    this.documentManager = new DocumentManager(this.swApplication);
                    this.commandHandler = new CommandHandler(this.SwApplication, this.documentManager, this.addInCookie);
                    this.eventHandlerManager = new EventHandlerManager(this.swApplication, this.documentManager);
                }
#if DEPENDENCY_INJECTION
                else
                {
                    this.documentManager = this.serviceProvider.GetRequiredService<IDocumentManagerInternals>();
                    this.commandHandler = this.serviceProvider.GetRequiredService<ICommandHandlerInternals>();
                    this.eventHandlerManager = this.serviceProvider.GetRequiredService<IEventHandlerManagerInternals>();
                }
#endif

                this.RegisterCommands(this.commandHandler);

                this.RegisterEventHandler(this.eventHandlerManager);

                AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
            }
            catch (Exception ex)
            {
                this.DisconnectFromSW();
            }

            return true;
        }

        /// <inheritdoc/>
        public bool DisconnectFromSW()
        {
            this.eventHandlerManager?.Dispose();
            this.eventHandlerManager = null;

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

        protected abstract void RegisterCommands(ICommandGroupHandler commandGroupHandler);

        protected abstract void RegisterEventHandler(IEventHandlerManager eventHandlerManager);

        /// <summary>
        /// Registers a window to be top most on the solid works screen.
        /// </summary>
        /// <param name="window">The window.</param>
        protected void RegisterWindow(object window)
        {
            if (Type.GetType("System.Windows.Interop.WindowInteropHelper") is Type interopHelperType &&
                interopHelperType.GetProperty("Owner") is PropertyInfo pi)
            {
                var interopHelper = Activator.CreateInstance(interopHelperType, new object[] { window });
                pi.SetValue(interopHelper, System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // this.LogException(e.ExceptionObject as Exception);
        }
    }
}
