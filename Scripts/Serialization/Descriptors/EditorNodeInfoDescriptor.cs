using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// An editor node info, meant for serialization.
    /// </summary>
    public sealed class EditorNodeInfoDescriptor
    {
        /* Public properties. */
        public int Priority { get; set; } = 0;
        public int MinWidth { get; set; } = 128;
        public int MinHeight { get; set; } = 32;
        public Color MainColor { get; set; } = Color.FromHtml("696969");
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