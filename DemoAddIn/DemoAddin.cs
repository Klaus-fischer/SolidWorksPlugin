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

        protected override void AddCommandTabMenu(ICommandTabManager tabManager)
        {
            tabManager.BuildCommandTab(
                "Mein Makro",
                builder =>
                {
                    builder.AddCommand(
                        this.CommandHandler.GetCommand(Commands.TrialCommand)!,
                        swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow);

                    builder.AddCommand(
                        this.CommandHandler.GetCommand(Commands.TrialCommand2)!,
                        swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow);

                    builder.AddSpacer();

                    builder.AddFlyout(this.CommandHandler.GetCommandGroup(2)!,
                        swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow,
                        swCommandTabButtonFlyoutStyle_e.swCommandTabButton_ActionFlyout);

                },

                swDocumentTypes_e.swDocASSEMBLY, swDocumentTypes_e.swDocPART);
        }

        protected override void RegisterEventHandler(IEventHandlerManager eventHandlerManager)
        {
        }

        protected override void OnConnectToSW(SldWorks swApplication, Cookie addInCookie)
        {
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

        private void BuildSubCommands(ICommandGroupBuilder<SubCommands> builder)
        {
            builder.AddCommand(
                 SubCommands.TrialCommand,
                 new RelaySwCommand(this.TrialExecuted));

            builder.AddCommand(
                SubCommands.TrialCommand2,
                new RelaySwCommand(this.TrialExecuted));
        }

        private void TrialExecuted(ISwDocument? obj)
        {
            this.SwApplication.SendMsgToUser2("Executed", (int)swMessageBoxIcon_e.swMbInformation, (int)swMessageBoxBtn_e.swMbOk);
        }
    }

    [CommandGroupSpec(1, "Main Commands", ToolTip = "Mein erstes Demo Projekt")]
    [CommandGroupIcons(
        IconsPath = @".\Icons\Toolbar{0}.png",
        MainIconPath = @".\Icons\Icon{0}.png")]
    public enum Commands
    {
        [CommandSpec("Trial Command", ImageIndex = 1, HasMenu = true, HasToolbar = true)]
        TrialCommand,

        [CommandSpec("Trial Command 2", ImageIndex = 2, HasMenu = true, HasToolbar = true, Tooltip = "MainMenu@Trial2")]
        TrialCommand2,
    }

    [CommandGroupSpec(2, "Main Commands\\Sub Commands", ToolTip = "Mein erstes Demo Projekt")]
    [CommandGroupIcons(
    IconsPath = @".\Icons\Toolbar{0}.png",
    MainIconPath = @".\Icons\Icon{0}.png")]
    public enum SubCommands
    {
        [CommandSpec("Trial Command", ImageIndex = 3, HasMenu = true, HasToolbar = true)]
        TrialCommand,

        [CommandSpec("Trial Command 2", ImageIndex = 4, HasMenu = true, HasToolbar = true, Tooltip = "MainMenu@Trial2")]
        TrialCommand2,
    }
}
