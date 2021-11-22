// <copyright file="FilePathExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin.Extensions
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Extensions for strings that represents a file path.
    /// </summary>
    public static class FilePathExtensions
    {
        /// <summary>
        /// Combines a relative path with an absolute path.
        /// </summary>
        /// <param name="root">The absolute base path.</param>
        /// <param name="relative">The relative path.</param>
        /// <returns>The combined string.</returns>
        public static string GetAbsolutePath(this string root, string relative)
        {
            string result = $"{root}\\{relative}";

            while (Regex.IsMatch(result, @"\\\.?\\"))
            {
                result = Regex.Replace(result, @"\\\.?\\", "\\");
            }

            while (result.Contains(@"\..\"))
            {
                result = Regex.Replace(result, @"\\[^\\]+?\\\.\.\\", "\\");
            }

            return result;
        }
    }
}
