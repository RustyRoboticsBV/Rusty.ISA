using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A set of instruction definitions.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class InstructionSet : CutsceneResource
    {
        /* Public properties. */
        /// <summary>
        /// The instruction definitions in this instruction set.
        /// </summary>
        [Export] public InstructionDefinition[] Definitions { get; private set; } = new InstructionDefinition[0];

        /* Private properties. */
        private Dictionary<string, InstructionDefinition> Lookup { get; set; }

        /* Constructors. */
        public InstructionSet() { }

        public InstructionSet(InstructionDefinition[] definitions)
        {
            Definitions = definitions;
        }

        /* Index operators. */
        public InstructionDefinition this[string opcode] => GetDefinition(opcode);

        /* Public methods. */
        public override string ToString()
        {
            string str = "";
            foreach (InstructionDefinition definition in Definitions)
            {
                if (str != "")
                    str += "\n";
                str += definition;
            }
            return str;
        }

        /// <summary>
        /// Find an instruction definition by its opcode.
        /// </summary>
        public InstructionDefinition GetDefinition(string opcode)
        {
            EnsureLookup();
            try
            {
                return Lookup[opcode];
            }
            catch
            {
                throw new ArgumentException($"Tried to find instruction with opcode {opcode}, but this instruction did not "
                    + "exist in the instruction set!");
            }
        }

        /// <summary>
        /// Check if an instruction with some opcode exists in this instruction set.
        /// </summary>
        public bool HasDefinition(string opcode)
        {
            EnsureLookup();
            return Lookup.ContainsKey(opcode);
        }

        /* Private methods. */
        /// <summary>
        /// Make sure that the lookup table exists and is properly set up.
        /// </summary>
        private void EnsureLookup()
        {
            Lookup = new();
            foreach (InstructionDefinition definition in Definitions)
            {
                Lookup.Add(definition.Opcode, definition);
            }
        }
    }
}