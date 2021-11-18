// <copyright file="ToggleCommand.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    public interface IToggleCommand : ISwCommand
    {
        bool IsActive { get; }
    }
}
