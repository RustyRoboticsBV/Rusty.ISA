using Godot;
using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// A descriptor for a type dependency, meant for serialization.
    /// </summary>
    public sealed partial class DependencyDescriptor
    {
        /* Public properties. */
        /// <summary>
        /// The name of the type. This should be the global class name that is exposed to Godot.
        /// </summary>
        [Export] public string Name { get; private set; } = "";

        /* Constructors. */
        public DependencyDescriptor() { }

        /// <summary>
        /// Generate a descriptor from constructor arguments.
        /// </summary>
        public DependencyDescriptor(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Generate a descriptor from a dependency.
        /// </summary>
        public DependencyDescriptor(Dependency dependency)
        {
            if (dependency != null)
            {
                Name = dependency.Name;
            }
        }

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public DependencyDescriptor(XmlElement xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == XmlKeywords.DisplayName)
                        Name = element.InnerText;
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate a dependency from this descriptor.
        /// </summary>
        public Dependency Generate()
        {
            return new Dependency(Name);
        }

        /// <summary>
        /// Generate XML for this descriptor.
        /// </summary>
        public string GetXml()
        {
            string str = $"<{XmlKeywords.Dependency}>";
            str += $"\n\t<{XmlKeywords.DisplayName}>{Name}</{XmlKeywords.DisplayName}>";
            str += $"\n</{XmlKeywords.Dependency}>";
            return str;
        }
    }
}