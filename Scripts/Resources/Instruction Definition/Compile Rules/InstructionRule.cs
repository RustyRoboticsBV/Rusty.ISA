using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A compile rule that compiles into a single instruction.
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
        [Export] public string Opcode { get; private set; }

        /* Constructors. */
        public InstructionRule() : base("", "", "") { }

        public InstructionRule(string id, string displayName, string description, string opcode)
            : base(id, displayName, description)
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