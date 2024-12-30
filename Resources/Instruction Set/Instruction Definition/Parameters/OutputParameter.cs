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
        /// <summary>
        /// If true, this will cause the default output of a node containing this parameter to not be used.
        /// </summary>
        [Export] public bool OverrideDefaultOutput { get; private set; }
        /// <summary>
        /// Use the value of another parameter as the label for this output. Reference this parameter by its id.
        /// </summary>
        [Export] public string UseParameterAsLabel { get; private set; } = "";

        /* Constructors. */
        public OutputParameter() : base() { }

        public OutputParameter(string id, string displayName, string description, bool overrideDefaultOutput, string useLabelParameter)
            : base(id, displayName, description)
        {
            OverrideDefaultOutput = overrideDefaultOutput;
            UseParameterAsLabel = useLabelParameter;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "Output: " + Id;
        }
    }
}
