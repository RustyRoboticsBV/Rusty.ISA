using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a int slider parameter. Used for serialization and deserialization.
    /// </summary>
    public class IntSliderParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public int DefaultValue { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        /* Constructors. */
        public IntSliderParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public IntSliderParameterDescriptor(string id, string name, string description, int defaultValue, int minValue,
            int maxValue, string preview) : base(id, name, description, preview)
        {
            DefaultValue = defaultValue;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public IntSliderParameterDescriptor(IntSliderParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
            MinValue = parameter.MinValue;
            MaxValue = parameter.MaxValue;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public IntSliderParameterDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == XmlKeywords.DefaultValue)
                        DefaultValue = Parser.ParseInt(element.InnerText);
                    else if (element.Name == XmlKeywords.MinValue)
                        MinValue = Parser.ParseInt(element.InnerText);
                    else if (element.Name == XmlKeywords.MaxValue)
                        MaxValue = Parser.ParseInt(element.InnerText);
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override IntSliderParameter Generate()
        {
            return new IntSliderParameter(ID, DisplayName, Description, DefaultValue, MinValue, MaxValue, Preview);
        }

        public override string GetXml()
        {
            return GetXml(XmlKeywords.IntSliderParameter, DefaultValue != 0 ? DefaultValue.ToString() : "",
                MinValue != 0 ? MinValue.ToString() : "", MaxValue != 0 ? MaxValue.ToString() : "");
        }
    }
}