using Godot;
using System.Collections.Generic;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A node that can play cutscene programs.
    /// </summary>
    [GlobalClass]
    public partial class CutscenePlayer : Node
    {
        /* Public properties. */
        [Export] public InstructionSet InstructionSet { get; set; }
        [Export] public CutsceneProgram Program { get; set; }

        public bool IsPlaying => Track != null;
        public int ProgramCounter => Track.ProgramCounter;

        /* Private properties. */
        private CutsceneTrack Track { get; set; }
        private Dictionary<string, Node> ExecutionHandlers { get; set; }

        private const string SkeletonCode = "extends Node;" +
            "\n" +
            "\nvar player : CutscenePlayer;" +
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

        /* Public methods. */
        public void Play()
        {
            Track = new(Program);
        }

        public void Play(string startPointName)
        {
            Play();
            Track.Jump(startPointName);
        }

        public void Stop()
        {
            Track = null;
        }

        public void Jump(string targetLabel)
        {
            if (Track != null)
                Track.Jump(targetLabel);
        }

        /* Godot overrides. */
        public override void _EnterTree()
        {
            Play();
        }

        public override void _Process(double delta)
        {
            if (Track != null)
            {
                if (Track.IsOutOfBounds)
                {
                    Stop();
                    return;
                }
                else
                {
                    Execute(Track.Current, delta);
                    Track?.Advance();
                }
            }
        }

        /* Private methods. */
        private void Execute(InstructionInstance instruction, double deltaTime)
        {
            EnsureExecutionHandlers();
            if (ExecutionHandlers.ContainsKey(instruction.Opcode))
                ExecutionHandlers[instruction.Opcode].Call("_execute", instruction.Arguments, deltaTime);
        }

        private void EnsureExecutionHandlers()
        {
            if (ExecutionHandlers != null)
                return;

            ExecutionHandlers = new();
            for (int i = 0; i < InstructionSet.Count; i++)
            {
                InstructionDefinition definition = InstructionSet[i];
                if (definition.Implementation != "")
                {
                    Node handler = GetExecutionHandler(definition);
                    handler.Call("_initialize", this);
                    ExecutionHandlers.Add(definition.Opcode, handler);
                }
            }
        }

        private Node GetExecutionHandler(InstructionDefinition definition)
        {
            // Get implementations.
            string execute = ProcessCode(definition.Implementation, definition);

            // Generate program.
            string code = SkeletonCode
                .Replace("%OPCODE%", $"\"{definition.Opcode}\"")
                .Replace("%INITIALIZE%", ProcessCode("pass;", definition))
                .Replace("%EXECUTE%", execute);

            // Fix used function arguments.
            if (execute.Contains("arguments"))
                code = code.Replace("_arguments", "arguments");
            if (execute.Contains("delta_time"))
                code = code.Replace("_delta_time", "delta_time");

            GD.Print(code);

            // Create GDScript.
            GDScript script = new();
            script.SourceCode = code;
            script.Reload();

            // Create node.
            Node node = (Node)script.New();
            AddChild(node);
            return node;
        }

        private string ProcessCode(string code, InstructionDefinition definition)
        {
            // Add indentation.
            code = "\t" + code.Replace("\n", "\n\t");

            // Replace $register$ statements.

            // Replace %parameter% statements.
            for (int i = 0; i < definition.Parameters.Length; i++)
            {
                string id = definition.Parameters[i].ID;
                code = code.Replace($"%{id}%", $"arguments[get_parameter_index(\"{id}\")]");
            }

            return code;
        }
    }
}