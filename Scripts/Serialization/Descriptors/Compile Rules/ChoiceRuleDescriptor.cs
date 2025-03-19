using System.Collections.Generic;
using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a choice rule. Used for serialization and deserialization.
    /// </summary>
    public class ChoiceRuleDescriptor : CompileRuleDescriptor
    {
        /* Public properties. */
        public List<CompileRuleDescriptor> Types { get; set; } = new();
        public int StartSelected { get; set; }

        /* Constructors. */
        public ChoiceRuleDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public ChoiceRuleDescriptor(ChoiceRule rule) : base(rule)
        {
            foreach (CompileRule type in rule.Types)
            {
                Types.Add(Create(type));
            }
            StartSelected = rule.DefaultSelected;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public ChoiceRuleDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (XmlKeywords.CompileRules.Contains(element.Name))
                        Types.Add(Create(element));
                    else if (element.Name == XmlKeywords.DefaultSelected)
                        StartSelected = Parser.ParseInt(element.InnerText);
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a rule from this descriptor.
        /// </summary>
        public override ChoiceRule Generate()
        {
            List<CompileRule> types = new();
            foreach (CompileRuleDescriptor type in Types)
            {
                types.Add(type.Generate());
            }
            return new ChoiceRule(ID, DisplayName, Description, types.ToArray(), StartSelected, Preview);
        }

        public override string GetXml()
        {
            return GetXml(XmlKeywords.ChoiceRule, "", false, StartSelected, "", "", Types.ToArray());
        }
    }
}