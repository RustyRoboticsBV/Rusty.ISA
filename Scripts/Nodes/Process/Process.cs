using Godot;
using System.Collections.Generic;

namespace Rusty.ISA
{
    /// <summary>
    /// A node that can excute instruction programs.
    /// </summary>
    [GlobalClass]
    public partial class Process : Node
    {
        /* Public properties. */
        /// <summary>
        /// The instruction set that this process uses.
        /// </summary>
        [Export] public InstructionSet InstructionSet { get; private set; }
        /// <summary>
        /// The program that this process will execute.
        /// </summary>
        [Export] public Program Program { get; private set; }
        /// <summary>
        /// When enabled, the process will print messages when it creates execution handlers and registers, when it starts or
        /// stops, and when it executes an instruction.
        /// </summary>
        [Export] public bool UseDebugPrints { get; set; }
        /// <summary>
        /// When enabled, the process will print the source code of generated instruction execution handlers.
        /// </summary>
        [Export] public bool PrintExecutionHandlerCode { get; set; }

        /// <summary>
        /// Whether or not the process is playing.
        /// </summary>
        public bool Playing { get; private set; }
        /// <summary>
        /// Whether or not execution has been paused.
        /// </summary>
        public bool Paused { get; set; }
        /// <summary>
        /// The process' current position in the program.
        /// </summary>
        public int ProgramCounter { get; private set; }
        /// <summary>
        /// The instruction instance at the program counter.
        /// </summary>
        public InstructionInstance Current => Playing ? Program[ProgramCounter] : null;

        /// <summary>
        /// The current registers of this process.
        /// </summary>
        public Dictionary<string, Register> Registers { get; } = new();

        /* Private properties. */
        private Dictionary<string, ExecutionHandler> ExecutionHandlers { get; set; }
        private Dictionary<string, int> StartPoints { get; set; }
        private Dictionary<string, int> Labels { get; set; }

        /* Public methods. */
        /// <summary>
        /// Start playing from a start point.
        /// </summary>
        public void Play(string startPointName)
        {
            if (Program == null)
            {
                GD.PrintErr($"Process '{Name}' cannot start playing, because its program is set to null.");
                return;
            }

            EnsureStartPoints();

            Playing = true;
            Paused = false;

            if (StartPoints.ContainsKey(startPointName))
            {
                ProgramCounter = StartPoints[startPointName];

                if (UseDebugPrints)
                {
                    GD.Print($"Process '{Name}' began executing program '{Program.Name}' at start point '{startPointName}' "
                        + $"(line #{ProgramCounter})");
                }
                else
                {
                    GD.PrintErr($"Tried to start process '{Name}' at start point '{startPointName}' of program '{Program.Name}', "
                        + "but the program did not contain this start point.");
                }
            }
        }

        /// <summary>
        /// Stop playing.
        /// </summary>
        public void Stop()
        {
            if (UseDebugPrints)
                GD.Print($"Process '{Name}' stopped executing program '{Program.Name}'.");
            Playing = false;
            Paused = false;
        }

        /// <summary>
        /// Jump to a label instruction with some name.
        /// </summary>
        public void Goto(string targetLabel)
        {
            if (!Playing)
                return;

            EnsureLabels();

            if (Labels.ContainsKey(targetLabel))
            {
                ProgramCounter = Labels[targetLabel];
                if (UseDebugPrints)
                    GD.Print($"Program '{Name}' jumped to label '{targetLabel}' (line #{ProgramCounter}).");
            }
            else
            {
                GD.PrintErr($"Tried to jump process '{Name}' to label '{targetLabel}' of program '{Program.Name}', but the "
                    + "program did not contain this label.");
            }
        }

        /// <summary>
        /// Print a warning message.
        /// </summary>
        public void Warning(string message)
        {
            if (!Playing)
                return;

            GD.PrintErr($"Warning in ISA program '{Program.Name}' at line #{ProgramCounter} ({Current}): '{message}'");
        }

