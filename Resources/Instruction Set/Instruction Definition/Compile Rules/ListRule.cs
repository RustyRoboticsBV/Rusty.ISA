using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// Defines a list of pre-instructions, where each instruction is of the same pre-instruction type.
    /// Combine with PreInstructionChoice for a list that allows for multiple types of pre-instructions.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class ListRule : CompileRule
    {
        /* Public properties. */
        [Export] public override string Id { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";
        /// <summary>
        /// The type of element that is contained in this list. Can be a pre-instruction or another container.
        /// </summary>
        [Export] public CompileRule Type { get; private set; }

        /// <summary>
        /// The text displayed on the "add item" button.
        /// </summary>
        [Export] public string AddButtonText { get; private set; } = "Add Item";
        /// <summary>
        /// The separation string that is inserted between element previews when getting a preview of this compile rule.
        /// </summary>
        [Export(PropertyHint.MultilineText)] public string PreviewSeparator { get; private set; } = "\n";

        /* Constructors. */
        public ListRule() : base("", "", "") { }

        public ListRule(string id, string displayName, string description, CompileRule type, string addButtonText,
            string previewSeparator) : base(id, displayName, description)
        {
            Type = type;
            AddButtonText = addButtonText;
            PreviewSeparator = previewSeparator;

            ResourceName = ToString();
        }

        /* Public methods. */
        public override string ToString()
        {
            string str = "";
            if (Type != null)
                str = Type.ToString();
            return $"List({str})";
        }
    }
}