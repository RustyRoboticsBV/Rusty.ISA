using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Adds a pre-instruction rule's preview to the node text.
    /// </summary>
    [Tool]
    [GlobalClass]
    public partial class PreRuleTerm : CompileRuleTerm
    {
        /* Constructors. */
        public PreRuleTerm() : base() { }

        public PreRuleTerm(HideIf visibility, string compileRuleID) : base(visibility, compileRuleID) { }

        /* Public methods. */
        public override string ToString()
        {
            return "Pre-Instruction Rule: " + RuleID;
        }
    }
}