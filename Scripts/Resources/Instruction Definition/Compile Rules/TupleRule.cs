using Godot;

namespace Rusty.ISA;

/// <summary>
/// A tuple of compile rules.
/// </summary>
[Tool, GlobalClass, XmlClass("tuple")]
public sealed partial class TupleRule : CompileRule
{
    /* Public properties. */
    [Export, XmlProperty("id")] public override string ID { get; protected set; } = "";
    [Export, XmlProperty("name")] public override string DisplayName { get; protected set; } = "";
    [Export(PropertyHint.MultilineText), XmlProperty("desc")] public override string Description { get; protected set; } = "";
    /// <summary>
    /// The items contained within this tuple. Can include both instruction rules and container rules.
    /// </summary>
    [Export, XmlProperty("types")] public CompileRule[] Types { get; private set; } = [];
    /// <summary>
    /// An expression that defines how previews will be generated for this rule. If left empty, then the previews of all
    /// elements are generated, separated by spaces.
    /// </summary>
    [Export(PropertyHint.MultilineText), XmlProperty("preview")] public override string Preview { get; protected set; } = "";

    /* Constructors. */
    public TupleRule() : base() { }

    public TupleRule(string id, string displayName, string description, CompileRule[] types, string preview)
        : base(id, displayName, description, preview)
    {
        if (types == null)
            types = [];

        Types = types;

        ResourceName = ToString();
    }

    /* Public methods. */
    public override string ToString()
    {
        string str = "";
        foreach (CompileRule type in Types)
        {
            if (str != "")
                str += ", ";
            if (type != null)
                str += type.ToString();
        }
        return $"Tuple({str})";
    }
}