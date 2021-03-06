// <copyright file="Mass.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace SIM.SolidWorksPlugin
{
    /// <summary>
    /// Data type for the mass property of a sold part or assembly.
    /// </summary>
    public struct Mass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mass"/> struct.
        /// </summary>
        /// <param name="value">The weight of the model.</param>
        /// <param name="isOverridden">The value indicating whether mass is overridden or calculated.</param>
        public Mass(double value, bool isOverridden)
        {
            this.Value = value;
            this.IsOverridden = isOverridden;
        }

        /// <summary>
        /// Gets the weight of the mass property in kg.
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Gets a value indicating whether mass is overridden or calculated.
        /// </summary>
        public bool IsOverridden { get; }

        /// <summary>
        /// Converter to use the mass property directly.
        /// </summary>
        /// <param name="mass">Mass to convert.</param>
        public static implicit operator double(Mass mass) => mass.Value;
    }
}
