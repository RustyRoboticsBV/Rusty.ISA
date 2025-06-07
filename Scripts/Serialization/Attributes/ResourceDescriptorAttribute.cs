using System;

namespace Rusty.ISA;

/// <summary>
/// An attribute that tells the module which ISA resource a descriptor class represents.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ResourceDescriptorAttribute : Attribute
{
    /* Public properties. */
    /// <summary>
    /// The corresponding resource type.
    /// </summary>
    public Type ResourceType { get; private set; }
    /// <summary>
    /// The default name for XML elements of this type.
    /// </summary>
    public string DefaultName { get; private set; } = "";

    /* Constructors. */
    public ResourceDescriptorAttribute(Type resourceType, string defaultName = "")
    {
        ResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
        DefaultName = defaultName;
    }
}