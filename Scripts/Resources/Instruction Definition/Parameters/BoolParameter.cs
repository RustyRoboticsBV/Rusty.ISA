using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A definition for a boolean instruction parameter.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class BoolParameter : Parameter
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";

        /// <summary>
        /// The default value of this parameter in the editor.
        /// </summary>
        [Export] public bool DefaultValue { get; private set; }

        /* Constructors. */
        public BoolParameter() : base() { }

        public BoolParameter(string id, string displayName, string description, bool defaultValue)
            : base(id, displayName, description)
        {
            DefaultValue = defaultValue;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"{ID} (bool)";
        }
    }
}
