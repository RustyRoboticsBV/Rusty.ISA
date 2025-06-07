using Godot;
using System.Collections.Generic;

namespace Rusty.ISA;

/// <summary>
/// An instruction definition descriptor.
/// </summary>
[ResourceDescriptor(typeof(InstructionDefinition), "definition")]
public sealed class InstructionDefinitionDescriptor : Descriptor
{
    /* Public properties. */
    public string FolderPath { get; set; } = "";

    // Definition.
    [XmlProperty("opcode")] public string Opcode { get; set; } = "";
    [XmlProperty("params")] public List<ParameterDescriptor> Parameters { get; set; } = new();
    [XmlProperty("impl")] public ImplementationDescriptor Implementation { get; set; } = null;

    // Metadata.
    public Texture2D IconTexture { get; set; }
    [XmlProperty("icon")] public string IconPath { get; set; } = "";
    [XmlProperty("name")] public string DisplayName { get; set; } = "";
    [XmlProperty("desc")] public string Description { get; set; } = "";
    [XmlProperty("category")] public string Category { get; set; } = null;

    // Editor.
    [XmlProperty("editor_node")] public EditorNodeInfoDescriptor EditorNodeInfo { get; set; } = null;
    [XmlProperty("preview")] public string Preview { get; set; } = "";
    [XmlProperty("pre")] public List<CompileRuleDescriptor> PreInstructions { get; } = new();
    [XmlProperty("post")] public List<CompileRuleDescriptor> PostInstructions { get; } = new();

    /* Public methods. */
    public override InstructionDefinition GenerateObject()
    {
        return GenerateObject(true);
    }

    /// <summary>
    /// Generate an object from this descriptor.
    /// </summary>
    public InstructionDefinition GenerateObject(bool iconIsAlphaTexture)
    {
        // Get icon.
        Texture2D iconTexture = IconTexture;
        if (iconTexture == null)
        {
            string iconPath = "";
            if (FolderPath != "")
                iconPath = FolderPath + "/" + IconPath;
            else
                iconPath = PathUtility.GetPath(IconPath);

            iconTexture = IconLoader.Load(iconPath, iconIsAlphaTexture);
        }
        else if (iconIsAlphaTexture)
        {
            GD.Print(Opcode);
            iconTexture = IconLoader.AlphaToColor(iconTexture);
        }

        // Generate parameters.
        Parameter[] parameters = new Parameter[Parameters.Count];
        for (int i = 0; i < parameters.Length; i++)
        {
            parameters[i] = Parameters[i].GenerateObject();
        }

        // Generate pre-instructions.
        CompileRule[] preInstructions = new CompileRule[PreInstructions.Count];
        for (int i = 0; i < preInstructions.Length; i++)
        {
            preInstructions[i] = PreInstructions[i].GenerateObject();
        }

        // Generate post-instructions.
        CompileRule[] postInstructions = new CompileRule[PostInstructions.Count];
        for (int i = 0; i < postInstructions.Length; i++)
        {
            postInstructions[i] = PostInstructions[i].GenerateObject();
        }

        // Create instruction definition.
        return new InstructionDefinition(Opcode, parameters, Implementation?.GenerateObject(),
            iconTexture, DisplayName, Description, Category,
            EditorNodeInfo?.GenerateObject(), Preview, preInstructions, postInstructions);
    }
}