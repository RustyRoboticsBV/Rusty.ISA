using Godot;
using System;
using System.Reflection;
using System.Xml;

namespace Rusty.ISA;

/// <summary>
/// A class for deserializing instruction resources from XML.
/// </summary>
public static class XmlDeserializer
{
    /// <summary>
    /// Deserialize an XML element into an instruction resource.
    /// </summary>
    public static InstructionResource Deserialize(string xml, string folderPath)
    {
        // Create XML document.
        XmlDocument document = new();
        document.LoadXml(xml);

        // Parse document.
        return Deserialize(document, folderPath);
    }

    /// <summary>
    /// Deserialize an XML document into an instruction resource.
    /// </summary>
    public static InstructionResource Deserialize(XmlDocument document, string folderPath)
    {
        // Parse root element.
        foreach (XmlNode node in document.ChildNodes)
        {
            if (node is XmlElement element)
                return Deserialize(element, folderPath);
        }

        // If there was no root element, throw an exception.
        throw new XmlException("The XML file had no root element.");
    }

    /// <summary>
    /// Deserialize an XML element into an instruction resource.
    /// </summary>
    public static InstructionResource Deserialize(XmlElement element, string folderPath)
    {
        // Find all types in the assembly.
        Type[] resourceTypes = Assembly.GetExecutingAssembly().GetTypes();

        // For each type in the assembly, try to parse the XML element as this type.
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
            MethodInfo genericMethod = null;
            MethodInfo[] methods = typeof(XmlDeserializer).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo methodInfo in methods)
            {
                if (methodInfo.Name == nameof(Deserialize) &&
                    methodInfo.IsGenericMethodDefinition &&
                    methodInfo.GetParameters().Length == 2 &&
                    methodInfo.GetGenericArguments().Length == 1)
                {
                    genericMethod = methodInfo;
                    break;
                }
            }

            // Make a closed generic method for this type.
            MethodInfo method = genericMethod.MakeGenericMethod(type);

            // Invoke it and return the result.
            var resource = (InstructionResource)method.Invoke(null, [element, folderPath]);
            resource.ResourceName = resource.ToString();
            return resource;
        }

        // If we get here, then we assume that the XML element was a wrapper.
        // Wrappers can only contain a single element; other elements are ignored.
        foreach (XmlNode node in element.ChildNodes)
        {
            if (node is XmlElement childElement)
                return Deserialize(childElement, folderPath);
        }

        // If we get here, the wrapper was empty.
        return null;
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
            {
                string innerText = childElement.InnerText;
                while (innerText.StartsWith("\n"))
                {
                    innerText = innerText.Substring(1);
                }
                while (innerText.StartsWith("\t"))
                {
                    innerText = innerText.Substring(1).Replace("\n\t", "\n");
                }
                while (innerText.EndsWith("\n") || innerText.EndsWith("\t"))
                {
                    innerText = innerText.Substring(0, innerText.Length - 1);
                }
                try
                {
                    value = Convert.ChangeType(innerText, property.PropertyType);
                }
                catch { }
            }

            property.SetValue(instance, value);
        }

        return instance;
    }
}