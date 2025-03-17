using Godot;
using static System.Formats.Asn1.AsnWriter;

namespace Rusty.ISA
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

        /// <summary>
        /// The generated source code.
        /// </summary>
        public string SourceCode { get; private set; } = "";
        /// <summary>
        /// The generated GDScript.
        /// </summary>
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
            "\n" +
            "\nvar process : Process;" +
            "\nvar found_dependencies : bool = false;%MEMBERS%" +
            "\n" +
            "\nfunc _initialize(_process : Process):" +
            "\n\tprocess = _process;" +
            "\n%DEP_CHECKS%found_dependencies = true;%INITIALIZE%" +
            "\n" +
            "\nfunc _execute(_arguments : Array[String], _delta_time : float):" +
            "\n%EXECUTE%" +
            "\n" +
            "\nfunc end():" +
            "\n\tprocess.Stop();" +
            "\n" +
            "\nfunc goto(target_label : String):" +
            "\n\tprocess.Goto(target_label);" +
            "\n" +
            "\nfunc warning(message : String):" +
            "\n\tprocess.Warning(message);" +
            "\n" +
            "\nfunc error(message : String):" +
            "\n\tprocess.Error(message);" +
            "\n" +
            "\nfunc get_register(register_name : String) -> Register:" +
            "\n\treturn process.GetRegister(register_name);" +
            "\n" +
            "\nfunc get_parameter_id(index : int) -> String:" +
            "\n\treturn process.InstructionSet.GetDefinition(OPCODE).Parameters[index].ID;" +
            "\n" +
            "\nfunc get_parameter_index(id : String) -> int:" +
            "\n\treturn process.InstructionSet.GetDefinition(OPCODE).GetParameterIndex(id);";

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
            // Get opcode.
            string opcode = InstructionDefinition.Opcode;

            // Generate dependency checks.
            string[] dependencies = InstructionDefinition.Implementation.Dependencies;
            string dependencyChecks = "";
            if (dependencies.Length > 0)
            {
                foreach (string dependency in dependencies)
                {
                    // Enclose in double-quotes.
                    string dep = $"\"{dependency}\"";

                    // Add type-check code.
                    if (dependencyChecks != "")
                        dependencyChecks += "\n";
                    dependencyChecks += $"if !type_exists({dep}):\n\tfailed = {dep};";
                }

                // Create type-check block.
                dependencyChecks = $"var failed : String = \"\";\n{dependencyChecks}\nif failed != \"\":"
                    + $"\n\twarning(\"Instructions with opcode '{opcode}' require that global class with name '\" + failed + \"' "
                    + "exists, but this class cannot be found. The instruction will not work and print a warning when executed!\")"
                    + "\n\treturn;\n";
            }

            // Generate implementations.
            string members = InstructionDefinition.Implementation.Members;
            if (members.Length > 0)
                members = "\n" + members;

            string initialize = ProcessCode(InstructionDefinition.Implementation.Initialize, InstructionDefinition);

            string execute = InstructionDefinition.Implementation.Execute;
            if (execute == "")
                execute = "\tpass;";
            else
                execute = ProcessCode(execute, InstructionDefinition);

            // Generate parameter list.
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
                .Replace("%OPCODE%", $"\"{opcode}\"")
                .Replace("%PARAMETERS%", $"{parameters}")
                .Replace("%PCOUNT%", $"{InstructionDefinition.Parameters.Length}")
                .Replace("%MEMBERS%", members)
                .Replace("%DEP_CHECKS%", dependencyChecks)
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
            // Process character-by-character...
            for (int i = 0; i < code.Length; i++)
            {
                // Store current string length.
                int oldLength = code.Length;

                // If we encounter a register starter symbol...
                if (code[i] == '$')
                {
                    int escape = code.IndexOf('$', i + 1);

                    // Do nothing if there is no closing character, or it is on another line.
                    if (escape == -1 && code.IndexOf('\n', i, escape - i) != -1)
                        continue;

                    // Parse register.
                    code = ParseRegister(code, i, escape);
                }

                // If we encountered a parameter starter symbol...
                else if (code[i] == '%')
                {
                    int escape = code.IndexOf('%', i + 1);

                    // Do nothing if there is no closing character, or it is on another line.
                    if (escape == -1 && code.IndexOf('\n', i, escape - i) != -1)
                        continue;

                    // Parse argument.
                    code = ParseArgument(code, i, escape);
                }

                // If the string length changed, update for loop index.
                if (code.Length != oldLength)
                    i += code.Length - oldLength;
            }

            // Add indentation.
            code = "\t" + code.Replace("\n", "\n\t");

            // Replace %argument% statements.
            for (int i = 0; i < definition.Parameters.Length; i++)
            {
                string id = definition.Parameters[i].ID;
                code = code.Replace($"%{id}%", $"arguments[get_parameter_index(\"{id}\")]");
            }

            return code;
        }

        /// <summary>
        /// Take a string of code and replace a $register$ shorthand.
        /// </summar>
        private static string ParseRegister(string code, int startIndex, int endIndex)
        {
            // Isolate register name.
            string registerName = code.Substring(startIndex + 1, endIndex - startIndex - 1);

            // If the register name does not equal "OPCODE", enclose it in double-quotes.
            if (registerName != "OPCODE")
                registerName = $"\"{registerName}\"";

            // Replace potential function call after register call with a pascal case name.
            // We need to do this because GDScript uses a snake case convention for function names, whereas CSharp uses a
            // pascal case convention.
            // We only change the first character because all register function names are a single word long.
            if (code[endIndex + 1] == '.')
                code = Replace(code, startIndex + 2, 1, code[endIndex + 2].ToString().ToUpper());

            // Replace register shorthand with proper getter code.
            return Replace(code, startIndex, endIndex - startIndex + 1, $"get_register({registerName})");
        }

        /// <summary>
        /// Take a string of code and replace an %argument% shorthand.
        /// </summar>
        private static string ParseArgument(string code, int startIndex, int endIndex)
        {
            // Isolate parameter ID.
            string parameterName = code.Substring(startIndex + 1, endIndex - startIndex - 1);

            // Generate new code snippet.
            string snippet = $"arguments[get_parameter_index(\"{parameterName}\")]";

            // Replace argument shorthand with code snippet.
            return Replace(code, startIndex, endIndex - startIndex + 1, snippet);
        }

        /// <summary>
        /// Replace a sub-string starting at some index and ending at another index with a new sub-string, and return the result.
        /// </summary>
        private static string Replace(string str, int startIndex, int length, string replacement)
        {
            return str.Substring(0, startIndex) + replacement + str.Substring(startIndex + length + 1);
        }
    }
}