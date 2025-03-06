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
            "\nconst OPCODE : String = %OPCODE%;" +
            "\nconst PARAMETERS : Array[String] = %PARAMETERS%;" +
            "\nconst PARAMETER_COUNT : int = %PCOUNT%;" +
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
            "\nfunc warning(message : String):" +
            "\n\tplayer.Warning(message);" +
            "\n" +
            "\nfunc error(message : String):" +
            "\n\tplayer.Error(message);" +
            "\n" +
            "\nfunc get_register(register_name : String) -> Register:" +
            "\n\treturn player.GetRegister(register_name);" +
            "\n" +
            "\nfunc get_parameter_id(index : int) -> String:" +
            "\n\treturn player.InstructionSet.GetDefinition(OPCODE).Parameters[index].ID;" +
            "\n" +
            "\nfunc get_parameter_index(id : String) -> int:" +
            "\n\treturn player.InstructionSet.GetDefinition(OPCODE).GetParameterIndex(id);";

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

            // List parameters.
            string parameters = "";
            foreach (ParameterDefinition parameter in InstructionDefinition.Parameters)
            {
                if (parameters.Length > 0)
                    parameters += ", ";
                parameters += $"\"{parameter.ID}\"";
            }
            parameters = $"[ {parameters} ]";

            // Generate program.
            string code = SkeletonCode
                .Replace("%OPCODE%", $"\"{InstructionDefinition.Opcode}\"")
                .Replace("%PARAMETERS%", $"{parameters}")
                .Replace("%PCOUNT%", $"{InstructionDefinition.Parameters.Length}")
                .Replace("%MEMBERS%", members)
                .Replace("%INITIALIZE%", initialize)
                .Replace("%EXECUTE%", execute);

            GD.Print(code);

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

            // Replace $register$ statements.
            // TODO.
            code = code.Replace("$OPCODE$", $"arguments[get_parameter_index(OPCODE)]");

            // Replace %argument% statements.
            for (int i = 0; i < definition.Parameters.Length; i++)
            {
                string id = definition.Parameters[i].ID;
                code = code.Replace($"%{id}%", $"arguments[get_parameter_index(\"{id}\")]");
            }

            return code;
        }
    }
}