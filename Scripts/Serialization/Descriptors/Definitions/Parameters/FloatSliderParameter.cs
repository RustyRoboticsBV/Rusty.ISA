namespace Rusty.ISA;

/// <summary>
/// A descriptor for a float slider parameter.
/// </summary>
[ResourceDescriptor(typeof(FloatSliderParameter), "fslider")]
public class FloatSliderParameterDescriptor : ParameterDescriptor
{
    /* Public properties. */
    [XmlProperty("default")] public float DefaultValue { get; set; }
    [XmlProperty("min")] public float MinValue { get; set; }
    [XmlProperty("max")] public float MaxValue { get; set; }

    /* Public methods. */
    public override FloatSliderParameter GenerateObject()
    {
        return new FloatSliderParameter(ID, DisplayName, Description, DefaultValue, MinValue, MaxValue, Preview);
    }
}