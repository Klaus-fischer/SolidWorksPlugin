// <copyright file="ISwDocument.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// Events that are used in <see cref="SwDocument"/>.
    /// </summary>
    public interface ISwDocument
    {
        /// <summary>
        /// When a user selects File,
        /// Save on a document that has never been saved,
        /// you receive a FileSaveAsNotify2 instead of FileSaveNotify.
        /// </summary>
        event DocumentEventHandler<string>? OnFileSave;

        /// <summary>
        /// Sends pre-notification before displaying the File, Save dialog.
        /// </summary>
        event DocumentEventHandler<string>? OnPreviewSaveAs;

        /// <summary>
        /// Post-notifies the user program when a part document is saved.
        /// </summary>
        event DocumentEventHandler<PostSaveAsEventArgs>? OnPostSaveAs;

        /// <summary>
        /// Post-notifies the user program when the user has added, changed or deleted a custom property.
        /// </summary>
        event DocumentEventHandler<PropertyChangedEventArgs>? OnPropertyChanged;

        /// <summary>
        /// Pre-notifies the user program when a part document is about to be destroyed.
        /// </summary>
        event DocumentEventHandler<swDestroyNotifyType_e>? OnDestroy;

        /// <summary>
        /// Gets the model of the document.
        /// </summary>
        IModelDoc2 Model { get; }
    }
}