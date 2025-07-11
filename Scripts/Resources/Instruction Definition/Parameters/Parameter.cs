using Godot;

namespace Rusty.ISA;

/// <summary>
/// A definition for an instruction parameter.
/// </summary>
[GlobalClass]
public abstract partial class Parameter : InstructionResource
{
    /* Public methods. */
    /// <summary>
    /// The internal identifier of this parameter, which can be used to get the value of this parameter in an instruction
    /// instance. Should be unique from other parameters that belong to the same instruction, and should preferably be as
    /// short as possible.
    /// </summary>
    public abstract string ID { get; protected set; }
    /// <summary>
    /// The human-readable name of this parameter. Used in the graph editor.
    /// </summary>
    public abstract string DisplayName { get; protected set; }
    /// <summary>
    /// The description of this parameter. Used for editor tooltips and documentation generation.
    /// </summary>
    public abstract string Description { get; protected set; }
    /// <summary>
    /// An expression that defines how previews will be generated for this parameter. If left empty, this will result in
    /// the corresponding argument value being printed.
    /// </summary>
    public abstract string Preview { get; protected set; }

    /* Constructors. */
    public Parameter() : base() { }

    public Parameter(string id, string displayName, string description, string preview) : base()
    {
        ID = id;
        DisplayName = displayName;
        Description = description;
        Preview = preview;

        ResourceName = ToString();
    }
}