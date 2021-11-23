// <copyright file="PropertyChangedEventArgs.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// Event arguments for <see cref="ISwDocument.OnPropertyChanged"/> event.
    /// </summary>
    public class PropertyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="configuration">Name of the current configuration.</param>
        /// <param name="oldValue">The old value (null if property added).</param>
        /// <param name="newValue">The new value (null if property removed).</param>
        /// <param name="valueType">The type of the property value.</param>
        public PropertyChangedEventArgs(string propertyName, string configuration, string? oldValue, string? newValue, swCustomInfoType_e valueType)
        {
            this.PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.ValueType = valueType;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public string PropertyName { get;  }

        /// <summary>
        /// Gets the name of the current configuration.
        /// </summary>
        public string Configuration { get; }

        /// <summary>
        /// Gets the old value (null if property added).
        /// </summary>
        public string? OldValue { get;  }

        /// <summary>
        /// Gets the new value (null if property removed).
        /// </summary>
        public string? NewValue { get; }

        /// <summary>
        /// Gets the type of the property value.
        /// </summary>
        public swCustomInfoType_e ValueType { get; }
    }
}
