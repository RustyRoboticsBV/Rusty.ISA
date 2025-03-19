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

        /* Constructors. */
        public OutputParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public OutputParameterDescriptor(string id, string name, string description, bool removeDefaultOutput, string preview)
            : base(id, name, description, preview)
        {
            RemoveDefaultOutput = removeDefaultOutput;
        }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public OutputParameterDescriptor(OutputParameter parameter) : base(parameter)
        {
            RemoveDefaultOutput = parameter.RemoveDefaultOutput;
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
                    if (element.Name == XmlKeywords.RemoveDefaultOutput)
                        RemoveDefaultOutput = true;
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override OutputParameter Generate()
        {
            return new OutputParameter(ID, DisplayName, Description, RemoveDefaultOutput, Preview);
        }

        public override string GetXml()
        {
            return GetXml(XmlKeywords.OutputParameter, "", "", "", RemoveDefaultOutput, Preview);
        }
    }
}