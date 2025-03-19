using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// A definition for an integer instruction parameter.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class IntParameter : Parameter
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";
        /// <summary>
        /// The default value of this parameter in the editor.
        /// </summary>
        [Export] public int DefaultValue { get; private set; }
        [Export(PropertyHint.MultilineText)] public override string Preview { get; protected set; } = "";

        /* Constructors. */
        public IntParameter() : base() { }

        public IntParameter(string id, string displayName, string description, int defaultValue, string preview)
            : base(id, displayName, description, preview)
        {
            DefaultValue = defaultValue;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"{ID} (int)";
        }
    }
}