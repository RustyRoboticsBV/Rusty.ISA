using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// The definition of a cutscene instruction.
    /// </summary>
    [Tool]
    [GlobalClass]
    public sealed partial class InstructionDefinition : CutsceneResource
    {
        /* Public properties. */
        // Main.
        /// <summary>
        /// The opcode of this instruction. This is the main identifier of an instruction, used in the cutscene program files
        /// that the editor generates. Should be a short as possible. Make sure each instruction's opcode is fully unique!
        /// </summary>
        [Export] public string Opcode { get; private set; } = "";
        /// <summary>
        /// The parameters of this instruction.
        /// </summary>
        [Export] public Parameter[] Parameters { get; private set; } = new Parameter[0];
        /// <summary>
        /// The implementation of this instruction (in GDScript).
        /// </summary>
        [Export(PropertyHint.MultilineText)] public Implementation Implementation { get; private set; }

        // Metadata.
        /// <summary>
        /// The icon of this instruction, used in the cutscene editor.
        /// </summary>
        [Export] public Texture2D Icon { get; private set; }
        /// <summary>
        /// The human-readable name of this instruction that is used in the cutscene editor.
        /// </summary>
        [Export] public string DisplayName { get; private set; } = "";
        /// <summary>
        /// A description of this instruction. Used for documentation generation, and as a tooltip of the corresponding graph
        /// editor node, should this instruction have one.
        /// </summary>
        [Export(PropertyHint.MultilineText)] public string Description { get; private set; } = "";
        /// <summary>
        /// The category of the instruction. Gets used to group instructions together in the editor and documentation generation.
        /// </summary>
        [Export] public string Category { get; private set; } = "";

        // Editor.
        /// <summary>
        /// Contains information related to this instruction's graph node.
        /// When instantiated, this allows this instruction to be placed as a node in the cutscene graph editor.
        /// Leave this empty if this instruction should only appear as a pre-instruction of another instruction.
        /// </summary>
        [Export] public EditorNodeInfo EditorNode { get; private set; }
        /// <summary>
        /// A list of rules that tell the editor how to generate node previews.
        /// </summary>
        [Export] public PreviewTerm[] Preview { get; private set; } = new PreviewTerm[0];
        /// <summary>
        /// Defines rules for how the editor may create additional instructions before instructions of this type.
        /// </summary>
        [Export] public CompileRule[] PreInstruction { get; private set; } = new CompileRule[0];
        /// <summary>
        /// Defines rules for how the editor may create additional instructions after instructions of this type.
        /// </summary>
        [Export] public CompileRule[] PostInstruction { get; private set; } = new CompileRule[0];

        /* Constructors. */
        public InstructionDefinition() { }

        public InstructionDefinition(string opcode, Parameter[] parameters, Implementation implementation,
            Texture2D icon, string displayName, string description, string category,
            EditorNodeInfo editorNode, PreviewTerm[] preview, CompileRule[] preInstructions, CompileRule[] postInstructions)
        {
            if (parameters == null)
                parameters = new Parameter[0];
            if (preview == null)
                preview = new PreviewTerm[0];
            if (preInstructions == null)
                preInstructions = new CompileRule[0];
            if (postInstructions == null)
                postInstructions = new CompileRule[0];

            Opcode = opcode;
            Parameters = parameters;
            Implementation = implementation;
            Icon = icon;
            DisplayName = displayName;
            Description = description;
            Category = category;
            EditorNode = editorNode;
            Preview = preview;
            PreInstruction = preInstructions;
            PostInstruction = postInstructions;

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
        /// Find the index of a parameter, using its name.
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
    }
}