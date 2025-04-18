﻿using System.Collections.Generic;
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

        /* Constructors. */
        public TupleRuleDescriptor() : base() { }

        public TupleRuleDescriptor(string id, string displayName, string description, List<CompileRuleDescriptor> types,
            string preview) : base(id, displayName, description, preview)
        {
            if (types == null)
                types = new();
            Types = types;
        }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public TupleRuleDescriptor(TupleRule rule) : base(rule)
        {
            foreach (CompileRule type in rule.Types)
            {
                Types.Add(Create(type));
            }
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
                    if (XmlKeywords.CompileRules.Contains(element.Name))
                        Types.Add(Create(element));
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
            return new TupleRule(ID, DisplayName, Description, types.ToArray(), Preview);
        }

        public override string GetXml()
        {
            return GetXml(XmlKeywords.TupleRule, "", false, -1, "", Types);
        }
    }
}