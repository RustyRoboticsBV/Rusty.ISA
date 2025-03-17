using System.Xml.Serialization;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a char parameter. Used for serialization and deserialization.
    /// </summary>
    public class CharParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        [XmlAttribute("default")]
        public char DefaultValue { get; set; }

        /* Constructors. */
        public CharParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public CharParameterDescriptor(CharParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override CharParameter Generate()
        {
            return new CharParameter(ID, DisplayName, Description, DefaultValue);
        }
    }
}