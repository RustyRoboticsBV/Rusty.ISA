namespace Rusty.ISA;

/// <summary>
/// A dependency descriptor.
/// </summary>
[ResourceDescriptor(typeof(Dependency), "dep")]
public sealed class DependencyDescriptor : Descriptor
{
    /* Public properties. */
    [XmlProperty("name")] public string Name { get; set; } = "";

    /* Public methods. */
    public override Dependency GenerateObject()
    {
        return new Dependency(Name);
    }
}