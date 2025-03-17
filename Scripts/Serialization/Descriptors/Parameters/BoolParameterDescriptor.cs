using System.Xml.Serialization;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a bool parameter. Used for serialization and deserialization.
    /// </summary>
    public class BoolParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        [XmlAttribute("default")]
        public bool DefaultValue { get; set; }

        /* Constructors. */
        public BoolParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public BoolParameterDescriptor(BoolParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override BoolParameter Generate()
        {
            return new BoolParameter(ID, DisplayName, Description, DefaultValue);
        }
    }
}