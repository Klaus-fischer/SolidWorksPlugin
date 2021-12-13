// <copyright file="ILoggerExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Runtime.CompilerServices;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Contains extension methods of <see cref="ILogger"/>.
    /// </summary>
    public static class ILoggerExtensions
    {
        /// <summary>
        /// Logs an exception and adds method name to message.
        /// </summary>
        /// <param name="logger">The logger instance to extend.</param>
        /// <param name="exception">THe extension to log.</param>
        /// <param name="name">The name of the calling method.</param>
        public static void LogError(
            this ILogger? logger,
            Exception exception,
            [CallerMemberName] string name = "")
        {
            logger?.LogError(exception, $"Error on {name}.");
        }
    }
}
