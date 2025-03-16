using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// A definition for a floating-point instruction parameter.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class FloatParameter : Parameter
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";

        /// <summary>
        /// The default value of this parameter in the editor.
        /// </summary>
        [Export] public float DefaultValue { get; private set; }

        /* Constructors. */
        public FloatParameter() : base() { }

        public FloatParameter(string id, string displayName, string description, float defaultValue)
            : base(id, displayName, description)
        {
            DefaultValue = defaultValue;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"{ID} (float)";
        }
    }
}
