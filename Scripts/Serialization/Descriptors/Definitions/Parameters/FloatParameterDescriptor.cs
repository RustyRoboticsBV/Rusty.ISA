﻿using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a float parameter. Used for serialization and deserialization.
    /// </summary>
    public class FloatParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public float DefaultValue { get; set; }

        /* Constructors. */
        public FloatParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public FloatParameterDescriptor(string id, string name, string description, float defaultValue, string preview)
            : base(id, name, description, preview)
        {
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public FloatParameterDescriptor(FloatParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public FloatParameterDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == XmlKeywords.DefaultValue)
                        DefaultValue = Parser.ParseFloat(element.InnerText);
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override FloatParameter Generate()
        {
            return new FloatParameter(ID, DisplayName, Description, DefaultValue, Preview);
        }

        public override string GetXml()
        {
            return GetXml(XmlKeywords.FloatParameter, DefaultValue != 0f ? DefaultValue.ToString() : "");
        }
    }
}