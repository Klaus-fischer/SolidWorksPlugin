namespace SIM.SolidWorksPlugin.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SolidWorks.Interop.sldworks;

    public static class IModelDoc2Extensions
    {
        public static IEnumerable<string> GetConfigurationNameStrings(this IModelDoc2 model)
        {
            if (model.GetConfigurationNames() is object[] configs)
            {
                return configs.OfType<string>();
            }

            return Array.Empty<string>();
        }
    }
}
