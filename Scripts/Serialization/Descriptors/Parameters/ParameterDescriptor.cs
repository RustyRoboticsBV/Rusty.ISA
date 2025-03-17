using System;
using System.Xml.Serialization;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a parameter. Used for serialization and deserialization.
    /// </summary>
    public abstract class ParameterDescriptor
    {
        /* Public properties. */
        [XmlAttribute("id")]
        public string ID { get; set; } = "";
        [XmlElement("name")]
        public string DisplayName { get; set; } = "";
        [XmlElement("desc")]
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
                case TextParameter text:
                    return new TextParameterDescriptor(text);
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
    }
}