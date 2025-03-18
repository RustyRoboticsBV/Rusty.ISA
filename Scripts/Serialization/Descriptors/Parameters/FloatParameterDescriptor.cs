namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a float parameter. Used for serialization and deserialization.
    /// </summary>
    public class FloatParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public float DefaultValue { get; set; }

        /* Constructors. */
        public FloatParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public FloatParameterDescriptor(FloatParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override FloatParameter Generate()
        {
            return new FloatParameter(ID, DisplayName, Description, DefaultValue);
        }

        public override string GetXml()
        {
            return GetXml("float", DefaultValue != 0f ? DefaultValue.ToString() : "");
        }
    }
}