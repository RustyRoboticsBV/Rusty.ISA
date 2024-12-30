
namespace Rusty.Cutscenes
{
    public enum HideIf
    {
        /// <summary>
        /// The preview term is always visible.
        /// </summary>
        Never,
        /// <summary>
        /// The preview term is only visible if it follows another term, and that term is not empty.
        /// </summary>
        PrevIsEmpty,
        /// <summary>
        /// The preview term is only visible if it precedes another term, and that term is not empty.
        /// </summary>
        NextIsEmpty,
        /// <summary>
        /// The preview term is only visible if it is surrounded by other terms, and both of those terms are not empty.
        /// </summary>
        EitherIsEmpty,
        /// <summary>
        /// The preview term is visible, unless both surrounding terms either don't exist or are empty.
        /// </summary>
        BothAreEmpty
    }
}
