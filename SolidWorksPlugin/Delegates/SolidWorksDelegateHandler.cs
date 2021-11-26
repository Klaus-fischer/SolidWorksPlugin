// <copyright file="SolidWorksDelegateHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swcommands;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// Handler for delegating callbacks to SolidWorks.
    /// </summary>
    internal class SolidWorksDelegateHandler : ISolidWorksDelegateHandler
    {
        /// <summary>
        /// Gets the reference to the current Solid-Works application.
        /// </summary>
        private readonly SldWorks swApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidWorksDelegateHandler"/> class.
        /// </summary>
        /// <param name="swApplication">The SolidWorks application.</param>
        public SolidWorksDelegateHandler(SldWorks swApplication)
        {
            this.swApplication = swApplication;
        }

        /// <inheritdoc/>
        public MessageToUserCallback SendMessage => this.SendMessageToUser;

        /// <inheritdoc/>
        public RunCommandDelegate RunCommand => this.RunSolidWorksCommand;

        /// <inheritdoc/>
        public SetUserPreferenceDoubleDelegate SetDoublePreference
            => this.SetUserPreferenceDouble;

        /// <inheritdoc/>
        public SetUserPreferenceStringDelegate SetStringPreference
            => this.SetUserPreferenceString;

        /// <inheritdoc/>
        public SetUserPreferenceIntegerDelegate SetIntegerPreference 
            => this.SetUserPreferenceInteger;

        /// <inheritdoc/>
        public ReplaceReferencedDocumentDelegate ReplaceReferencedDocuments 
            => this.swApplication.ReplaceReferencedDocument;

        private swMessageBoxResult_e SendMessageToUser(
            string message,
            swMessageBoxIcon_e icon = swMessageBoxIcon_e.swMbInformation,
            swMessageBoxBtn_e buttons = swMessageBoxBtn_e.swMbOk)
        {
            return (swMessageBoxResult_e)this.swApplication.SendMsgToUser2(message, (int)icon, (int)buttons);
        }

        private bool RunSolidWorksCommand(swCommands_e command, string title)
            => this.swApplication.RunCommand((int)command, title);

        private bool SetUserPreferenceDouble(swUserPreferenceDoubleValue_e preference, double value)
            => this.swApplication!.SetUserPreferenceDoubleValue((int)preference, value);

        private bool SetUserPreferenceString(swUserPreferenceStringValue_e preference, string value)
            => this.swApplication!.SetUserPreferenceStringValue((int)preference, value);

        private bool SetUserPreferenceInteger(swUserPreferenceIntegerValue_e preference, int value)
            => this.swApplication!.SetUserPreferenceIntegerValue((int)preference, value);
    }
}
