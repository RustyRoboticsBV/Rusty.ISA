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
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public CompileRuleDescriptor(XmlElement xml)
        {
            ID = xml.GetAttribute("id");
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == "name")
                        DisplayName = element.InnerText;
                    else if (element.Name == "desc")
                        Description = element.InnerText;
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
                case "instruction":
                    return new InstructionRuleDescriptor(xml);
                case "option":
                    return new OptionRuleDescriptor(xml);
                case "choice":
                    return new ChoiceRuleDescriptor(xml);
                case "tuple":
                    return new TupleRuleDescriptor(xml);
                case "list":
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
                str += $"\n  <opcode>{opcode}</opcode>";
            if (DisplayName != "")
                str += $"\n  <name>{DisplayName}</name>";
            if (Description != "")
                str += $"\n  <desc>{Description}</desc>";
            if (startEnabled)
                str += $"\n  <enabled>true</enabled>";
            if (startSelected > 0)
                str += $"\n  <selected>{startSelected}</selected>";
            if (separator != "")
                str += $"\n  <separator>{separator}</separator>";
            if (addButtonText != "")
                str += $"\n  <button_text>{addButtonText}</button_text>";
            foreach (CompileRuleDescriptor child in children)
            {
                str += "\n  " + child.GetXml().Replace("\n", "\n  ");
            }
            str += $"\n</{type}>";
            return str;
        }
    }
}