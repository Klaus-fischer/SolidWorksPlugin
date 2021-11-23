// <copyright file="DemoAddin.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.DemoAddin
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using SIM.SolidWorksPlugin;
    using SIM.SolidWorksPlugin.Commands;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    [Guid("C0E8D5B0-5773-4FDD-9ECE-C2C570CA1F65")]
    [ComVisible(true)]
    [DisplayName("Demo Addin")]
    [Description("Default description.")]
    public class DemoAddin : SolidWorksAddin
    {
        /// <inheritdoc/>
        protected override void RegisterCommands(ICommandGroupHandler commandManager)
        {
            commandManager.AddCommandGroup<Commands>(this.BuildCommands);
            commandManager.AddCommandGroup<SubCommands>(this.BuildSubCommands);
        }

        protected override void RegisterEventHandler(IEventHandlerManager eventHandlerManager)
        {
        }

        protected override void OnConnectToSW(SldWorks swApplication, Cookie addInCookie)
        {
            var cmdMan = swApplication.GetCommandManager(addInCookie);

            var cmdTab = cmdMan.AddCommandTab((int)swDocumentTypes_e.swDocASSEMBLY, "AssemblyTabName");

            var cmdTabBox = cmdTab.AddCommandTabBox();
        }

        private void BuildCommands(ICommandGroupBuilder<Commands> commandHandler)
        {
            commandHandler.AddCommand(
                 Commands.TrialCommand,
                 new RelaySwCommand(this.TrialExecuted));

            commandHandler.AddCommand(
                Commands.TrialCommand2,
                new RelaySwCommand(this.TrialExecuted));
        }

        private void BuildSubCommands(ICommandGroupBuilder<SubCommands> commandHandler)
        {
            commandHandler.AddCommand(
                 SubCommands.TrialCommand,
                 new RelaySwCommand(this.TrialExecuted));

            commandHandler.AddCommand(
                SubCommands.TrialCommand2,
                new RelaySwCommand(this.TrialExecuted));
        }

        private void TrialExecuted(ISwDocument? obj)
        {
            this.SwApplication.SendMsgToUser2("Executed", (int)swMessageBoxIcon_e.swMbInformation, (int)swMessageBoxBtn_e.swMbOk);
        }
    }

    [CommandGroupInfo(1, "Main Commands", ToolTip = "Mein erstes Demo Projekt")]
    [CommandGroupIcons(
        IconsPath = @".\Icons\Toolbar{0}.png",
        MainIconPath = @".\Icons\Icon{0}.png")]
    public enum Commands
    {
        [CommandInfo("Trial Command", ImageIndex = 1, HasMenu = true, HasToolbar = true)]
        TrialCommand,

        [CommandInfo("Trial Command 2", ImageIndex = 2, HasMenu = true, HasToolbar = true, Tooltip = "MainMenu@Trial2")]
        TrialCommand2,
    }

    [CommandGroupInfo(2, "Main Commands\\Sub Commands", ToolTip = "Mein erstes Demo Projekt")]
    [CommandGroupIcons(
    IconsPath = @".\Icons\Toolbar{0}.png",
    MainIconPath = @".\Icons\Icon{0}.png")]
    public enum SubCommands
    {
        [CommandInfo("Trial Command", ImageIndex = 3, HasMenu = true, HasToolbar = true)]
        TrialCommand,

        [CommandInfo("Trial Command 2", ImageIndex = 4, HasMenu = true, HasToolbar = true, Tooltip = "MainMenu@Trial2")]
        TrialCommand2,
    }
}
