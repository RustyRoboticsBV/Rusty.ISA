using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// A definition for a floating-point slider instruction parameter.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class FloatSliderParameter : Parameter
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";
        /// <summary>
        /// The default value of this parameter in the editor.
        /// </summary>
        [Export] public float DefaultValue { get; private set; }
        /// <summary>
        /// The minimum value of this parameter in the editor.
        /// </summary>
        [Export] public float MinValue { get; private set; }
        /// <summary>
        /// The maximum value of this parameter in the editor.
        /// </summary>
        [Export] public float MaxValue { get; private set; } = 100f;
        [Export(PropertyHint.MultilineText)] public override string Preview { get; protected set; } = "";

        /* Constructors. */
        public FloatSliderParameter() : base() { }

        public FloatSliderParameter(string id, string displayName, string description, float defaultValue, float minValue
            , float maxValue, string preview) : base(id, displayName, description, preview)
        {
            DefaultValue = defaultValue;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"{ID} (fslider)";
        }
    }
}