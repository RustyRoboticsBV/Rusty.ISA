using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.ISA;

/// <summary>
/// A set of instruction definitions.
/// </summary>
[Tool, GlobalClass]
public sealed partial class InstructionSet : InstructionResource
{
    /* Public properties. */
    /// <summary>
    /// The name of this instruction set.
    /// </summary>
    [Export] public string Name { get; private set; } = "";
    /// <summary>
    /// The description of this instruction set.
    /// </summary>
    [Export(PropertyHint.MultilineText)] public string Description { get; private set; } = "";
    /// <summary>
    /// The author of this instruction set.
    /// </summary>
    [Export] public string Author { get; private set; } = "";
    /// <summary>
    /// The version of this instruction set.
    /// </summary>
    [Export] public string Version { get; private set; } = "1.0.0";
    /// <summary>
    /// The instruction definitions in local to this instruction set.
    /// </summary>
    [Export] public InstructionDefinition[] Local { get; private set; } = { };
    /// <summary>
    /// A list of modules of instructions that need to be included in this instruction set.
    /// </summary>
    [Export] public InstructionSet[] Modules { get; private set; } = { };

    /// <summary>
    /// The number of instruction definitions in this instruction set. This includes both local and module instructions.
    /// </summary>
    public int Count => Definitions.Length;
    /// <summary>
    /// All instructions in this instruction set, including both local and module instructions.
    /// </summary>
    public InstructionDefinition[] Definitions
    {
        get
        {
            EnsureDefinitions();
            return _Definitions;
        }
    }

    /* Private properties. */
    private InstructionDefinition[] _Definitions { get; set; }
    private Dictionary<string, InstructionDefinition> Lookup { get; set; }

    /* Constructors. */
    public InstructionSet() { }

    public InstructionSet(string name, string description, string author, string version,
        InstructionDefinition[] definitions, InstructionSet[] modules)
    {
        Name = name;
        Description = description;
        Author = author;
        Version = version;

        Local = definitions;
        Modules = modules;

        ResourceName = Name + $"[{Definitions.Length}]";
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
        string str = $"{Name} v{Version} by {Author}:\n\"{Description}\"";
        foreach (InstructionDefinition definition in Local)
        {
            str += "\n-" + definition;
        }
        foreach (InstructionSet module in Modules)
        {
            str += "\n+" + module.ToString().Replace("\n", "\n ");
        }
        return str;
    }

    /// <summary>
    /// Find an instruction definition by its opcode.
    /// </summary>
    public InstructionDefinition GetDefinition(string opcode)
    {
        // Remove tabs and line-breaks.
        opcode = FixOpcode(opcode);
        
        // Ensure that the look-up table exits.
        EnsureLookup();
        
        // Retrieve the instruction definition.
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
        // Remove tabs and line-breaks.
        opcode = FixOpcode(opcode);

        // Ensure that the look-up table exits.
        EnsureLookup();

        // Check if the definition exists.
        return Lookup.ContainsKey(opcode);
    }

    /* Private methods. */
    /// <summary>
    /// Make sure that the combined array of local and module instructions exists and is properly set up.
    /// </summary>
    private void EnsureDefinitions()
    {
        if (_Definitions != null)
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

        _Definitions = list.ToArray();
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
            string opcode = FixOpcode(definition.Opcode);
            if (!Lookup.ContainsKey(opcode))
                Lookup.Add(opcode, definition);
            else
            {
                GD.PrintErr($"Duplicate opcode '{opcode}' encountered in instruction set! This instruction will not be "
                    + "discoverable!");
            }
        }
    }
    
    /// <summary>
    /// Remove tabs and line-breaks from an opcode.
    /// </summary>
    private static string FixOpcode(string opcode)
    {
        return opcode.Replace("\n", "")
            .Replace("\r", "")
            .Replace("\t", "");
    }
}