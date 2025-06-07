namespace Rusty.ISA;

/// <summary>
/// A descriptor for a list rule.
/// </summary>
[ResourceDescriptor(typeof(ListRule), "list")]
public sealed class ListRuleDescriptor : CompileRuleDescriptor
{
    /* Public properties. */
    [XmlProperty("type")] public CompileRuleDescriptor Type { get; set; }
    [XmlProperty("button_text")] public string AddButtonText { get; set; } = "";

    /* Public methods. */
    public override ListRule GenerateObject()
    {
        CompileRule type = Type.GenerateObject();
        return new ListRule(ID, DisplayName, Description, type, AddButtonText, Preview);
    }
}