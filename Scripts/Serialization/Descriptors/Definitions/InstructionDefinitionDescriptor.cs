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
        public string FolderPath { get; set; } = "";

        // Definition.
        public string Opcode { get; set; } = "";
        public List<ParameterDescriptor> Parameters { get; set; } = new();
        public ImplementationDescriptor Implementation { get; set; } = null;

        // Metadata.
        public Texture2D IconTexture { get; set; }
        public string IconPath { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = null;

        // Editor.
        public EditorNodeInfoDescriptor EditorNodeInfo { get; set; } = null;
        public string Preview { get; set; } = "";
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
            Implementation = new(definition.Implementation);

            if (definition.Icon != null)
            {
                IconTexture = definition.Icon;
                IconPath = definition.Icon.ResourcePath;
            }
            DisplayName = definition.DisplayName;
            Description = definition.Description;
            Category = definition.Category;

            if (definition.EditorNode != null)
                EditorNodeInfo = new(definition.EditorNode);
            Preview = definition.Preview;
            foreach (CompileRule pre in definition.PreInstructions)
            {
                PreInstructions.Add(CompileRuleDescriptor.Create(pre));
            }
            foreach (CompileRule post in definition.PostInstructions)
            {
                PostInstructions.Add(CompileRuleDescriptor.Create(post));
            }
        }

        public InstructionDefinitionDescriptor(string opcode, List<ParameterDescriptor> parameters,
            ImplementationDescriptor implemementation, Texture2D iconTexture, string displayName, string description,
            string category, EditorNodeInfoDescriptor editorNodeInfo, string preview,
            List<CompileRuleDescriptor> preInstructions, List<CompileRuleDescriptor> postInstructions)
        {
            Opcode = opcode;
            Parameters = parameters;
            Implementation = implemementation;
            if (IconTexture != null)
            {
                IconTexture = iconTexture;
                IconPath = iconTexture.ResourcePath;
            }
            DisplayName = displayName;
            Description = description;
            Category = category;
            EditorNodeInfo = editorNodeInfo;
            Preview = preview;
            PreInstructions = preInstructions;
            PostInstructions = postInstructions;
        }

        public InstructionDefinitionDescriptor(string opcode, List<ParameterDescriptor> parameters,
            ImplementationDescriptor implemementation, string iconPath, string displayName, string description, string category,
            EditorNodeInfoDescriptor editorNodeInfo, string preview, List<CompileRuleDescriptor> preInstructions,
            List<CompileRuleDescriptor> postInstructions)
        {
            Opcode = opcode;
            Parameters = parameters;
            Implementation = implemementation;
            IconPath = iconPath;
            DisplayName = displayName;
            Description = description;
            Category = category;
            EditorNodeInfo = editorNodeInfo;
            Preview = preview;
            PreInstructions = preInstructions;
            PostInstructions = postInstructions;
        }

        /// <summary>
        /// Generate a descriptor for an instruction definition from XML.
        /// </summary>
        public InstructionDefinitionDescriptor(XmlDocument xml)
        {
            // Find root element.
            XmlElement root = null;
            foreach (XmlNode rootNode in xml.ChildNodes)
            {
                if (rootNode is XmlElement element && element.Name == XmlKeywords.InstructionDefinition)
                    root = element;
            }

            if (root == null)
            {
                GD.PrintErr($"Invalid XML file: the file had no \"{XmlKeywords.InstructionDefinition}\" root element.");
                return;
            }

            // Get opcodes.
            Opcode = root.GetAttribute(XmlKeywords.Opcode);

            // Parse elements...
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node is XmlElement element)
                {
                    // Parameters.
                    if (XmlKeywords.Parameters.Contains(element.Name))
                        Parameters.Add(ParameterDescriptor.Create(element));

                    // Implementation.
                    else if (element.Name == XmlKeywords.Implementation)
                        Implementation = new(element);

                    // Metadata.
                    else if (element.Name == XmlKeywords.Icon)
                        IconPath = element.InnerText;
                    else if (element.Name == XmlKeywords.DisplayName)
                        DisplayName = element.InnerText;
                    else if (element.Name == XmlKeywords.Description)
                        Description = element.InnerText;
                    else if (element.Name == XmlKeywords.Category)
                        Category = element.InnerText;

                    // Editor node info.
                    else if (element.Name == XmlKeywords.EditorNode)
                        EditorNodeInfo = new(element);

                    // Preview terms.
                    else if (element.Name == XmlKeywords.Preview)
                        Preview = element.InnerText;

                    // Pre-instructions.
                    else if (element.Name == XmlKeywords.PreInstructions)
                    {
                        foreach (XmlNode preNode in element.ChildNodes)
                        {
                            if (preNode is XmlElement preElement && XmlKeywords.CompileRules.Contains(preElement.Name))
                                PreInstructions.Add(CompileRuleDescriptor.Create(preElement));
                        }
                    }

                    // Post-instructions.
                    else if (element.Name == XmlKeywords.PostInstructions)
                    {
                        foreach (XmlNode postNode in element.ChildNodes)
                        {
                            if (postNode is XmlElement postElement && XmlKeywords.CompileRules.Contains(postElement.Name))
                                PostInstructions.Add(CompileRuleDescriptor.Create(postElement));
                        }
                    }
                }
            }
        }

        /* Public methods. */
        /// <summary>
        /// Generate an instruction definition from this descriptor.
        /// </summary>
        public InstructionDefinition Generate(bool iconIsAlphaTexture)
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
                parameters[i] = Parameters[i].Generate();
            }

            // Generate pre-instructions.
            CompileRule[] preInstructions = new CompileRule[PreInstructions.Count];
            for (int i = 0; i < preInstructions.Length; i++)
            {
                preInstructions[i] = PreInstructions[i].Generate();
            }

            // Generate post-instructions.
            CompileRule[] postInstructions = new CompileRule[PostInstructions.Count];
            for (int i = 0; i < postInstructions.Length; i++)
            {
                postInstructions[i] = PostInstructions[i].Generate();
            }

            // Create instruction definition.
            return new InstructionDefinition(Opcode, parameters, Implementation?.Generate(),
                iconTexture, DisplayName, Description, Category,
                EditorNodeInfo?.Generate(), Preview, preInstructions, postInstructions);
        }

        /// <summary>
        /// Generate XML from this descriptor.
        /// </summary>
        public string GetXml()
        {
            string str = $"<{XmlKeywords.InstructionDefinition} {XmlKeywords.Opcode}=\"{Opcode}\">";

            // Parameters.
            foreach (ParameterDescriptor parameter in Parameters)
            {
                str += $"\n  {parameter.GetXml().Replace("\n", "\n  ")}";
            }

            // Implementation.
            if (Implementation != null)
                str += $"\n  {Implementation.GetXml().Replace("\n", "\n  ")}";

            // Metadata.
            if (IconPath != "")
                str += $"\n  <{XmlKeywords.Icon}>{IconPath}</{XmlKeywords.Icon}>";
            if (DisplayName != "")
                str += $"\n  <{XmlKeywords.DisplayName}>{DisplayName}</{XmlKeywords.DisplayName}>";
            if (Description != "")
                str += $"\n  <{XmlKeywords.Description}>{Description}</{XmlKeywords.Description}>";
            if (Category != "")
                str += $"\n  <{XmlKeywords.Category}>{Category}</{XmlKeywords.Category}>";

            // Editor node info.
            if (EditorNodeInfo != null)
                str += $"\n  {EditorNodeInfo.GetXml().Replace("\n", "\n  ")}";

            // Preview terms.
            if (Preview != "")
                str += $"\n  <{XmlKeywords.Preview}>{Preview}</{XmlKeywords.Preview}>";

            // Pre-instructions.
            if (PreInstructions.Count > 0)
            {
                str += $"\n  <{XmlKeywords.PreInstructions}>";
                foreach (CompileRuleDescriptor rule in PreInstructions)
                {
                    str += $"\n    {rule.GetXml().Replace("\n", "\n    ")}";
                }
                str += $"\n  </{XmlKeywords.PreInstructions}>";
            }

            // Post-instructions.
            if (PostInstructions.Count > 0)
            {
                str += $"\n  <{XmlKeywords.PostInstructions}>";
                foreach (CompileRuleDescriptor rule in PostInstructions)
                {
                    str += $"\n    {rule.GetXml().Replace("\n", "\n    ")}";
                }
                str += $"\n  </{XmlKeywords.PostInstructions}>";
            }

            str += $"\n</{XmlKeywords.InstructionDefinition}>";
            return str;
        }
    }
}