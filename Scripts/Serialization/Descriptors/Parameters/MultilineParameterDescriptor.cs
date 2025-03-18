using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a multiline parameter. Used for serialization and deserialization.
    /// </summary>
    public class MultilineParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public string DefaultValue { get; set; }

        /* Constructors. */
        public MultilineParameterDescriptor() : base() { }

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
                    if (element.Name == "default")
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
            return new MultilineParameter(ID, DisplayName, Description, DefaultValue);
        }

        public override string GetXml()
        {
            return GetXml("multiline", DefaultValue);
        }
    }
}