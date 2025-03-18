using Godot;
using System.Xml;
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

        public EditorNodeInfoDescriptor(XmlElement xml)
        {
            foreach (XmlNode child in xml.ChildNodes)
            {
                if (child is XmlElement element)
                {
                    if (element.Name == "priority")
                        Priority = Parser.ParseInt(element.InnerText);
                    if (element.Name == "min_width")
                        MinWidth = Parser.ParseInt(element.InnerText);
                    if (element.Name == "min_height")
                        MinHeight = Parser.ParseInt(element.InnerText);
                    if (element.Name == "main_color")
                        MainColor = Parser.ParseColor(element.InnerText);
                    if (element.Name == "text_color")
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