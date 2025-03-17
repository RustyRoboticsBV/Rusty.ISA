using Godot;
using System.Collections.Generic;
using System.IO;

namespace Rusty.ISA
{
    /// <summary>
    /// An instruction definition descriptor. It's mostly the same as the instruction definition, except its mutable and contains
    /// an icon path instead of an icon texture reference. Serves as an intermediary class during serialization and
    /// deserialization.
    /// </summary>
    public sealed class InstructionDefinitionDescriptor
    {
        /* Public properties. */
        // Definition.
        public string Opcode { get; set; } = "";
        public List<Parameter> Parameters { get; } = new();
        public Implementation Implementation { get; set; } = null;

        // Metadata.
        public string IconPath { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";

        // Editor.
        public EditorNodeInfo EditorNodeInfo { get; set; } = null;
        public List<PreviewTerm> PreviewTerms { get; } = new();
        public List<CompileRule> PreInstructions { get; } = new();
        public List<CompileRule> PostInstructions { get; } = new();

        /* Constructors. */
        /// <summary>
        /// Generate a definition descriptor from an instruction definition.
        /// </summary>
        public InstructionDefinitionDescriptor(InstructionDefinition definition)
        {
            Opcode = definition.Opcode;
            foreach (Parameter parameter in definition.Parameters)
            {
                Parameters.Add(parameter);
            }
            Implementation = definition.Implementation;

            IconPath = definition.Icon.ResourcePath;
            DisplayName = definition.DisplayName;
            Description = definition.Description;
            Category = definition.Category;

            EditorNodeInfo = definition.EditorNode;
            foreach (PreviewTerm term in definition.PreviewTerms)
            {
                PreviewTerms.Add(term);
            }
            foreach (CompileRule pre in definition.PreInstructions)
            {
                PreInstructions.Add(pre);
            }
            foreach (CompileRule post in definition.PostInstructions)
            {
                PreInstructions.Add(post);
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate an instruction definition from this descriptor.
        /// </summary>
        public InstructionDefinition Generate(bool makeIconTransparent)
        {
            // Get icon image.
            string globalIconPath = PathUtility.GetPath(IconPath);
            byte[] iconBytes = File.ReadAllBytes(IconPath);
            Image iconImage = new();
            iconImage.LoadPngFromBuffer(iconBytes);

            // Make icon image transparent (if enabled).
            if (makeIconTransparent)
            {
                for (int i = 0; i < iconImage.GetWidth(); i++)
                {
                    for (int j = 0; j < iconImage.GetHeight(); j++)
                    {
                        if (iconImage.GetPixel(i, j) == Colors.Black)
                            iconImage.SetPixel(i, j, Colors.Transparent);
                        else
                            iconImage.SetPixel(i, j, Colors.White);
                    }
                }
            }

            // Create icon texture.
            ImageTexture iconTexture = new ImageTexture();
            iconTexture.SetImage(iconImage);

            // Create instruction definition.
            return new InstructionDefinition(Opcode, Parameters.ToArray(), Implementation,
                iconTexture, DisplayName, Description, Category,
                EditorNodeInfo, PreviewTerms.ToArray(), PreInstructions.ToArray(), PostInstructions.ToArray());
        }
    }
}