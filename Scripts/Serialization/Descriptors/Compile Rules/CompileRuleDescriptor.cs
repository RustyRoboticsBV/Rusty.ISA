using System;
using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a compile rule. Used for serialization and deserialization.
    /// </summary>
    public abstract class CompileRuleDescriptor
    {
        /* Public properties. */
        public string ID { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public string Preview { get; set; } = "";

        /* Constructors. */
        public CompileRuleDescriptor() { }

        /// <summary>
        /// Generate a descriptor for a compile rule.
        /// </summary>
        public CompileRuleDescriptor(CompileRule rule)
        {
            ID = rule.ID;
            DisplayName = rule.DisplayName;
            Description = rule.Description;
            Preview = rule.Preview;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public CompileRuleDescriptor(XmlElement xml)
        {
            ID = xml.GetAttribute(XmlKeywords.ID);
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == XmlKeywords.DisplayName)
                        DisplayName = element.InnerText;
                    else if (element.Name == XmlKeywords.Description)
                        Description = element.InnerText;
                    else if (element.Name == XmlKeywords.Preview)
                        Preview = element.InnerText;
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a compile rule from this descriptor.
        /// </summary>
        public abstract CompileRule Generate();

        /// <summary>
        /// Create a new descriptor from a compile rule of any type.
        /// </summary>
        public static CompileRuleDescriptor Create(CompileRule rule)
        {
            switch (rule)
            {
                case InstructionRule instruction:
                    return new InstructionRuleDescriptor(instruction);
                case OptionRule option:
                    return new OptionRuleDescriptor(option);
                case ChoiceRule choice:
                    return new ChoiceRuleDescriptor(choice);
                case TupleRule tuple:
                    return new TupleRuleDescriptor(tuple);
                case ListRule list:
                    return new ListRuleDescriptor(list);
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// Create a new  descriptor for a compile rule of any type, from an XML element.
        /// </summary>
        public static CompileRuleDescriptor Create(XmlElement xml)
        {
            switch (xml.Name)
            {
                case XmlKeywords.InstructionRule:
                    return new InstructionRuleDescriptor(xml);
                case XmlKeywords.OptionRule:
                    return new OptionRuleDescriptor(xml);
                case XmlKeywords.ChoiceRule:
                    return new ChoiceRuleDescriptor(xml);
                case XmlKeywords.TupleRule:
                    return new TupleRuleDescriptor(xml);
                case XmlKeywords.ListRule:
                    return new ListRuleDescriptor(xml);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Convert to XML.
        /// </summary>
        public abstract string GetXml();

        /* Protected methods. */
        protected string GetXml(string type, string opcode, bool startEnabled, int startSelected, string separator, string addButtonText,
            params CompileRuleDescriptor[] children)
        {
            string str = $"<{type} id=\"{ID}\">";
            if (opcode != "")
                str += $"\n  <{XmlKeywords.Opcode}>{opcode}</{XmlKeywords.Opcode}>";
            if (DisplayName != "")
                str += $"\n  <{XmlKeywords.DisplayName}>{DisplayName}</{XmlKeywords.DisplayName}>";
            if (Description != "")
                str += $"\n  <{XmlKeywords.Description}>{Description}</{XmlKeywords.Description}>";
            if (startEnabled)
                str += $"\n  <{XmlKeywords.DefaultEnabled}>true</{XmlKeywords.DefaultEnabled}>";
            if (startSelected > 0)
                str += $"\n  <{XmlKeywords.DefaultSelected}>{startSelected}</{XmlKeywords.DefaultSelected}>";
            if (addButtonText != "")
                str += $"\n  <{XmlKeywords.AddButtonText}>{addButtonText}</{XmlKeywords.AddButtonText}>";
            foreach (CompileRuleDescriptor child in children)
            {
                str += "\n  " + child.GetXml().Replace("\n", "\n  ");
            }
            str += $"\n</{type}>";
            return str;
        }
    }
}