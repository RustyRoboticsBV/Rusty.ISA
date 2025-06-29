using System.Collections.Generic;
using System.Xml;

namespace Rusty.ISA
{
    /// <summary>
    /// An implementation descriptor, meant for serialization.
    /// </summary>
    [ResourceDescriptor(typeof(Implementation), "impl")]
    public sealed class ImplementationDescriptor : Descriptor
    {
        /* Public properties. */
        [XmlProperty("deps")] public List<DependencyDescriptor> Dependencies { get; } = new();
        [XmlProperty("members")] public string Members { get; set; } = "";
        [XmlProperty("init")] public string Initialize { get; set; } = "";
        [XmlProperty("exec")] public string Execute { get; set; } = "";

        /* Public methods. */
        public override Implementation GenerateObject()
        {
            List<Dependency> dependencies = new();
            foreach (DependencyDescriptor dependency in Dependencies)
            {
                dependencies.Add(dependency.GenerateObject());
            }

            return new(dependencies.ToArray(), Members, Initialize, Execute);
        }
    }
}