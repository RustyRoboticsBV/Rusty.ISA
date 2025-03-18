using System.Collections.Generic;
using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a tuple rule. Used for serialization and deserialization.
    /// </summary>
    public class TupleRuleDescriptor : CompileRuleDescriptor
    {
        /* Public properties. */
        public List<CompileRuleDescriptor> Types { get; set; } = new();
        public string Separator { get; set; } = "";

        /* Constructors. */
        public TupleRuleDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public TupleRuleDescriptor(TupleRule rule) : base(rule)
        {
            foreach (CompileRule type in rule.Types)
            {
                Types.Add(Create(type));
            }
            Separator = rule.PreviewSeparator;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public TupleRuleDescriptor(XmlElement xml) : base(xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == "instruction" || element.Name == "tuple" || element.Name == "tuple"
                        || element.Name == "tuple" || element.Name == "list")
                    {
                        Types.Add(Create(element));
                    }
                    else if (element.Name == "separator")
                        Separator = element.InnerText;
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a rule from this descriptor.
        /// </summary>
        public override TupleRule Generate()
        {
            List<CompileRule> types = new();
            foreach (CompileRuleDescriptor type in Types)
            {
                types.Add(type.Generate());
            }
            return new TupleRule(ID, DisplayName, Description, types.ToArray(), Separator);
        }

        public override string GetXml()
        {
            return GetXml("tuple", "", false, -1, Separator, "", Types.ToArray());
        }
    }
}