namespace Rusty.ISA;

/// <summary>
/// A descriptor for a bool parameter.
/// </summary>
[ResourceDescriptor(typeof(BoolParameter), "bool")]
public class BoolParameterDescriptor : ParameterDescriptor
{
    /* Public properties. */
    [XmlProperty("default")] public bool DefaultValue { get; set; }

    /* Public methods. */
    public override BoolParameter GenerateObject()
    {
        return new BoolParameter(ID, DisplayName, Description, DefaultValue, Preview);
    }
}