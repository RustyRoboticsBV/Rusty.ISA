namespace Rusty.ISA;

/// <summary>
/// A descriptor for a text line parameter.
/// </summary>
[ResourceDescriptor(typeof(TextlineParameter), "textline")]
public class TextlineParameterDescriptor : ParameterDescriptor
{
    /* Public properties. */
    [XmlProperty("default")] public string DefaultValue { get; set; } = "";

    /* Public methods. */
    public override TextlineParameter GenerateObject()
    {
        return new TextlineParameter(ID, DisplayName, Description, DefaultValue, Preview);
    }
}