using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// A type dependency that must exist for an instruction to function properly.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class Dependency : InstructionResource
    {
        /* Public properties. */
        /// <summary>
        /// The name of the type.
        /// </summary>
        [Export] public string Name { get; private set; } = "";

        /* Constructors. */
        public Dependency() { }

        public Dependency(string name)
        {
            Name = name;
        }
    }
}