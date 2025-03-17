using System.Xml.Serialization;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a parameter. Used for serialization and deserialization.
    /// </summary>
    public abstract class ParameterDescriptor
    {
        /* Public properties. */
        [XmlAttribute("id")]
        public string ID { get; set; } = "";
        [XmlElement("name")]
        public string DisplayName { get; set; } = "";
        [XmlElement("desc")]
        public string Description { get; set; } = "";

        /* Constructors. */
        public ParameterDescriptor() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public ParameterDescriptor(Parameter parameter)
        {
            ID = parameter.ID;
            DisplayName = parameter.DisplayName;
            Description = parameter.Description;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public abstract Parameter Generate();
    }
}