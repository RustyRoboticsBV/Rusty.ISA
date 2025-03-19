using System;
using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a preview term. Used for serialization and deserialization.
    /// </summary>
    public class PreviewTermDescriptor
    {
        /* Public properties. */
        public string Type { get; set; } = "";
        public string Value { get; set; } = "";
        public HideIf HideIf { get; set; } = HideIf.Never;

        /* Constructors. */
        public PreviewTermDescriptor() { }

        public PreviewTermDescriptor(string type, string value, HideIf hideIf)
        {
            Type = type;
            Value = value;
            HideIf = hideIf;
        }

        /// <summary>
        /// Generate a descriptor for a preview term.
        /// </summary>
        public PreviewTermDescriptor(PreviewTerm term)
        {
            HideIf = term.HideIf;
            switch (term)
            {
                case TextTerm text:
                    Type = "text";
                    Value = text.Text;
                    break;
                case ArgumentTerm argument:
                    Type = "arg";
                    Value = argument.ParameterID;
                    break;
                case CompileRuleTerm rule:
                    Type = "rule";
                    Value = rule.RuleID;
                    break;
            }
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public PreviewTermDescriptor(XmlElement xml) { }

        /* Public methods. */
        /// <summary>
        /// Generate a preview term from this descriptor.
        /// </summary>
        public PreviewTerm Generate()
        {
            switch (Type)
            {
                case "text":
                    return new TextTerm(HideIf, Value);
                case "arg":
                    return new ArgumentTerm(HideIf, Value);
                case "rule":
                    return new CompileRuleTerm(HideIf, Value);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Convert to XML.
        /// </summary>
        public string GetXml()
        {
            string hideIf = "";
            if (HideIf == HideIf.PrevIsEmpty)
                hideIf = "prev";
            else if (HideIf == HideIf.NextIsEmpty)
                hideIf = "next";
            else if (HideIf == HideIf.BothAreEmpty)
                hideIf = "both";
            else if (HideIf == HideIf.EitherIsEmpty)
                hideIf = "either";

            string str = $"<{Type}_term>";
            if (Type == "text")
                str += $"\n  <text>{Value}</text>";
            else if (Type == "arg")
                str += $"\n  <id>{Value}</id>";
            else if (Type == "rule")
                str += $"\n  <id>{Value}</id>";
            if (hideIf != "")
                str += $"\n  <hideif>{hideIf}</hideif>";
            str += $"\n</{Type}_term>";
            return str;
        }

        /* Protected methods. */
        protected string GetXml(string type, string value)
        {
            string str = $"<{type}>";
            str += $"\n  <{value}>{Value}</{value}>";
            str += $"\n</{type}>";
            return str;
        }
    }
}