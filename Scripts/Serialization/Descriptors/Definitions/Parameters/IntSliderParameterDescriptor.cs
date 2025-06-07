namespace Rusty.ISA;

/// <summary>
/// A descriptor for an int slider parameter.
/// </summary>
[ResourceDescriptor(typeof(IntSliderParameter), "islider")]
public class IntSliderParameterDescriptor : ParameterDescriptor
{
    /* Public properties. */
    [XmlProperty("default")] public int DefaultValue { get; set; }
    [XmlProperty("min")] public int MinValue { get; set; }
    [XmlProperty("max")] public int MaxValue { get; set; }

    /* Public methods. */
    public override IntSliderParameter GenerateObject()
    {
        return new IntSliderParameter(ID, DisplayName, Description, DefaultValue, MinValue, MaxValue, Preview);
    }
}