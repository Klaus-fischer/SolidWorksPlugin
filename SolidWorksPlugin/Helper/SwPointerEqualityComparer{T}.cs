// <copyright file="SwPointerEqualityComparer{T}.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System.Collections.Generic;
    using SolidWorks.Interop.sldworks;

    /// <summary>
    /// Generic implementation of a <see cref="SwPointerEqualityComparer"/> class.
    /// </summary>
    /// <typeparam name="T">Type of the objects to validate.</typeparam>
    public abstract class SwPointerEqualityComparer<T> : SwPointerEqualityComparer, IEqualityComparer<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwPointerEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="swApplication">The current solid works application.</param>
        protected SwPointerEqualityComparer(ISldWorks swApplication)
            : base(swApplication)
        {
        }

        /// <inheritdoc/>
        public bool Equals(T x, T y) => base.Equals(x, y);

        /// <inheritdoc/>
        public int GetHashCode(T obj) => base.GetHashCode(obj);

        /// <summary>
        /// Tests if solid works object is alive.
        /// </summary>
        /// <param name="obj">Object to test.</param>
        /// <returns>True on success.</returns>
        protected abstract bool IsAlive(T obj);

        /// <inheritdoc/>
        protected sealed override bool IsAlive(object obj)
        {
            if (obj is T o)
            {
                return this.IsAlive(o);
            }

            return false;
        }
    }
}
