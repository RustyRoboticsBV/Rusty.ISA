using System.Collections.Generic;

namespace Rusty.ISA
{
    /// <summary>
    /// The XML keywords that are used for instruction definition serialization and deserialization.
    /// </summary>
    public static class XmlKeywords
    {
        public const string InstructionDefinition = "definition";
        public const string Opcode = "opcode";

        public const string BoolParameter = "bool";
        public const string IntParameter = "int";
        public const string IntSliderParameter = "islider";
        public const string FloatParameter = "f";
        public const string FloatSliderParameter = "fslider";
        public const string CharParameter = "char";
        public const string TextlineParameter = "textline";
        public const string MultilineParameter = "multiline";
        public const string ColorParameter = "color";
        public const string OutputParameter = "output";
        public static List<string> Parameters => new(new string[]
        {
                BoolParameter,
                IntParameter,
                IntSliderParameter,
                FloatParameter,
                FloatSliderParameter,
                CharParameter,
                TextlineParameter,
                MultilineParameter,
                ColorParameter,
                OutputParameter
        });

        public const string ID = "id";
        public const string DefaultValue = "default";
        public const string MinValue = "min";
        public const string MaxValue = "max";
        public const string RemoveDefaultOutput = "remove_default";

        public const string Implementation = "impl";
        public const string Dependencies = "deps";
        public const string Members = "members";
        public const string Initialize = "init";
        public const string Execute = "exec";

        public const string Icon = "icon";
        public const string DisplayName = "name";
        public const string Description = "desc";
        public const string Category = "category";

        public const string EditorNode = "editor_node";

        public const string Priority = "priority";
        public const string MinWidth = "min_width";
        public const string MinHeight = "min_height";
        public const string MainColor = "main_color";
        public const string TextColor = "text_color";
        public const string EnableWordWrap = "word_wrap";

        public const string Preview = "preview";

        public const string PreInstructions = "pre";
        public const string PostInstructions = "post";

        public const string InstructionRule = "instr";
        public const string OptionRule = "option";
        public const string ChoiceRule = "choice";
        public const string TupleRule = "tuple";
        public const string ListRule = "list";
        public static List<string> CompileRules => new(new string[]
        {
                InstructionRule,
                OptionRule,
                ChoiceRule,
                TupleRule,
                ListRule
        });

        public const string DefaultEnabled = "enabled";
        public const string DefaultSelected = "selected";
        public const string AddButtonText = "button_text";
    }
}