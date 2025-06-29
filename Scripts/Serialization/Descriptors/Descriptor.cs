using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Rusty.ISA;

/// <summary>
/// A base class for all resource descriptors. Used for serialization and deserialization.
/// </summary>
public abstract class Descriptor
{
    /* Public methods. */
    /// <summary>
    /// Generate a descriptor from a resource object.
    /// </summary>
    public static Descriptor FromObject(InstructionResource resource)
    {
        // Get the resource's type.
        Type objType = resource.GetType();

        // Loop through each type in the assembly...
        Type[] types = Assembly.GetAssembly(typeof(Descriptor)).GetTypes();
        foreach (Type type in types)
        {
            var descriptorAttribute = type.GetCustomAttribute<ResourceDescriptorAttribute>();
            if (descriptorAttribute?.ResourceType == objType)
            {
                Descriptor descriptorInstance = (Descriptor)Activator.CreateInstance(type);
                descriptorInstance.CopyObject(resource);
                return descriptorInstance;
            }
        }

        throw new InvalidOperationException($"No matching descriptor class found for object type '{objType.Name}'>");
    }

    /// <summary>
    /// Generate a descriptor for an XML element.
    /// </summary>
    public static Descriptor FromXml(XmlElement element)
    {
        // Loop through each type in the assembly...
        Type[] types = Assembly.GetAssembly(typeof(Descriptor)).GetTypes();
        foreach (Type type in types)
        {
            var descriptorAttribute = type.GetCustomAttribute<ResourceDescriptorAttribute>();
            if (descriptorAttribute?.DefaultName == element.Name)
            {
                Descriptor descriptorInstance = (Descriptor)Activator.CreateInstance(type);
                descriptorInstance.CopyXml(element);
                return descriptorInstance;
            }
        }

        throw new InvalidOperationException($"No matching descriptor class found for xml element '{element.Name}'>");
    }

    /// <summary>
    /// Fill this descriptor with values from a source object.
    /// </summary>
    public void CopyObject(InstructionResource resource)
    {
        Type objType = resource.GetType();

        PropertyInfo[] myProperties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var myProperty in myProperties)
        {
            // Get XML property attribute.
            var propertyAttribute = myProperty.GetCustomAttribute<XmlPropertyAttribute>();
            if (propertyAttribute == null)
                continue;

            // Find corresponding property in source object.
            object propertyValue = null;
            foreach (var property in objType.GetProperties())
            {
                if (property.Name == myProperty.Name)
                {
                    propertyValue = property.GetValue(resource);
                    break;
                }
            }

            // Copy property value.
            if (propertyValue == null)
                continue;

            if (propertyValue is InstructionResource nestedResource)
                myProperty.SetValue(this, FromObject(nestedResource));

            else if (propertyValue is Array array && myProperty.PropertyType.IsGenericType
                && myProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                // Ensure the list is empty before adding elements.
                IList list = myProperty.GetValue(this) as IList;
                list.Clear();

                // Add each element from the array to the list.
                foreach (var item in array)
                {
                    if (item is InstructionResource itemResource)
                        list.Add(FromObject(itemResource));
                }
            }

            else
                myProperty.SetValue(this, propertyValue);
        }
    }

    /// <summary>
    /// Fill this descriptor with values from an XML element.
    /// </summary>
    public void CopyXml(XmlElement element)
    {
        // Get descriptor properties info.
        Type type = GetType();
        PropertyInfo[] myProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // For each descriptor property...
        foreach (PropertyInfo myProperty in myProperties)
        {
            // Make sure it has the XML property attribute.
            var propertyAttribute = myProperty.GetCustomAttribute<XmlPropertyAttribute>();
            if (propertyAttribute == null)
                continue;

            // Find the coresponding XML child node (if it exists).
            string tagName = propertyAttribute.XmlTag;
            XmlNode childNode = element.SelectSingleNode(tagName);
            if (childNode == null)
                continue;

            // Deserialize the node's contents.

            // Nested descriptor.
            if (typeof(Descriptor).IsAssignableFrom(myProperty.PropertyType))
            {
                Descriptor nestedDescriptor = (Descriptor)Activator.CreateInstance(myProperty.PropertyType);
                nestedDescriptor.CopyXml((XmlElement)childNode);
                myProperty.SetValue(this, nestedDescriptor);
            }

            // List of descriptors.
            else if (typeof(IList).IsAssignableFrom(myProperty.PropertyType))
            {
                // Ensure the list is empty before adding elements.
                IList list = myProperty.GetValue(this) as IList;
                list.Clear();

                // Parse each child element.
                foreach (XmlElement child in element.ChildNodes)
                {
                    list.Add(FromXml(child));
                }
            }

            // Color.
            else if (myProperty.PropertyType == typeof(Color))
            {
                Color color = Color.FromHtml(childNode.InnerText);
                myProperty.SetValue(this, color);
            }

            // Simple types.
            else
            {
                object value = Convert.ChangeType(childNode.InnerText.Trim(), myProperty.PropertyType);
                myProperty.SetValue(this, value);
            }
        }
    }

    /// <summary>
    /// Generate an object from this descriptor.
    /// </summary>
    public abstract object GenerateObject();

    /// <summary>
    /// Convert this descriptor to XML.
    /// </summary>
    public string GenerateXml(string tag)
    {
        // Get type info.
        Type type = GetType();

        // Use class attribute tag if the method argument is the empty string.
        var descriptorAttribute = type.GetCustomAttribute<ResourceDescriptorAttribute>();
        if (tag == "" && descriptorAttribute != null && descriptorAttribute?.DefaultName != "")
            tag = descriptorAttribute.DefaultName;

        // Get all public properties of this class.
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Loop through all properties...
        string xml = "";
        foreach (PropertyInfo property in properties)
        {
            // Check if the property has the correct attribute.
            var propertyAttribute = property.GetCustomAttribute<XmlPropertyAttribute>();
            if (propertyAttribute != null)
            {
                // Get property value.
                object value = property.GetValue(this);

                // Generate XML for property.
                string newXml = Serialize(propertyAttribute.XmlTag, value);
                if (newXml == "")
                    continue;

                // Append to previously-generated XML.
                if (xml != "")
                    xml += "\n";
                xml += newXml;
            }
        }

        // Return finished XML.
        if (xml != "")
            return $"<{tag}>\n{Indent(xml)}\n</{tag}>";
        else
            return xml;
    }

    /* Private methods. */
    /// <summary>
    /// Return a copy of a string where each line has been indented.
    /// </summary>
    private static string Indent(string str, string indentation = "\t")
    {
        if (str.StartsWith('\n'))
            return str.Replace("\n", $"\n{indentation}");
        else
            return $"{indentation}{str.Replace("\n", $"\n{indentation}")}";
    }

    /// <summary>
    /// Serialize an object.
    /// </summary>
    private static string Serialize(string tag, object obj)
    {
        if (obj == null || (obj is string strval && strval == ""))
            return "";

        if (obj is Descriptor descriptor)
            return descriptor.GenerateXml(tag);

        else if (obj is IList list)
        {
            string xml = "";
            foreach (object item in list)
            {
                if (xml != "")
                    xml += "\n";
                xml += Serialize("", item);
            }
            if (xml != "")
                return $"<{tag}>\n{Indent(xml)}\n</{tag}>";
            else
                return "";
        }

        else if (obj is Color color)
            return $"<{tag}>#{color.ToHtml(color.A < 1f)}</{tag}>";

        else
            return $"<{tag}>{obj}</{tag}>";
    }
}