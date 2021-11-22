// <copyright file="IToggleCommand.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// Adds <see cref="IsActive"/> function to an <see cref="ISwCommand"/>.
    /// </summary>
    public interface IToggleCommand : ISwCommand
    {
        /// <summary>
        /// Gets a value indicating whether command is active (optical hint in command manager).
        /// </summary>
        bool IsActive { get; }
    }
}
