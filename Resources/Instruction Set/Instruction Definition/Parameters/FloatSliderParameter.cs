using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Meta-data for an instruction parameter with an floating-point value with a min and max value.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class FloatSliderParameter : ParameterDefinition
    {
        /* Public properties. */
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
            return "FloatSlider: " + Id;
        }
    }
}
