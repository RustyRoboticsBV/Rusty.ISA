namespace Rusty.ISA;

/// <summary>
/// A descriptor for a float parameter.
/// </summary>
[ResourceDescriptor(typeof(FloatParameter), "float")]
public class FloatParameterDescriptor : ParameterDescriptor
{
    /* Public properties. */
    [XmlProperty("default")] public float DefaultValue { get; set; }

    /* Public methods. */
    public override FloatParameter GenerateObject()
    {
        return new FloatParameter(ID, DisplayName, Description, DefaultValue, Preview);
    }
}