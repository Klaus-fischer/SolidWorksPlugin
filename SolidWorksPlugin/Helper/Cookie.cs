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
        public Cookie(int value)
        {
            this.Value = value;
        }

        public int Value { get; }

        public static implicit operator int(Cookie c) => c.Value;
    }
}
