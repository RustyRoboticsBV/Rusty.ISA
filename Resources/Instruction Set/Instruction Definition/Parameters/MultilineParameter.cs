using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Meta-data for an instruction parameter with a string value that allows line-breaks.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class MultilineParameter : ParameterDefinition
    {
        /* Public properties. */
        [Export] public override string Id { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";

        /// <summary>
        /// The default value of this parameter in the editor.
        /// </summary>
        [Export(PropertyHint.MultilineText)] public string DefaultValue { get; private set; } = "";

        /* Constructors. */
        public MultilineParameter() : base() { }

        public MultilineParameter(string id, string displayName, string description, string defaultValue)
            : base(id, displayName, description)
        {
            DefaultValue = defaultValue;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "Multiline: " + Id;
        }
    }
}
