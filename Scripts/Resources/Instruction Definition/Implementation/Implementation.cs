using Godot;

namespace Rusty.ISA;

/// <summary>
/// The implementation of an instruction.
/// </summary>
[Tool, GlobalClass, XmlClass("impl")]
public sealed partial class Implementation : InstructionResource
{
    /* Public properties. */
    /// <summary>
    /// The types that must exist for this instruction to function.
    /// </summary>
    [Export, XmlProperty("deps")] public Dependency[] Dependencies { get; private set; } = { };

    /// <summary>
    /// The GDScript code used for the instruction handler's member declarations.
    /// </summary>
    [Export(PropertyHint.MultilineText), XmlProperty("members")] public string Members { get; private set; } = "";
    /// <summary>
    /// The GDScript code used for the instruction handler's initialize function.
    /// </summary>
    [Export(PropertyHint.MultilineText), XmlProperty("init")] public string Initialize { get; private set; } = "";
    /// <summary>
    /// The GDScript code used for the instruction handler's execute function.
    /// </summary>
    [Export(PropertyHint.MultilineText), XmlProperty("exec")] public string Execute { get; private set; } = "";

    /* Constructors. */
    public Implementation() { }

    public Implementation(Dependency[] dependencies, string members, string initialize, string execute)
    {
        Dependencies = dependencies;
        Members = members;
        Initialize = initialize;
        Execute = execute;

        ResourceName = ToString();
    }

    /* Public methods. */
    public override string ToString()
    {
        return GetType().Name;
    }
}