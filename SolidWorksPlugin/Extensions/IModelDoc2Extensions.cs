// <copyright file="IModelDoc2Extensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Extensions for <see cref="IModelDoc2"/> interface.
    /// </summary>
    internal static class IModelDoc2Extensions
    {
        /// <summary>
        /// Calls <see cref="ModelDoc2.GetConfigurationNames()"/> and converts the result to a string enumeration.
        /// </summary>
        /// <param name="model">The model to get configuration names from.</param>
        /// <returns>Collection of strings.</returns>
        public static string[] GetConfigurationNameStrings(this IModelDoc2 model)
        {
            if (model.GetConfigurationNames() is object[] configs)
            {
                return configs.OfType<string>().ToArray();
            }

            return Array.Empty<string>();
        }
    }
}
