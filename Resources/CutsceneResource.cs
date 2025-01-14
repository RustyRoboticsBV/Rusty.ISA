using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A base class for all cutscene resources. Has no function and exists purely to group all such resources together in the
    /// editor.
    /// </summary>
    [GlobalClass]
    [Icon("CutsceneResource.svg")]
    public abstract partial class CutsceneResource : Resource { }
}
