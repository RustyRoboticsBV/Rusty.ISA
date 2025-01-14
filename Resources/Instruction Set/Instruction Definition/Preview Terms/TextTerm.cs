using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Adds a simple string to the node preview.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class TextTerm : PreviewTerm
    {
        /* Public properties. */
        [Export] public string Text { get; private set; } = "";

        /* Constructors. */
        public TextTerm() : base() { }

        public TextTerm(HideIf visibility, string text)
            : base(visibility)
        {
            Text = text;

            ResourceName = ToString();
        }

        /* Public methods. */
        public override string ToString()
        {
            return "Text: " + Text;
        }
    }
}