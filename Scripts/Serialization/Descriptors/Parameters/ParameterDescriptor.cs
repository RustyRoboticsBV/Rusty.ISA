using System;
using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a parameter. Used for serialization and deserialization.
    /// </summary>
    public abstract class ParameterDescriptor
    {
        /* Public properties. */
        public string ID { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public string Preview { get; set; } = "";

        /* Constructors. */
        public ParameterDescriptor() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public ParameterDescriptor(string id, string displayName, string description, string preview)
        {
            ID = id;
            DisplayName = displayName;
            Description = description;
            Preview = preview;
        }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public ParameterDescriptor(Parameter parameter)
        {
            ID = parameter.ID;
            DisplayName = parameter.DisplayName;
            Description = parameter.Description;
            Preview = parameter.Preview;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public ParameterDescriptor(XmlElement xml)
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
                    else if (child.Name == XmlKeywords.Preview)
                        Preview = element.InnerText;
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public abstract Parameter Generate();

        /// <summary>
        /// Create a new parameter descriptor from a parameter of any type.
        /// </summary>
        public static ParameterDescriptor Create(Parameter parameter)
        {
            switch (parameter)
            {
                case BoolParameter @bool:
                    return new BoolParameterDescriptor(@bool);
                case IntParameter @int:
                    return new IntParameterDescriptor(@int);
                case IntSliderParameter islider:
                    return new IntSliderParameterDescriptor(islider);
                case FloatParameter @float:
                    return new FloatParameterDescriptor(@float);
                case FloatSliderParameter fslider:
                    return new FloatSliderParameterDescriptor(fslider);
                case CharParameter @char:
                    return new CharParameterDescriptor(@char);
                case TextlineParameter text:
                    return new TextlineParameterDescriptor(text);
                case MultilineParameter multiline:
                    return new MultilineParameterDescriptor(multiline);
                case ColorParameter color:
                    return new ColorParameterDescriptor(color);
                case OutputParameter output:
                    return new OutputParameterDescriptor(output);
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// Create a new parameter descriptor for a parameter of any type, from an XML element.
        /// </summary>
        public static ParameterDescriptor Create(XmlElement xml)
        {
            switch (xml.Name)
            {
                case XmlKeywords.BoolParameter:
                    return new BoolParameterDescriptor(xml);
                case XmlKeywords.IntParameter:
                    return new IntParameterDescriptor(xml);
                case XmlKeywords.IntSliderParameter:
                    return new IntSliderParameterDescriptor(xml);
                case XmlKeywords.FloatParameter:
                    return new FloatParameterDescriptor(xml);
                case XmlKeywords.FloatSliderParameter:
                    return new FloatSliderParameterDescriptor(xml);
                case XmlKeywords.CharParameter:
                    return new CharParameterDescriptor(xml);
                case XmlKeywords.TextlineParameter:
                    return new TextlineParameterDescriptor(xml);
                case XmlKeywords.MultilineParameter:
                    return new MultilineParameterDescriptor(xml);
                case XmlKeywords.ColorParameter:
                    return new ColorParameterDescriptor(xml);
                case XmlKeywords.OutputParameter:
                    return new OutputParameterDescriptor(xml);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Convert to XML.
        /// </summary>
        public abstract string GetXml();

        /* Protected methods. */
        protected string GetXml(string type, string defaultValue = "", string minValue = "", string maxValue = "",
            bool removeDefaultOutput = false, string preview = "")
        {
            string str = $"<{type} {XmlKeywords.ID}=\"{ID}\">";
            if (DisplayName != "")
                str += $"\n  <{XmlKeywords.DisplayName}>{DisplayName}</{XmlKeywords.DisplayName}>";
            if (Description != "")
                str += $"\n  <{XmlKeywords.Description}>{Description}</{XmlKeywords.Description}>";
            if (defaultValue != "")
                str += $"\n  <{XmlKeywords.DefaultValue}>{defaultValue}</{XmlKeywords.DefaultValue}>";
            if (minValue != "")
                str += $"\n  <{XmlKeywords.MinValue}>{minValue}</{XmlKeywords.MinValue}>";
            if (maxValue != "")
                str += $"\n  <{XmlKeywords.MaxValue}>{maxValue}</{XmlKeywords.MaxValue}>";
            if (removeDefaultOutput)
                str += $"\n  <{XmlKeywords.RemoveDefaultOutput}/>";
            if (preview != "")
                str += $"\n  <{XmlKeywords.Preview}>{preview}</{XmlKeywords.Preview}>";
            str += $"\n</{type}>";
            return str;
        }
    }
}