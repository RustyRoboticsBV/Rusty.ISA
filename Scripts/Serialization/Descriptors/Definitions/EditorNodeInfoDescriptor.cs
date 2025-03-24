﻿using Godot;
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
        public string Preview { get; set; } = "";
        public bool EnableWordWrap { get; set; } = false;

        /* Constructors. */
        public EditorNodeInfoDescriptor() { }

        /// <summary>
        /// Generate a descriptor from an editor node info.
        /// </summary>
        public EditorNodeInfoDescriptor(int priority, int minWidth, int minHeight, Color mainColor, Color textColor,
            string preview, bool enableWordWrap)
        {
            Priority = priority;
            MinWidth = minWidth;
            MinHeight = minHeight;
            MainColor = mainColor;
            TextColor = textColor;
            Preview = preview;
            EnableWordWrap = enableWordWrap;
        }

        /// <summary>
        /// Generate a descriptor from an editor node info.
        /// </summary>
        public EditorNodeInfoDescriptor(EditorNodeInfo node)
        {
            if (node != null)
            {
                Priority = node.Priority;
                MinWidth = node.MinWidth;
                MinHeight = node.MinHeight;
                MainColor = node.MainColor;
                TextColor = node.TextColor;
                Preview = node.Preview;
                EnableWordWrap = node.EnableWordWrap;
            }
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
                    if (element.Name == XmlKeywords.Priority)
                        Priority = Parser.ParseInt(element.InnerText);
                    else if (element.Name == XmlKeywords.MinWidth)
                        MinWidth = Parser.ParseInt(element.InnerText);
                    else if (element.Name == XmlKeywords.MinHeight)
                        MinHeight = Parser.ParseInt(element.InnerText);
                    else if (element.Name == XmlKeywords.MainColor)
                        MainColor = Parser.ParseColor(element.InnerText);
                    else if (element.Name == XmlKeywords.TextColor)
                        TextColor = Parser.ParseColor(element.InnerText);
                    else if (element.Name == XmlKeywords.Preview)
                        Preview = element.InnerText;
                    else if (element.Name == XmlKeywords.EnableWordWrap)
                        EnableWordWrap = Parser.ParseBool(element.InnerText);
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate an editor node info from this descriptor.
        /// </summary>
        public EditorNodeInfo Generate()
        {
            return new(Priority, MinWidth, MinHeight, MainColor, TextColor, Preview, EnableWordWrap);
        }

        /// <summary>
        /// Generate XML for this descriptor.
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            string mainColor = '#' + MainColor.ToHtml(MainColor.A < 1f);
            string textColor = '#' + TextColor.ToHtml(TextColor.A < 1f);

            string str = $"<{XmlKeywords.EditorNode}>";
            if (Priority != 0)
                str += $"\n  <{XmlKeywords.Priority}>{Priority}</{XmlKeywords.Priority}>";
            if (MinWidth != 128)
                str += $"\n  <{XmlKeywords.MinWidth}>{MinWidth}</{XmlKeywords.MinWidth}>";
            if (MinHeight != 32)
            str += $"\n  <{XmlKeywords.MinHeight}>{MinHeight}</{XmlKeywords.MinHeight}>";
            if (mainColor != "696969")
                str += $"\n  <{XmlKeywords.MainColor}>{mainColor}</{XmlKeywords.MainColor}>";
            if (textColor != "ffffff")
                str += $"\n  <{XmlKeywords.TextColor}>{textColor}</{XmlKeywords.TextColor}>";
            if (Preview != "")
                str += $"\n  <{XmlKeywords.Preview}>{Preview}</{XmlKeywords.Preview}>";
            if (EnableWordWrap)
                str += $"\n  <{XmlKeywords.EnableWordWrap}>{EnableWordWrap}</{XmlKeywords.EnableWordWrap}>";
            str += $"\n</{XmlKeywords.EditorNode}>";
            return str;
        }
    }
}