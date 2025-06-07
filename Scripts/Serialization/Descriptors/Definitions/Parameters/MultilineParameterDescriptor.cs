namespace Rusty.ISA;

/// <summary>
/// A descriptor for a multiline parameter.
/// </summary>
[ResourceDescriptor(typeof(MultilineParameter), "multiline")]
public class MultilineParameterDescriptor : ParameterDescriptor
{
    /* Public properties. */
    [XmlProperty("default")] public string DefaultValue { get; set; } = "";

    /* Public methods. */
    public override MultilineParameter GenerateObject()
    {
        return new MultilineParameter(ID, DisplayName, Description, DefaultValue, Preview);
    }
}