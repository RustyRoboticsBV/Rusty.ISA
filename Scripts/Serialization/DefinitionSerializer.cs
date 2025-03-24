namespace Rusty.ISA
{
    /// <summary>
    /// An utility class that can serialize instruction definitions.
    /// </summary>
    public static class DefinitionSerializer
    {
        /* Public methods. */
        /// <summary>
        /// Convert an instruction definition into an XML string.
        /// </summary>
        public static string Serialize(InstructionDefinition definition)
        {
            InstructionDefinitionDescriptor descriptor = new(definition);
            return Serialize(descriptor);
        }

        /// <summary>
        /// Convert an instruction definition descriptor into an XML string.
        /// </summary>
        public static string Serialize(InstructionDefinitionDescriptor descriptor)
        {
            return descriptor.GetXml();
        }
    }
}