        /// <summary>
        /// Print an error message.
        /// </summary>
        public void Error(string message)
        {
            if (!Playing)
                return;

            GD.PrintErr($"Error in ISA program '{Program.Name}' at line #{ProgramCounter} ({Current}): '{message}'");
            Stop();
        }

        /// <summary>
        /// Get a register by name. The register is created if it didn't exist yet.
        /// </summary>
        public Register GetRegister(string name)
        {
            if (!Registers.ContainsKey(name))
            {
                if (UseDebugPrints)
                    GD.Print($"Added register '{name}' to process '{Name}'.");
                Registers.Add(name, new());
            }
            return Registers[name];
        }

        /// <summary>
        /// Change the program of this process. If the process was executing, it will terminate.
        /// </summary>
        public void ChangeProgram(Program program)
        {
            Stop();
            if (UseDebugPrints)
            {
                GD.Print($"Process '{Name}' changed from program '{(Program != null ? Program.Name : "(null)")}' to program "
                    + $"'{(program != null ? program.Name : "(null)")}'.");
            }
            Program = program;
        }

        /* Godot overrides. */
        public override void _Process(double deltaTime)
        {
            if (Playing)
            {
                if (ProgramCounter < 0 || ProgramCounter >= Program.Length)
                {
                    GD.PrintErr($"Process '{Name}' had its program counter go out-of-bounds at '{ProgramCounter}'. Execution "
                        + $"was terminated. Are you missing an END instruction?");
                    Stop();
                    return;
                }
                else
                {
                    if (UseDebugPrints)
                        GD.Print($"Process '{Name}': executing program '{Program.Name}' at line #{ProgramCounter}: {Current}.");
                    Execute(Current, deltaTime);
                    ProgramCounter++;
                }
            }
        }

        /* Private methods. */
        /// <summary>
        /// Execute an instruction.
        /// </summary>
        private void Execute(InstructionInstance instruction, double deltaTime)
        {
            EnsureExecutionHandlers();

            if (ExecutionHandlers.ContainsKey(instruction.Opcode))
                ExecutionHandlers[instruction.Opcode].Execute(instruction.Arguments, deltaTime);
        }

        /// <summary>
        /// Make sure that the start points have been found.
        /// </summary>
        private void EnsureStartPoints()
        {
            if (StartPoints != null)
                return;

            StartPoints = new();
            if (Program == null)
                return;
            for (int i = 0; i < Program.Length; i++)
            {
                if (Program[i].Opcode == "BEG")
                    StartPoints.Add(Program[i].Arguments[0], i);
            }
        }

        /// <summary>
        /// Make sure that the labels have been found.
        /// </summary>
        private void EnsureLabels()
        {
            if (Labels != null)
                return;

            Labels = new();
            if (Program == null)
                return;
            for (int i = 0; i < Program.Length; i++)
            {
                if (Program[i].Opcode == "LAB")
                    Labels.Add(Program[i].Arguments[0], i);
            }
        }

        /// <summary>
        /// Make sure that the execution handlers have been instantiated.
        /// </summary>
        private void EnsureExecutionHandlers()
        {
            if (ExecutionHandlers != null)
                return;

            ExecutionHandlers = new();
            if (InstructionSet == null)
                return;
            for (int i = 0; i < InstructionSet.Count; i++)
            {
                InstructionDefinition definition = InstructionSet[i];
                if (definition.Implementation != null)
                {
                    ExecutionHandler handler = new ExecutionHandlerGenerator(definition).Instantiate();
                    if (!ExecutionHandlers.ContainsKey(definition.Opcode))
                        ExecutionHandlers.Add(definition.Opcode, handler);
                    handler.Initialize(this);

                    if (UseDebugPrints)
                    {
                        GD.Print($"Generated execution handler for instruction with opcode '{definition.Opcode}'.");
                    }
                    if (PrintExecutionHandlerCode)
                        GD.Print(handler.SourceCode + "\n");
                }
            }
        }
    }
}