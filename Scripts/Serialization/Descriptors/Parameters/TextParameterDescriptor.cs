using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a text parameter. Used for serialization and deserialization.
    /// </summary>
    public class TextParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public string DefaultValue { get; set; }

        /* Constructors. */
        public TextParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public TextParameterDescriptor(TextParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public TextParameterDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == "default")
                        DefaultValue = element.InnerText;
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override TextParameter Generate()
        {
            return new TextParameter(ID, DisplayName, Description, DefaultValue);
        }

        public override string GetXml()
        {
            return GetXml("text", DefaultValue);
        }
    }
}