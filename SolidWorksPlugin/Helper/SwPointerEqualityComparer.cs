// <copyright file="SwPointerEqualityComparer.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System.Collections;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    /// <summary>
    /// Implementation of an equality comparer for solid works objects.
    /// </summary>
    public abstract class SwPointerEqualityComparer : IEqualityComparer
    {
        /// <summary>
        /// The current solid works application.
        /// </summary>
        private readonly ISldWorks swApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwPointerEqualityComparer"/> class.
        /// </summary>
        /// <param name="swApplication">The current solid works application.</param>
        protected SwPointerEqualityComparer(ISldWorks swApplication)
        {
            this.swApplication = swApplication;
        }

        /// <inheritdoc/>
        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            try
            {
                // Note: ISldWorks::IsSame can crash if pointer is disconnected
                if (this.IsAlive(x) && this.IsAlive(y))
                {
                    return this.swApplication.IsSame(x, y) == (int)swObjectEquality.swObjectSame;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public int GetHashCode(object obj)
        {
            return obj?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Tests if solid works object is alive.
        /// </summary>
        /// <param name="obj">Object to test.</param>
        /// <returns>True on success.</returns>
        protected abstract bool IsAlive(object obj);
    }
}
