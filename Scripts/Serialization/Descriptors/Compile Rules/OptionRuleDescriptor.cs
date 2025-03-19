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
        public bool DefaultEnabled { get; set; }

        /* Constructors. */
        public OptionRuleDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public OptionRuleDescriptor(string id, string displayName, string description, CompileRuleDescriptor type,
            bool defaultEnabled, string preview) : base(id, displayName, description, preview)
        {
            Type = type;
            DefaultEnabled = defaultEnabled;
        }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public OptionRuleDescriptor(OptionRule rule) : base(rule)
        {
            Type = Create(rule.Type);
            DefaultEnabled = rule.DefaultEnabled;
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
                    if (XmlKeywords.CompileRules.Contains(element.Name))
                        Type = Create(element);
                    else if (element.Name == XmlKeywords.DefaultEnabled)
                        DefaultEnabled = Parser.ParseBool(element.InnerText);
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a rule from this descriptor.
        /// </summary>
        public override OptionRule Generate()
        {
            return new OptionRule(ID, DisplayName, Description, Type.Generate(), DefaultEnabled, Preview);
        }

        public override string GetXml()
        {
            return GetXml(XmlKeywords.OptionRule, "", DefaultEnabled, -1, "", Type);
        }
    }
}