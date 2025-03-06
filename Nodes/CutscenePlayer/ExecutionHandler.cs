using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// An utility for creating instruction execution handler nodes.
    /// </summary>
    public partial class ExecutionHandler : Resource
    {
        /* Public methods. */
        /// <summary>
        /// The instruction definition, used as an implementation source by the execution handler.
        /// </summary>
        [Export] public InstructionDefinition InstructionDefinition { get; private set; }

        /* Private constants. */
        /// <summary>
        /// The skeleton code that execution handler programs use.
        /// </summary>
        private const string SkeletonCode = "extends Node;" +
            "\n" +
            "\nvar player : CutscenePlayer;%MEMBERS%" +
            "\n" +
            "\nfunc _initialize(_player : CutscenePlayer):" +
            "\n\tplayer = _player;" +
            "\n%INITIALIZE%" +
            "\n" +
            "\nfunc _execute(_arguments : Array[String], _delta_time : float):" +
            "\n%EXECUTE%" +
            "\n" +
            "\nfunc end():" +
            "\n\tplayer.Stop();" +
            "\n" +
            "\nfunc goto(target_label : String):" +
            "\n\tplayer.Jump(target_label);" +
            "\n" +
            "\nfunc error(message : String):" +
            "\n\tplayer.Error(self, message);" +
            "\n" +
            "\nfunc get_register(register_name : String) -> Register:" +
            "\n\treturn player.GetRegister(register_name);" +
            "\n" +
            "\nfunc get_definition() -> InstructionDefinition:" +
            "\n\treturn player.InstructionSet.GetDefinition(%OPCODE%);" +
            "\n" +
            "\nfunc get_parameter_id(index : int) -> String:" +
            "\n\treturn get_definition().Parameters[index].ID;" +
            "\n" +
            "\nfunc get_parameter_index(id : String) -> int:" +
            "\n\treturn get_definition().GetParameterIndex(id);\n";

        /* Constructors. */
        public ExecutionHandler(InstructionDefinition instructionDefinition)
        {
            InstructionDefinition = instructionDefinition;
        }

        /* Public methods. */
        /// <summary>
        /// Generate a GDScript source code program from the instruction definition.
        /// </summary>
        public string GetSourceCode()
        {
            // Get implementations.
            string members = InstructionDefinition.Implementation.Members;
            if (members.Length > 0)
                members = "\n" + members;

            string initialize = ProcessCode(InstructionDefinition.Implementation.Initialize, InstructionDefinition);

            string execute = ProcessCode(InstructionDefinition.Implementation.Execute, InstructionDefinition);
            if (execute == "")
                execute = "\tpass;";

            // Generate program.
            string code = SkeletonCode
                .Replace("%OPCODE%", $"\"{InstructionDefinition.Opcode}\"")
                .Replace("%MEMBERS%", members)
                .Replace("%INITIALIZE%", initialize)
                .Replace("%EXECUTE%", execute);

            // Fix used function arguments.
            if (execute.Contains("arguments"))
                code = code.Replace("_arguments", "arguments");
            if (execute.Contains("delta_time"))
                code = code.Replace("_delta_time", "delta_time");

            return code;
        }

        /// <summary>
        /// Create an execution handler node for the instruction definition.
        /// </summary>
        public Node GetNode()
        {
            // Generate source code program.
            string code = GetSourceCode();

            // Create GDScript.
            GDScript script = new()
            {
                SourceCode = code
            };
            script.Reload();

            // Create node.
            Node node = (Node)script.New();
            return node;
        }

        /* Private methods. */
        /// <summary>
        /// Process the code of an implementation method. This adds indentation and replaces $register$ and %parameter%
        /// calls.
        /// </summary>
        private static string ProcessCode(string code, InstructionDefinition definition)
        {
            // Add indentation.
            code = "\t" + code.Replace("\n", "\n\t");

            // Replace OPCODE statements.
            code = code.Replace("OPCODE", $"\"{definition.Opcode}\"");

            // Replace $register$ statements.
            // TODO.

            // Replace %argument% statements.
            for (int i = 0; i < definition.Parameters.Length; i++)
            {
                string id = definition.Parameters[i].ID;
                code = code.Replace($"%{id}%", $"arguments[get_parameter_index(\"{id}\")]");
            }
            code = code.Replace("%OPCODE%", $"arguments[get_parameter_index(\"{definition.Opcode}\")]");

            return code;
        }
    }
}