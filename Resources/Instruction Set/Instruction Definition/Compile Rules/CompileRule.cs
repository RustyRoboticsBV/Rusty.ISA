using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A generic pre-instruction compile rule of a cutscene instruction.
    /// Pre-instructions are extra instructions that a node generates upon compilation (in addition to its main
    /// instruction). These extra instructions are placed before the main instruction in the compiled program.
    /// </summary>
    [GlobalClass]
    public abstract partial class CompileRule : CutsceneResource
    {
        /* Public properties. */
        /// <summary>
        /// The identifier of this compile rule, with which it can be refenced.
        /// </summary>
        public abstract string ID { get; protected set; }
        /// <summary>
        /// The name of this compile rule in the editor.
        /// </summary>
        public abstract string DisplayName { get; protected set; }
        /// <summary>
        /// The description of this compile rule in the editor.
        /// </summary>
        public abstract string Description { get; protected set; }

        /* Constructors. */
        public CompileRule(string id, string displayName, string description) : base()
        {
            ID = id;
            DisplayName = displayName;
            Description = description;
        }
    }
}