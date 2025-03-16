using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// A definition for a character instruction parameter.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class CharParameter : Parameter
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";

        /// <summary>
        /// The default value of this parameter in the editor.
        /// </summary>
        [Export] public char DefaultValue { get; private set; } = 'A';

        /* Constructors. */
        public CharParameter() : base() { }

        public CharParameter(string id, string displayName, string description, char defaultValue)
            : base(id, displayName, description)
        {
            DefaultValue = defaultValue;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"{ID} (char)";
        }
    }
}