using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a output parameter. Used for serialization and deserialization.
    /// </summary>
    public class OutputParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public bool RemoveDefaultOutput { get; set; }
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

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public OutputParameterDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == "remove_default")
                        RemoveDefaultOutput = true;
                    else if (element.Name == "preview_arg")
                        PreviewArgument = element.InnerText;
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override OutputParameter Generate()
        {
            return new OutputParameter(ID, DisplayName, Description, RemoveDefaultOutput, PreviewArgument);
        }

        public override string GetXml()
        {
            return GetXml("output", "", "", "", RemoveDefaultOutput, PreviewArgument);
        }
    }
}