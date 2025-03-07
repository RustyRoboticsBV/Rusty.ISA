using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A tuple of compile rules.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class TupleRule : CompileRule
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";

        /// <summary>
        /// The items contained within this tuple. Can include both instruction rules and container rules.
        /// </summary>
        [Export] public CompileRule[] Types { get; private set; } = new CompileRule[0];

        /// <summary>
        /// The separation string that is inserted between element previews when getting a preview of this compile rule.
        /// </summary>
        [Export(PropertyHint.MultilineText)] public string PreviewSeparator { get; private set; } = "\n";

        /* Constructors. */
        public TupleRule() : base("", "", "") { }

        public TupleRule(string id, string displayName, string description, CompileRule[] types, string previewSeparator)
            : base(id, displayName, description)
        {
            Types = types;
            PreviewSeparator = previewSeparator;

            ResourceName = ToString();
        }

        /* Public methods. */
        public override string ToString()
        {
            string str = "";
            foreach (CompileRule type in Types)
            {
                if (str != "")
                    str += ", ";
                if (type != null)
                    str += type.ToString();
            }
            return $"Tuple({str})";
        }
    }
}