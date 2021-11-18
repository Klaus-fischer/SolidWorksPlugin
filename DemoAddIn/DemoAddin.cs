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
    [DisplayName("Demo Addin")]
    [Description("Default description.")]

    public class DemoAddin : SolidWorksAddin
#endif
    {
        [ComRegisterFunction]
        public static void RegisterFunction(Type t) => SwComInterop.RegisterFunction(t);

        [ComUnregisterFunction]
        public static void UnregisterFunction(Type t) => SwComInterop.UnregisterFunction(t);

        /// <inheritdoc/>
        protected override void RegisterCommands(ICommandGroupHandler commandManager)
        {
            commandManager.AddCommandGroup<Commands>(this.BuildCommands);
        }

        protected override void RegisterEventHandler(IEventHandlerManager eventHandlerManager)
        {
        }

        private void BuildCommands(ICommandHandler<Commands> commandHandler)
        {
            commandHandler.RegisterCommand(
                 Commands.TrialCommand,
                 new RelaySwCommand(this.TrialExecuted));

            commandHandler.RegisterCommand(
                Commands.TrialCommand2,
                new RelaySwCommand(this.TrialExecuted));
        }

        private void TrialExecuted(SwDocument? obj)
        {
            this.SwApplication.SendMsgToUser2("Executed", (int)swMessageBoxIcon_e.swMbInformation, (int)swMessageBoxBtn_e.swMbOk);
        }
    }

    [CommandGroupInfo(1, "Main Commands", ToolTip = "Mein erstes Demo Projekt")]
    [CommandGroupIcons(
        IconsPath = @".\Icons\Toolbar20.png|.\Icons\Toolbar32.png|.\Icons\Toolbar40.png|.\Icons\Toolbar64.png",
        MainIconPath = @".\Icons\Icon20.png|.\Icons\Icon32.png|.\Icons\Icon40.png|.\Icons\Icon64.png")]
    public enum Commands
    {
        [CommandInfo("Trial Command", ImageIndex = 1, HasMenu = true, HasToolbar = true)]
        TrialCommand,

        [CommandInfo("Trial Command 2", ImageIndex = 2, HasMenu = true, HasToolbar = true, Tooltip = "MainMenu@Trial2")]
        TrialCommand2,
    }
}
