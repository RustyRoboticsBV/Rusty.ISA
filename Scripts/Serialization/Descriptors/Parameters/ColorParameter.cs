﻿using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a color parameter. Used for serialization and deserialization.
    /// </summary>
    public class ColorParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public Color DefaultValue { get; set; }

        /* Constructors. */
        public ColorParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public ColorParameterDescriptor(ColorParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override ColorParameter Generate()
        {
            return new ColorParameter(ID, DisplayName, Description, DefaultValue);
        }

        public override string GetXml()
        {
            return GetXml("color", DefaultValue != Colors.White ? Serialize(DefaultValue) : "");
        }

        /* Private methods. */
        private static string Serialize(Color color)
        {
            bool includeAlpha = color.A < 1f;
            return '#' + color.ToHtml(includeAlpha);
        }
    }
}