using Godot;

namespace Rusty.ISA;

/// <summary>
/// A base class for all compile rules. Compile rules define secondary instructions that can be generated if the owner of
/// said rule appears in an editor node. These extra instructions can either appear before or after the main instruction.
/// Compile rules have no in-game meaning.
/// </summary>
[GlobalClass]
public abstract partial class CompileRule : InstructionResource
{
    /* Public properties. */
    /// <summary>
    /// The identifier of this compile rule, with which it can be refenced.
    /// </summary>
    public abstract string ID { get; protected set; }
    /// <summary>
    /// The name of this compile rule in the editor.
    /// </summary>
    public abstract string DisplayName { get; protected set; }
    /// <summary>
    /// The description of this compile rule in the editor.
    /// </summary>
    public abstract string Description { get; protected set; }
    /// <summary>
    /// An expression that defines how previews will be generated for this compile rule. If left empty, the resulting
    /// behavior depends on the type of rule.
    /// </summary>
    public abstract string Preview { get; protected set; }

    /* Constructors. */
    public CompileRule() : base() { }

    public CompileRule(string id, string displayName, string description, string preview) : base()
    {
        ID = id;
        DisplayName = displayName;
        Description = description;
        Preview = preview;
    }
}