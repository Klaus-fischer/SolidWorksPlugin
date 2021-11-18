// <copyright file="SwModelPointerEqualityComparer.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;

    internal class SwModelPointerEqualityComparer : SwPointerEqualityComparer<IModelDoc2>
    {
        internal SwModelPointerEqualityComparer(ISldWorks app)
            : base(app)
        {
        }

        protected override bool IsAlive(IModelDoc2 model)
        {
            try
            {
                var title = model.GetTitle();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
