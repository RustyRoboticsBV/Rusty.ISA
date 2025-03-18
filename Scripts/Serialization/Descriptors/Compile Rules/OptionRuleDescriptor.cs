using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a option rule. Used for serialization and deserialization.
    /// </summary>
    public class OptionRuleDescriptor : CompileRuleDescriptor
    {
        /* Public properties. */
        public CompileRuleDescriptor Type { get; set; }
        public bool StartEnabled { get; set; }

        /* Constructors. */
        public OptionRuleDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public OptionRuleDescriptor(OptionRule rule) : base(rule)
        {
            Type = Create(rule.Type);
            StartEnabled = rule.StartEnabled;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public OptionRuleDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == "instruction" || element.Name == "option" || element.Name == "choice"
                        || element.Name == "tuple" || element.Name == "list")
                    {
                        Type = Create(element);
                    }
                    else if (element.Name == "enabled")
                        StartEnabled = Parser.ParseBool(element.InnerText);
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a rule from this descriptor.
        /// </summary>
        public override OptionRule Generate()
        {
            return new OptionRule(ID, DisplayName, Description, Type.Generate(), StartEnabled);
        }

        public override string GetXml()
        {
            return GetXml("option", "", StartEnabled, -1, "", "", Type);
        }
    }
}