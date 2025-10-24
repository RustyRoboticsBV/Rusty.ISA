using Godot;

namespace Rusty.ISA;

/// <summary>
/// A definition for an integer instruction parameter.
/// </summary>
[Tool, GlobalClass, XmlClass("int")]
public sealed partial class IntParameter : Parameter
{
    /* Public properties. */
    [Export, XmlProperty("id")] public override string ID { get; protected set; } = "";
    [Export, XmlProperty("name")] public override string DisplayName { get; protected set; } = "";
    [Export(PropertyHint.MultilineText), XmlProperty("desc")] public override string Description { get; protected set; } = "";
    /// <summary>
    /// The default value of this parameter in the editor.
    /// </summary>
    [Export, XmlProperty("default")] public int DefaultValue { get; private set; }
    /// <summary>
    /// Whether or not this parameter can be localized to different languages.
    /// </summary>
    [Export, XmlProperty("localize")] public bool Localizable { get; private set; }
    [Export(PropertyHint.MultilineText), XmlProperty("preview")] public override string Preview { get; protected set; } = "";

    /* Constructors. */
    public IntParameter() : base() { }

    public IntParameter(string id, string displayName, string description, int defaultValue, bool localizable, string preview)
        : base(id, displayName, description, preview)
    {
        DefaultValue = defaultValue;
        Localizable = localizable;
    }

    /* Public methods. */
    public override string ToString()
    {
        return $"{ID} (int)";
    }
}