using Godot;

namespace Rusty.ISA;

/// <summary>
/// A list of compile rules, where each entry is of the same type.
/// </summary>
[Tool, GlobalClass, XmlClass("list")]
public sealed partial class ListRule : CompileRule
{
    /* Public properties. */
    [Export, XmlProperty("id")] public override string ID { get; protected set; } = "";
    [Export, XmlProperty("name")] public override string DisplayName { get; protected set; } = "";
    [Export(PropertyHint.MultilineText), XmlProperty("desc")] public override string Description { get; protected set; } = "";
    /// <summary>
    /// The type of element that is contained in this list. Can be an instruction rule or another container rule.
    /// </summary>
    [Export, XmlProperty("type")] public CompileRule Type { get; private set; }
    /// <summary>
    /// The text displayed on the "add item" button in the inspector.
    /// </summary>
    [Export, XmlProperty("button_text")] public string AddButtonText { get; private set; } = "Add Item";
    /// <summary>
    /// An expression that defines how previews will be generated for this rule. If left empty, then the previews of all
    /// elements are generated, separated by line-breaks.
    /// </summary>
    [Export(PropertyHint.MultilineText), XmlProperty("preview")] public override string Preview { get; protected set; } = "";

    /* Constructors. */
    public ListRule() : base() { }

    public ListRule(string id, string displayName, string description, CompileRule type, string addButtonText, string preview)
        : base(id, displayName, description, preview)
    {
        Type = type;
        AddButtonText = addButtonText;

        ResourceName = ToString();
    }

    /* Public methods. */
    public override string ToString()
    {
        string str = "";
        if (Type != null)
            str = Type.ToString();
        return $"List({str})";
    }
}