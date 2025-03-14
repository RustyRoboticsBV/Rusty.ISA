using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Adds a post-instruction rule's preview to the node text.
    /// </summary>
    [Tool]
    [GlobalClass]
    public partial class PostRuleTerm : CompileRuleTerm
    {
        /* Constructors. */
        public PostRuleTerm() : base() { }

        public PostRuleTerm(HideIf visibility, string compileRuleID) : base(visibility, compileRuleID) { }

        /* Public methods. */
        public override string ToString()
        {
            return "Post-Instruction Rule: " + RuleID;
        }
    }
}