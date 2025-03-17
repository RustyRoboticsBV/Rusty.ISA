using Godot;
using System.Xml.Serialization;

namespace Rusty.ISA
{
    /// <summary>
    /// An editor node info, meant for serialization.
    /// </summary>
    public sealed class EditorNodeInfoDescriptor
    {
        /* Public properties. */
        [XmlElement("priority")]
        public int Priority { get; set; } = 0;
        [XmlElement("min_width")]
        public int MinWidth { get; set; } = 128;
        [XmlElement("min_height")]
        public int MinHeight { get; set; } = 32;
        [XmlElement("main_color")]
        public Color MainColor { get; set; } = Color.FromHtml("696969");
        [XmlElement("text_color")]
        public Color TextColor { get; set; } = Colors.White;

        /* Constructors. */
        public EditorNodeInfoDescriptor() { }

        /// <summary>
        /// Generate a descriptor from an editor node info.
        /// </summary>
        public EditorNodeInfoDescriptor(EditorNodeInfo node)
        {
            Priority = node.Priority;
            MinWidth = node.MinWidth;
            MinHeight = node.MinHeight;
            MainColor = node.MainColor;
            TextColor = node.TextColor;
        }

        /* Public methods. */
        /// <summary>
        /// Generate an editor node info from this descriptor.
        /// </summary>
        public EditorNodeInfo Generate()
        {
            return new(Priority, MinWidth, MinHeight, MainColor, TextColor);
        }
    }
}