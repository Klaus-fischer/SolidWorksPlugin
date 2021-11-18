// <copyright file="SwAddin.cs" company="SIM Automation">
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
    /// <typeparam name="TCommandEnum">Type of the command enumeration, that holds for every command an unique value.</typeparam>
    public abstract partial class SwAddin<TCommandEnum> : ISwAddin
        where TCommandEnum : struct, Enum
    {
        private int addInCookie;
        private CommandManager<TCommandEnum>? commandManager;
        private DocumentManager? documentManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwAddin{TCommandEnum}"/> class.
        /// </summary>
        public SwAddin()
        {
        }

        /// <summary>
        /// Gets the reference to the current Solid-Works application.
        /// </summary>
        public SldWorks SwApplication
        {
            get; private set;
        }

        /// <summary>
        /// Gets the main command id.
        /// </summary>
        public abstract int MainCommandId { get; }

        /// <summary>
        /// Gets the title of the current add-in.
        /// </summary>
        public virtual string Title
            => this.GetType().GetCustomAttribute<SolidWorksPluginAttribute>()?.Title
            ?? this.GetType().Name;
        /// <summary>
        /// Gets the path to the current assembly directory.
        /// </summary>
        public static string AssemblyPath { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <inheritdoc/>
        public bool ConnectToSW(object ThisSW, int cookie)
        {
            try
            {
                this.SwApplication = (SldWorks)ThisSW;
                this.addInCookie = cookie;

                this.commandManager = new CommandManager<TCommandEnum>(this.SwApplication, this.addInCookie, this.MainCommandId, this.DocumentManager);

                this.GetCommandManagerInfos(out var tooltip, out var icons, out var mainIcon, out var position);

                this.commandManager.InitializeCommandManager(this.Title, tooltip, icons, mainIcon, position);

                this.RegisterCommands(this.commandManager);

                this.commandManager.Activate();

                // this.RegisterDocumentHandler(this.DocumentManager);
                AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
            }
            catch (Exception ex)
            {
                this.DisconnectFromSW();
            }

            return true;
        }


        protected abstract void GetCommandManagerInfos(out string tooltip, out string[] iconList, out string[] mainIcon, out int position);

        protected abstract void RegisterCommands(ICommandManager<TCommandEnum> commandManager);

        protected void AddCommandGroup<T>(Action<ICommandManager<T>> registerCommands)
            where T : struct, Enum
        {
            var commandGroup = new CommandManager<T>(this.SwApplication, this.addInCookie, -1, this.documentManager!);

            registerCommands(commandGroup);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.LogException(e.ExceptionObject as Exception);
        }

        /// <inheritdoc/>
        public bool DisconnectFromSW()
        {
            //this.UnregisterAllSolidWorksEventHandler();

            this.documentManager?.Dispose();
            this.documentManager = null;

            this.commandManager?.Dispose();
            this.commandManager = null;

            this.SwApplication = null;

            // The add-in _must_ call GC.Collect() here in order to retrieve all managed code pointers.
            GC.Collect();
            GC.WaitForPendingFinalizers();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return true;
        }


        protected void RegisterWindow(object window)
        {
            if (Type.GetType("System.Windows.Interop.WindowInteropHelper") is Type interopHelperType &&
                interopHelperType.GetProperty("Owner") is PropertyInfo pi)
            {
                var interopHelper = Activator.CreateInstance(interopHelperType, new object[] { window });
                pi.SetValue(interopHelper, System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
            }
        }

    }
}
