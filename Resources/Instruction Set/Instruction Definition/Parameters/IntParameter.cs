using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Meta-data for an instruction parameter with an integer value.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class IntParameter : ParameterDefinition
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";

        /// <summary>
        /// The default value of this parameter in the editor.
        /// </summary>
        [Export] public int DefaultValue { get; private set; }

        /* Constructors. */
        public IntParameter() : base() { }

        public IntParameter(string id, string displayName, string description, int defaultValue)
            : base(id, displayName, description)
        {
            DefaultValue = defaultValue;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "Int: " + ID;
        }
    }
}