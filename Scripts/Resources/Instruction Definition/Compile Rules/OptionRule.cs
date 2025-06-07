using Godot;

namespace Rusty.ISA;

/// <summary>
/// An optional compile rule, that can be toggled on or off.
/// </summary>
[Tool]
[GlobalClass]
public sealed partial class OptionRule : CompileRule
{
    /* Public properties. */
    [Export] public override string ID { get; protected set; } = "";
    [Export] public override string DisplayName { get; protected set; } = "";
    [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";
    /// <summary>
    /// The thing that can be toggled on and off. Can be an instruction rule or another container rule.
    /// </summary>
    [Export] public CompileRule Type { get; private set; }
    /// <summary>
    /// Whether the toggle is enabled by default or not.
    /// </summary>
    [Export] public bool DefaultEnabled { get; private set; }
    /// <summary>
    /// An expression that defines how previews will be generated for this rule. If left empty, then the preview of the
    /// target rule is generated if the option is enabled, and the empty string is generated if the option is disabled.
    /// </summary>
    [Export(PropertyHint.MultilineText)] public override string Preview { get; protected set; } = "";

    /* Constructors. */
    public OptionRule() : base() { }

    public OptionRule(string id, string displayName, string description, CompileRule target, bool defaultEnabled,
        string preview) : base(id, displayName, description, preview)
    {
        Type = target;
        DefaultEnabled = defaultEnabled;

        ResourceName = ToString();
    }

    /* Public methods. */
    public override string ToString()
    {
        string str = "";
        if (Type != null)
            str = Type.ToString();
        return $"Option({str})";
    }
}