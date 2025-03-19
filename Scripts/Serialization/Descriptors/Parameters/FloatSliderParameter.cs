using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a float slider parameter. Used for serialization and deserialization.
    /// </summary>
    public class FloatSliderParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public float DefaultValue { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }

        /* Constructors. */
        public FloatSliderParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public FloatSliderParameterDescriptor(string id, string name, string description, float defaultValue, float minValue,
            float maxValue) : base(id, name, description)
        {
            DefaultValue = defaultValue;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public FloatSliderParameterDescriptor(FloatSliderParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
            MinValue = parameter.MinValue;
            MaxValue = parameter.MaxValue;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public FloatSliderParameterDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == "default")
                        DefaultValue = Parser.ParseFloat(element.InnerText);
                    else if (element.Name == "min")
                        MinValue = Parser.ParseFloat(element.InnerText);
                    else if (element.Name == "max")
                        MaxValue = Parser.ParseFloat(element.InnerText);
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override FloatSliderParameter Generate()
        {
            return new FloatSliderParameter(ID, DisplayName, Description, DefaultValue, MinValue, MaxValue);
        }

        public override string GetXml()
        {
            return GetXml("fslider", DefaultValue != 0f ? DefaultValue.ToString() : "",
                MinValue != 0f ? MinValue.ToString() : "", MaxValue != 0f ? MaxValue.ToString() : "");
        }
    }
}