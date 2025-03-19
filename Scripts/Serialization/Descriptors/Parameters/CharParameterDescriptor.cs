﻿using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a char parameter. Used for serialization and deserialization.
    /// </summary>
    public class CharParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public char DefaultValue { get; set; }

        /* Constructors. */
        public CharParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public CharParameterDescriptor(string id, string name, string description, char defaultValue)
            : base(id, name, description)
        {
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public CharParameterDescriptor(CharParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public CharParameterDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == "default")
                        DefaultValue = Parser.ParseChar(element.InnerText);
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override CharParameter Generate()
        {
            return new CharParameter(ID, DisplayName, Description, DefaultValue);
        }

        public override string GetXml()
        {
            return GetXml("char", DefaultValue != 'A' ? DefaultValue.ToString() : "");
        }
    }
}