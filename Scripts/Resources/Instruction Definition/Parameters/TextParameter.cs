using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A definition for a string instruction parameter that doesn't allow for line-breaks.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class TextParameter : Parameter
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";

        /// <summary>
        /// The default value of this parameter in the editor.
        /// </summary>
        [Export] public string DefaultValue { get; private set; } = "";

        /* Constructors. */
        public TextParameter() : base() { }

        public TextParameter(string id, string displayName, string description, string defaultValue)
            : base(id, displayName, description)
        {
            DefaultValue = defaultValue;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"{ID} (text)";
        }
    }
}