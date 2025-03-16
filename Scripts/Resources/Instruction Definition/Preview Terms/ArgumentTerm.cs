using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// Adds a parameter value to the node preview.
    /// </summary>
    [Tool]
    [GlobalClass]
    public partial class ArgumentTerm : PreviewTerm
    {
        /* Public properties. */
        [Export] public string ParameterID { get; private set; } = "";

        /* Constructors. */
        public ArgumentTerm() : base() { }

        public ArgumentTerm(HideIf visibility, string parameterID) : base(visibility)
        {
            ParameterID = parameterID;

            ResourceName = ToString();
        }

        /* Public methods. */
        public override string ToString()
        {
            return "Argument: " + ParameterID;
        }
    }
}
