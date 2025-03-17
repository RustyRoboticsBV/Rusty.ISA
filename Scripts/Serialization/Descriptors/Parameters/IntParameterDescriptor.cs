﻿using System.Xml.Serialization;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a int parameter. Used for serialization and deserialization.
    /// </summary>
    public class IntParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        [XmlAttribute("default")]
        public int DefaultValue { get; set; }

        /* Constructors. */
        public IntParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public IntParameterDescriptor(IntParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override IntParameter Generate()
        {
            return new IntParameter(ID, DisplayName, Description, DefaultValue);
        }
    }
}