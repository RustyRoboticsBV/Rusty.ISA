using System.Collections.Generic;
using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// An implementation descriptor, meant for serialization.
    /// </summary>
    public sealed class ImplementationDescriptor
    {
        /* Public properties. */
        public List<string> Dependencies { get; set; } = new();
        public string Members { get; set; } = "";
        public string Initialize { get; set; } = "";
        public string Execute { get; set; } = "";

        /* Constructors. */
        public ImplementationDescriptor() { }

        /// <summary>
        /// Generate a descriptor from constructor arguments.
        /// </summary>
        public ImplementationDescriptor(string[] dependencies, string members, string initialize, string execute)
        {
            Dependencies = new(dependencies);
            Members = members;
            Initialize = initialize;
            Execute = execute;
        }

        /// <summary>
        /// Generate a descriptor from an implementation.
        /// </summary>
        public ImplementationDescriptor(Implementation implementation)
        {
            if (implementation != null)
            {
                Dependencies = new(implementation.Dependencies);
                Members = implementation.Members;
                Initialize = implementation.Initialize;
                Execute = implementation.Execute;
            }
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public ImplementationDescriptor(XmlElement xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == XmlKeywords.Dependencies)
                        Dependencies = new(Parser.ParseStrings(element.InnerText));
                    else if (element.Name == XmlKeywords.Members)
                        Members = element.InnerText;
                    else if (element.Name == XmlKeywords.Initialize)
                        Initialize = element.InnerText;
                    else if (element.Name == XmlKeywords.Execute)
                        Execute = element.InnerText;
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate an implementation from this descriptor.
        /// </summary>
        public Implementation Generate()
        {
            return new(Dependencies.ToArray(), Members, Initialize, Execute);
        }

        /// <summary>
        /// Generate XML for this descriptor.
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            string dependencies = "";
            foreach (string dependency in Dependencies)
            {
                if (dependencies.Length > 0)
                    dependencies += ",";
                dependencies += dependency;
            }

            string str = $"<{XmlKeywords.Implementation}>";
            if (dependencies != "")
                str += $"\n  <{XmlKeywords.Dependencies}>{dependencies}</{XmlKeywords.Dependencies}>";
            if (Members != "")
                str += $"\n  <{XmlKeywords.Members}>{Members}</{XmlKeywords.Members}>";
            if (Initialize != "")
                str += $"\n  <{XmlKeywords.Initialize}>{Initialize}</{XmlKeywords.Initialize}>";
            if (Execute != "")
                str += $"\n  <{XmlKeywords.Execute}>{Execute}</{XmlKeywords.Execute}>";
            str += $"\n</{XmlKeywords.Implementation}>";
            return str;
        }
    }
}