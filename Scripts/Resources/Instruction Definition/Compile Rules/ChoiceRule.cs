using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A choice between various compile rules. Exactlty one of the options must be instantiated.
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
        /// The possible items that this container can compile to. Can include both instruction rules and container rules.
        /// </summary>
        [Export] public CompileRule[] Types { get; private set; } = new CompileRule[0];
        /// <summary>
        /// The item that is selected at start.
        /// </summary>
        [Export] public int StartSelected { get; private set; }

        /* Constructors. */
        public ChoiceRule() : base("", "", "") { }

        public ChoiceRule(string id, string displayName, string description, CompileRule[] types, int selected)
            : base(id, displayName, description)
        {
            Types = types;
            StartSelected = selected;

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