using Godot;

namespace Rusty.ISA;

/// <summary>
/// A definition for an integer slider instruction parameter.
/// </summary>
[Tool]
[GlobalClass]
public sealed partial class IntSliderParameter : Parameter
{
    /* Public properties. */
    [Export] public override string ID { get; protected set; } = "";
    [Export] public override string DisplayName { get; protected set; } = "";
    [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";
    /// <summary>
    /// The default value of this parameter in the editor.
    /// </summary>
    [Export] public int DefaultValue { get; private set; }
    /// <summary>
    /// The minimum value of this parameter in the editor.
    /// </summary>
    [Export] public int MinValue { get; private set; }
    /// <summary>
    /// The maximum value of this parameter in the editor.
    /// </summary>
    [Export] public int MaxValue { get; private set; } = 100;
    [Export(PropertyHint.MultilineText)] public override string Preview { get; protected set; } = "";

    /* Constructors. */
    public IntSliderParameter() : base() { }

    public IntSliderParameter(string id, string displayName, string description, int defaultValue, int minValue,
        int maxValue, string preview) : base(id, displayName, description, preview)
    {
        DefaultValue = defaultValue;
        MinValue = minValue;
        MaxValue = maxValue;
    }

    /* Public methods. */
    public override string ToString()
    {
        return $"{ID} (islider)";
    }
}