using Godot;

namespace Rusty.ISA;

/// <summary>
/// A definition for an output instruction parameter.
/// </summary>
[Tool]
[GlobalClass]
public sealed partial class OutputParameter : Parameter
{
    /* Public properties. */
    [Export] public override string ID { get; protected set; } = "";
    [Export] public override string DisplayName { get; protected set; } = "";
    [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";
    /// <summary>
    /// If enabled, then any node that contains this parameter will not have a default output.
    /// </summary>
    [Export] public bool RemoveDefaultOutput { get; private set; }
    /// <summary>
    /// An expression that defines how previews will be generated for this output. If left empty, this will result in
    /// the empty string being printed.
    /// </summary>
    [Export(PropertyHint.MultilineText)] public override string Preview { get; protected set; } = "";

    /* Constructors. */
    public OutputParameter() : base() { }

    public OutputParameter(string id, string displayName, string description, bool removeDefaultOutput, string preview)
        : base(id, displayName, description, preview)
    {
        RemoveDefaultOutput = removeDefaultOutput;
    }

    /* Public methods. */
    public override string ToString()
    {
        return $"{ID} (output)";
    }
}