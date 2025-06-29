using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ISA;

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
    [Export] public InstructionDefinition[] Local { get; private set; } = { };
    /// <summary>
    /// A list of modules of instructions that need to be included in this instruction set.
    /// </summary>
    [Export] public InstructionSet[] Modules { get; private set; } = { };

    /// <summary>
    /// All instructions in this instruction set, including both local and module instructions.
    /// </summary>
    public InstructionDefinition[] Definitions
    {
        get
        {
            EnsureBuild();
            return Build;
        }
    }
    /// <summary>
    /// The number of instruction definitions in this instruction set. This includes both local and module instructions.
    /// </summary>
    public int Count => Definitions.Length;

    /* Private properties. */
    private InstructionDefinition[] Build { get; set; }
    private Dictionary<string, InstructionDefinition> Lookup { get; set; }

    /* Constructors. */
    public InstructionSet() { }

    public InstructionSet(InstructionDefinition[] definitions)
    {
        Local = definitions;
    }

    public InstructionSet(InstructionDefinition[] definitions, InstructionSet[] modules)
    {
        Local = definitions;
        Modules = modules;
    }

    /* Indexers. */
    /// <summary>
    /// Access an instruction definition by its opcode.
    /// </summary>
    public InstructionDefinition this[string opcode] => GetDefinition(opcode);
    /// <summary>
    /// Access an instruction definition by its index in the instruction set.
    /// </summary>
    public InstructionDefinition this[int index] => GetDefinition(index);

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
        if (index >= 0 && index < Count)
            return Definitions[index];
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
    /// <summary>
    /// Make sure that the combined array of local and module instructions exists and is properly set up.
    /// </summary>
    private void EnsureBuild()
    {
        if (Build != null)
            return;

        List<InstructionDefinition> list = new();

        // Add module instructions.
        foreach (InstructionSet module in Modules)
        {
            foreach (InstructionDefinition definition in module.Definitions)
            {
                list.Add(definition);
            }
        }

        // Add local instructions.
        foreach (InstructionDefinition definition in Local)
        {
            list.Add(definition);
        }

        Build = list.ToArray();
    }

    /// <summary>
    /// Make sure that the lookup table exists and is properly set up.
    /// </summary>
    private void EnsureLookup()
    {
        if (Lookup != null)
            return;

        Lookup = new();
        foreach (InstructionDefinition definition in Definitions)
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