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
        public List<CompileRuleDescriptor> Choices { get; set; } = new();
        public int DefaultSelected { get; set; }

        /* Constructors. */
        public ChoiceRuleDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public ChoiceRuleDescriptor(string id, string displayName, string description, List<CompileRuleDescriptor> choices,
            int defaultSelected, string preview) : base(id, displayName, description, preview)
        {
            if (choices == null)
                choices = new();
            Choices = choices;
            DefaultSelected = defaultSelected;
        }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public ChoiceRuleDescriptor(ChoiceRule rule) : base(rule)
        {
            foreach (CompileRule type in rule.Types)
            {
                Choices.Add(Create(type));
            }
            DefaultSelected = rule.DefaultSelected;
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
                        Choices.Add(Create(element));
                    else if (element.Name == XmlKeywords.DefaultSelected)
                        DefaultSelected = Parser.ParseInt(element.InnerText);
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
            foreach (CompileRuleDescriptor type in Choices)
            {
                types.Add(type.Generate());
            }
            return new ChoiceRule(ID, DisplayName, Description, types.ToArray(), DefaultSelected, Preview);
        }

        public override string GetXml()
        {
            return GetXml(XmlKeywords.ChoiceRule, "", false, DefaultSelected, "", Choices);
        }
    }
}