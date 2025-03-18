namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a float slider parameter. Used for serialization and deserialization.
    /// </summary>
    public class FloatSliderParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public float DefaultValue { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }

        /* Constructors. */
        public FloatSliderParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public FloatSliderParameterDescriptor(FloatSliderParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
            MinValue = parameter.MinValue;
            MaxValue = parameter.MaxValue;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override FloatSliderParameter Generate()
        {
            return new FloatSliderParameter(ID, DisplayName, Description, DefaultValue, MinValue, MaxValue);
        }

        public override string GetXml()
        {
            return GetXml("fslider", DefaultValue != 0f ? DefaultValue.ToString() : "",
                MinValue != 0f ? MinValue.ToString() : "", MaxValue != 0f ? MaxValue.ToString() : "");
        }
    }
}