using System.Collections.Generic;

namespace Rusty.ISA
{
    /// <summary>
    /// An instruction set descriptor. It's mostly the same as the instruction set, except its mutable and contains
    /// resource descriptors instead of resource references. Serves as an intermediary class during serialization and
    /// deserialization.
    /// </summary>
    public sealed class InstructionSetDescriptor
    {
        /* Public properties. */
        public List<InstructionDefinitionDescriptor> Definitions { get; } = new();
        public List<InstructionSetDescriptor> Modules { get; } = new();

        /* Constructors. */
        public InstructionSetDescriptor() { }

        /// <summary>
        /// Generate a set descriptor from an instruction definition.
        /// </summary>
        public InstructionSetDescriptor(InstructionSet set)
        {
            foreach (InstructionDefinition definition in set.Local)
            {
                Definitions.Add(new(definition));
            }
            foreach (InstructionSet module in set.Modules)
            {
                Modules.Add(new(module));
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate an instruction set from this descriptor.
        /// </summary>
        public InstructionSet Generate(bool makeIconsTransparent)
        {
            // Generate instruction definitions.
            List<InstructionDefinition> definitions = new();
            foreach (InstructionDefinitionDescriptor definition in Definitions)
            {
                definitions.Add(definition.Generate(makeIconsTransparent));
            }
            
            // Generate modules.
            List<InstructionSet> modules = new();
            foreach (InstructionSetDescriptor module in Modules)
            {
                modules.Add(module.Generate(makeIconsTransparent));
            }
            
            // Generate instruction set.
            return new(definitions.ToArray(), modules.ToArray());
        }
    }
}