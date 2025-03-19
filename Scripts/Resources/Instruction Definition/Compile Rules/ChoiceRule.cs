using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// A choice between various compile rules. It generates to exactly one compile rule.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class ChoiceRule : CompileRule
    {
        /* Public properties. */
        [Export] public override string ID { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";
        /// <summary>
        /// The possible rules that can be chosen.
        /// </summary>
        [Export] public CompileRule[] Types { get; private set; } = new CompileRule[0];
        /// <summary>
        /// The item that is selected by default.
        /// </summary>
        [Export] public int DefaultSelected { get; private set; }
        /// <summary>
        /// An expression that defines how previews will be generated for this compile rule. If left empty, the preview of the
        /// selected choice will be generated.
        /// </summary>
        [Export(PropertyHint.MultilineText)] public override string Preview { get; protected set; }

        /* Constructors. */
        public ChoiceRule() : base() { }

        public ChoiceRule(string id, string displayName, string description, CompileRule[] types, int defaultSelected,
            string preview) : base(id, displayName, description, preview)
        {
            Types = types;
            DefaultSelected = defaultSelected;

            ResourceName = ToString();
        }

        /* Public methods. */
        public override string ToString()
        {
            string str = "";
            foreach (CompileRule type in Types)
            {
                if (str != "")
                    str += " / ";
                if (type != null)
                    str += type.ToString();
            }
            return $"Choice({str})";
        }
    }
}