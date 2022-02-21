// <copyright file="SwDocument.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

#pragma warning disable SA1401 // Fields should be private

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// The base document class.
    /// </summary>
    [DebuggerDisplay("{Filename}")]
    public abstract class SwDocument : ISwDocument
    {
        /// <summary>
        /// Callback to get the get the property manager.
        /// </summary>
        internal Func<IModelDoc2, IPropertyManager>? PropertyManagerCallBack;

        private DateTime lastModifiedDateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwDocument"/> class.
        /// </summary>
        /// <param name="model">The model of the document.</param>
        protected SwDocument(IModelDoc2 model)
        {
            this.Model = model;
            this.TryGetLastWriteTime(out this.lastModifiedDateTime);
        }

        /// <summary>
        /// When a user selects File,
        /// Save on a document that has never been saved,
        /// you receive a FileSaveAsNotify2 instead of FileSaveNotify.
        /// </summary>
        public event DocumentEventHandler<string>? OnFileSave;

        /// <summary>
        /// Sends pre-notification before displaying the File, Save dialog.
        /// </summary>
        public event DocumentEventHandler<string>? OnPreviewSaveAs;

        /// <summary>
        /// Post-notifies the user program when a part document is saved.
        /// </summary>
        public event DocumentEventHandler<PostSaveAsEventArgs>? OnPostSaveAs;

        /// <summary>
        /// Post-notifies the user program when the user has added, changed or deleted a custom property.
        /// </summary>
        public event DocumentEventHandler<PropertyChangedEventArgs>? OnPropertyChanged;

        /// <summary>
        /// Pre-notifies the user program when a part document is about to be destroyed.
        /// </summary>
        public event DocumentEventHandler<swDestroyNotifyType_e>? OnDestroy;

        /// <summary>
        /// Gets the property manager of this document.
        /// </summary>
        public IPropertyManager Properties => this.PropertyManagerCallBack!(this.Model);

        /// <summary>
        /// Gets the model of the document.
        /// </summary>
        public IModelDoc2 Model { get; }

        /// <summary>
        /// Gets the result of <see cref="IModelDoc2.GetPathName"/>.
        /// </summary>
        public string FilePath => this.Model.GetPathName();

        /// <summary>
        /// Gets the filename of the model.
        /// </summary>
        public string Filename => Path.GetFileName(this.Model.GetPathName());

        /// <summary>
        /// Gets the file extension of the current document.
        /// </summary>
        public string FileExtension => Path.GetExtension(this.Model.GetPathName());

        /// <inheritdoc/>
        public bool FileHasChanged =>
            this.TryGetLastWriteTime(out var date) && date != this.lastModifiedDateTime;

        /// <summary>
        /// Handler for <see cref="DPartDocEvents_Event.FileSaveNotify"/> events.
        /// </summary>
        /// <param name="filename">Name of the saved file.</param>
        /// <returns>0 on success.</returns>
        protected int OnFileSaveNotify(string filename)
            => this.OnFileSave?.Invoke(this, filename) ?? 0;

        /// <summary>
        /// Handler for <see cref="DPartDocEvents_Event.FileSaveAsNotify2"/> events.
        /// </summary>
        /// <param name="filename">Name of the saved file.</param>
        /// <returns>0 on success.</returns>
        protected int OnFileSaveAsNotify2(string filename)
            => this.OnPreviewSaveAs?.Invoke(this, filename) ?? 0;

        /// <summary>
        /// Handler for <see cref="DPartDocEvents_Event.FileSavePostNotify"/> events.
        /// </summary>
        /// <param name="saveType">Type of save as defined in <see cref="swFileSaveTypes_e"/>.</param>
        /// <param name="filename">Saved file name.</param>
        /// <returns>0 on success.</returns>
        protected int OnFileSavePostNotify(int saveType, string filename)
        {
            // update last modified date time after document was saved.
            this.TryGetLastWriteTime(out this.lastModifiedDateTime);

            var args = new PostSaveAsEventArgs(filename, (swFileSaveTypes_e)saveType);
            return this.OnPostSaveAs?.Invoke(this, args) ?? 0;
        }

        /// <summary>
        /// Handler for <see cref="DPartDocEvents_Event.AddCustomPropertyNotify"/> events.
        /// </summary>
        /// <param name="propName">Name of the changed property.</param>
        /// <param name="configuration">Configuration that contains the property.</param>
        /// <param name="newValue">New value of the property.</param>
        /// <param name="valueType">Valid type for VARIANT; see Microsoft MSDN for a list of valid VARIANT types.</param>
        /// <returns>0 on success.</returns>
        protected int OnAddCustomPropertyNotify(string propName, string configuration, string newValue, int valueType)
            => this.OnChangeCustomPropertyNotify(propName, configuration, null, newValue, valueType);

        /// <summary>
        /// Handler for <see cref="DPartDocEvents_Event.DeleteCustomPropertyNotify"/> events.
        /// </summary>
        /// <param name="propName">Name of the changed property.</param>
        /// <param name="configuration">Configuration that contains the property.</param>
        /// <param name="oldValue">Previous value of the property.</param>
        /// <param name="valueType">Valid type for VARIANT; see Microsoft MSDN for a list of valid VARIANT types.</param>
        /// <returns>0 on success.</returns>
        protected int OnDeleteCustomPropertyNotify(string propName, string configuration, string oldValue, int valueType)
            => this.OnChangeCustomPropertyNotify(propName, configuration, oldValue, null, valueType);

        /// <summary>
        /// Handler for <see cref="DPartDocEvents_Event.ChangeCustomPropertyNotify"/> events.
        /// </summary>
        /// <param name="propName">Name of the changed property.</param>
        /// <param name="configuration">Configuration that contains the property.</param>
        /// <param name="oldValue">Previous value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        /// <param name="valueType">Valid type for VARIANT; see Microsoft MSDN for a list of valid VARIANT types.</param>
        /// <returns>0 on success.</returns>
        protected int OnChangeCustomPropertyNotify(string propName, string configuration, string? oldValue, string? newValue, int valueType)
        {
            var args = new PropertyChangedEventArgs(
                propertyName: propName,
                configuration: configuration,
                oldValue: oldValue,
                newValue: newValue,
                valueType: (swCustomInfoType_e)valueType);

            return this.OnPropertyChanged?.Invoke(this, args) ?? 0;
        }

        /// <summary>
        /// Handler for <see cref="DPartDocEvents_Event.DestroyNotify2"/> events.
        /// </summary>
        /// <param name="destroyType">Value as defined by <see cref="swDestroyNotifyType_e"/>.</param>
        /// <returns>0 on success.</returns>
        protected int OnDestroyNotify2(int destroyType)
            => this.OnDestroy?.Invoke(this, (swDestroyNotifyType_e)destroyType) ?? 0;

        /// <summary>
        /// Tries to get the last write time of the current file.
        /// </summary>
        /// <param name="lastWriteTime">Read date time.</param>
        /// <returns>True if date time could be read.</returns>
        private bool TryGetLastWriteTime(out DateTime lastWriteTime)
        {
            if (File.Exists(this.FilePath))
            {
                lastWriteTime = File.GetLastWriteTime(this.FilePath);
                return true;
            }

            lastWriteTime = default;
            return false;
        }
    }
}
