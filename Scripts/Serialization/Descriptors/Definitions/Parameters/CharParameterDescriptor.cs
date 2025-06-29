namespace Rusty.ISA;

/// <summary>
/// A descriptor for a char parameter.
/// </summary>
[ResourceDescriptor(typeof(CharParameter), "char")]
public class CharParameterDescriptor : ParameterDescriptor
{
    /* Public properties. */
    [XmlProperty("default")] public char DefaultValue { get; set; }

    /* Public methods. */
    public override CharParameter GenerateObject()
    {
        return new CharParameter(ID, DisplayName, Description, DefaultValue, Preview);
    }
}