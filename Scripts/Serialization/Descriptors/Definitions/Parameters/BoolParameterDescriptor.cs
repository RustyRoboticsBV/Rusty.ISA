using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a bool parameter. Used for serialization and deserialization.
    /// </summary>
    public class BoolParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public bool DefaultValue { get; set; }

        /* Constructors. */
        public BoolParameterDescriptor() : base() { }

        public BoolParameterDescriptor(string id, string name, string description, bool defaultValue, string preview)
            : base(id, name, description, preview)
        {
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public BoolParameterDescriptor(BoolParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public BoolParameterDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == XmlKeywords.DefaultValue)
                        DefaultValue = Parser.ParseBool(element.InnerText);
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override BoolParameter Generate()
        {
            return new BoolParameter(ID, DisplayName, Description, DefaultValue, Preview);
        }

        public override string GetXml()
        {
            return GetXml(XmlKeywords.BoolParameter, DefaultValue ? "true" : "");
        }
    }
}