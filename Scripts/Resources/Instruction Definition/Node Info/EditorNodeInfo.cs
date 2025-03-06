using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// The meta information of an instruction's associated editor node.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class EditorNodeInfo : CutsceneResource
    {
        /* Public properties. */
        public static Color SelectedMainColor => Colors.Gray;
        public static Color SelectedTextColor => new Color(0.35f, 0.35f, 0.35f);

        /// <summary>
        /// The menu priority of this node in the "create node" menu. Determines how high in the menu the node will appear.
        /// </summary>
        [Export] public int Priority { get; private set; } = -1;
        /// <summary>
        /// The minimum width of the editor node (in pixels).
        /// </summary>
        [Export] public int MinWidth { get; private set; } = 128;
        /// <summary>
        /// The background color of the editor node.
        /// </summary>
        [Export] public Color MainColor { get; private set; } = Colors.DimGray;
        /// <summary>
        /// The text color of the editor node.
        /// </summary>
        [Export] public Color TextColor { get; private set; } = Colors.White;

        /* Constructors. */
        public EditorNodeInfo() { }

        public EditorNodeInfo(int priority, int minWidth, Color mainColor, Color textColor)
        {
            Priority = priority;
            MinWidth = minWidth;
            MainColor = mainColor;
            TextColor = textColor;
        }
    }
}