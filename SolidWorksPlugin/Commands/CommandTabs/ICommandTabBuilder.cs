// <copyright file="ICommandTabBuilder.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// Definition of a command tab builder.
    /// </summary>
    public interface ICommandTabBuilder
    {
        /// <summary>
        /// Adds a command to the current tab page.
        /// </summary>
        /// <param name="commandInfo">The command info.</param>
        /// <param name="textDisplay">The text display options.</param>
        void AddCommand(ICommandInfo commandInfo, swCommandTabButtonTextDisplay_e textDisplay);

        /// <summary>
        /// Adds a spacer to the current tab page.
        /// </summary>
        void AddSpacer();

        /// <summary>
        /// Adds a tool bar as a fly out menu to the current tab page.
        /// </summary>
        /// <param name="commandGroupInfo">The command group info to display.</param>
        /// <param name="textDisplay">The text display style.</param>
        /// <param name="flyoutStyle">The fly out display style.</param>
        public void AddFlyout(
            ICommandGroupInfo commandGroupInfo,
            swCommandTabButtonTextDisplay_e textDisplay,
            swCommandTabButtonFlyoutStyle_e flyoutStyle);
    }
}
