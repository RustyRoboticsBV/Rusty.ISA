using Godot;
using System.Collections.Generic;
using System.IO;
using System.Xml;

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
        public List<PreviewTermDescriptor> PreviewTerms { get; } = new();
        public List<CompileRuleDescriptor> PreInstructions { get; } = new();
        public List<CompileRuleDescriptor> PostInstructions { get; } = new();

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
                PreviewTerms.Add(new(term));
            }
            foreach (CompileRule pre in definition.PreInstructions)
            {
                PreInstructions.Add(CompileRuleDescriptor.Create(pre));
            }
            foreach (CompileRule post in definition.PostInstructions)
            {
                PostInstructions.Add(CompileRuleDescriptor.Create(post));
            }
        }

        /// <summary>
        /// Generate a descriptor for an instruction definition from XML.
        /// </summary>
        public InstructionDefinitionDescriptor(XmlElement xml)
        {
            Opcode = xml.GetAttribute("opcode");

            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node is XmlElement element)
                {
                    // Parameters.
                    if (element.Name == "bool" || element.Name == "int" || element.Name == "islider" || element.Name == "float"
                        || element.Name == "fslider" || element.Name == "char" || element.Name == "textline"
                        || element.Name == "multiline" || element.Name == "color" || element.Name == "output")
                    {
                        Parameters.Add(ParameterDescriptor.Create(element));
                    }

                    // Implementation.
                    else if (element.Name == "impl")
                    { }

                    // Metadata.
                    else if (element.Name == "icon")
                        IconPath = element.InnerText;
                    else if (element.Name == "name")
                        DisplayName = element.InnerText;
                    else if (element.Name == "desc")
                        Description = element.InnerText;
                    else if (element.Name == "category")
                        Category = element.InnerText;

                    // Editor node info.
                    else if (element.Name == "editor_node")
                        EditorNodeInfo = new(element);

                    // Preview terms.
                    else if (element.Name == "text_term" || element.Name == "arg_term" || element.Name == "rule_term")
                    {
                        PreviewTerms.Add(new(element));
                    }

                    // Pre-instructions.
                    else if (element.Name == "pre")
                    {
                        foreach (XmlNode preNode in element.ChildNodes)
                        {
                            if (preNode is XmlElement preElement && preElement.Name == "instruction"
                                 && preElement.Name == "option" && preElement.Name == "choice" && preElement.Name == "tuple"
                                 && preElement.Name == "list")
                            {
                                PreInstructions.Add(CompileRuleDescriptor.Create(preElement));
                            }
                        }
                    }

                    // Post-instructions.
                    else if (element.Name == "post")
                    {
                        foreach (XmlNode postNode in element.ChildNodes)
                        {
                            if (postNode is XmlElement postElement && postElement.Name == "instruction"
                                 && postElement.Name == "option" && postElement.Name == "choice" && postElement.Name == "tuple"
                                 && postElement.Name == "list")
                            {
                                PostInstructions.Add(CompileRuleDescriptor.Create(postElement));
                            }
                        }
                    }
                }
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

            // Generate preview terms.
            PreviewTerm[] previewTerms = new PreviewTerm[0];

            // Generate pre-instructions.
            CompileRule[] preInstructions = new CompileRule[PreInstructions.Count];
            for (int i = 0; i < preInstructions.Length; i++)
            {
                preInstructions[i] = PreInstructions[i].Generate();
            }

            // Generate post-instructions.
            CompileRule[] postInstructions = new CompileRule[PostInstructions.Count];
            for (int i = 0; i < preInstructions.Length; i++)
            {
                postInstructions[i] = PostInstructions[i].Generate();
            }

            // Create instruction definition.
            return new InstructionDefinition(Opcode, parameters, Implementation,
                iconTexture, DisplayName, Description, Category,
                EditorNodeInfo.Generate(), previewTerms, preInstructions, postInstructions);
        }

        /// <summary>
        /// Generate XML from this descriptor.
        /// </summary>
        public string GetXml()
        {
            string str = $"<definition opcode=\"{Opcode}\">";

            // Parameters.
            foreach (ParameterDescriptor parameter in Parameters)
            {
                str += "\n  " + parameter.GetXml().Replace("\n", "\n  ");
            }

            // Implementation.
            if (Implementation != null)
            { }

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

            // Preview terms.
            if (PreviewTerms.Count > 0)
            {
                foreach (PreviewTermDescriptor term in PreviewTerms)
                {
                    str += "\n  " + term.GetXml().Replace("\n", "\n  ");
                }
            }

            // Pre-instructions.
            if (PreInstructions.Count > 0)
            {
                str += "\n  <pre>";
                foreach (CompileRuleDescriptor rule in PreInstructions)
                {
                    str += "\n    " + rule.GetXml().Replace("\n", "\n    ");
                }
                str += "\n  </pre>";
            }

            // Post-instructions.
            if (PostInstructions.Count > 0)
            {
                str += "\n  <post>";
                foreach (CompileRuleDescriptor rule in PostInstructions)
                {
                    str += "\n    " + rule.GetXml().Replace("\n", "\n    ");
                }
                str += "\n  </post>";
            }

            str += "\n</definition>";
            return str;
        }
    }
}