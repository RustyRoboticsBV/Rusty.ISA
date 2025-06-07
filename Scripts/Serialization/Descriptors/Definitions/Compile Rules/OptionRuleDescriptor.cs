namespace Rusty.ISA;

/// <summary>
/// A descriptor for a option rule.
/// </summary>
[ResourceDescriptor(typeof(OptionRule), "option")]
public sealed class OptionRuleDescriptor : CompileRuleDescriptor
{
    /* Public properties. */
    [XmlProperty("type")] public CompileRuleDescriptor Type { get; set; }
    [XmlProperty("enabled")] public bool DefaultEnabled { get; set; }

    /* Public methods. */
    public override OptionRule GenerateObject()
    {
        CompileRule type = Type.GenerateObject();
        return new OptionRule(ID, DisplayName, Description, type, DefaultEnabled, Preview);
    }
}