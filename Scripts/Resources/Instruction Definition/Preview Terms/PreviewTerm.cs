using Godot;

namespace Rusty.ISA
{
	/// <summary>
	/// A base class for all preview rules. Preview rules allow you to specify how an editor node should draw its preview.
	/// </summary>
	[GlobalClass]
	public abstract partial class PreviewTerm : InstructionResource
	{
		/* Public properties. */
		[Export] public HideIf HideIf { get; private set; } = HideIf.Never;

		/* Constructors. */
		public PreviewTerm() { }

		public PreviewTerm(HideIf visibility)
		{
			HideIf = visibility;
		}
	}
}