using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a instruction rule. Used for serialization and deserialization.
    /// </summary>
    public class InstructionRuleDescriptor : CompileRuleDescriptor
    {
        /* Public properties. */
        public string Opcode { get; set; } = "";

        /* Constructors. */
        public InstructionRuleDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public InstructionRuleDescriptor(string id, string displayName, string description, string opcode, string preview)
            : base(id, displayName, description, preview)
        {
            Opcode = opcode;
        }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public InstructionRuleDescriptor(InstructionRule rule) : base(rule)
        {
            Opcode = rule.Opcode;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public InstructionRuleDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == XmlKeywords.Opcode)
                        Opcode = element.InnerText;
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a rule from this descriptor.
        /// </summary>
        public override InstructionRule Generate()
        {
            return new InstructionRule(ID, DisplayName, Description, Opcode, Preview);
        }

        public override string GetXml()
        {
            return GetXml(XmlKeywords.InstructionRule, Opcode, false, -1, "", null);
        }
    }
}