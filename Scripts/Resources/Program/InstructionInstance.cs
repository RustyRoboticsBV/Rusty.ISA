using Godot;
using System;

namespace Rusty.ISA;

/// <summary>
/// An instance of an instruction.
/// </summary>
[Tool, GlobalClass]
public sealed partial class InstructionInstance : InstructionResource
{
    /* Public properties. */
    /// <summary>
    /// The opcode of the associated instruction definition.
    /// </summary>
    [Export] public string Opcode { get; private set; }
    /// <summary>
    /// The arguments that will be passed to the execution handler.
    /// </summary>
    [Export] public string[] Arguments { get; private set; } = new string[0];

    /* Constructors. */
    public InstructionInstance() : this("", 0) { }

    public InstructionInstance(InstructionDefinition definition)
        : this(definition.Opcode, definition.Parameters.Length) { }

    public InstructionInstance(string opcode, int argumentCount)
        : this(opcode, new string[argumentCount]) { }

    public InstructionInstance(string opcode, string[] arguments)
    {
        Opcode = opcode;
        Arguments = new string[arguments.Length];
        Array.Copy(arguments, Arguments, Arguments.Length);
    }

    public InstructionInstance(InstructionInstance other)
        : this(other.Opcode, other.Arguments) { }

    /* Public methods. */
    public override string ToString()
    {
        string str = Opcode;

        // Add arguments.
        str += "(";
        for (int i = 0; i < Arguments.Length; i++)
        {
            if (i > 0)
                str += ", ";
            str += $"\"{Arguments[i]}\"";
        }
        str += ")";

        return str;
    }
}