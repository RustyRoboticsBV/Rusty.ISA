namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a int slider parameter. Used for serialization and deserialization.
    /// </summary>
    public class IntSliderParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public int DefaultValue { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        /* Constructors. */
        public IntSliderParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public IntSliderParameterDescriptor(IntSliderParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
            MinValue = parameter.MinValue;
            MaxValue = parameter.MaxValue;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override IntSliderParameter Generate()
        {
            return new IntSliderParameter(ID, DisplayName, Description, DefaultValue, MinValue, MaxValue);
        }

        public override string GetXml()
        {
            return GetXml("islider", DefaultValue != 0 ? DefaultValue.ToString() : "",
                MinValue != 0 ? MinValue.ToString() : "", MaxValue != 0 ? MaxValue.ToString() : "");
        }
    }
}