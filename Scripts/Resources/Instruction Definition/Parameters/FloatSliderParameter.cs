using Godot;

namespace Rusty.Cutscenes
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
        /// Sliders only: the minimum value of this parameter in the editor.
        /// </summary>
        [Export] public float MinValue { get; private set; }
        /// <summary>
        /// Sliders only: the maximum value of this parameter in the editor.
        /// </summary>
        [Export] public float MaxValue { get; private set; } = 100f;

        /* Constructors. */
        public FloatSliderParameter() : base() { }

        public FloatSliderParameter(string id, string displayName, string description, float defaultValue,
            float minValue, float maxValue)
            : base(id, displayName, description)
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
