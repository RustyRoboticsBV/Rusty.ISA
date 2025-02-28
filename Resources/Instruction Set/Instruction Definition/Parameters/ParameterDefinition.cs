using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Meta-data for a cutscene instruction parameter.
    /// </summary>
    [GlobalClass]
    public abstract partial class ParameterDefinition : CutsceneResource
    {
        /* Public methods. */
        /// <summary>
        /// The internal identifier of this parameter, which can be used to get the value of this parameter in an instruction
        /// instance. Should be unique from other parameters that belong to the same instruction, and should preferably be as
        /// short as possible.
        /// </summary>
        public abstract string ID { get; protected set; }
        /// <summary>
        /// The human-readable name of this parameter. Used in the cutscene editor.
        /// </summary>
        public abstract string DisplayName { get; protected set; }
        /// <summary>
        /// The description of this parameter. Used for editor tooltips and documentation generation.
        /// </summary>
        public abstract string Description { get; protected set; }

        /* Constructors. */
        public ParameterDefinition() { }

        public ParameterDefinition(string id, string displayName, string description)
        {
            ID = id;
            DisplayName = displayName;
            Description = description;

            ResourceName = ToString();
        }
    }
}
