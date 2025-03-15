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
        [Export] public bool StartPlaying { get; set; }
        [Export] public string StartPoint { get; set; } = "Start";

        public bool IsPlaying => Track != null;
        public bool IsPaused { get; private set; }
        public int ProgramCounter => Track.ProgramCounter;

        /* Private properties. */
        private CutsceneTrack Track { get; set; }
        private Dictionary<string, ExecutionHandler> ExecutionHandlers { get; set; }

        /* Public methods. */
        /// <summary>
        /// Start playing from the instruction specified by the StartPoint property.
        /// </summary>
        public void Play()
        {
            Play(StartPoint);
        }

        /// <summary>
        /// Start playing from a start point.
        /// </summary>
        public void Play(string startPointName)
        {
            Track = new(Program);

            if (!string.IsNullOrEmpty(startPointName))
                Track.Start(startPointName);

            IsPaused = false;
        }

        /// <summary>
        /// Pause the cutscene.
        /// </summary>
        public void Pause()
        {
            IsPaused = true;
        }

        public void Unpause()
        {
            IsPaused = false;
        }

        /// <summary>
        /// Stop playing.
        /// </summary>
        public void Stop()
        {
            Track = null;
            IsPaused = false;
        }

        /// <summary>
        /// Jump to a label instruction with some name.
        /// </summary>
        public void Jump(string targetLabel)
        {
            if (Track != null)
                Track.Jump(targetLabel);
        }

        /// <summary>
        /// Print a warning message.
        /// </summary>
        public void Warning(string message)
        {
            if (!IsPlaying)
                return;

            GD.PrintErr($"Warning in cutscene program '{Program.Name}' at line {Track.ProgramCounter} ({Track.Current}): '{message}'");
        }

        /// <summary>
        /// Print an error message.
        /// </summary>
        public void Error(string message)
        {
            if (!IsPlaying)
                return;

            GD.PrintErr($"Error in cutscene program '{Program.Name}' at line {Track.ProgramCounter} ({Track.Current}): '{message}'");
            Stop();
        }

        /* Godot overrides. */
        public override void _Ready()
        {
            if (StartPlaying)
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