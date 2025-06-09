using Godot;
using System;
using System.Reflection;
using System.Xml;

namespace Rusty.ISA;

/// <summary>
/// A class for serializing instruction resources to and from XML.
/// </summary>
public static class XmlSerializer
{
    /// <summary>
    /// Serialize an instruction resource to a string of XML.
    /// </summary>
    public static string Serialize(InstructionResource resource)
    {
        // Get class attribute.
        var attribute = GetAttribute<XmlClassAttribute>(resource);

        // Serialize.
        return Serialize(attribute.XmlKeyword, resource);
    }

    /* Private methods. */
    private static string Serialize(string keyword, InstructionResource resource)
    {
        // Get all properties of the resource.
        PropertyInfo[] properties = resource.GetType().GetProperties();

        // Create start tag.
        string xml = $"<{keyword}>";

        // For each property...
        foreach (PropertyInfo property in properties)
        {
            // Get attribute.
            var attribute = property.GetCustomAttribute<XmlPropertyAttribute>();
            if (attribute == null)
                continue;

            // Get property value.
            object value = property.GetValue(resource);

            // Convert to XML.
            string innerXml = "";
            if (value is InstructionResource nested)
                innerXml = Serialize(attribute.XmlKeyword, nested);
            if (value is Array array)
                innerXml = Serialize(attribute.XmlKeyword, array);
            else if (value is Color color)
                innerXml = Serialize(attribute.XmlKeyword, color);
            else if (value is Texture2D texture)
                innerXml = Serialize(attribute.XmlKeyword, texture);
            else
                innerXml = Serialize(attribute.XmlKeyword, value);

            // Indent and add to the XML that was generated before.
            xml += "\n" + Indent(innerXml);
        }

        // Add end tag.
        xml += $"\n</{keyword}>";
        return xml;
    }

    private static string Serialize(string keyword, Array array)
    {
        // Create start tag.
        string xml = $"<{keyword}>";

        // Handle elements.
        foreach (object obj in array)
        {
            if (obj is InstructionResource resource)
                xml += "\n" + Indent(Serialize(resource));
            else
                throw new InvalidOperationException($"Arrays of type '{obj.GetType().Name}' are not supported!");
        }

        // Create end tag.
        xml += $"\n</{keyword}>";
        return xml;
    }

    private static string Serialize(string keyword, Color color)
    {
        return $"<{keyword}>#{color.ToHtml(color.A < 1f)}</{keyword}>";
    }

    private static string Serialize(string keyword, Texture2D texture)
    {
        return $"<{keyword}>Icon.png</{keyword}>";
    }

    private static string Serialize(string keyword, object obj)
    {
        return $"<{keyword}>#{obj}</{keyword}>";
    }

    private static string Indent(string str)
    {
        return "\t" + str.Replace("\n", "\n\t");
    }

    private static T GetAttribute<T>(object obj) where T : Attribute
    {
        // Get all attributes on this object.
        Type type = obj.GetType();
        var attributes = type.GetCustomAttributes(true);

        // Find the correct one.
        foreach (Attribute attribute in attributes)
        {
            if (attribute is T result)
                return result;
        }

        // If the clas didn't have one, throw an exception.
        throw new InvalidOperationException($"Type '{type.Name}' had no {typeof(T).Name} attribute!");
    }
}
