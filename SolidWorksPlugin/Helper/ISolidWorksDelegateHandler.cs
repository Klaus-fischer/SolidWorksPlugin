// <copyright file="ISolidWorksDelegateHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Handler for delegating callbacks to SolidWorks.
    /// </summary>
    public interface ISolidWorksDelegateHandler
    {
        /// <summary>
        /// Gets the default callback to run a solid works command.
        /// </summary>
        RunCommandDelegate RunCommand { get; }

        /// <summary>
        /// Gets the default callback to send messages
        /// by calling <see cref="ISldWorks.SendMsgToUser2(string, int, int)"/>.
        /// </summary>
        MessageToUserCallback SendMessage { get; }

        /// <summary>
        /// Gets the default callback to set user preferences
        /// by calling <see cref="ISldWorks.SetUserPreferenceDoubleValue(int, double)"/>.
        /// </summary>
        SetUserPreferenceDoubleDelegate SetDoublePreference { get; }

        /// <summary>
        /// Gets the default callback to set user preferences
        /// by calling <see cref="ISldWorks.SetUserPreferenceIntegerValue(int, int)"/>.
        /// </summary>
        SetUserPreferenceIntegerDelegate SetIntegerPreference { get; }

        /// <summary>
        /// Gets the default callback to set user preferences
        /// by calling <see cref="ISldWorks.SetUserPreferenceStringValue(int, string)"/>.
        /// </summary>
        SetUserPreferenceStringDelegate SetStringPreference { get; }
    }
}