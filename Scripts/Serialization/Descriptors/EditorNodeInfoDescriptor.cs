using Godot;
using System.Xml;

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
        public EditorNodeInfoDescriptor(int priority, int minWidth, int minHeight, Color mainColor, Color textColor)
        {
            Priority = priority;
            MinWidth = minWidth;
            MinHeight = minHeight;
            MainColor = mainColor;
            TextColor = textColor;
        }

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

        /// <summary>
        /// Generate a descriptor from an XML element.
        /// </summary>
        public EditorNodeInfoDescriptor(XmlElement xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == "priority")
                        Priority = Parser.ParseInt(element.InnerText);
                    else if (element.Name == "min_width")
                        MinWidth = Parser.ParseInt(element.InnerText);
                    else if (element.Name == "min_height")
                        MinHeight = Parser.ParseInt(element.InnerText);
                    else if (element.Name == "main_color")
                        MainColor = Parser.ParseColor(element.InnerText);
                    else if (element.Name == "text_color")
                        TextColor = Parser.ParseColor(element.InnerText);
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate an editor node info from this descriptor.
        /// </summary>
        public EditorNodeInfo Generate()
        {
            return new(Priority, MinWidth, MinHeight, MainColor, TextColor);
        }

        /// <summary>
        /// Generate XML for this descriptor.
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            string mainColor = MainColor.ToHtml(MainColor.A < 1f);
            string textColor = TextColor.ToHtml(TextColor.A < 1f);

            string str = "<editor_node>";
            if (Priority != 0)
                str += $"\n  <priority>{Priority}</priority>";
            if (MinWidth != 128)
                str += $"\n  <min_width>{MinWidth}</min_width>";
            if (MinHeight != 32)
            str += $"\n  <min_height>{MinHeight}</min_height>";
            if (mainColor != "696969")
                str += $"\n  <main_color>#{mainColor}</main_color>";
            if (textColor != "ffffff")
                str += $"\n  <text_color>#{textColor}</text_color>";
            str += "\n</editor_node>";
            return str;
        }
    }
}