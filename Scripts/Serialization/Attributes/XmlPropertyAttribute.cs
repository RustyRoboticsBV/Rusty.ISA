using System;

namespace Rusty.ISA;

/// <summary>
/// An attribute that tells the module that a descriptor's property must be (de)serialized.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class XmlPropertyAttribute : Attribute
{
    /* Public properties. */
    /// <summary>
    /// The XML tag that gets used for the target property.
    /// </summary>
    public string XmlTag { get; private set; } = "";

    /* Constructors. */
    public XmlPropertyAttribute() { }

    public XmlPropertyAttribute(string xmlTag)
    {
        XmlTag = xmlTag;
    }
}