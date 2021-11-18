// <copyright file="DemoAddin.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

#if QUICKDRAWMOCK
namespace SIM.QuickDraw
#else
namespace SIM.DemoAddin
#endif
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using SIM.SolidWorksPlugin;
    using SIM.SolidWorksPlugin.Commands;
    using SolidWorks.Interop.swconst;

#if QUICKDRAWMOCK
    [Guid("7ccf38b9-39af-3fc1-8bdc-a4a41ade8bbe")]
    [ComVisible(true)]
    public class QuickDrawAddinIntegration : SwAddin<Commands>
#else
    [Guid("C0E8D5B0-5773-4FDD-9ECE-C2C570CA1F65")]
    [ComVisible(true)]
    [SolidWorksPlugin("Demo Addin", "first working demo add-in")]

    public class DemoAddin : SwAddin<Commands>
#endif
    {
        [ComRegisterFunction]
        public static void RegisterFunction(Type t) => SwComInterop.RegisterFunction(t);

        [ComUnregisterFunction]
        public static void UnregisterFunction(Type t) => SwComInterop.UnregisterFunction(t);

        /// <inheritdoc/>
        public override int MainCommandId => 1;

        /// <inheritdoc/>
        public override string Title => "SIM-QuickDraw";

        /// <inheritdoc/>
        protected override void GetCommandManagerInfos(out string tooltip, out string[] iconList, out string[] mainIcon, out int position)
        {
            tooltip = this.Title;
            position = -1;
            iconList = new string[]
            {
                @$"{AssemblyPath}\Icons\Toolbar20.png",
                @$"{AssemblyPath}\Icons\Toolbar32.png",
                @$"{AssemblyPath}\Icons\Toolbar40.png",
                @$"{AssemblyPath}\Icons\Toolbar64.png",
            };

            mainIcon = new string[]
            {
                @$"{AssemblyPath}\Icons\Icon20.png",
                @$"{AssemblyPath}\Icons\Icon32.png",
                @$"{AssemblyPath}\Icons\Icon40.png",
                @$"{AssemblyPath}\Icons\Icon64.png",
            };
        }

        /// <inheritdoc/>
        protected override void RegisterCommands(ICommandManager<Commands> commandManager)
        {
            commandManager.RegisterCommand(
                Commands.TrialCommand,
                new RelaySwCommand(this.TrialExecuted));

            commandManager.RegisterCommand(
                Commands.TrialCommand2,
                new RelaySwCommand(this.TrialExecuted));
        }

        private void TrialExecuted(SwDocument? obj)
        {
            this.SwApplication.SendMsgToUser2("Executed", (int)swMessageBoxIcon_e.swMbInformation, (int)swMessageBoxBtn_e.swMbOk);
        }
    }

    public enum Commands
    {
        [CommandInfo(ImageIndex = 1, HasMenu = true, HasToolbar = true)]
        TrialCommand,

        [CommandInfo(ImageIndex = 2, HasMenu = true, HasToolbar = true, Text = "MainMenu@Trial2")]
        TrialCommand2,
    }
}
