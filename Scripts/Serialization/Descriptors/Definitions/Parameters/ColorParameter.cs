using Godot;

namespace Rusty.ISA;

/// <summary>
/// A descriptor for a color parameter.
/// </summary>
[ResourceDescriptor(typeof(ColorParameter), "color")]
public class ColorParameterDescriptor : ParameterDescriptor
{
    /* Public properties. */
    [XmlProperty("default")] public Color DefaultValue { get; set; }

    /* Public methods. */
    public override ColorParameter GenerateObject()
    {
        return new ColorParameter(ID, DisplayName, Description, DefaultValue, Preview);
    }
}