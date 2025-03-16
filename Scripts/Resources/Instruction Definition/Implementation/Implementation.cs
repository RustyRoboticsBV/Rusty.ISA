using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// The implementation of an instruction.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class Implementation : InstructionResource
    {
        /* Public properties. */
        /// <summary>
        /// The GDScript code used for the instruction handler's member declarations.
        /// </summary>
        [Export(PropertyHint.MultilineText)] public string Members { get; private set; } = "";
        /// <summary>
        /// The GDScript code used for the instruction handler's initialize function.
        /// </summary>
        [Export(PropertyHint.MultilineText)] public string Initialize { get; private set; } = "";
        /// <summary>
        /// The GDScript code used for the instruction handler's execute function.
        /// </summary>
        [Export(PropertyHint.MultilineText)] public string Execute { get; private set; } = "";

        /* Constructors. */
        public Implementation() { }

        public Implementation(string members, string initialize, string execute)
        {
            Members = members;
            Initialize = initialize;
            Execute = execute;
        }
    }
}