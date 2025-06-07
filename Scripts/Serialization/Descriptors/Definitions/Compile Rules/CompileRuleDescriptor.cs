namespace Rusty.ISA;

/// <summary>
/// A descriptor for a compile rule.
/// </summary>
public abstract class CompileRuleDescriptor : Descriptor
{
    /* Public properties. */
    [XmlProperty("id")] public string ID { get; set; } = "";
    [XmlProperty("name")] public string DisplayName { get; set; } = "";
    [XmlProperty("desc")] public string Description { get; set; } = "";
    [XmlProperty("preview")] public string Preview { get; set; } = "";

    /* Public methods. */
    /// <summary>
    /// Generate a compile rule from this descriptor.
    /// </summary>
    public abstract override CompileRule GenerateObject();
}