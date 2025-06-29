using Godot;

namespace Rusty.ISA;

/// <summary>
/// An editor node info, meant for serialization.
/// </summary>
[ResourceDescriptor(typeof(EditorNodeInfo), "editor_node")]
public sealed class EditorNodeInfoDescriptor : Descriptor
{
    /* Public properties. */
    [XmlProperty("priority")] public int Priority { get; set; } = 0;
    [XmlProperty("min_width")] public int MinWidth { get; set; } = 128;
    [XmlProperty("min_height")] public int MinHeight { get; set; } = 32;
    [XmlProperty("main_color")] public Color MainColor { get; set; } = Color.FromHtml("696969");
    [XmlProperty("text_color")] public Color TextColor { get; set; } = Colors.White;
    [XmlProperty("preview")] public string Preview { get; set; } = "";
    [XmlProperty("word_wrap")] public bool EnableWordWrap { get; set; } = false;

    /* Public methods. */
    public override EditorNodeInfo GenerateObject()
    {
        return new(Priority, MinWidth, MinHeight, MainColor, TextColor, Preview, EnableWordWrap);
    }
}