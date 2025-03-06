using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Meta-data for an instruction parameter with a color value.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class ColorParameter : ParameterDefinition
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";

        /// <summary>
        /// The default value of this parameter in the editor.
        /// </summary>
        [Export] public Color DefaultValue { get; private set; } = Colors.White;

        /* Constructors. */
        public ColorParameter() : base() { }

        public ColorParameter(string id, string displayName, string description, Color defaultValue)
            : base(id, displayName, description)
        {
            DefaultValue = defaultValue;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "Color: " + ID;
        }
    }
}
