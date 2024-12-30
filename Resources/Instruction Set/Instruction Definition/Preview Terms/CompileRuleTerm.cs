using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Adds a compile rule's preview to the node text.
    /// </summary>
    [Tool]
    [GlobalClass]
    public partial class CompileRuleTerm : PreviewTerm
    {
        /* Public properties. */
        /// <summary>
        /// The index of the compile rule whose preview text we want to include.
        /// </summary>
        [Export] public string CompileRuleId { get; private set; }

        /* Constructors. */
        public CompileRuleTerm() : base() { }

        public CompileRuleTerm(HideIf visibility, string compileRuleId) : base(visibility)
        {
            CompileRuleId = compileRuleId;

            ResourceName = ToString();
        }

        /* Public methods. */
        public override string ToString()
        {
            return "{rule: " + CompileRuleId + "}";
        }
    }
}
