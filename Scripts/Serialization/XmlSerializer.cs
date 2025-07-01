using Godot;
using System;
using System.Reflection;

namespace Rusty.ISA;

/// <summary>
/// A class for serializing instruction resources into XML.
/// </summary>
public static class XmlSerializer
{
    /// <summary>
    /// Serialize an instruction resource to a string of XML.
    /// </summary>
    public static string Serialize(InstructionResource resource)
    {
        // Get class attribute.
        var attribute = resource.GetType().GetCustomAttribute<XmlClassAttribute>();

        // Serialize.
        return Serialize(attribute.XmlKeyword, resource);
    }

    /* Private methods. */
    /// <summary>
    /// Serialize an instruction resource.
    /// </summary>
    private static string Serialize(string keyword, InstructionResource resource)
    {
        string xml = "";

        // Get all properties of the resource.
        PropertyInfo[] properties = resource.GetType().GetProperties();

        // For each property...
        foreach (PropertyInfo property in properties)
        {
            // Get attribute.
            var attribute = property.GetCustomAttribute<XmlPropertyAttribute>();
            if (attribute == null)
                continue;

            // Get keyword. We use the class' keyword if the property had no keyword.
            string propKeyword = attribute.XmlKeyword;
            if (propKeyword == "")
                propKeyword = property.PropertyType.GetCustomAttribute<XmlClassAttribute>().XmlKeyword;

            // Get property value.
            object value = property.GetValue(resource);

            // Skip if default.
            if (ShouldSkipValue(value))
                continue;

            // Convert to XML.
            string innerXml = "";
            if (value is InstructionResource nested)
            {
                if (property.PropertyType.IsAbstract)
                {
                    string typeKeyword = value.GetType().GetCustomAttribute<XmlClassAttribute>().XmlKeyword;
                    innerXml = Wrap(propKeyword, Serialize(typeKeyword, nested));
                }
                else
                    innerXml = Serialize(propKeyword, nested);
            }
            else if (value is Array array)
                innerXml = Serialize(propKeyword, array);
            else if (value is Color color)
                innerXml = Serialize(propKeyword, color);
            else if (value is Texture2D texture)
                innerXml = Serialize(propKeyword, texture);
            else
                innerXml = Serialize(propKeyword, value);

            // Indent and add to the XML that was generated before.
            xml = Newline(xml, innerXml);
        }

        // Add end tag.
        xml = Wrap(keyword, xml);
        return xml;
    }

    /// <summary>
    /// Serialize an array.
    /// </summary>
    private static string Serialize(string keyword, Array array)
    {
        string xml = "";

        // Handle elements.
        foreach (object obj in array)
        {
            if (obj is InstructionResource resource)
                xml = Newline(xml, Serialize(resource));
            else
                throw new InvalidOperationException($"Arrays of type '{obj.GetType().Name}' are not supported!");
        }

        return Wrap(keyword, xml);
    }

    /// <summary>
    /// Serialize a color.
    /// </summary>
    private static string Serialize(string keyword, Color color)
    {
        return Wrap(keyword, $"#{color.ToHtml(color.A < 1f)}");
    }

    /// <summary>
    /// Serialize a texture.
    /// </summary>
    private static string Serialize(string keyword, Texture2D texture)
    {
        return Wrap(keyword, "Icon.png");
    }

    /// <summary>
    /// Serialize a generic object.
    /// </summary>
    private static string Serialize(string keyword, object obj)
    {
        return Wrap(keyword, $"{obj}");
    }

    /// <summary>
    /// Check if a value should be serialized.
    /// </summary>
    private static bool ShouldSkipValue(object value)
    {
        if (value == null)
            return true;

        switch (value)
        {
            case bool b when b == false:
            case int i when i == 0:
            case float f when Math.Abs(f) < 0.00001f:
            case double d when Math.Abs(d) < 0.00001:
            case char c when c == '0':
            case string s when s == "":
                return true;

            case Color color when color.R == 0 && color.G == 0 && color.B == 0 && color.A == 0:
                return true;

            case Array array when array.Length == 0:
                return true;

            default:
                return false;
        }
    }

    /// <summary>
    /// Wrap a string of XML within a tag.
    /// </summary>
    private static string Wrap(string keyword, string innerXml)
    {
        string xml = $"<{keyword}>";
        if (innerXml.Contains("\n") || (innerXml.TrimStart().StartsWith('<') && innerXml.TrimEnd().EndsWith('>')))
            xml += "\n" + Indent(innerXml) + "\n";
        else
            xml += innerXml;
        xml += $"</{keyword}>";
        return xml;
    }

    /// <summary>
    /// Append a string to another string, on a new line. Doesn't create a new line if the string is empty or ends with a
    /// line-break.
    /// </summary>
    private static string Newline(string previousXml, string addedXml)
    {
        if (previousXml == "")
            return addedXml;
        else
            return $"{previousXml}\n{addedXml}";
    }

    /// <summary>
    /// Indent a string.
    /// </summary>
    private static string Indent(string str)
    {
        return "\t" + str.Replace("\n", "\n\t");
    }
}