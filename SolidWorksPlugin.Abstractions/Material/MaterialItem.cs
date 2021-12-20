// <copyright file="MaterialItem.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System.Diagnostics;

    /// <summary>
    /// Contains name and database informations of Material (Werkstoff).
    /// </summary>
    [DebuggerDisplay("{Name} [{Database}]")]
    public class MaterialItem
    {
        /// <summary>
        /// Gets the default empty value.
        /// </summary>
        public static readonly MaterialItem Empty = new("-", "kein Werkstoff", string.Empty);

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialItem"/> class.
        /// </summary>
        /// <param name="name">Name of the Material.</param>
        /// <param name="category">The category of the material.</param>
        /// <param name="database">The file path to the database.</param>
        public MaterialItem(string name, string category, string database)
        {
            this.Name = name;
            this.Category = category;
            this.Database = database;
        }

        /// <summary>
        /// Gets the name of the material.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the file path to the database.
        /// </summary>
        public string Database { get; }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public string Category { get; }
    }
}
