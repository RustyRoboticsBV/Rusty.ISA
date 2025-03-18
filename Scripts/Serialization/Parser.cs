using Godot;
using System;

namespace Rusty.ISA
{
    /// <summary>
    /// An utility class for parsing values.
    /// </summary>
    public static class Parser
    {
        /* Public methods. */
        /// <summary>
        /// Try to parse a bool. Returns false if the value could not be parsed.
        /// </summary>
        public static bool ParseBool(string str)
        {
            if (bool.TryParse(str, out bool result))
                return result;
            return false;
        }

        /// <summary>
        /// Try to parse an integer. Returns 0 if the value could not be parsed.
        /// </summary>
        public static int ParseInt(string str)
        {
            if (int.TryParse(str, out int result))
                return result;
            return 0;
        }

        /// <summary>
        /// Try to parse a float. Returns 0 if the value could not be parsed.
        /// </summary>
        public static float ParseFloat(string str)
        {
            if (float.TryParse(str, out float result))
                return result;
            return 0;
        }

        /// <summary>
        /// Try to parse a char. Returns '0' if the value could not be parsed.
        /// </summary>
        public static char ParseChar(string str)
        {
            if (str.Length > 0)
                return str[0];
            return '0';
        }

        /// <summary>
        /// Try to parse a color. Returns (0, 0, 0, 0) if the value could not be parsed.
        /// </summary>
        public static Color ParseColor(string str)
        {
            try
            {
                return Color.FromHtml(str);
            }
            catch
            {
                try
                {
                    return ParseColorName(str);
                }
                catch
                {
                    return new Color(0f, 0f, 0f, 0f);
                }
            }
        }

        /// <summary>
        /// Try to parse a string array.
        /// </summary>
        public static string[] ParseStrings(string str)
        {
            return str.Split(',');
        }

        /* Private methods. */
        /// <summary>
        /// Parse a color name.
        /// </summary>
        private static Color ParseColorName(string colorName)
        {
            switch (colorName)
            {
                case "white":
                    return Color.FromHtml("FFFFFFFF");
                case "gray":
                case "grey":
                    return Color.FromHtml("808080FF");
                case "black":
                    return Color.FromHtml("000000FF");
                case "red":
                    return Color.FromHtml("FF0000FF");
                case "orange":
                    return Color.FromHtml("FF8000FF");
                case "yellow":
                    return Color.FromHtml("FFFF00FF");
                case "lime":
                case "chartreuse":
                    return Color.FromHtml("80FF00FF");
                case "green":
                    return Color.FromHtml("00FF00FF");
                case "mint":
                case "spring_green":
                    return Color.FromHtml("00FF80FF");
                case "cyan":
                    return Color.FromHtml("00FFFFFF");
                case "azure":
                    return Color.FromHtml("0080FFFF");
                case "blue":
                    return Color.FromHtml("0000FFFF");
                case "violet":
                    return Color.FromHtml("8000FFFF");
                case "magenta":
                    return Color.FromHtml("FF00FFFF");
                case "rose":
                    return Color.FromHtml("FF0080FF");
                case "brown":
                    return Color.FromHtml("804000FF");
                case "purple":
                    return Color.FromHtml("C000FFFF");
                case "pink":
                    return Color.FromHtml("FFBFDFFF");
                case "clear":
                case "transparent":
                    return Color.FromHtml("00000000");
                default:
                    throw new ArgumentException($"The color name '{colorName}' does not correspond to a valid color.");
            }
        }
    }
}
