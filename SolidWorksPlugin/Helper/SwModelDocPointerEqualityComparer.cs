// <copyright file="SwModelDocPointerEqualityComparer.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Concrete implementation of an <see cref="SwPointerEqualityComparer{T}"/> for <see cref="IModelDoc2"/> objects.
    /// </summary>
    internal sealed class SwModelDocPointerEqualityComparer : SwPointerEqualityComparer<IModelDoc2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwModelDocPointerEqualityComparer"/> class.
        /// </summary>
        /// <param name="swApplication">The current solid works application.</param>
        internal SwModelDocPointerEqualityComparer(ISldWorks swApplication)
            : base(swApplication)
        {
        }

        /// <inheritdoc/>
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
