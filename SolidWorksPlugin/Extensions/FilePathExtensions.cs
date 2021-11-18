// <copyright file="FilePathExtensions.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin.Extensions
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

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

        public static string GetAbsolutePathSpan(this string root, string relative)
        {
            int relativeUpCount = 0;

            var relSpan = relative.AsSpan();

            while (relSpan[0] == '.' || relSpan[0] == '\\')
            {
                if (relSpan[0] == '\\')
                {
                    relSpan = relSpan.Slice(1);
                }

                if (relSpan[0] == '.')
                {
                    if (relSpan[1] == '.')
                    {
                        relativeUpCount++;
                        relSpan = relSpan.Slice(2);
                    }
                    else
                    {
                        relSpan = relSpan.Slice(1);
                    }
                }
            }

            var rootSpan = root.AsSpan().TrimEnd('\\');

            while (relativeUpCount > 0 && !rootSpan.IsEmpty)
            {
                if (rootSpan[rootSpan.Length - 1] == '\\')
                {
                    relativeUpCount--;
                }

                rootSpan = rootSpan.Slice(0, rootSpan.Length - 1);
            }

            return string.Concat(rootSpan, "\\", relSpan);
        }
    }
}
