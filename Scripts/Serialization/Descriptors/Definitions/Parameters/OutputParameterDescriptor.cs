namespace Rusty.ISA;

/// <summary>
/// A descriptor for an output parameter.
/// </summary>
[ResourceDescriptor(typeof(OutputParameter), "output")]
public class OutputParameterDescriptor : ParameterDescriptor
{
    /* Public properties. */
    [XmlProperty("remove_default")] public bool RemoveDefaultOutput { get; set; }

    /* Public methods. */
    public override OutputParameter GenerateObject()
    {
        return new OutputParameter(ID, DisplayName, Description, RemoveDefaultOutput, Preview);
    }
}