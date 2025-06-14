using Godot;

namespace Rusty.ISA;

/// <summary>
/// A definition for a string instruction parameter that doesn't allow for line-breaks.
/// </summary>
[Tool, GlobalClass, XmlClass("textline")]
public sealed partial class TextlineParameter : Parameter
{
    /* Public properties. */
    [Export, XmlProperty("id")] public override string ID { get; protected set; } = "";
    [Export, XmlProperty("name")] public override string DisplayName { get; protected set; } = "";
    [Export(PropertyHint.MultilineText), XmlProperty("desc")] public override string Description { get; protected set; } = "";
    /// <summary>
    /// The default value of this parameter in the editor.
    /// </summary>
    [Export, XmlProperty("default")] public string DefaultValue { get; private set; } = "";
    [Export(PropertyHint.MultilineText), XmlProperty("preview")] public override string Preview { get; protected set; } = "";

    /* Constructors. */
    public TextlineParameter() : base() { }

    public TextlineParameter(string id, string displayName, string description, string defaultValue, string preview)
        : base(id, displayName, description, preview)
    {
        DefaultValue = defaultValue;
    }

    /* Public methods. */
    public override string ToString()
    {
        return $"{ID} (text)";
    }
}