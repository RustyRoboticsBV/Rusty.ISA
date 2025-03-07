using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A definition for an output instruction parameter.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class OutputParameter : Parameter
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";
        /// <summary>
        /// If enabled, then any node that contains this parameter will not have a default output.
        /// </summary>
        [Export] public bool RemoveDefaultOutput { get; private set; }

        /// <summary>
        /// Use the value of another parameter as the label for this output. Reference this parameter by its ID.
        /// </summary>
        [Export] public string UseArgumentAsPreview { get; private set; } = "";

        /* Constructors. */
        public OutputParameter() : base() { }

        public OutputParameter(string id, string displayName, string description, bool removeDefaultOutput,
            string useParameterAsPreview) : base(id, displayName, description)
        {
            RemoveDefaultOutput = removeDefaultOutput;
            UseArgumentAsPreview = useParameterAsPreview;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"{ID} (output)";
        }
    }
}
