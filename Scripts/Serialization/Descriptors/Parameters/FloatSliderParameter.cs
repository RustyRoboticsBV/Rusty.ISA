using System.Xml.Serialization;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a float slider parameter. Used for serialization and deserialization.
    /// </summary>
    public class FloatSliderParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        [XmlAttribute("default")]
        public float DefaultValue { get; set; }
        [XmlAttribute("min")]
        public float MinValue { get; set; }
        [XmlAttribute("max")]
        public float MaxValue { get; set; }

        /* Constructors. */
        public FloatSliderParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public FloatSliderParameterDescriptor(FloatSliderParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
            MinValue = parameter.MinValue;
            MaxValue = parameter.MaxValue;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override FloatSliderParameter Generate()
        {
            return new FloatSliderParameter(ID, DisplayName, Description, DefaultValue, MinValue, MaxValue);
        }
    }
}