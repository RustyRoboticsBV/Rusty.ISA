using Godot;

namespace Rusty.ISA;

/// <summary>
/// A definition for a floating-point slider instruction parameter.
/// </summary>
[Tool, GlobalClass, XmlClass("fslider")]
public sealed partial class FloatSliderParameter : Parameter
{
    /* Public properties. */
    [Export, XmlProperty("id")] public override string ID { get; protected set; } = "";
    [Export, XmlProperty("name")] public override string DisplayName { get; protected set; } = "";
    [Export(PropertyHint.MultilineText), XmlProperty("desc")] public override string Description { get; protected set; } = "";
    /// <summary>
    /// The default value of this parameter in the editor.
    /// </summary>
    [Export, XmlProperty("default")] public float DefaultValue { get; private set; }
    /// <summary>
    /// The minimum value of this parameter in the editor.
    /// </summary>
    [Export, XmlProperty("min")] public float MinValue { get; private set; }
    /// <summary>
    /// The maximum value of this parameter in the editor.
    /// </summary>
    [Export, XmlProperty("max")] public float MaxValue { get; private set; } = 100f;
    /// <summary>
    /// Whether or not this parameter can be localized to different languages.
    /// </summary>
    [Export, XmlProperty("localize")] public bool Localizable { get; private set; }
    [Export(PropertyHint.MultilineText), XmlProperty("preview")] public override string Preview { get; protected set; } = "";

    /* Constructors. */
    public FloatSliderParameter() : base() { }

    public FloatSliderParameter(string id, string displayName, string description, float defaultValue, float minValue
        , float maxValue, bool localizable, string preview) : base(id, displayName, description, preview)
    {
        DefaultValue = defaultValue;
        MinValue = minValue;
        MaxValue = maxValue;
        Localizable = localizable;
    }

    /* Public methods. */
    public override string ToString()
    {
        return $"{ID} (fslider)";
    }
}