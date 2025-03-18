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

        /* Constructors. */
        public ParameterDescriptor() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public ParameterDescriptor(Parameter parameter)
        {
            ID = parameter.ID;
            DisplayName = parameter.DisplayName;
            Description = parameter.Description;
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public ParameterDescriptor(XmlElement xml)
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
                case "bool":
                    return new BoolParameterDescriptor(xml);
                case "int":
                    return new IntParameterDescriptor(xml);
                case "islider":
                    return new IntSliderParameterDescriptor(xml);
                case "float":
                    return new FloatParameterDescriptor(xml);
                case "fslider":
                    return new FloatSliderParameterDescriptor(xml);
                case "char":
                    return new CharParameterDescriptor(xml);
                case "textline":
                    return new TextlineParameterDescriptor(xml);
                case "multiline":
                    return new MultilineParameterDescriptor(xml);
                case "color":
                    return new ColorParameterDescriptor(xml);
                case "output":
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
            bool removeDefaultOutput = false, string previewArgument = "")
        {
            string str = $"<{type} id=\"{ID}\">";
            if (DisplayName != "")
                str += $"\n  <name>{DisplayName}</name>";
            if (Description != "")
                str += $"\n  <desc>{Description}</desc>";
            if (defaultValue != "")
                str += $"\n  <default>{defaultValue}</default>";
            if (minValue != "")
                str += $"\n  <min>{minValue}</min>";
            if (maxValue != "")
                str += $"\n  <max>{maxValue}</max>";
            if (removeDefaultOutput)
                str += $"\n  <remove_default/>";
            if (previewArgument != "")
                str += $"\n  <preview_arg>{previewArgument}</preview_arg>";
            str += $"\n</{type}>";
            return str;
        }
    }
}