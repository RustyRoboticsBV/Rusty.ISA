using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a int parameter. Used for serialization and deserialization.
    /// </summary>
    public class IntParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
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

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public IntParameterDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == "default")
                        DefaultValue = Parser.ParseInt(element.InnerText);
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override IntParameter Generate()
        {
            return new IntParameter(ID, DisplayName, Description, DefaultValue);
        }

        public override string GetXml()
        {
            return GetXml("int", DefaultValue != 0 ? DefaultValue.ToString() : "");
        }
    }
}