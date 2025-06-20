using System;

namespace Rusty.ISA;

/// <summary>
/// An attribute that contains serialization info about a class.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class XmlPropertyAttribute : Attribute
{
    /* Public properties. */
    /// <summary>
    /// The XML tag that gets used for the target property.
    /// </summary>
    public string XmlKeyword { get; private set; } = "";

    /* Constructors. */
    public XmlPropertyAttribute(string xmlKeyword)
    {
        XmlKeyword = xmlKeyword;
    }
}