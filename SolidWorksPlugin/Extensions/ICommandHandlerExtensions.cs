// <copyright file="ICommandHandlerExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class ICommandHandlerExtensions
    {
        public static ICommandInfo? GetCommand<T>(this ICommandHandler commandHandler, T id)
            where T : struct, Enum
        {
            (var info, _) = typeof(T).GetIconsAndInfo();

            return commandHandler.GetCommand(info.CommandGroupId, (int)(object)id);
        }
    }
}
