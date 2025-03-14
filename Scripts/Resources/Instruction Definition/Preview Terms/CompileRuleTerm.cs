using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Adds a compile rule's preview to the node text.
    /// </summary>
    [Tool]
    [GlobalClass]
    public abstract class CompileRuleTerm : PreviewTerm
    {
        /* Public properties. */
        /// <summary>
        /// The index of the compile rule whose preview text we want to include.
        /// </summary>
        [Export] public string RuleID { get; private set; }

        /* Constructors. */
        public CompileRuleTerm() : base() { }

        public CompileRuleTerm(HideIf visibility, string compileRuleID) : base(visibility)
        {
            RuleID = compileRuleID;

            ResourceName = ToString();
        }

        /* Public methods. */
        public override string ToString()
        {
            return "Compile Rule: " + RuleID;
        }
    }
}