// <copyright file="ISwPart.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Document class for <see cref="PartDoc"/> models.
    /// </summary>
    public interface ISwPart : ISwDocument
    {
        /// <summary>
        /// Gets the model of the document as <see cref="IPartDoc"/>.
        /// </summary>
        IPartDoc Part { get; }
    }
}