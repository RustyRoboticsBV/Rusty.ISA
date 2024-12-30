using Godot;
using System.Collections.Generic;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A cutsscene program that can be executed by a CutscenePlayer node.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class CutsceneProgram : CutsceneResource
    {
        /* Public properties. */
        [Export] public InstructionInstance[] Instructions { get; private set; } = new InstructionInstance[0];

        public int Length => Instructions.Length;

        /* Constructors. */
        public CutsceneProgram() : this(new InstructionInstance[0]) { }

        public CutsceneProgram(InstructionInstance[] instructions)
        {
            Instructions = instructions;

            foreach (InstructionInstance instruction in Instructions)
            {
                NameInstruction(instruction);
            }
        }

        public CutsceneProgram(CutsceneProgram other)
        {
            // Copy all instructions.
            Instructions = new InstructionInstance[other.Instructions.Length];
            for (int i = 0; i < Instructions.Length; i++)
            {
                Instructions[i] = new InstructionInstance(other.Instructions[i]);
            }

            // Name instructions.
            foreach (InstructionInstance instruction in Instructions)
            {
                NameInstruction(instruction);
            }
        }

        /* Indexers. */
        public InstructionInstance this[int index] => Instructions[index];

        /* Private methods. */
        /// <summary>
        /// Sets the resource name of an instruction.
        /// </summary>
        private static void NameInstruction(InstructionInstance instruction)
        {
            instruction.ResourceName = instruction.ToString();
        }
    }
}