using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A pre-instruction container that can be freely enabled or disabled by the user.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class OptionRule : CompileRule
    {
        /* Public properties. */
        [Export] public override string Id { get; protected set; } = "";
        [Export] public override string DisplayName { get; protected set; } = "";
        [Export(PropertyHint.MultilineText)] public override string Description { get; protected set; } = "";

        /// <summary>
        /// The thing that can be toggled on and off. Can be a pre-instruction or another container.
        /// </summary>
        [Export] public CompileRule Type { get; private set; }
        /// <summary>
        /// Whether the toggle starts as enabled or not.
        /// </summary>
        [Export] public bool StartEnabled { get; private set; }

        /* Constructors. */
        public OptionRule() : base("", "", "") { }

        public OptionRule(string id, string displayName, string description, CompileRule target, bool startEnabled)
            : base(id, displayName, description)
        {
            Type = target;
            StartEnabled = startEnabled;

            ResourceName = ToString();
        }

        public override string ToString()
        {
            string str = "";
            if (Type != null)
                str = Type.ToString();
            return $"Option({str})";
        }
    }
}