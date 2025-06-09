using Godot;

namespace Rusty.ISA;

/// <summary>
/// A definition for an output instruction parameter.
/// </summary>
[Tool, GlobalClass, XmlClass("output")]
public sealed partial class OutputParameter : Parameter
{
    /* Public properties. */
    [Export, XmlProperty("id")] public override string ID { get; protected set; } = "";
    [Export, XmlProperty("name")] public override string DisplayName { get; protected set; } = "";
    [Export(PropertyHint.MultilineText), XmlProperty("desc")] public override string Description { get; protected set; } = "";
    /// <summary>
    /// If enabled, then any node that contains this parameter will not have a default output.
    /// </summary>
    [Export, XmlProperty("remove_default")] public bool RemoveDefaultOutput { get; private set; }
    [Export(PropertyHint.MultilineText), XmlProperty("preview")] public override string Preview { get; protected set; } = "";

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