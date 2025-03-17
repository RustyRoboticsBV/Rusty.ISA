using System.Xml.Serialization;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a output parameter. Used for serialization and deserialization.
    /// </summary>
    public class OutputParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        [XmlAttribute("remove_default")]
        public bool RemoveDefaultOutput { get; set; }
        [XmlAttribute("preview_argument")]
        public string PreviewArgument { get; set; } = "";

        /* Constructors. */
        public OutputParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public OutputParameterDescriptor(OutputParameter parameter) : base(parameter)
        {
            RemoveDefaultOutput = parameter.RemoveDefaultOutput;
            PreviewArgument = parameter.UseArgumentAsPreview;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override OutputParameter Generate()
        {
            return new OutputParameter(ID, DisplayName, Description, RemoveDefaultOutput, PreviewArgument);
        }
    }
}