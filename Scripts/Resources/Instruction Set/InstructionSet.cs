using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ISA
{
    /// <summary>
    /// A set of instruction definitions.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class InstructionSet : InstructionResource
    {
        /* Public properties. */
        /// <summary>
        /// The instruction definitions in local to this instruction set.
        /// </summary>
        [Export] public InstructionDefinition[] Definitions { get; private set; } = { };
        /// <summary>
        /// A list of modules of instructions that need to be included in this instruction set.
        /// </summary>
        [Export] public InstructionSet[] Modules { get; private set; } = { };
        /// <summary>
        /// The number of instruction definitions in this instruction set (if you combine all local instructions and the
        /// instructions from the referenced modules).
        /// </summary>
        public int Count
        {
            get
            {
                EnsureList();
                return List.Count;
            }
        }

        /* Private properties. */
        private List<InstructionDefinition> List { get; set; }
        private Dictionary<string, InstructionDefinition> Lookup { get; set; }

        /* Constructors. */
        public InstructionSet() { }

        public InstructionSet(InstructionDefinition[] definitions)
        {
            Definitions = definitions;
        }

        public InstructionSet(InstructionDefinition[] definitions, InstructionSet[] modules)
        {
            Definitions = definitions;
            Modules = modules;
        }

        /* Indexers. */
        public InstructionDefinition this[string opcode] => GetDefinition(opcode);
        public InstructionDefinition this[int index] => GetDefinition(index);

        /* Public methods. */
        public override string ToString()
        {
            EnsureList();

            string str = "";
            foreach (InstructionDefinition definition in List)
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

            if (Lookup.ContainsKey(opcode))
                return Lookup[opcode];
            else
            {
                throw new ArgumentException($"Tried to get instruction definition with opcode '{opcode}', but this instruction "
                    + "did not exist in the instruction set!");
            }
        }

        /// <summary>
        /// Find an instruction definition by its index.
        /// </summary>
        public InstructionDefinition GetDefinition(int index)
        {
            EnsureList();

            if (index >= 0 && index < List.Count)
                return List[index];
            else
            {
                throw new ArgumentException($"Tried to get instruction definition with index '{index}', but this index was out "
                    + "of bounds!");
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
        private void EnsureList()
        {
            if (List != null)
                return;

            List = new();
            foreach (InstructionSet module in Modules)
            {
                module.EnsureList();
                foreach (InstructionDefinition definition in module.List)
                {
                    List.Add(definition);
                }
            }

            foreach (InstructionDefinition definition in Definitions)
            {
                List.Add(definition);
            }
        }

        /// <summary>
        /// Make sure that the lookup table exists and is properly set up.
        /// </summary>
        private void EnsureLookup()
        {
            if (Lookup != null)
                return;

            EnsureList();

            Lookup = new();
            foreach (InstructionDefinition definition in List)
            {
                if (!Lookup.ContainsKey(definition.Opcode))
                    Lookup.Add(definition.Opcode, definition);
                else
                {
                    GD.PrintErr($"Duplicate opcode '{definition.Opcode}' encountered in instruction set! This instruction "
                        + "will not be discoverable!");
                }
            }
        }
    }
}