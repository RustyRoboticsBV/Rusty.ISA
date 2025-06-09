using Godot;
using System;
using System.Reflection;
using System.Xml;

namespace Rusty.ISA;

/// <summary>
/// A class for serializing instruction resources to and from XML.
/// </summary>
public static class XmlDeserializer
{
    /// <summary>
    /// Deserialize an XML element into an instruction resource.
    /// </summary>
    public static InstructionResource Deserialize(XmlElement element, string folderPath)
    {
        // Find all types in the assembly.
        Type[] resourceTypes = Assembly.GetExecutingAssembly().GetTypes();
        foreach (Type type in resourceTypes)
        {
            // Ignore classes that are not resources.
            if (!typeof(InstructionResource).IsAssignableFrom(type))
                continue;

            // Find class attribute.
            var attribute = type.GetCustomAttribute<XmlClassAttribute>();
            if (attribute == null)
                continue;

            // Check if this type matches the XML element name.
            if (attribute.XmlKeyword != element.Name)
                continue;

            // Get MethodInfo for the generic Deserialize<T> method.
            MethodInfo genericMethod = typeof(XmlDeserializer)
                .GetMethod(nameof(Deserialize), [ typeof(XmlElement), typeof(string) ]);

            // Make a closed generic method for this type>
            MethodInfo method = genericMethod.MakeGenericMethod(type);

            // Invoke it and return the result.
            return (InstructionResource)method.Invoke(null, [ element, folderPath ]);
        }

        throw new InvalidOperationException($"No resource type found for XML element '{element.Name}'.");
    }

    /// <summary>
    /// Deserialize an XML element into an instruction resource.
    /// </summary>
    public static T Deserialize<T>(XmlElement element, string folderPath) where T : InstructionResource, new()
    {
        // Create a resource instance.
        T instance = new();

        // For each property...
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (PropertyInfo property in properties)
        {
            // Find property attribute.
            var propAttr = property.GetCustomAttribute<XmlPropertyAttribute>();
            if (propAttr == null)
                continue;

            // Find corresponding XML element.
            var childElement = element[propAttr.XmlKeyword];
            if (childElement == null)
                continue;

            object value = null;

            if (typeof(InstructionResource).IsAssignableFrom(property.PropertyType))
                value = Deserialize(childElement, folderPath);
            else if (property.PropertyType.IsArray)
            {
                Type elementType = property.PropertyType.GetElementType();
                XmlNodeList nodeList = childElement.ChildNodes;
                Array array = Array.CreateInstance(elementType, nodeList.Count);

                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (nodeList[i] is XmlElement child)
                        array.SetValue(Deserialize(child, folderPath), i);
                }

                value = array;
            }
            else if (property.PropertyType == typeof(Color))
                value = Color.FromHtml(childElement.InnerText);
            else if (property.PropertyType == typeof(Texture2D))
                value = IconLoader.Load(folderPath + "/" + childElement.InnerText);
            else
                value = Convert.ChangeType(childElement.InnerText, property.PropertyType);

            property.SetValue(instance, value);
        }

        return instance;
    }
}