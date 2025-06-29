namespace Rusty.ISA;

/// <summary>
/// A descriptor for an int parameter.
/// </summary>
[ResourceDescriptor(typeof(IntParameter), "int")]
public class IntParameterDescriptor : ParameterDescriptor
{
    /* Public properties. */
    [XmlProperty("default")] public int DefaultValue { get; set; }

    /* Public methods. */
    public override IntParameter GenerateObject()
    {
        return new IntParameter(ID, DisplayName, Description, DefaultValue, Preview);
    }
}