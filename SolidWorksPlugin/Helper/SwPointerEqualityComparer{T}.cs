// <copyright file="SwPointerEqualityComparer{T}.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    using System.Collections;
    using System.Collections.Generic;
    using SolidWorks.Interop.sldworks;
    using SolidWorks.Interop.swconst;

    public class SwPointerEqualityComparer<T> : SwPointerEqualityComparer, IEqualityComparer<T>
        where T : class
    {
        internal SwPointerEqualityComparer(ISldWorks app)
            : base(app)
        {
        }

        /// <inheritdoc/>
        public bool Equals(T x, T y) => base.Equals(x, y);

        /// <inheritdoc/>
        public int GetHashCode(T obj) => base.GetHashCode(obj);

        protected virtual bool IsAlive(T obj) => base.IsAlive(obj);
    }

    public class SwPointerEqualityComparer : IEqualityComparer
    {
        private readonly ISldWorks m_App;

        internal SwPointerEqualityComparer(ISldWorks app)
        {
            this.m_App = app;
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
                    return this.m_App.IsSame(x, y) == (int)swObjectEquality.swObjectSame;
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

        protected virtual bool IsAlive(object obj) => true;

        /// <inheritdoc/>
        public int GetHashCode(object obj)
        {
            return obj?.GetHashCode() ?? 0;
        }
    }
}
