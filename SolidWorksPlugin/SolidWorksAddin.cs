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

    /// <summary>
    /// The base class for a solid works add-in.
    /// </summary>
    public abstract partial class SolidWorksAddin : ISwAddin
    {
        private int addInCookie;
        private CommandHandler? commandHandler;
        private EventHandlerManager? eventHandlerManager;
        private DocumentManager? documentManager;
        private SldWorks? swApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidWorksAddin"/> class.
        /// </summary>
        public SolidWorksAddin()
        {
        }

        /// <summary>
        /// Gets the path to the current assembly directory.
        /// </summary>
        public static string AssemblyPath { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        /// <summary>
        /// Gets the reference to the current Solid-Works application.
        /// </summary>
        public SldWorks SwApplication => this.swApplication!;

        /// <inheritdoc/>
        public bool ConnectToSW(object ThisSW, int cookie)
        {
            try
            {
                this.swApplication = (SldWorks)ThisSW;
                this.addInCookie = cookie;

                this.documentManager = new DocumentManager(this.swApplication);

                this.commandHandler = new CommandHandler(this.SwApplication, this.documentManager, this.addInCookie);
                this.RegisterCommands(this.commandHandler);

                this.eventHandlerManager = new EventHandlerManager(this.swApplication, this.documentManager);
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
