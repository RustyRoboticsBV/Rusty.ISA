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
        [Export] public InstructionSet InstructionSet { get; set; }
        /// <summary>
        /// The program that this process will execute.
        /// </summary>
        [Export] public Program Program { get; set; }

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
            EnsureStartPoints();

            Playing = true;
            Paused = false;

            if (StartPoints.ContainsKey(startPointName))
                ProgramCounter = StartPoints[startPointName];
            else
            {
                GD.PrintErr($"Tried to start process '{Name}' at start point '{startPointName}' of program '{Program.Name}', "
                    + "but the program did not contain this start point.");
            }
        }

        /// <summary>
        /// Stop playing.
        /// </summary>
        public void Stop()
        {
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
                ProgramCounter = Labels[targetLabel];
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

            GD.PrintErr($"Warning in ISA program '{Program.Name}' at line {ProgramCounter} ({Current}): '{message}'");
        }

        /// <summary>
        /// Print an error message.
        /// </summary>
        public void Error(string message)
        {
            if (!Playing)
                return;

            GD.PrintErr($"Error in ISA program '{Program.Name}' at line {ProgramCounter} ({Current}): '{message}'");
            Stop();
        }
        
        /* Godot overrides. */
        /// <summary>
        /// Perform a single execution loop.
        /// </summary>
        public override void _Process(double deltaTime)
        {
            if (Playing)
            {
                if (ProgramCounter < 0 || ProgramCounter >= Program.Length)
                {
                    Error("Program counter was out of bounds. Execution was terminated.");
                    return;
                }
                else
                {
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
            for (int i = 0; i < InstructionSet.Count; i++)
            {
                InstructionDefinition definition = InstructionSet[i];
                if (definition.Implementation != null)
                {
                    ExecutionHandler handler = new ExecutionHandlerGenerator(definition).Instantiate();
                    if (!ExecutionHandlers.ContainsKey(definition.Opcode))
                        ExecutionHandlers.Add(definition.Opcode, handler);
                    handler.Initialize(this);
                }
            }
        }
    }
}