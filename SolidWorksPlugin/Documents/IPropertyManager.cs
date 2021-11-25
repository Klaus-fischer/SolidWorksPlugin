// <copyright file="IPropertyManager.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;

    /// <summary>
    /// Grants access to the properties of the model.
    /// </summary>
    public interface IPropertyManager
    {
        /// <summary>
        /// Gets or sets the title of the summary info.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the subject of the summary info.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the author of the summary info.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the keywords of the summary info.
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the comments of the summary info.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets the save date time of the summary info.
        /// </summary>
        public DateTime SaveDate { get; }

        /// <summary>
        /// Gets the create date time of the summary info.
        /// </summary>
        public DateTime CreateDate { get; }

        /// <summary>
        /// Gets or sets the name of the configuration to read the custom properties from.
        /// </summary>
        string CustomPropertyConfiguration { get; set; }

        /// <summary>
        /// Gets or sets a custom property as string.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Value of the property.</returns>
        string? this[string propertyName] { get; set; }

        /// <summary>
        /// Deletes a custom property from a single configuration or model document.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        void DeleteProperty(string propertyName);

        /// <summary>
        /// Adds a custom property to a single configuration or model document.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        void SetStringProperty(string propertyName, string? value);

        /// <summary>
        /// Gets the value and the evaluated value of the specified custom property.
        /// http://help.solidworks.com/2016/english/api/sldworksapi/SolidWorks.Interop.sldworks~SolidWorks.Interop.sldworks.ICustomPropertyManager~Get5.html.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Value of the property.</returns>
        string? GetStringProperty(string propertyName);

        /// <summary>
        /// Gets the value and the evaluated value of the specified custom property.
        /// http://help.solidworks.com/2016/english/api/sldworksapi/SolidWorks.Interop.sldworks~SolidWorks.Interop.sldworks.ICustomPropertyManager~Get5.html.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Value of the property.</returns>
        DateTime? GetDateProperty(string propertyName);

        /// <summary>
        /// Adds a custom property to a single configuration or model document.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        void SetDateProperty(string propertyName, DateTime? value);

        /// <summary>
        /// Overrides mass of part of assembly if difference &gt; 5%.
        /// </summary>
        /// <param name="weight">Weight to apply in Kg.</param>
        public void SetWeight(double weight);

        /// <summary>
        /// Gets the mass of an assembly or a part.
        /// </summary>
        /// <returns>Mass in Kg.</returns>
        public double GetWeight();

        /// <summary>
        /// Gets the names of all configurations.
        /// </summary>
        /// <returns>Collection of strings.</returns>
        public string[] GetConfigurationNames();

        /// <summary>
        /// Gets the names of all properties in the current active configuration.
        /// </summary>
        /// <returns>Collection of strings.</returns>
        public string[] GetPropertyNames();
    }
}
