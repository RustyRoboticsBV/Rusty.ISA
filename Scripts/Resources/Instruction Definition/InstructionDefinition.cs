using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection.Emit;

namespace Rusty.ISA;

/// <summary>
/// The definition of an instruction.
/// </summary>
[Tool, GlobalClass, XmlClass("definition")]
public sealed partial class InstructionDefinition : InstructionResource
{
    /* Public properties. */
    // Main.
    /// <summary>
    /// The opcode of this instruction. This is the main identifier of an instruction, used in the program files
    /// that the editor generates. Should be a short as possible. Make sure each instruction's opcode is fully unique!
    /// </summary>
    [Export, XmlProperty("opcode")] public string Opcode { get; private set; } = "";
    /// <summary>
    /// The parameters of this instruction.
    /// </summary>
    [Export, XmlProperty("params")] public Parameter[] Parameters { get; private set; } = [];
    /// <summary>
    /// The implementation of this instruction (in GDScript).
    /// </summary>
    [Export(PropertyHint.MultilineText), XmlProperty("impl")] public Implementation Implementation { get; private set; }

    // Metadata.
    /// <summary>
    /// The icon of this instruction, used in the graph editor.
    /// </summary>
    [Export, XmlProperty("icon")] public Texture2D Icon { get; private set; }
    /// <summary>
    /// The human-readable name of this instruction that is used in the graph editor.
    /// </summary>
    [Export, XmlProperty("name")] public string DisplayName { get; private set; } = "";
    /// <summary>
    /// A description of this instruction. Used for documentation generation, and as a tooltip of the corresponding graph
    /// editor node, should this instruction have one.
    /// </summary>
    [Export(PropertyHint.MultilineText), XmlProperty("desc")] public string Description { get; private set; } = "";
    /// <summary>
    /// The category of the instruction. Gets used to group instructions together in the editor and documentation generation.
    /// </summary>
    [Export, XmlProperty("category")] public string Category { get; private set; } = "";

    // Editor.
    /// <summary>
    /// Contains information related to this instruction's graph node.
    /// When instantiated, this allows this instruction to be placed as a node in the graph editor.
    /// Leave this empty if this instruction should only appear as a pre-instruction of another instruction.
    /// </summary>
    [Export, XmlProperty("node")] public EditorNodeInfo EditorNode { get; private set; }

    /// <summary>
    /// An expression that defines how previews for this instruction will be generated. If left empty, the empty string is
    /// generated.
    /// </summary>
    [Export(PropertyHint.MultilineText), XmlProperty("preview")] public string Preview { get; private set; } = "";

    /// <summary>
    /// Defines rules for how the editor may create additional instructions before instructions of this type.
    /// </summary>
    [Export, XmlProperty("pre")] public CompileRule[] PreInstructions { get; private set; } = [];
    /// <summary>
    /// Defines rules for how the editor may create additional instructions after instructions of this type.
    /// </summary>
    [Export, XmlProperty("post")] public CompileRule[] PostInstructions { get; private set; } = [];

    /* Private properties. */
    private Dictionary<string, Parameter> ParameterLookup { get; set; }
    private Dictionary<string, CompileRule> PreInstructionLookup { get; set; }
    private Dictionary<string, CompileRule> PostInstructionLookup { get; set; }

    /* Constructors. */
    public InstructionDefinition() { }

    public InstructionDefinition(string opcode, Parameter[] parameters, Implementation implementation,
        Texture2D icon, string displayName, string description, string category,
        EditorNodeInfo editorNode, string preview, CompileRule[] preInstructions, CompileRule[] postInstructions)
    {
        if (opcode == null)
            opcode = "";
        if (displayName == null)
            displayName = "";
        if (description == null)
            description = "";
        if (category == null)
            category = "";
        if (preview == null)
            preview = "";

        if (parameters == null)
            parameters = [];
        if (preInstructions == null)
            preInstructions = [];
        if (postInstructions == null)
            postInstructions = [];

        Opcode = opcode;
        Parameters = parameters;
        Implementation = implementation;
        Icon = icon;
        DisplayName = displayName;
        Description = description;
        Category = category;
        EditorNode = editorNode;
        Preview = preview;
        PreInstructions = preInstructions;
        PostInstructions = postInstructions;

        ResourceName = ToString();
    }

