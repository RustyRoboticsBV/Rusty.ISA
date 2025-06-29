using System;

namespace Rusty.ISA;

/// <summary>
/// An attribute that contains serialization info about a class.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class XmlClassAttribute : Attribute
{
    /* Public properties. */
    /// <summary>
    /// The default name for XML elements of this type.
    /// </summary>
    public string XmlKeyword { get; private set; } = "";

    /* Constructors. */
    public XmlClassAttribute(string xmlKeyword = "")
    {
        XmlKeyword = xmlKeyword;
    }
}