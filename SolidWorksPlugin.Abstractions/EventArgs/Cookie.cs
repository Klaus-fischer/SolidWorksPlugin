// <copyright file="Cookie.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// Defines the cookie as type save item.
    /// </summary>
    public struct Cookie
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cookie"/> struct.
        /// </summary>
        /// <param name="value">Value of the cookie.</param>
        public Cookie(int value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the value of the cookie.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Converts the cookie to an integer.
        /// </summary>
        /// <param name="c">the cookie.</param>
        public static implicit operator int(Cookie c) => c.Value;
    }
}