    /* Public methods. */
    public override string ToString()
    {
        // Add opcode.
        string str = Opcode + "(";

        // Add parameters.
        if (Parameters != null)
        {
            for (int i = 0; i < Parameters.Length; i++)
            {
                if (i > 0)
                    str += ", ";
                str += Parameters[i].ID;
            }
        }
        str += ")";

        return str;
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() == typeof(InstructionDefinition))
            return ((InstructionDefinition)obj).Opcode == Opcode;
        return false;
    }

    public override int GetHashCode()
    {
        return Opcode.GetHashCode();
    }

    /// <summary>
    /// Find the index of a parameter, using its ID.
    /// </summary>
    public int GetParameterIndex(string id)
    {
        for (int i = 0; i < Parameters.Length; i++)
        {
            if (Parameters[i].ID == id)
                return i;
        }
        return -1;
    }

    /// <summary>
    /// Get a parameter with some ID.
    /// </summary>
    public Parameter GetParameter(string id)
    {
        // Remove tabs and line-breaks.
        id = FixID(id);

        // Ensure that the look-up table exits.
        EnsureParameterLookup();

        // Retrieve the parameter.
        if (ParameterLookup.ContainsKey(id))
            return ParameterLookup[id];
        else
        {
            throw new ArgumentException($"Tried to get parameter with ID '{id}', but it did not exist on instruction "
                + $"'{Opcode}'!");
        }
    }

    /// <summary>
    /// Check if this instruction removes the default output from editor nodes that contain it.
    /// </summary>
    public bool RemovesDefaultOutput()
    {
        for (int i = 0; i < Parameters.Length; i++)
        {
            if (Parameters[i] is OutputParameter output && output.RemoveDefaultOutput)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Find the index of a pre-instruction, using its ID.
    /// </summary>
    public int GetPreInstructionIndex(string id)
    {
        for (int i = 0; i < PreInstructions.Length; i++)
        {
            if (PreInstructions[i].ID == id)
                return i;
        }
        return -1;
    }

    /// <summary>
    /// Get a pre-instruction with some ID.
    /// </summary>
    public CompileRule GetPreInstruction(string id)
    {
        // Remove tabs and line-breaks.
        id = FixID(id);

        // Ensure that the look-up table exits.
        EnsurePreInstructionLookup();

        // Retrieve the pre-instruction.
        if (PreInstructionLookup.ContainsKey(id))
            return PreInstructionLookup[id];
        else
        {
            throw new ArgumentException($"Tried to get pre-instruction with ID '{id}', but it did not exist on instruction "
                + $"'{Opcode}'!");
        }
    }

    /// <summary>
    /// Find the index of a post-instruction, using its ID.
    /// </summary>
    public int GetPostInstructionIndex(string id)
    {
        for (int i = 0; i < PostInstructions.Length; i++)
        {
            if (PostInstructions[i].ID == id)
                return i;
        }
        return -1;
    }

    /// <summary>
    /// Get a post-instruction with some ID.
    /// </summary>
    public CompileRule GetPostInstruction(string id)
    {
        // Remove tabs and line-breaks.
        id = FixID(id);

        // Ensure that the look-up table exits.
        EnsurePostInstructionLookup();

        // Retrieve the post-instruction.
        if (PostInstructionLookup.ContainsKey(id))
            return PostInstructionLookup[id];
        else
        {
            throw new ArgumentException($"Tried to get post-instruction with ID '{id}', but it did not exist on instruction "
                + $"'{Opcode}'!");
        }
    }

    /* Private methods. */
    /// <summary>
    /// Make sure that the parameter lookup table exists and is properly set up.
    /// </summary>
    private void EnsureParameterLookup()
    {
        if (ParameterLookup != null)
            return;

        ParameterLookup = new();
        foreach (Parameter parameter in Parameters)
        {
            string id = FixID(parameter.ID);
            if (!ParameterLookup.ContainsKey(id))
                ParameterLookup.Add(id, parameter);
            else
            {
                GD.PrintErr($"Duplicate rule ID '{id}' detected in instruction '{Opcode}'! This rule will not be "
                    + "discoverable!");
            }
        }
    }

    /// <summary>
    /// Make sure that the pre-instruction lookup table exists and is properly set up.
    /// </summary>
    private void EnsurePreInstructionLookup()
    {
        if (PreInstructionLookup != null)
            return;

        PreInstructionLookup = new();
        foreach (CompileRule rule in PreInstructions)
        {
            string id = FixID(rule.ID);
            if (!PreInstructionLookup.ContainsKey(id))
                PreInstructionLookup.Add(id, rule);
            else
            {
                GD.PrintErr($"Duplicate pre-instruction ID '{id}' detected in instruction '{Opcode}'! This pre-instruction will "
                    + "not be discoverable!");
            }
        }
    }

    /// <summary>
    /// Make sure that the post-instruction lookup table exists and is properly set up.
    /// </summary>
    private void EnsurePostInstructionLookup()
    {
        if (PostInstructionLookup != null)
            return;

        PostInstructionLookup = new();
        foreach (CompileRule rule in PostInstructions)
        {
            string id = FixID(rule.ID);
            if (!PostInstructionLookup.ContainsKey(id))
                PostInstructionLookup.Add(id, rule);
            else
            {
                GD.PrintErr($"Duplicate post-instruction ID '{id}' detected in instruction '{Opcode}'! This post-instruction "
                    + "will not be discoverable!");
            }
        }
    }

    /// <summary>
    /// Remove tabs and line-breaks from an ID.
    /// </summary>
    private static string FixID(string opcode)
    {
        return opcode.Replace("\n", "")
            .Replace("\r", "")
            .Replace("\t", "")
            .Replace(" ", "");
    }
}