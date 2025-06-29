using Godot;
using System.Xml.Linq;

namespace Rusty.ISA;

/// <summary>
/// The meta information of an instruction's associated editor node.
/// </summary>
[Tool, GlobalClass, XmlClass("node")]
public sealed partial class EditorNodeInfo : InstructionResource
{
    /* Public properties. */
    public static Color SelectedMainColor => Colors.Gray;
    public static Color SelectedTextColor => new Color(0.35f, 0.35f, 0.35f);

    /// <summary>
    /// The menu priority of this node in the "create node" menu. Determines how high in the menu the node will appear.
    /// </summary>
    [Export, XmlProperty("priority")] public int Priority { get; private set; } = 0;
    /// <summary>
    /// The minimum width of the editor node (in pixels).
    /// </summary>
    [Export, XmlProperty("min_width")] public int MinWidth { get; private set; } = 128;
    /// <summary>
    /// The minimum height of the editor node (in pixels).
    /// </summary>
    [Export, XmlProperty("min_height")] public int MinHeight { get; private set; } = 32;
    /// <summary>
    /// The background color of the editor node.
    /// </summary>
    [Export, XmlProperty("main_color")] public Color MainColor { get; private set; } = Colors.DimGray;
    /// <summary>
    /// The text color of the editor node.
    /// </summary>
    [Export, XmlProperty("text_color")] public Color TextColor { get; private set; } = Colors.White;
    /// <summary>
    /// An expression that defines how previews for this node will be generated. If left empty, then the preview of the
    /// associated instruction definition is used instead.
    /// </summary>
    [Export(PropertyHint.MultilineText), XmlProperty("preview")] public string Preview { get; private set; } = "";
    /// <summary>
    /// Whether or the editor node preview should automatically break between lines. This disables automatic horizontal
    /// resizing of the node.
    /// </summary>
    [Export, XmlProperty("word_wrap")] public bool EnableWordWrap { get; private set; } = false;

    /* Constructors. */
    public EditorNodeInfo() { }

    public EditorNodeInfo(int priority, int minWidth, int minHeight, Color mainColor, Color textColor, string preview,
        bool enableWordWrap)
    {
        Priority = priority;
        MinWidth = minWidth;
        MinHeight = minHeight;
        MainColor = mainColor;
        TextColor = textColor;
        Preview = preview;
        EnableWordWrap = enableWordWrap;

        ResourceName = ToString();
    }

    /* Public methods. */
    public override string ToString()
    {
        return GetType().Name;
    }
}