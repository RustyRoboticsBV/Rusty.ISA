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
                    if (element.Name == "opcode")
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
            return new InstructionRule(ID, DisplayName, Description, Opcode);
        }

        public override string GetXml()
        {
            return GetXml("instruction", Opcode, false, -1, "", "", null);
        }
    }
}