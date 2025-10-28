using Godot;

namespace Rusty.ISA;

/// <summary>
/// A program that can be executed by a process node.
/// </summary>
[Tool, GlobalClass]
public sealed partial class Program : InstructionResource
{
    /* Public properties. */
    /// <summary>
    /// The name of this program.
    /// </summary>
    [Export] public string Name { get; private set; }
    /// <summary>
    /// The instructions in this program.
    /// </summary>
    [Export] public InstructionInstance[] Instructions { get; private set; } = [];

    /// <summary>
    /// The number of instructions in this program.
    /// </summary>
    public int Length => Instructions.Length;

    /* Indexers. */
    /// <summary>
    /// Get an instruction, using its index.
    /// </summary>
    public InstructionInstance this[int index] => Instructions[index];

    /* Constructors. */
    public Program() : this("", []) { }

    public Program(InstructionInstance[] instructions) : this("", instructions) { }

    public Program(string name, InstructionInstance[] instructions)
    {
        Name = name;
        Instructions = instructions;

        foreach (InstructionInstance instruction in Instructions)
        {
            NameInstruction(instruction);
        }
    }

    public Program(Program other)
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

    /* Public methods. */
    public override string ToString()
    {
        if (Instructions.Length == 0)
            return "";
        string str = Instructions[0].ToString();
        for (int i = 1; i < Instructions.Length; i++)
        {
            str += $"\n{Instructions[i]}";
        }
        return str;
    }

    /* Private methods. */
    /// <summary>
    /// Sets the resource name of an instruction.
    /// </summary>
    private static void NameInstruction(InstructionInstance instruction)
    {
        instruction.ResourceName = instruction.ToString();
    }
}