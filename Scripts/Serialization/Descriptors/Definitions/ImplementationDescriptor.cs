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
        public List<DependencyDescriptor> Dependencies { get; private set; } = new();
        public string Members { get; set; } = "";
        public string Initialize { get; set; } = "";
        public string Execute { get; set; } = "";

        /* Constructors. */
        public ImplementationDescriptor() { }

        /// <summary>
        /// Generate a descriptor from constructor arguments.
        /// </summary>
        public ImplementationDescriptor(DependencyDescriptor[] dependencies, string members, string initialize,
            string execute)
        {
            Dependencies = new(dependencies);
            Members = members;
            Initialize = initialize;
            Execute = execute;
        }

        /// <summary>
        /// Generate a descriptor from constructor arguments.
        /// </summary>
        public ImplementationDescriptor(Dependency[] dependencies, string members, string initialize,
            string execute)
        {
            foreach (Dependency dependency in dependencies)
            {
                Dependencies.Add(new(dependency));
            }
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
                foreach (Dependency dependency in implementation.Dependencies)
                {
                    Dependencies.Add(new(dependency));
                }
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
                        Dependencies.Add(new(element));
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
            List<Dependency> dependencies = new();
            foreach (DependencyDescriptor dependency in Dependencies)
            {
                dependencies.Add(dependency.Generate());
            }

            return new(dependencies.ToArray(), Members, Initialize, Execute);
        }

        /// <summary>
        /// Generate XML for this descriptor.
        /// </summary>
        public string GetXml()
        {
            string str = $"<{XmlKeywords.Implementation}>";
            foreach (var dependency in Dependencies)
            {
                str += $"\n\t{dependency.GetXml().Replace("\n", "\n\t")}";
            }
            if (Members != "")
                str += $"\n\t<{XmlKeywords.Members}>{Members}</{XmlKeywords.Members}>";
            if (Initialize != "")
                str += $"\n\t<{XmlKeywords.Initialize}>{Initialize}</{XmlKeywords.Initialize}>";
            if (Execute != "")
                str += $"\n\t<{XmlKeywords.Execute}>{Execute}</{XmlKeywords.Execute}>";
            str += $"\n</{XmlKeywords.Implementation}>";
            return str;
        }
    }
}