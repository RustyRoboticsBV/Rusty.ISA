using Godot;

namespace Rusty.ISA;

/// <summary>
/// A base class for all instruction resources. Has no function, and exists purely to group all such resources together in
/// the "create new resource" window.
/// </summary>
[GlobalClass, Icon("InstructionResource.svg")]
public abstract partial class InstructionResource : Resource { }