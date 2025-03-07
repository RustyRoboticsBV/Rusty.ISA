using Godot;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A utility class for creating instruction execution handlers.
    /// </summary>
    internal sealed partial class ExecutionHandlerGenerator : Resource
    {
        /* Public methods. */
        /// <summary>
        /// The instruction definition, used as an implementation source by the execution handler.
        /// </summary>
        [Export] public InstructionDefinition InstructionDefinition { get; private set; }

        public string SourceCode { get; private set; } = "";
        public GDScript GDScript { get; private set; }

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
        public ExecutionHandlerGenerator(InstructionDefinition instructionDefinition)
        {
            InstructionDefinition = instructionDefinition;

            SourceCode = GenerateSourceCode();
            GDScript = new()
            {
                SourceCode = SourceCode
            };
            GDScript.Reload();
        }

        /* Public methods. */
        /// <summary>
        /// Create an execution handler node for the instruction definition.
        /// </summary>
        public ExecutionHandler Instantiate()
        {
            return new(GDScript);
        }

        /* Private methods. */
        /// <summary>
        /// Generate a GDScript source code program from the instruction definition.
        /// </summary>
        private string GenerateSourceCode()
        {
            // Get implementations.
            string members = InstructionDefinition.Implementation.Members;
            if (members.Length > 0)
                members = "\n" + members;

            string initialize = ProcessCode(InstructionDefinition.Implementation.Initialize, InstructionDefinition);

            string execute = InstructionDefinition.Implementation.Execute;
            if (execute == "")
                execute = "\tpass;";
            else
                execute = ProcessCode(execute, InstructionDefinition);

            // List parameters.
            string parameters = "";
            foreach (Parameter parameter in InstructionDefinition.Parameters)
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

            // Fix used function arguments.
            if (execute.Contains("arguments"))
                code = code.Replace("_arguments", "arguments");
            if (execute.Contains("delta_time"))
                code = code.Replace("_delta_time", "delta_time");

            return code;
        }

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