namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a text parameter. Used for serialization and deserialization.
    /// </summary>
    public class TextParameterDescriptor : ParameterDescriptor
    {
        /* Public properties. */
        public string DefaultValue { get; set; }

        /* Constructors. */
        public TextParameterDescriptor() : base() { }

        /// <summary>
        /// Generate a descriptor for a parameter.
        /// </summary>
        public TextParameterDescriptor(TextParameter parameter) : base(parameter)
        {
            DefaultValue = parameter.DefaultValue;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a parameter from this descriptor.
        /// </summary>
        public override TextParameter Generate()
        {
            return new TextParameter(ID, DisplayName, Description, DefaultValue);
        }

        public override string GetXml()
        {
            return GetXml("text", DefaultValue);
        }
    }
}