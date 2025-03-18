using Godot;
using System.Collections.Generic;
using System.IO;

namespace Rusty.ISA
{
    /// <summary>
    /// An instruction definition descriptor. Used for serialization and deserialization.
    /// </summary>
    public sealed class InstructionDefinitionDescriptor
    {
        /* Public properties. */
        // Definition.
        public string Opcode { get; set; } = "";
        public List<ParameterDescriptor> Parameters { get; set; } = new();
        public Implementation Implementation { get; set; } = null;

        // Metadata.
        public string IconPath { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = null;

        // Editor.
        public EditorNodeInfoDescriptor EditorNodeInfo { get; set; } = null;
        public List<PreviewTerm> PreviewTerms { get; } = new();
        public List<CompileRule> PreInstructions { get; } = new();
        public List<CompileRule> PostInstructions { get; } = new();

        /* Constructors. */
        public InstructionDefinitionDescriptor() { }

        /// <summary>
        /// Generate a descriptor for an instruction definition.
        /// </summary>
        public InstructionDefinitionDescriptor(InstructionDefinition definition)
        {
            Opcode = definition.Opcode;
            foreach (Parameter parameter in definition.Parameters)
            {
                Parameters.Add(ParameterDescriptor.Create(parameter));
            }
            Implementation = definition.Implementation;

            IconPath = definition.Icon.ResourcePath;
            DisplayName = definition.DisplayName;
            Description = definition.Description;
            Category = definition.Category;

            EditorNodeInfo = new(definition.EditorNode);
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

            // Generate parameters.
            Parameter[] parameters = new Parameter[Parameters.Count];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = Parameters[i].Generate();
            }

            // Create instruction definition.
            return new InstructionDefinition(Opcode, parameters, Implementation,
                iconTexture, DisplayName, Description, Category,
                EditorNodeInfo.Generate(), PreviewTerms.ToArray(), PreInstructions.ToArray(), PostInstructions.ToArray());
        }

        public string GetXml()
        {
            string str = $"<definition opcode=\"{Opcode}\">";

            // Parameters.
            foreach (ParameterDescriptor parameter in Parameters)
            {
                str += "\n  " + parameter.GetXml().Replace("\n", "\n  ");
            }

            // Implementation.

            // Metadata.
            if (IconPath != "")
                str += $"\n  <icon>{IconPath}</icon>";
            if (DisplayName != "")
                str += $"\n  <name>{DisplayName}</name>";
            if (Description != "")
                str += $"\n  <desc>{Description}</desc>";
            if (Category != "")
                str += $"\n  <category>{Category}</category>";

            // Editor node info.
            if (EditorNodeInfo != null)
                str += "\n  " + EditorNodeInfo.GetXml().Replace("\n", "\n  ");

            str += "\n</definition>";
            return str;
        }
    }
}