using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a multiline parameter. Used for serialization and deserialization.
    /// </summary>
    public class MultilineParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public string DefaultValue { get; set; } = "";

        /* Constructors. */
        public MultilineParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public MultilineParameterDescriptor(string id, string name, string description, string defaultValue, string preview)
            : base(id, name, description, preview)
        {
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public MultilineParameterDescriptor(MultilineParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public MultilineParameterDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == XmlKeywords.DefaultValue)
                        DefaultValue = element.InnerText;
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override MultilineParameter Generate()
        {
            return new MultilineParameter(ID, DisplayName, Description, DefaultValue, Preview);
        }

        public override string GetXml()
        {
            return GetXml(XmlKeywords.MultilineParameter, DefaultValue);
        }
    }
}