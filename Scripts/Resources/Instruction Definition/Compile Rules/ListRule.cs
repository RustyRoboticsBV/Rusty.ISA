using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A list of compile rules, where each entry is of the same instruction type.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class ListRule : CompileRule
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";

        /// <summary>
        /// The type of element that is contained in this list. Can be an instruction rule or another container rule.
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