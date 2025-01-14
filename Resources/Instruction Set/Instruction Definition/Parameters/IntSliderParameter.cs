using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Meta-data for an instruction parameter with an integer value with a min and max value.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class IntSliderParameter : ParameterDefinition
    {
        /* Public properties. */
        [Export] public override string Id { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";

        /// <summary>
        /// The default value of this parameter in the editor.
        /// </summary>
        [Export] public int DefaultValue { get; private set; }
        /// <summary>
        /// Sliders only: the minimum value of this parameter in the editor.
        /// </summary>
        [Export] public int MinValue { get; private set; }
        /// <summary>
        /// Sliders only: the maximum value of this parameter in the editor.
        /// </summary>
        [Export] public int MaxValue { get; private set; } = 100;

        /* Constructors. */
        public IntSliderParameter() : base() { }

        public IntSliderParameter(string id, string displayName, string description, int defaultValue,
            int minValue, int maxValue)
            : base(id, displayName, description)
        {
            DefaultValue = defaultValue;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "IntSlider: " + Id;
        }
    }
}
