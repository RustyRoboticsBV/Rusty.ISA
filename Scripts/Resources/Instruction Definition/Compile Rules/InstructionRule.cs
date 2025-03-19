using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// A compile rule that generates an instance of an instruction definition, as well as its compile rules.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class InstructionRule : CompileRule
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";
        /// <summary>
        /// The opcode of the instruction.
        /// </summary>
        [Export] public string Opcode { get; private set; } = "";
        /// <summary>
        /// An expression that defines how previews will be generated for this rule. If left empty, the preview of the target
        /// instruction is generated.
        /// </summary>
        [Export(PropertyHint.MultilineText)] public override string Preview { get; protected set; } = "";

        /* Constructors. */
        public InstructionRule() : base() { }

        public InstructionRule(string id, string displayName, string description, string opcode, string preview)
            : base(id, displayName, description, preview)
        {
            Opcode = opcode;

            ResourceName = ToString();
        }

        /* Public methods. */
        public override string ToString()
        {
            return Opcode;
        }
    }
}