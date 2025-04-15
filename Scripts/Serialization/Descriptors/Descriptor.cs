using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// A base class for all resource descriptors. Used for serialization and deserialization.
    /// </summary>
    public abstract class Descriptor
    {
        /* Protected methods. */
        /// <summary>
        /// Return a copy of a string that has been encapsulated in an XML block.
        /// </summary>
        protected string Encapsulate(string tag, object contents, bool forceMultiline = false)
        {
            string str = contents.ToString();
            if (forceMultiline || str.Contains('\n'))
                return $"<{tag}>\n{Indent(str)}\n</{tag}>";
            else
                return $"<{tag}>{str}</{tag}>";
        }

        /// <summary>
        /// Return a copy of a string where each line has been indented.
        /// </summary>
        protected static string Indent(string str, string indentation = XmlKeywords.Indent)
        {
            if (str.StartsWith('\n'))
                return str.Replace("\n", $"\n{indentation}");
            else
                return $"{indentation}{str.Replace("\n", $"\n{indentation}")}";
        }

        /// <summary>
        /// Return a copy of a string where each line has been indented twice.
        /// </summary>
        protected static string Indent2(string str)
        {
            return Indent(str, XmlKeywords.Indent2);
        }

        /// <summary>
        /// Generate a comment.
        /// </summary>
        protected static string Comment(string str)
        {
            return $"<!-- {str} -->";
        }
    }
}