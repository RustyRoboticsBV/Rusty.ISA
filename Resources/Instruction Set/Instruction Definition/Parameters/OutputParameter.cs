using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A definition for an instruction parameter with an output slot.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class OutputParameter : ParameterDefinition
    {
        /* Public properties. */
        [Export] public override string Id { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";

        /// <summary>
        /// Use the value of another parameter as the label for this output. Reference this parameter by its id.
        /// </summary>
        [Export] public string UseParameterAsPreview { get; private set; } = "";

        /* Constructors. */
        public OutputParameter() : base() { }

        public OutputParameter(string id, string displayName, string description, string useParameterAsPreview)
            : base(id, displayName, description)
        {
            UseParameterAsPreview = useParameterAsPreview;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "Output: " + Id;
        }
    }
}
