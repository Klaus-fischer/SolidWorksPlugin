// <copyright file="IMaterialBrowserExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// Declaration of an material browser.
    /// </summary>
    public static class IMaterialBrowserExtensions
    {
        /// <summary>
        /// Assign material to an solid works part.
        /// </summary>
        /// <param name="browser">The material browser to extend.</param>
        /// <param name="swPart">The part to assign material to.</param>
        /// <param name="name">Name of the material to assign.</param>
        public static void AssignMaterial(this IMaterialBrowser browser, ISwPart swPart, string name)
        {
            if (browser.FindMaterialbyName(name) is MaterialItem material)
            {
                browser.AssignMaterial(swPart, material);
            }
        }
    }
}