using System.IO;
using System.Xml.Serialization;

namespace Rusty.ISA
{
    /// <summary>
    /// An utility class that can serialize instruction definitions.
    /// </summary>
    public class DefinitionSerializer
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
            XmlSerializer serializer = new(typeof(InstructionDefinitionDescriptor));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, descriptor);
            writer.Close();
            return writer.ToString();
        }
    }
}
