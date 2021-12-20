// <copyright file="IMaterialBrowser.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// Declaration of an material browser.
    /// </summary>
    public interface IMaterialBrowser
    {
        /// <summary>
        /// Assign material to an solid works part.
        /// </summary>
        /// <param name="swPart">The part to assign material to.</param>
        /// <param name="material">the material to assign to.</param>
        void AssignMaterial(ISwPart swPart, MaterialItem material);

        /// <summary>
        /// Browse by name.
        /// </summary>
        /// <param name="name">Name of the material.</param>
        /// <returns>The found item or <see cref="MaterialItem.Empty"/> if not found.</returns>
        MaterialItem FindMaterialbyName(string name);
    }
}