using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a text line parameter. Used for serialization and deserialization.
    /// </summary>
    public class TextlineParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public string DefaultValue { get; set; }

        /* Constructors. */
        public TextlineParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public TextlineParameterDescriptor(string id, string name, string description, string defaultValue)
            : base(id, name, description)
        {
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public TextlineParameterDescriptor(TextlineParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public TextlineParameterDescriptor(XmlElement xml) : base(xml)
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
        public override TextlineParameter Generate()
        {
            return new TextlineParameter(ID, DisplayName, Description, DefaultValue);
        }

        public override string GetXml()
        {
            return GetXml("text", DefaultValue);
        }
    }
}