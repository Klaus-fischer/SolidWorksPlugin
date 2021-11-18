// <copyright file="LogHandler.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System;
    using System.IO;
    using System.Text;

    internal static class LogHandler
    {
        private static readonly object syncLock = new object();

        public static void LogException(Exception e, string message = null)
        {
            var sb = new StringBuilder();

            if (message != null)
            {
                sb.AppendLine($"{message}");
            }

            sb.AppendLine($"Exception: {e.GetType().Name} at {DateTime.Now:hh:mm:ss dd.MM.yyyy}");
            sb.AppendLine($"Message: {e.Message}");
            sb.AppendLine($"StackTrace: {e.StackTrace}");

            var inner = e.InnerException;
            while (inner != null)
            {
                sb.AppendLine();

                sb.AppendLine($"InnerException: {inner.GetType().Name}");
                sb.AppendLine($"Message: {inner.Message}");
                sb.AppendLine($"StackTrace: {inner.StackTrace}");
                sb.AppendLine($"InnerException: {inner.GetType().Name}");
                sb.AppendLine($"Message: {inner.Message}");
                sb.AppendLine($"StackTrace: {inner.StackTrace}");
                inner = inner.InnerException;
            }

            sb.AppendLine();
            sb.AppendLine();

            var fname = Path.Combine(Path.GetTempPath(), "SIM-Toolbox.log");

            lock (syncLock)
            {
                if (File.Exists(fname) && File.GetLastWriteTime(fname) > DateTime.Now.AddMinutes(-1))
                {
                    File.AppendAllText(fname, sb.ToString());
                }
                else
                {
                    File.WriteAllText(fname, sb.ToString());
                }
            }
        }
    }
}
