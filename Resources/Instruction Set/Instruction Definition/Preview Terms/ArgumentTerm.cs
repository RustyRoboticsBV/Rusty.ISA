using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Adds a parameter value to the node preview.
    /// </summary>
    [Tool]
    [GlobalClass]
    public partial class ArgumentTerm : PreviewTerm
    {
        /* Public properties. */
        [Export] public string ParameterId { get; private set; } = "";

        /* Constructors. */
        public ArgumentTerm() : base() { }

        public ArgumentTerm(HideIf visibility, string parameterId) : base(visibility)
        {
            ParameterId = parameterId;

            ResourceName = ToString();
        }

        /* Public methods. */
        public override string ToString()
        {
            return "{arg: " + ParameterId + "}";
        }
    }
}
