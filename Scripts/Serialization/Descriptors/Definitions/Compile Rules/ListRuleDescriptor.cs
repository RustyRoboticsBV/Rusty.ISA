using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a list rule. Used for serialization and deserialization.
    /// </summary>
    public class ListRuleDescriptor : CompileRuleDescriptor
    {
        /* Public properties. */
        public CompileRuleDescriptor Type { get; set; }
        public string AddButtonText { get; set; } = "";

        /* Constructors. */
        public ListRuleDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public ListRuleDescriptor(string id, string displayName, string description, CompileRuleDescriptor type,
            string addButtonText, string preview) : base(id, displayName, description, preview)
        {
            Type = type;
            AddButtonText = addButtonText;
        }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public ListRuleDescriptor(ListRule rule) : base(rule)
        {
            Type = Create(rule.Type);
            AddButtonText = rule.AddButtonText;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public ListRuleDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (XmlKeywords.CompileRules.Contains(element.Name))
                        Type = Create(element);
                    else if (element.Name == XmlKeywords.AddButtonText)
                        AddButtonText = element.InnerText;
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a rule from this descriptor.
        /// </summary>
        public override ListRule Generate()
        {
            return new ListRule(ID, DisplayName, Description, Type.Generate(), AddButtonText, Preview);
        }

        public override string GetXml()
        {
            return GetXml(XmlKeywords.ListRule, "", false, -1, AddButtonText, Type);
        }
    }
}