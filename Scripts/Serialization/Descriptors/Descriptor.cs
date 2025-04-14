namespace Rusty.ISA
{
    /// <summary>
    /// A base class for all resource descriptors. Used for serialization and deserialization.
    /// </summary>
    public abstract class Descriptor
    {
        /* Protected methods. */
        /// <summary>
        /// Return a copy of a string where each line has been indented.
        /// </summary>
        protected static string Indent(string str, string indentation = XmlKeywords.Indent)
        {
            return $"{indentation}{str.Replace("\n", $"\n{indentation}")}";
        }

        /// <summary>
        /// Return a copy of a string where each line has been indented twice.
        /// </summary>
        protected static string Indent2(string str)
        {
            return Indent(str, XmlKeywords.Indent2);
        }
    }
}