namespace SIM.SolidWorksPlugin
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal static class ObjectExtensions
    {
        public static IEnumerable<T> Enumerate<T>(this object swResult)
        {
            if (swResult is IEnumerable enumerable)
            {
                return enumerable.OfType<T>();
            }

            return Array.Empty<T>();
        }
    }
}
